using FluentUI.Blazor.Community.Extensions;
using FluentUI.Blazor.Community.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that displays a file in a chat context.
/// </summary>
public partial class ChatFileViewItem
    : FluentComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Represents a value indicating whether the item has been loaded.
    /// </summary>
    private bool _itemLoaded;

    /// <summary>
    /// Represents the base64 encoded data of the file.
    /// </summary>
    private string? _base64Data;

    /// <summary>
    /// Represents the javaScript file that contains the logic for the chat file view item component.
    /// </summary>
    private const string JavascriptFile = "./_content/FluentUI.Blazor.Community.Components/Components/Chat/Viewers/ChatFileViewItem.razor.js";

    /// <summary>
    /// Represents the javaScript module reference for the chat file view item component.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatFileViewItem"/> class with a new identifier.
    /// </summary>
    public ChatFileViewItem()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// 
    /// </summary>
    private string? Css => new CssBuilder(Class)
        .AddClass("chat-file-view-item ")
        .Build();

    /// <summary>
    /// Gets or sets the file to display in the chat file view item.
    /// </summary>
    [Parameter]
    public ChatFileEventArgs? Item { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the file view item is dismissed.
    /// </summary>
    [Parameter]
    public EventCallback OnDismiss { get; set; }

    /// <summary>
    /// Gets or sets the item template to use for rendering the file in the chat file view item.
    /// </summary>
    [Parameter]
    public RenderFragment<ChatFileEventArgs>? Template { get; set; }

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
    /// Gets or sets the js runtime to use for invoking JavaScript functions.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Occurs when the file view item is dismissed.
    /// </summary>
    /// <returns>Returns a task which dismiss the file item when completed.</returns>
    private async Task OnDismissAsync()
    {
        if (OnDismiss.HasDelegate)
        {
            await OnDismiss.InvokeAsync();
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptFile);

            if (Item is not null && !_itemLoaded)
            {
                if (Item.ContentType.StartsWith("audio") ||
                    Item.ContentType.StartsWith("image"))
                {
                    if (Item.DataFunc is not null)
                    {
                        var data = await Item.DataFunc();
                        _base64Data = Base64ContentHelper.GetBase64Content(data, Item.ContentType);
                    }
                    else if (Item.Data.Length > 0)
                    {
                        _base64Data = Base64ContentHelper.GetBase64Content(Item.Data, Item.ContentType);
                    }
                }
                else if (Item.ContentType.StartsWith("video") && Item.Data.Length > 0)
                {
                    _base64Data = Base64ContentHelper.GetBase64Content(Item.Data, Item.ContentType);
                    await _module!.InvokeVoidAsync("loadVideo", Id, _base64Data, Item!.ContentType);
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
