using System.Drawing;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the layout measurements and positioning information for an overlay, including cell, label, and content
/// sizes, padding, gaps, and header dimensions.
/// </summary>
internal record MeasureLayout
{
    /// <summary>
    /// Gets the rectangular region that defines the overlay area.
    /// </summary>
    public RectangleF Overlay { get; init; }

    /// <summary>
    /// Gets the size of each cell, in device-independent pixels.
    /// </summary>
    public SizeF CellSize { get; init; }

    /// <summary>
    /// Gets the size of the label in device-independent units.
    /// </summary>
    public SizeF LabelSize { get; init; }

    /// <summary>
    /// Gets the size of the content area, measured in device-independent pixels.
    /// </summary>
    public RectangleF ContentSize {  get; init; }

    /// <summary>
    /// Gets the padding applied to the content, represented as a set of floating-point values for each edge.
    /// </summary>
    public PaddingF Padding { get; init; }

    /// <summary>
    /// Gets the gap value used to separate elements or components.
    /// </summary>
    public float Gap { get; init; }

    /// <summary>
    /// Gets the vertical space, in device-independent units, available for displaying content within the layout.
    /// </summary>
    public float UsableHeight { get; init; }

    /// <summary>
    /// Gets the height of the header area, in device-independent units (DIPs).
    /// </summary>
    public float HeaderHeight { get; init; }

    /// <summary>
    /// Gets the local coordinates represented by this point.
    /// </summary>
    public PointF Local { get; init; }
}
