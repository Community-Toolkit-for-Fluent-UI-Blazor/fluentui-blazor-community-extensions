namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the logical position of an anchor point within a rectangular area, such as a UI element or container.
/// </summary>
/// <remarks>This enumeration is typically used to define alignment or placement of elements relative to a
/// bounding rectangle. Positions are grouped by vertical alignment (Top, Middle, Bottom) and horizontal alignment
/// (Left, Center, Right).</remarks>
public enum AnchorLogicalPosition
{
    /// <summary>
    /// Specifies that the element is aligned to the top-left corner.
    /// </summary>
    TopLeft = 0,

    /// <summary>
    /// Specifies that the content is aligned to the top center of the container.
    /// </summary>
    TopCenter = 1,

    /// <summary>
    /// Specifies that the element is positioned in the top-right corner.
    /// </summary>
    TopRight = 2,

    /// <summary>
    /// Specifies that the content is aligned to the middle of the left edge.
    /// </summary>
    MiddleLeft = 3,

    /// <summary>
    /// Specifies that the content is aligned to the center both vertically and horizontally.
    /// </summary>
    MiddleCenter = 4,

    /// <summary>
    /// Specifies that the element is aligned to the middle of the right edge.
    /// </summary>
    MiddleRight = 5,

    /// <summary>
    /// Specifies that the element is aligned to the bottom left corner.
    /// </summary>
    BottomLeft = 6,

    /// <summary>
    /// Specifies that the element is positioned at the bottom center of its container.
    /// </summary>
    BottomCenter = 7,

    /// <summary>
    /// Specifies that the element is aligned to the bottom-right corner.
    /// </summary>
    BottomRight = 8
}
