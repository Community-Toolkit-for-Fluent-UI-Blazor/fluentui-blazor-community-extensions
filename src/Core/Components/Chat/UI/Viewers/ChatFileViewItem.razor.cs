using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Viewers;

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
    /// Represents the URL of the file.
    /// </summary>
    private string? _url;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatFileViewItem"/> class with a new identifier.
    /// </summary>
    /// <param name="configuration">The library configuration.</param>
    public ChatFileViewItem(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets the CSS class to apply to the chat file view item.
    /// </summary>
    private string? Css => DefaultClassBuilder
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
    /// 
    /// </summary>
    [Inject]
    private IFileUploader FileUploader { get; set; } = default!;

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

        if (!firstRender || Item is null || _itemLoaded)
        {
            return;
        }

        var data = await Item.GetDataAsync();

        if (data.Length == 0)
        {
            _itemLoaded = true;
            await InvokeAsync(StateHasChanged);
            return;
        }

        var contentType = Item.ContentType;

        if (contentType.StartsWith("image") ||
            contentType.StartsWith("audio") ||
            contentType.StartsWith("video"))
        {
            _url = await FileUploader.UploadFileAsync(
                Id!,
                data,
                contentType);

            _itemLoaded = true;
            await InvokeAsync(StateHasChanged);
        }
        else
        {
            _itemLoaded = true;
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        try
        {
            if (FileUploader is not null && !string.IsNullOrEmpty(_url))
            {
                await FileUploader.RevokeUrlAsync(_url);
            }
        }
        catch (JSDisconnectedException)
        {
        }

        GC.SuppressFinalize(this);
    }
}
