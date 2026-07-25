using FluentUI.Blazor.Community.Components.Chat.Files;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Components.Base;
using FluentUI.Blazor.Community.Components.Media;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Dialogs;

/// <summary>
/// Displays a chat message in the user interface.
/// </summary>
public partial class ChatMessageDialog : IAsyncDisposable
{
    private readonly string Id = Identifier.NewId();

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
    /// Gets or sets the message to be displayed in the viewer.
    /// </summary>
    [Parameter]
    public ChatMessage Message { get; set; } = default!;

    /// <summary>
    /// Gets or sets the event callback that is triggered to retrieve the chapters for the message, allowing for dynamic content loading and interaction within the viewer.
    /// </summary>
    [Parameter]
    public EventCallback<ChapterEventArgs> GetChapters { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is triggered to retrieve the video metadata.
    /// </summary>
    [Parameter]
    public EventCallback<VideoMetadataEventArgs> GetVideoMetadata { get; set; }

    /// <inheritdoc />
    protected override async Task OnActionClickedAsync(bool primary)
    {
        await DialogInstance.CloseAsync();
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
