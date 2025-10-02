namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a shadow effect with customizable properties such as offset, blur radius, spread radius, and color.
/// </summary>
public class Shadow
{
    /// <summary>
    /// Gets or sets the horizontal offset of the shadow.
    /// </summary>
    public CssLength OffsetX { get; set; } = new(0);

    /// <summary>
    /// Gets or sets the vertical offset of the shadow.
    /// </summary>
    public CssLength OffsetY { get; set; } = new(0);

    /// <summary>
    /// Gets or sets the blur radius of the shadow.
    /// </summary>
    public CssLength BlurRadius { get; set; } = new(5);

    /// <summary>
    /// Gets or sets the spread radius of the shadow.
    /// </summary>
    public CssLength SpreadRadius { get; set; } = new(0);

    /// <summary>
    /// Gets or sets the color of the shadow.
    /// </summary>
    public RgbaColor Color { get; set; } = new(0, 0, 0, 0.5);

    /// <summary>
    /// Returns the CSS representation of the shadow.
    /// </summary>
    /// <returns>Returns the CSS representation of the shadow.</returns>
    public string? ToCss() => $"{OffsetX} {OffsetY} {BlurRadius} {SpreadRadius} {Color}";
}
