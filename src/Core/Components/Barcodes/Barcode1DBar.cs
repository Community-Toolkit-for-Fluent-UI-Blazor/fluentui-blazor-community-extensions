namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a single bar element in a one-dimensional barcode, including its position, size, and color.
/// </summary>
/// <remarks>This class is typically used to describe the graphical representation of individual bars when
/// rendering or processing 1D barcodes.</remarks>
public sealed class Barcode1DBar
{
    /// <summary>
    /// Gets the X-coordinate value.
    /// </summary>
    public double X { get; internal set; }

    /// <summary>
    /// Gets the Y-coordinate value.
    /// </summary>
    public double Y { get; internal set; }

    /// <summary>
    /// Gets the width value for the current instance.
    /// </summary>
    public double Width { get; internal set; }

    /// <summary>
    /// Gets the height value for the current instance.
    /// </summary>
    public double Height { get; internal set; }
}
