using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the overflow button for the <see cref="FluentCxPathBar"/>.
/// </summary>
public partial class OverflowButton : FluentComponentBase
{
    /// <summary>
    /// Represents if the menu is open.
    /// </summary>
    private bool _isMenuOpen;

    /// <summary>
    /// Gets or sets the parent of the button.
    /// </summary>
    [CascadingParameter]
    private FluentCxPathBar? Parent { get; set; }

    /// <summary>
    /// Gets or sets the items inside the overflow.
    /// </summary>
    [Parameter]
    public List<IPathBarItem> Items { get; set; } = [];

    /// <summary>
    /// Occurs when the menu is selected.
    /// </summary>
    /// <param name="item">Represents the selected item.</param>
    private async Task OnItemTappedAsync(IPathBarItem item)
    {
        _isMenuOpen = false;

        if (Parent is not null)
        {
            await Parent.UpdatePathAsync(item);
        }
    }
}
