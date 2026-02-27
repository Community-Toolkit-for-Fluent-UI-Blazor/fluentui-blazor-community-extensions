namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the visual theme settings for axes in a signature component, including color, opacity, stroke width, dash
/// style, margin, and rendering layer.
/// </summary>
/// <remarks>Use this class to configure the appearance of axes when rendering signature or drawing components.
/// Properties allow customization of line style and placement. The settings are typically applied when drawing grid
/// lines or axes in a background or foreground layer.</remarks>
public class SignatureAxesTheme
{
    /// <summary>
    /// Gets or sets the color value represented as a hexadecimal string.
    /// </summary>
    public string Color { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the opacity level of the content.
    /// </summary>
    /// <remarks>The value determines the transparency of the content, where 1.0 represents fully opaque and
    /// 0.0 represents fully transparent. Values outside the range of 0.0 to 1.0 may not be supported by all rendering
    /// systems.</remarks>
    public double Opacity { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the width of the stroke used to render the component.
    /// </summary>
    /// <remarks>The stroke width determines the thickness of lines or borders drawn by the component. Adjust
    /// this value to control visual emphasis or clarity in rendering scenarios.</remarks>
    public double StrokeWidth { get; set; } = 2.0;

    /// <summary>
    /// Gets or sets the dash pattern used for rendering lines or borders.
    /// </summary>
    /// <remarks>The dash pattern is specified as a comma-separated string of numbers, where each number
    /// represents the length of a dash or gap. For example, "5,2" creates a pattern of 5 units dash followed by 2 units
    /// gap. If the value is null or empty, a solid line is rendered.</remarks>
    public string? DashArray { get; set; }
}
