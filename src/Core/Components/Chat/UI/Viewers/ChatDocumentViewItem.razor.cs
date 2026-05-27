using FluentUI.Blazor.Community.Components.Chat.Files;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Viewers;

/// <summary>
/// Represents a document viewer.
/// </summary>
public partial class ChatDocumentViewItem
    : FluentComponentBase
{
    /// <summary>
    /// Represents a value indicating whether the item has been loaded.
    /// </summary>
    private bool _itemLoaded;

    /// <summary>
    /// Represents the data of the file (url or bytes).
    /// </summary>
    private string? _source;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatDocumentViewItem"/> class with a new identifier.
    /// </summary>
    /// <param name="configuration">The library configuration.</param>
    public ChatDocumentViewItem(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the item to view.
    /// </summary>
    [Parameter]
    public IChatFile? Item { get; set; }

    /// <summary>
    /// Gets or sets the item template to use for rendering the file in the chat file view item.
    /// </summary>
    [Parameter]
    public RenderFragment<IChatFile>? Template { get; set; }

    /// <summary>
    /// Gets or sets the loading item content to use for rendering the loading state of the file in the chat file view item.
    /// </summary>
    [Parameter]
    public RenderFragment? LoadingContent { get; set; }

    /// <summary>
    /// Gets or sets the file uploader service to use for uploading files.
    /// </summary>
    [Inject]
    private IFileUploader FileUploader { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!firstRender)
        {
            return;
        }

        if (Item is not null && !_itemLoaded)
        {
            if (Item is IUrlChatFile urlChatFile)
            {
                _source = urlChatFile.Url;
            }
            else if (Item is IBinaryChatFile binaryChatFile)
            {
                _source = await FileUploader.UploadFileAsync(Id!, binaryChatFile.Data, Item.ContentType);
            }

            _itemLoaded = true;
            await InvokeAsync(StateHasChanged);
        }
    }
}
