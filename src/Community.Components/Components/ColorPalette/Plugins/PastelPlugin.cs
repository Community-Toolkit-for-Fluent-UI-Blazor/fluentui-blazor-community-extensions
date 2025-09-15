using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a plugin that generates a pastel color gradient based on a specified base color.
/// </summary>
/// <remarks>This plugin adjusts the saturation and lightness of the base color to create a pastel effect and
/// generates a gradient using a hue-shifting strategy.</remarks>
public class PastelPlugin : IColorPlugin
{
    /// <inheritdoc />
    public string Name => "Pastel";

    /// <inheritdoc />
    public List<string> Generate(string baseColor, int steps, GenerationOptions options)
    {
        if (!ColorUtils.IsValidHexOrCssName(baseColor))
        {
            throw new ArgumentException("Base color must be a valid hex color code.", nameof(baseColor));
        }

        var (h, _, _) = ColorUtils.HexToHsl(baseColor);
        var s = 0.4;
        var l = 0.85;

        return ColorUtils.GenerateGradient(ColorUtils.HslToHex(h, s, l), steps, GradientStrategy.HueShift, options);
    }
}
