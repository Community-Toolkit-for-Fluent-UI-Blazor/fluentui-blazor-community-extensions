namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for applying a shadow effect.
/// </summary>
/// <remarks>This class provides properties to control the appearance of a shadow, including its visibility, 
/// color, blur radius, and offsets. These options can be used to customize the shadow effect  for visual
/// elements.</remarks>
public class ShadowOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the shadow effect is enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the color of the shadow. This can be any valid CSS color value, such as a hex code, RGB, or RGBA.
    /// </summary>
    public string Color { get; set; } = "rgba(0,0,0,0.3)";

    /// <summary>
    /// Gets or sets the blur radius of the shadow. A higher value results in a more blurred shadow.
    /// </summary>
    public double Blur { get; set; } = 2.0;

    /// <summary>
    /// Gets or sets the horizontal offset of the shadow. Positive values move the shadow to the right, while negative values move it to the left.
    /// </summary>
    public double OffsetX { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the vertical offset of the shadow. Positive values move the shadow down, while negative values move it up.
    /// </summary>
    public double OffsetY { get; set; } = 1.0;

    /// <summary>
    /// Resets all properties to their default values.
    /// </summary>
    public void Reset()
    {
        Enabled = false;
        Color = "rgba(0,0,0,0.3)";
        Blur = 2.0;
        OffsetX = 1.0;
        OffsetY = 1.0;
    }
}
