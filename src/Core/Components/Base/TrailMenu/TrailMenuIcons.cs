using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Filled;

namespace FluentUI.Blazor.Community.Components.TrailMenu;

/// <summary>
/// Provides a set of static icons commonly used for navigation and actions in trail menus.
/// </summary>
/// <remarks>The icons exposed by this class are instantiated from the Size24 collection, ensuring consistent
/// appearance and sizing across menu components. Use these icons to visually indicate navigation direction or
/// additional actions within a trail menu.</remarks>
internal static class TrailMenuIcons
{
    /// <summary>
    /// Represents the chevron icon pointing to the right, used to indicate navigation to next item in a trail menu.
    /// </summary>
    public static readonly Icon ChevronRight = new Size24.ChevronRight();

    /// <summary>
    /// Represents the more horizontal icon, typically used to indicate an overflow menu in a trail menu.
    /// </summary>

    public static readonly Icon More = new Size24.MoreHorizontal();

    /// <summary>
    /// Represents the chevron icon pointing downwards, used to indicate navigation to a submenu in a trail menu.
    /// </summary>
    public static readonly Icon ChevronDown = new Size24.ChevronDown();

    /// <summary>
    /// Represents the home icon.
    /// </summary>
    public static readonly Icon HomeIcon = new Size24.Home();

    /// <summary>
    /// Represents the desktop icon.
    /// </summary>
    public static readonly Icon DesktopIcon = new Size24.Desktop();

    /// <summary>
    /// Represents the phone icon.
    /// </summary>
    public static readonly Icon PhoneIcon = new Size24.Phone();
}
