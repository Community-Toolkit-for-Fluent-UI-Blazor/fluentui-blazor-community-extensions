namespace FluentUI.Blazor.Community.Components.Charts.Drawing;

/// <summary>
/// Represents the rectangular area within a chart where data is plotted.
/// </summary>
/// <remarks>The plot area defines the region of the chart that contains the graphical representation of
/// data series, excluding chart elements such as titles, legends, and axes. Coordinates and dimensions are
/// typically specified in chart-relative units.</remarks>
public sealed class ChartRect
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChartRect"/> class with the specified parameters.
    /// </summary>
    /// <param name="x">The X-coordinate of the rectangle.</param>
    /// <param name="y">The Y-coordinate of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    /// <param name="parent">The parent chart rectangle that contains this rectangle, if applicable.</param>
    public ChartRect(double x, double y, double width, double height, ChartRect? parent = null)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Parent = parent;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChartRect"/> class with default values.
    /// </summary>
    public ChartRect() : this(0, 0, 0, 0)
    {
    }

    /// <summary>
    /// Gets an empty rectangle with all coordinates and dimensions set to zero.
    /// </summary>
    /// <remarks>Use this property to represent a rectangle with no area or to initialize variables before
    /// assigning a specific rectangle value.</remarks>
    public static ChartRect Empty => new(0, 0, 0, 0);

    /// <summary>
    /// Gets the X-coordinate value.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Gets the Y-coordinate value.
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Gets the width value.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// Gets the height value.
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// Gets the bottom value of the rectangle, calculated as Y + Height.
    /// </summary>
    public double Bottom => Y + Height;

    /// <summary>
    /// Gets the right value of the rectangle, calculated as X + Width.
    /// </summary>
    public double Right => X + Width;

    /// <summary>
    /// Gets the parent chart rectangle that contains this rectangle, if applicable.
    /// </summary>
    public ChartRect? Parent { get; init; }

    /// <summary>
    /// Gets the horizontal distance between this element and its parent element, measured from their respective X
    /// coordinates.
    /// </summary>
    /// <remarks>If the element does not have a parent, the left margin is zero.</remarks>
    public double LeftMargin => Parent is null ? 0 : X - Parent.X;

    /// <summary>
    /// Gets the distance between the right edge of this element and the right edge of its parent element.
    /// </summary>
    /// <remarks>If the element does not have a parent, the right margin is zero.</remarks>
    public double RightMargin => Parent is null ? 0 : (Parent.X + Parent.Width) - (X + Width);

    /// <summary>
    /// Gets the vertical distance between this element and its parent element, measured in the same coordinate space.
    /// </summary>
    /// <remarks>If the element has no parent, the top margin is zero. This property is useful for layout
    /// calculations where relative positioning is required.</remarks>
    public double TopMargin => Parent is null ? 0 : Y - Parent.Y;

    /// <summary>
    /// Gets the distance, in device-independent units (DIU), between the bottom edge of this element and the bottom
    /// edge of its parent element.
    /// </summary>
    /// <remarks>If the element does not have a parent, the value is 0. This property is useful for layout
    /// calculations where alignment or spacing relative to the parent container is required.</remarks>
    public double BottomMargin => Parent is null ? 0 : (Parent.Y + Parent.Height) - (Y + Height);
}
