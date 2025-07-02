using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an item of the <see cref="FileManagerNavigationView"/>.
/// </summary>
public partial class FileManagerNavigationViewItem
    : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the text of the item.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets the event callback which occurs when the item is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<FileManagerNavigationViewItem> OnClick { get; set; }

    /// <summary>
    /// Occurs when the item is clicked.
    /// </summary>
    /// <returns>Returns a task which click on the menu when completed.</returns>
    private async Task OnClickItemAsync()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(this);
        }
    }
}
