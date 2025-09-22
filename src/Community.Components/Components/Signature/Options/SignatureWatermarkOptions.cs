using System.Drawing;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the watermark options for the signature component.
/// </summary>
public class SignatureWatermarkOptions
{
    /// <summary>
    /// Gets or sets the text to be displayed as the watermark. If both Text and ImageUrl are provided, the image will take precedence.
    /// </summary>
    public string? Text { get; set; } = "Signature";

    /// <summary>
    /// Gets or sets the URL of the image to be used as the watermark. If both Text and ImageUrl are provided, the image will take precedence.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the watermark, ranging from 0.0 (fully transparent) to 1.0 (fully opaque).
    /// </summary>
    public double Opacity { get; set; } = 0.1;

    /// <summary>
    /// Gets or sets the font family for the watermark text. This can be any valid CSS font-family value.
    /// </summary>
    public string FontFamily { get; set; } = "sans-serif";

    /// <summary>
    /// Gets or sets the font size for the watermark text, in pixels.
    /// </summary>
    public double FontSize { get; set; } = 48;

    /// <summary>
    /// Gets or sets the font weight for the watermark text. This can be any valid CSS font-weight value, such as "normal", "bold", or a numeric value like "400" or "700".
    /// </summary>
    public string FontWeight { get; set; } = "bold";

    /// <summary>
    /// Gets or sets the color of the watermark text. This can be any valid CSS color value, such as a hex code, RGB, or RGBA.
    /// </summary>
    public string Color { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the rotation angle of the watermark text or image, in degrees. Positive values rotate clockwise, while negative values rotate counterclockwise.
    /// </summary>
    public double Rotation { get; set; } = -30;

    /// <summary>
    /// Gets or sets the position of the watermark within the signature area, represented as a Point with X and Y coordinates (in percentage). The default position is centered at (50, 50).
    /// </summary>
    public Point Position { get; set; } = new(50, 50);

    /// <summary>
    /// Gets or sets a value indicating whether the watermark should be repeated across the signature area. If true, the watermark will be tiled; if false, it will appear only once at the specified position.
    /// </summary>
    public bool Repeat { get; set; }

    /// <summary>
    /// Gets or sets the vertical alignment of the watermark text. This can be "start", "middle", or "end".
    /// </summary>
    public WatermarkVerticalAlignment TextAlign { get; set; } = WatermarkVerticalAlignment.Middle;

    /// <summary>
    /// Gets or sets the letter spacing for the watermark text, in pixels.
    /// </summary>
    public double LetterSpacing { get; set; }

    /// <summary>
    /// Resets all properties to their default values.
    /// </summary>
    public void Reset()
    {
        Text = "Signature";
        ImageUrl = null;
        Opacity = 0.1;
        FontFamily = "sans-serif";
        FontSize = 48;
        FontWeight = "bold";
        Color = "#000000";
        Rotation = -30;
        Position = new(50, 50);
        Repeat = false;
        TextAlign = WatermarkVerticalAlignment.Middle;
        LetterSpacing = 0;
    }
}
