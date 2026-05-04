namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a rectangular region used for barcode rendering or detection.
/// </summary>
/// <param name="x">The X-coordinate of the upper-left corner of the rectangle, typically in device-independent units.</param>
/// <param name="y">The Y-coordinate of the upper-left corner of the rectangle, typically in device-independent units.</param>
/// <param name="width">The width of the rectangle, typically in device-independent units. Must be non-negative.</param>
/// <param name="height">The height of the rectangle, typically in device-independent units. Must be non-negative.</param>
public sealed record BarcodeRectangle(double x, double y, double width, double height)
{
    /// <summary>
    /// Gets or sets the X-coordinate value.
    /// </summary>
    public double X { get; set; } = x;

    /// <summary>
    /// Gets or sets the Y-coordinate value.
    /// </summary>
    public double Y { get; set; } = y;

    /// <summary>
    /// Gets or sets the width value.
    /// </summary>
    public double Width { get; set; } = width;

    /// <summary>
    /// Gets or sets the height value.
    /// </summary>
    public double Height { get; set; } = height;
}
