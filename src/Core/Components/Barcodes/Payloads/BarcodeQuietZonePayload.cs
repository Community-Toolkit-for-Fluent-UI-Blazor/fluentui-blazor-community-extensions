namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the dimensions and position of a quiet zone surrounding a barcode.
/// </summary>
/// <remarks>A quiet zone is a clear area around a barcode that ensures reliable scanning by separating the
/// barcode from surrounding elements. The properties specify the location and size of the quiet zone in the same
/// coordinate space as the barcode.</remarks>
public sealed class BarcodeQuietZonePayload
{
    /// <summary>
    /// Gets the X-coordinate value.
    /// </summary>
    public double X { get; init; }

    /// <summary>
    /// Gets the Y-coordinate value.
    /// </summary>
    public double Y { get; init; }

    /// <summary>
    /// Gets the width value of the quiet zone.
    /// </summary>
    public double Width { get; init; }

    /// <summary>
    /// Gets the height value of the quiet zone.
    /// </summary>
    public double Height { get; init; }
}
