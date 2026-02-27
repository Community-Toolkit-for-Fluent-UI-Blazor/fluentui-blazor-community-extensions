using System.Drawing;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for customizing the appearance and behavior of a signature watermark, including
/// text, image, style, opacity, positioning, and repetition settings.
/// </summary>
/// <remarks>Use this class to specify how a watermark should be rendered on a document or component. Options
/// allow for text or image watermarks, control over font and color, opacity, rotation, alignment, and repetition.
/// Changing properties affects the resulting watermark's visual presentation. The default values provide a basic text
/// watermark centered on the target area.</remarks>
public class SignatureWatermarkOptions
{
    /// <summary>
    /// Gets or sets the text displayed for the signature component.
    /// </summary>
    public string? Text { get; set; } = "Signature";

    /// <summary>
    /// Gets or sets the URL of the image associated with this instance.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the mode used to render the watermark content.
    /// </summary>
    /// <remarks>Use this property to specify whether the watermark should display text, an image, or another
    /// supported format. The default value is <see cref="WatermarkMode.Text"/>.</remarks>
    public WatermarkMode Mode { get; set; } = WatermarkMode.Text;

    /// <summary>
    /// Gets or sets the opacity level of the component.
    /// </summary>
    /// <remarks>The value determines the transparency of the component, where 0 represents fully transparent
    /// and 1 represents fully opaque. Values outside the range [0, 1] may result in undefined behavior.</remarks>
    public double Opacity { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the opacity level applied to the text content.
    /// </summary>
    /// <remarks>The value should be between 0.0 and 1.0, where 0.0 represents fully transparent and 1.0
    /// represents fully opaque. Values outside this range may result in undefined behavior.</remarks>
    public double TextOpacity { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the opacity level applied to the image.
    /// </summary>
    /// <remarks>The value should be between 0.0 and 1.0, where 0.0 represents full transparency and 1.0
    /// represents full opacity.</remarks>
    public double ImageOpacity { get; set; } = 0.1;

    /// <summary>
    /// Gets or sets the font family used to render text within the component.
    /// </summary>
    public string FontFamily { get; set; } = "sans-serif";

    /// <summary>
    /// Gets or sets the font size used to display the content.
    /// </summary>
    public double FontSize { get; set; } = 48;

    /// <summary>
    /// Gets or sets the font weight applied to the component's content.
    /// </summary>
    /// <remarks>The value should be a valid CSS font-weight string, such as "normal", "bold", or a numeric
    /// value. The default is "bold". Changing this property affects the visual appearance of the text rendered by the
    /// component.</remarks>
    public string FontWeight { get; set; } = "bold";

    /// <summary>
    /// Gets or sets the color value represented as a hexadecimal string.
    /// </summary>
    /// <remarks>The color should be specified in the standard hexadecimal format (e.g., "#RRGGBB").</remarks>
    public string Color { get; set; } = "red";

    /// <summary>
    /// Gets or sets the rotation angle, in degrees, applied to the element.
    /// </summary>
    public double Rotation { get; set; } = -30;

    /// <summary>
    /// Gets or sets the position of the element as a two-dimensional point.
    /// </summary>
    public Point Position { get; set; } = new(50, 50);

    /// <summary>
    /// Gets or sets a value indicating whether the operation should repeat automatically.
    /// </summary>
    public bool Repeat { get; set; } = true;

    /// <summary>
    /// Gets or sets the horizontal spacing, in pixels, between repeated elements.
    /// </summary>
    public double RepeatSpacingX { get; set; } = 200;

    /// <summary>
    /// Gets or sets the vertical spacing, in pixels, between repeated elements.
    /// </summary>
    public double RepeatSpacingY { get; set; } = 200;

    /// <summary>
    /// Gets or sets the vertical alignment of the watermark content within its container.
    /// </summary>
    public WatermarkVerticalAlignment VerticalAlignment { get; set; } = WatermarkVerticalAlignment.Center;

    /// <summary>
    /// Gets or sets the horizontal alignment of the watermark content within its container.
    /// </summary>
    public WatermarkHorizontalAlignment HorizontalAlignment { get; set; } = WatermarkHorizontalAlignment.Center;

    /// <summary>
    /// Gets or sets the amount of spacing, in pixels, between characters in the rendered text.
    /// </summary>
    public double LetterSpacing { get; set; }

    /// <summary>
    /// Gets or sets the scale factor applied to the content.
    /// </summary>
    /// <remarks>A value of 1.0 represents the original size. Values greater than 1.0 increase the size, while
    /// values less than 1.0 decrease it. Negative values may result in unexpected behavior.</remarks>
    public double Scale { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets a value indicating whether the component is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the visual bias applied to the watermark.
    /// </summary>
    public double VisualBias { get; set; } = 0.20;

    /// <summary>
    /// Resets all watermark properties to their default values.
    /// </summary>
    /// <remarks>Use this method to restore the watermark configuration to its initial state. This is useful
    /// when you want to discard any customizations and revert to the standard settings for text, image, appearance, and
    /// positioning.</remarks>
    public void Reset()
    {
        Text = "Signature";
        ImageUrl = null;
        Mode = WatermarkMode.Text;
        Opacity = 0.1;
        TextOpacity = 0.1;
        ImageOpacity = 0.1;
        FontFamily = "sans-serif";
        FontSize = 48;
        FontWeight = "bold";
        Color = "#000000";
        Rotation = -30;
        Position = new(50, 50);
        Repeat = false;
        RepeatSpacingX = 200;
        RepeatSpacingY = 200;
        VerticalAlignment = WatermarkVerticalAlignment.Center;
        HorizontalAlignment = WatermarkHorizontalAlignment.Center;
        LetterSpacing = 0;
        Scale = 1.0;
        Enabled = true;
        VisualBias = 0.20;
    }
}
