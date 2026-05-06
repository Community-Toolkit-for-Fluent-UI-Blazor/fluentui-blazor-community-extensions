namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the various placement options for a popup component,
///  indicating the position of the popup relative to its anchor element.
///  The placement options include specific positions such as top-left, top-center, top-right, bottom-left,
///  bottom-center, bottom-right, left-top, left-center, left-bottom, right-top, right-center, and right-bottom.
///  The 'Auto' option allows the system to automatically determine the best placement based on available space and
///  other contextual factors.
/// </summary>
public enum PopupPlacement
{
    /// <summary>
    /// Specifies that the system determines the behavior automatically based on the current context.
    /// </summary>
    Auto,

    /// <summary>
    /// Gets or sets the coordinates of the top-left corner of the rectangle.
    /// </summary>
    /// <remarks>This property represents the starting point of the rectangle in a two-dimensional space. It
    /// is important for layout calculations and graphical rendering.</remarks>
    TopLeft,

    /// <summary>
    /// Specifies that the element is positioned at the top center of its container.
    /// </summary>
    TopCenter,

    /// <summary>
    /// Gets or sets the coordinates of the top-right corner of the rectangle.
    /// </summary>
    TopRight,

    /// <summary>
    /// Specifies that the position is aligned to the bottom-left corner of the element.
    /// </summary>
    BottomLeft,

    /// <summary>
    /// Specifies that the element is aligned at the bottom center of its container.
    /// </summary>
    /// <remarks>Use this value to position an element so that it is centered horizontally and aligned to the
    /// bottom edge of its parent container. The effect of this alignment may vary depending on the layout behavior of
    /// the container.</remarks>
    BottomCenter,

    /// <summary>
    /// Gets or sets the coordinates of the bottom-right corner of the rectangle.
    /// </summary>
    BottomRight,

    /// <summary>
    /// Gets or sets the position of the element's top-left corner.
    /// </summary>
    LeftTop,

    /// <summary>
    /// Specifies the alignment position corresponding to the horizontal center of the left edge of an element.
    /// </summary>
    LeftCenter,

    /// <summary>
    /// Gets or sets the position of the left bottom corner of the element.
    /// </summary>
    LeftBottom,

    /// <summary>
    /// Gets or sets the position of the right top corner of the element.
    /// </summary>
    RightTop,

    /// <summary>
    /// Specifies an alignment position that represents the center of the right edge of a layout.
    /// </summary>
    RightCenter,

    /// <summary>
    /// Gets or sets the position of the right bottom corner of the element.
    /// </summary>
    RightBottom
}
