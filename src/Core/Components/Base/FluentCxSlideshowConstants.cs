using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Filled;

namespace FluentUI.Blazor.Community.Components.Components.Base;

/// <summary>
/// Provides a set of constants representing chevron icons commonly used for directional navigation in user interfaces.
/// </summary>
/// <remarks>Each constant exposes an icon corresponding to a specific navigation direction: left, right, up, or
/// down. These icons can be used to indicate navigation actions such as moving between slides or sections.</remarks>
internal static class FluentCxSlideshowConstants
{
    /// <summary>
    /// Gets the icon for a left-pointing chevron, commonly used for navigation controls.
    /// </summary>
    public static Icon ChevronLeft { get; } = new Size24.ChevronLeft();

    /// <summary>
    /// Gets the icon for a right-pointing chevron, commonly used for navigation controls.
    /// </summary>
    public static Icon ChevronRight { get; } = new Size24.ChevronRight();

    /// <summary>
    /// Gets the icon for an upward-pointing chevron, commonly used for navigation controls.
    /// </summary>
    public static Icon ChevronUp { get; } = new Size24.ChevronUp();

    /// <summary>
    /// Gets the icon for an downward-pointing chevron, commonly used for navigation controls.
    /// </summary>
    public static Icon ChevronDown { get; } = new Size24.ChevronDown();
}
