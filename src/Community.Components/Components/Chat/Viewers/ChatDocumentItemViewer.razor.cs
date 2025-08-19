using FluentUI.Blazor.Community.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a document viewer.
/// </summary>
public partial class ChatDocumentItemViewer
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
    /// Represents the javaScript file that contains the logic for the chat file view item component.
    /// </summary>
    private const string JavascriptFile = "./_content/FluentUI.Blazor.Community.Components/Components/Chat/Viewers/ChatDocumentItemViewer.razor.js";

    /// <summary>
    /// Represents the javaScript module reference for the chat file view item component.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Gets or sets the item to view.
    /// </summary>
    [Parameter]
    public IChatFile? Item { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatDocumentItemViewer"/> class with a new identifier.
    /// </summary>
    public ChatDocumentItemViewer()
    {
        Id = Identifier.NewId();
    }

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
    /// Gets or sets the label to display when the file is loading.
    /// </summary>
    [Parameter]
    public string? LoadingLabel { get; set; }

    /// <summary>
    /// Gets or sets the label to display when the audio file is playing.
    /// </summary>
    [Parameter]
    public string? PlayLabel { get; set; }

    /// <summary>
    /// Gets or sets the label to display when the audio file is paused.
    /// </summary>
    [Parameter]
    public string? PauseLabel { get; set; }

    /// <summary>
    /// Gets or sets the js runtime to use for invoking JavaScript functions.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptFile);

            if (Item is not null && !_itemLoaded)
            {
                if (Item is IUrlChatFile urlChatFile)
                {
                    _source = urlChatFile.Url;
                }
                else if (Item is IBinaryChatFile binaryChatFile)
                {
                    if (Item.ContentType.StartsWith("audio") ||
                        Item.ContentType.StartsWith("image"))
                    {
                        _source = Base64ContentHelper.GetBase64Content(binaryChatFile.Data, Item.ContentType);
                    }
                    else
                    {
                        _source = Base64ContentHelper.GetBase64Content(binaryChatFile.Data, Item.ContentType);
                        await _module!.InvokeVoidAsync("loadVideo", Id, _source, Item!.ContentType);
                    }
                }

                _itemLoaded = true;
                await InvokeAsync(StateHasChanged);
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
