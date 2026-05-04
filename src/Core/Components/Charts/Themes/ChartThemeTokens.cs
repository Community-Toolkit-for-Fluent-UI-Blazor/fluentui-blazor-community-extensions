using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a static class that defines constant color tokens used in chart themes.
/// </summary>
internal static class ChartThemeTokens
{
    /// <summary>
    /// Gets the dark background.
    /// </summary>
    public static Srgb8 DarkBackground { get; } = new Srgb8(41, 41, 41);

    /// <summary>
    /// Gets the light background color.
    /// </summary>
    public static Srgb8 LightBackground { get; } = new Srgb8(255, 255, 255);

    /// <summary>
    /// Gets the color value used for foreground elements in dark themes.
    /// </summary>
    public static Srgb8 DarkForeground { get; } = new(255, 255, 255);

    /// <summary>
    /// Gets the color value used for foreground elements in light themes.
    /// </summary>
    public static Srgb8 LightForeground { get; } = new Srgb8(36, 36, 36);

    /// <summary>
    /// Gets the color value used for grid lines in light themes.
    /// </summary>
    public static Srgb8 LightGrid { get; } = new(199, 199, 199);

    /// <summary>
    /// Gets a dark gray color suitable for use as a grid background or accent.
    /// </summary>
    public static Srgb8 DarkGrid { get; } = new(102, 102, 102);

    /// <summary>
    /// Gets the sRGB color representing the dark axis.
    /// </summary>
    public static Srgb8 DarkAxis { get; } = new(255, 255, 255);

    /// <summary>
    /// Gets the sRGB color representing the light axis.
    /// </summary>
    public static Srgb8 LightAxis { get; } = new(36, 36, 36);
}
