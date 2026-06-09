using FluentUI.Blazor.Community.Components.Chat.Files;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Dialogs;

/// <summary>
/// Displays a chat message in the user interface.
/// </summary>
public partial class ChatMessageDialog : IAsyncDisposable
{
    private readonly string Id = Identifier.NewId();

    private const string JavascriptModulePath = FluentCxConstants.JAVASCRIPT_ROOT + "Chat/UI/Dialogs/ChatMessageDialog.razor.js";

    /// <summary>
    /// Represents the files to be displayed in the viewer.
    /// </summary>
    private readonly List<IUrlChatFile> _files = [];

    /// <summary>
    /// Represents the URLs to be revoked after the viewer is closed to free up resources.
    /// </summary>
    private readonly List<string?> _urlToRevoke = [];

    /// <summary>
    /// Gets or sets the dynamic state for the chat message, which includes read states, reactions, and files.
    /// </summary>
    [Inject]
    private ChatMessageDynamicState DynamicState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the state for the chat, which includes information about the current room and user.
    /// </summary>
    [Inject]
    private ChatState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the file uploader service used to upload files in the chat.
    /// </summary>
    [Inject]
    private IFileUploader FileUploader { get; set; } = default!;

    /// <summary>
    /// Gets or sets the JavaScript runtime for invoking JavaScript functions.
    /// </summary>
    [Inject]
    private IJSRuntime Runtime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the message to be displayed in the viewer.
    /// </summary>
    [Parameter]
    public ChatMessage Message { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnActionClickedAsync(bool primary)
    {
        await DialogInstance.CloseAsync();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!firstRender)
        {
            return;
        }

        var module = await Runtime.InvokeAsync<IJSObjectReference>("import", JavascriptModulePath);
        await module.InvokeVoidAsync("FluentUI.Blazor.Community.ChatMessageDialog.SetDialogContentHeight", Id);
    }

    private IReadOnlyList<IChatFile> GetFiles()
    {
        if (State.RoomView is null)
        {
            return [];
        }

        return DynamicState.GetFiles(State.RoomView.Room, Message.Id);
    }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var files = GetFiles();
        _files.AddRange(files.OfType<IUrlChatFile>());

        var binaryFiles = files.OfType<IBinaryChatFile>();

        foreach (var file in binaryFiles)
        {
            var url = await FileUploader.UploadFileAsync(Invariant.ToString(file.Id), file.Content, file.ContentType);

            _files.Add(new UrlChatFile()
            {
                Id = file.Id,
                MessageId = file.MessageId,
                CreatedDate = file.CreatedDate,
                Name = file.Name,
                Url = url,
                ContentType = file.ContentType
            });

            _urlToRevoke.Add(url);
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        foreach (var url in _urlToRevoke)
        {
            if (!string.IsNullOrEmpty(url))
            {
                await FileUploader.RevokeUrlAsync(url);
            }
        }

        GC.SuppressFinalize(this);
    }
}
