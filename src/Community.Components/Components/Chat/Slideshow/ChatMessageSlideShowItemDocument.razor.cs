using FluentUI.Blazor.Community.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

public partial class ChatMessageSlideShowItemDocument<TItem>
    : FluentComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Represents a value indicating whether the item has been loaded.
    /// </summary>
    private bool _itemLoaded;

    /// <summary>
    /// Represents the content of the file.
    /// </summary>
    private string? _content;

    /// <summary>
    /// Represents the javaScript file that contains the logic for the chat message slide show item component.
    /// </summary>
    private const string JavascriptFile = "./_content/FluentUI.Blazor.Community.Components/Components/Chat/Slideshow/ChatMessageSlideShowItemDocument.razor.js";

    /// <summary>
    /// Represents the javaScript module reference for the chat file view item component.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatMessageSlideShowItemDocument{TItem}"/> class with a new identifier.
    /// </summary>
    public ChatMessageSlideShowItemDocument()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the file to display in the chat file view item.
    /// </summary>
    [Parameter]
    public IChatFile? Item { get; set; }

    /// <summary>
    /// Gets or sets the label to display when the file is loading.
    /// </summary>
    [Parameter]
    public string? LoadingLabel { get; set; }

    /// <summary>
    /// Gets or sets the label to display when an audio is playing.
    /// </summary>
    [Parameter]
    public string? PauseLabel { get; set; }

    /// <summary>
    /// Gets or sets the label to display when an audio is paused.
    /// </summary>
    [Parameter]
    public string? PlayLabel { get; set; }

    /// <summary>
    /// Gets or sets the js runtime to use for invoking JavaScript functions.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the direct parent.
    /// </summary>
    [CascadingParameter]
    private SlideshowItem<TItem> DirectParent { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptFile);

            if (Item is not null && !_itemLoaded)
            {
                if (Item is BinaryChatFile binaryChatFile)
                {
                    _content = Base64ContentHelper.GetBase64Content(binaryChatFile.Data, Item.ContentType);

                    if (Item.ContentType.StartsWith("video") && _content?.Length > 0)
                    {
                        await _module!.InvokeVoidAsync("loadVideo", Id, _content, Item!.ContentType);
                    }
                }
                else if (Item is UrlChatFile urlChatFile)
                {
                    _content = urlChatFile.Url;
                }

                _itemLoaded = true;
                await InvokeAsync(StateHasChanged);

                if (DirectParent is not null)
                {
                    await DirectParent.RefreshAsync();
                }
            }
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.DisposeAsync();
                _module = null;
            }
        }
        catch (JSDisconnectedException)
        {
        }

        GC.SuppressFinalize(this);
    }
}
