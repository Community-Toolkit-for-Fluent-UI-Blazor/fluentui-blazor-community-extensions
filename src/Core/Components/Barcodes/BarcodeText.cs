using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the text label associated with a one-dimensional barcode, including its position and alignment settings.
/// </summary>
/// <remarks>Use this class to specify the display text for a 1D barcode, along with its coordinates and alignment
/// relative to the barcode image. The properties are typically set when rendering or exporting barcode
/// visuals.</remarks>
public sealed class BarcodeText
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
    /// Gets the text content associated with this instance.
    /// </summary>
    public string Text { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets the alignment of the text content within the component.
    /// </summary>
    public SvgTextAnchor Anchor { get; internal set; } = SvgTextAnchor.Middle;
}
