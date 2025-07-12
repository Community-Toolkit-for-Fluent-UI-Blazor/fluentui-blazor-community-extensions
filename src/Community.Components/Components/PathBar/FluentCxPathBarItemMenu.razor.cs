using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Filled;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a menu for the <see cref="FluentCxPathBarItem"/>.
/// </summary>
public partial class FluentCxPathBarItemMenu
    : FluentComponentBase
{
    /// <summary>
    /// Represents the opened state of the menu.
    /// </summary>
    private bool _isMenuOpen;

    /// <summary>
    /// Represents the chevron right when the menu is not opened.
    /// </summary>
    private static readonly Icon _chevronRight = new Size20.ChevronRight();

    /// <summary>
    /// Represents the chevron down when the menu is opened.
    /// </summary>
    private static readonly Icon _chevronDown = new Size20.ChevronDown();

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxPathBarItemMenu"/> class.
    /// </summary>
    public FluentCxPathBarItemMenu()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the parent of this menu.
    /// </summary>
    [CascadingParameter]
    private FluentCxPathBarItem Parent { get; set; } = default!;

    /// <summary>
    /// Gets or sets the items of this menu.
    /// </summary>
    [Parameter]
    public IEnumerable<IPathBarItem> Items { get; set; } = [];

    /// <summary>
    /// Occurs when the menu is selected.
    /// </summary>
    /// <param name="item">Represents the selected item.</param>
    private void OnItemTapped(IPathBarItem item)
    {
        _isMenuOpen = false;
        
        Parent?.Ancestor?.SetPath(PathBarItem.GetPath(item));
    }
}
