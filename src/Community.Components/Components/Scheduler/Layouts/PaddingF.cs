namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents padding values for the left, top, right, and bottom edges of a rectangle using floating-point units.
/// </summary>
/// <remarks>Use this structure to specify or retrieve padding measurements with sub-pixel precision, such as when
/// laying out graphical elements or controls. All values are expressed in pixels as floating-point numbers, allowing
/// for fine-grained layout adjustments.</remarks>
internal struct PaddingF
{
    /// <summary>
    /// Initializes a new instance of the PaddingF structure with the specified left, top, right, and bottom padding
    /// values.
    /// </summary>
    /// <param name="left">The width, in floating-point units, of the left padding.</param>
    /// <param name="top">The width, in floating-point units, of the top padding.</param>
    /// <param name="right">The width, in floating-point units, of the right padding.</param>
    /// <param name="bottom">The width, in floating-point units, of the bottom padding.</param>
    public PaddingF(float left, float top, float right, float bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    /// <summary>
    /// Gets or sets the distance, in pixels, between the left edge of the element and its container.
    /// </summary>
    public float Left { get; set; }

    /// <summary>
    /// Gets or sets the distance, in pixels, between the top edge of the element and its container.
    /// </summary>
    public float Top { get; set; }

    /// <summary>
    /// Gets or sets the right coordinate value.
    /// </summary>
    public float Right { get; set; }

    /// <summary>
    /// Gets or sets the bottom coordinate value.
    /// </summary>
    public float Bottom { get; set; }
}
