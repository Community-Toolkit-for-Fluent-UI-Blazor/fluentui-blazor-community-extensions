namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the theme settings for a signature watermark, including visual appearance and behavior options.
/// </summary>
public class SignatureWatermarkTheme
{
    /// <summary>
    /// Gets or sets the color value represented as a hexadecimal string.
    /// </summary>
    public string Color { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the opacity level of the component.
    /// </summary>
    public double Opacity { get; set; } = 0.1;

    /// <summary>
    /// Gets or sets the font size used to display text content.
    /// </summary>
    public double FontSize { get; set; } = 24;

    /// <summary>
    /// Gets or sets the font family used to render text within the component.
    /// </summary>
    public string FontFamily { get; set; } = "Arial";

    /// <summary>
    /// Gets or sets the font weight applied to the component's text content.
    /// </summary>
    public string FontWeight { get; set; } = "normal";
}

