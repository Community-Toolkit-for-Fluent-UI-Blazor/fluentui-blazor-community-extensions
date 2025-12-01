using System.Drawing;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a rectangular region mapped to an item, including options to display anchor indicators on each side.
/// </summary>
/// <remarks>Use this class to define the position and visual anchor points of an item within a coordinate space.
/// Anchor properties indicate which sides of the rectangle should display anchor markers, which can be used for
/// alignment or connection purposes in graphical interfaces.</remarks>
public record MappedItemRect
{
    /// <summary>
    /// Gets or sets the rectangular region represented by this instance.
    /// </summary>
    public RectangleF Rect { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the left anchor is displayed.
    /// </summary>
    public bool ShowLeftAnchor { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the right anchor is displayed in the user interface.
    /// </summary>
    public bool ShowRightAnchor { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the top anchor is displayed in the user interface.
    /// </summary>
    public bool ShowTopAnchor { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the bottom anchor is displayed in the user interface.
    /// </summary>
    public bool ShowBottomAnchor { get; init; }
}
