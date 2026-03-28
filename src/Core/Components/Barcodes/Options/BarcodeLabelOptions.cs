using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for displaying a label beneath a barcode.
/// </summary>
/// <remarks>Use this class to customize the appearance and positioning of the label rendered below a barcode,
/// including font, color, and vertical offset. All properties have default values suitable for typical usage.</remarks>
public sealed class BarcodeLabelOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the component is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the font family used to render text.
    /// </summary>
    public string FontFamily { get; set; } = StylesVariables.Fonts.Family.Base;

    /// <summary>
    /// Gets or sets the font size used to display text.
    /// </summary>
    public double FontSize { get; set; } = 12;

    /// <summary>
    /// Gets or sets the color value represented as a hexadecimal string.
    /// </summary>
    /// <remarks>The color should be specified in a valid hexadecimal format (for example, "#RRGGBB" or
    /// "#AARRGGBB").</remarks>
    public string Color { get; set; } = StylesVariables.Colors.Brand.Foreground1;

    /// <summary>
    /// Gets or sets the vertical offset applied to the element, in device-independent pixels.
    /// </summary>
    public double OffsetY { get; set; } = 12;
}

