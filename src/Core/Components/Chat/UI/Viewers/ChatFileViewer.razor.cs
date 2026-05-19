using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Viewers;

/// <summary>
/// Represents a component that displays a list of files in a chat context.
/// </summary>
public partial class ChatFileViewer
    : FluentComponentBase
{
    private readonly string? _defaultCss;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatFileViewer"/> class.
    /// </summary>
    /// <param name="configuration">The library configuration.</param>
    public ChatFileViewer(LibraryConfiguration configuration)
        : base(configuration)
    {
        _defaultCss = DefaultClassBuilder
        .AddClass("chat-file-viewer")
        .Build();
    }

    /// <summary>
    /// Gets or sets the files to display in the chat file viewer.
    /// </summary>
    [Parameter]
    public List<ChatFileEventArgs> Files { get; set; } = [];

    /// <summary>
    /// Gets or sets the callback that is invoked when a file is dismissed.
    /// </summary>
    [Parameter]
    public EventCallback<ChatFileEventArgs> OnDismiss { get; set; }

    /// <summary>
    /// Gets or sets the item template to use for rendering each file in the chat file viewer.
    /// </summary>
    [Parameter]
    public RenderFragment<ChatFileEventArgs>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the loading item content to use for rendering the loading state of the file in the chat file viewer.
    /// </summary>
    [Parameter]
    public RenderFragment? LoadingItemContent { get; set; }

    /// <summary>
    /// Gets the CSS class to apply to the chat file viewer.
    /// </summary>
    private string? Css => _defaultCss;

    private static string InternalPadding => Microsoft.FluentUI.AspNetCore.Components.Padding.Horizontal1;

    /// <summary>
    /// Occurs when a file is dismissed.
    /// </summary>
    /// <param name="file">File to dismiss.</param>
    /// <returns>Returns a task which dismiss a file when completed.</returns>
    private async Task OnDismissAsync(ChatFileEventArgs file)
    {
        if (OnDismiss.HasDelegate)
        {
            await OnDismiss.InvokeAsync(file);
        }
    }
}
