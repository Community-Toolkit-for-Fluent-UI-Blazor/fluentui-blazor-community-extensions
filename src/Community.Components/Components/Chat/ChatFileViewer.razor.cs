using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that displays a list of files in a chat context.
/// </summary>
public partial class ChatFileViewer
    : FluentComponentBase
{
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
    private string? Css => new CssBuilder(Class)
        .AddClass("chat-file-viewer")
        .Build();

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
            await InvokeAsync(StateHasChanged);
        }
    }
}
