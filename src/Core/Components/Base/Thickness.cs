namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a set of thickness values describing the left, top, right, and bottom margins or padding of a rectangle.
/// </summary>
/// <remarks>Use the Thickness structure to specify uniform or individual side measurements for layout margins or
/// padding in user interface elements. Each property corresponds to one edge of the rectangle, allowing for flexible
/// configuration of spacing around or within elements.</remarks>
public struct Thickness
{
    /// <summary>
    /// Initializes a new instance of the Thickness class with the specified thickness values for each side of a
    /// rectangle.
    /// </summary>
    /// <remarks>Use this constructor to specify different thickness values for each side when creating a
    /// Thickness instance. This is useful for defining asymmetric margins or padding in layout scenarios.</remarks>
    /// <param name="left">The thickness of the left side, measured in device-independent units (1/96th inch per unit).</param>
    /// <param name="top">The thickness of the top side, measured in device-independent units (1/96th inch per unit).</param>
    /// <param name="right">The thickness of the right side, measured in device-independent units (1/96th inch per unit).</param>
    /// <param name="bottom">The thickness of the bottom side, measured in device-independent units (1/96th inch per unit).</param>
    public Thickness(double left, double top, double right, double bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    /// <summary>
    /// Initializes a new instance of the Thickness structure with the same length applied to all four sides.
    /// </summary>
    /// <remarks>Use this constructor when you want all sides of the Thickness to have the same value. This is
    /// commonly used for setting uniform margins or padding.</remarks>
    /// <param name="uniformLength">The length, in device-independent units (1/96th inch per unit), to apply uniformly to the left, top, right, and
    /// bottom sides.</param>
    public Thickness(double uniformLength)
        : this(uniformLength, uniformLength, uniformLength, uniformLength)
    {
    }

    /// <summary>
    /// Initializes a new instance of the Thickness class with a uniform thickness of 10 units.
    /// </summary>
    public Thickness() : this(10) { }

    /// <summary>
    /// Gets or sets the left margin value.
    /// </summary>
    public double Left { get; set; }

    /// <summary>
    /// Gets or sets the top margin value.
    /// </summary>
    public double Top { get; set; }

    /// <summary>
    /// Gets or sets the right margin value.
    /// </summary>
    public double Right { get; set; }

    /// <summary>
    /// Gets or sets the bottom margin value.
    /// </summary>
    public double Bottom { get; set; }

    /// <summary>
    /// Gets horizontal size by summing the left and right values.
    /// </summary>
    public readonly double Horizontal => Left + Right;

    /// <summary>
    /// Gets the vertical size by adding the top and bottom values.
    /// </summary>
    public readonly double Vertical => Top + Bottom;

    /// <summary>
    /// Resets all boundary values to zero.
    /// </summary>
    /// <remarks>Use this method to clear the current boundary settings and restore them to their default
    /// state. This is typically used when reinitializing or reusing the object.</remarks>
    internal void Reset()
    {
        Left = 0;
        Top = 0;
        Right = 0;
        Bottom = 0;
    }
}

