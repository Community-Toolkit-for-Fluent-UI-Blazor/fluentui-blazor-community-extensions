namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the options for generating a color palette in the ColorPalette component.
/// </summary>
public class GenerationOptions
{
    /// <summary>
    /// Gets or sets the minimum saturation value for the color palette generation.
    /// </summary>
    public double SaturationMin { get; set; }

    /// <summary>
    /// Gets or sets the maximum saturation value for the color palette generation.
    /// </summary>
    public double SaturationMax { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the minimum lightness value for the color palette generation.
    /// </summary>
    public double LightnessMin { get; set; } = 0.05;

    /// <summary>
    /// Gets or sets the maximum lightness value for the color palette generation.
    /// </summary>
    public double LightnessMax { get; set; } = 0.95;

    /// <summary>
    /// Gets or sets a value indicating whether the generated colors should be in reverse order.
    /// </summary>
    public bool Reverse { get; set; }

    /// <summary>
    /// Gets or sets the easing function to be used for color transitions in the palette.
    /// </summary>
    public ColorPaletteEasing Easing { get; set; } = ColorPaletteEasing.Linear;
}
