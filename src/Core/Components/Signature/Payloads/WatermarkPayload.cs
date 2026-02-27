namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the configuration settings for applying a watermark, including appearance, positioning, and repetition
/// options.
/// </summary>
public sealed class WatermarkPayload : IEquatable<WatermarkPayload>
{
    /// <summary>
    /// Gets or sets a value indicating whether the watermark should be repeated across the target area.
    /// </summary>
    public bool Repeat { get; set; }

    /// <summary>
    /// Gets or sets the mode used to render the watermark. The value determines the type or style of watermark applied.
    /// </summary>
    public WatermarkMode Mode { get; set; }

    /// <summary>
    /// Gets or sets the color to use for the watermark, specified as a string (for example, a hex color code or color name).
    /// </summary>
    public string Color { get; set; } = "transparent";

    /// <summary>
    /// Gets or sets the text to display in the watermark. If null, no text watermark is applied.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the URL of the image to use as a watermark. If null, no image watermark is applied.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the overall opacity of the watermark, where 0 is fully transparent and 1 is fully opaque.
    /// </summary>
    public double Opacity { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the text portion of the watermark, where 0 is fully transparent and 1 is fully opaque.
    /// </summary>
    public double TextOpacity { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the image portion of the watermark, where 0 is fully transparent and 1 is fully opaque.
    /// </summary>
    public double ImageOpacity { get; set; }

    /// <summary>
    /// Gets or sets the font size to use for the text in the watermark, specified in pixels or other CSS units.
    /// </summary>
    public double FontSize { get; set; }

    /// <summary>
    /// Gets or sets the font family to use for the text in the watermark. This can be a specific font name (e.g., "Arial") or a generic font family (e.g., "sans-serif").
    /// </summary>
    public string FontFamily { get; set; } = "Arial";

    /// <summary>
    /// Gets or sets the font weight to use for the text in the watermark. This can be a specific weight (e.g., "Bold") or a numeric value (e.g., "400" for normal, "700" for bold).
    /// </summary>
    public string FontWeight { get; set; } = "Normal";

    /// <summary>
    /// Gets or sets the letter spacing to use for the text in the watermark, specified in pixels or other CSS units. This controls the spacing between characters in the text.
    /// </summary>
    public double LetterSpacing { get; set; }

    /// <summary>
    /// Gets or sets the scale factor to apply to the watermark, where 1 is the original size, values less than 1 reduce the size, and values greater than 1 increase the size.
    /// </summary>
    public double Scale { get; set; }

    /// <summary>
    /// Gets or sets the rotation angle to apply to the watermark, specified in degrees. Positive values rotate clockwise, while negative values rotate counterclockwise.
    /// </summary>
    public double Rotation { get; set; }

    /// <summary>
    /// Gets or sets the X-coordinate for the position of the watermark, specified in pixels or other CSS units. The exact meaning of this value may depend on the alignment settings.
    /// </summary>
    public double PositionX { get; set; }

    /// <summary>
    /// Gets or sets the Y-coordinate for the position of the watermark, specified in pixels or other CSS units. The exact meaning of this value may depend on the alignment settings.
    /// </summary>
    public double PositionY { get; set; }

    /// <summary>
    /// Gets or sets the horizontal alignment for the watermark. This value determines how the watermark is aligned horizontally within the target area (e.g., left, center, right).
    /// </summary>
    public int HorizontalAlignment { get; set; }

    /// <summary>
    /// Gets or sets the vertical alignment for the watermark. This value determines how the watermark is aligned vertically within the target area (e.g., top, middle, bottom).
    /// </summary>
    public int VerticalAlignment { get; set; }

    /// <summary>
    /// Gets or sets the horizontal spacing between repeated watermarks when the Repeat property is true, specified in pixels or other CSS units. This controls how far apart the watermarks are spaced horizontally.
    /// </summary>
    public double RepeatSpacingX { get; set; }

    /// <summary>
    /// Gets or sets the vertical spacing between repeated watermarks when the Repeat property is true, specified in pixels or other CSS units. This controls how far apart the watermarks are spaced vertically.
    /// </summary>
    public double RepeatSpacingY { get; set; }

    /// <summary>
    /// Gets or sets the visual bias value of the watermark.
    /// </summary>
    public double VisualBias { get; set; }

    /// <inheritdoc />
    public bool Equals(WatermarkPayload? other)
    {
        if (other is null)
        {
            return false;
        }

        return
            Repeat == other.Repeat &&
            Mode == other.Mode &&
            Color == other.Color &&
            Text == other.Text &&
            ImageUrl == other.ImageUrl &&
            Opacity == other.Opacity &&
            TextOpacity == other.TextOpacity &&
            ImageOpacity == other.ImageOpacity &&
            FontSize == other.FontSize &&
            FontFamily == other.FontFamily &&
            FontWeight == other.FontWeight &&
            LetterSpacing == other.LetterSpacing &&
            Scale == other.Scale &&
            Rotation == other.Rotation &&
            PositionX == other.PositionX &&
            PositionY == other.PositionY &&
            HorizontalAlignment == other.HorizontalAlignment &&
            VerticalAlignment == other.VerticalAlignment &&
            RepeatSpacingX == other.RepeatSpacingX &&
            RepeatSpacingY == other.RepeatSpacingY &&
            VisualBias == other.VisualBias;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        Equals(obj as WatermarkPayload);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Repeat);
        hash.Add(Mode);
        hash.Add(Color);
        hash.Add(Text);
        hash.Add(ImageUrl);
        hash.Add(Opacity);
        hash.Add(TextOpacity);
        hash.Add(ImageOpacity);
        hash.Add(FontSize);
        hash.Add(FontFamily);
        hash.Add(FontWeight);
        hash.Add(LetterSpacing);
        hash.Add(Scale);
        hash.Add(Rotation);
        hash.Add(PositionX);
        hash.Add(PositionY);
        hash.Add(HorizontalAlignment);
        hash.Add(VerticalAlignment);
        hash.Add(RepeatSpacingX);
        hash.Add(RepeatSpacingY);
        hash.Add(VisualBias);

        return hash.ToHashCode();
    }
}
