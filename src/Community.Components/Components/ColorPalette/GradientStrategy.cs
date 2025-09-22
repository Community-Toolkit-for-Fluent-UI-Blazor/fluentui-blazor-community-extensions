namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the strategy used to generate color gradients.
/// </summary>
public enum GradientStrategy
{
    /// <summary>
    /// Represents a gradient strategy that generates shades of a color.
    /// </summary>
    Shades,

    /// <summary>
    /// Represents a gradient strategy that generates tints of a color.
    /// </summary>
    Tints,

    /// <summary>
    /// Represents a gradient strategy that generates tones of a color.
    /// </summary>
    Saturation,

    /// <summary>
    /// Represents a gradient strategy that shifts the hue of a color.
    /// </summary>
    HueShift
}
