using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a plugin that generates a monochromatic color palette based on a given base color.
/// </summary>
/// <remarks>This plugin creates a gradient of colors with a constant hue, varying only in saturation and
/// lightness. It is useful for generating visually cohesive color schemes where all colors share the same
/// hue.</remarks>
public class MonochromaticPlugin : IColorPlugin
{
    /// <inheritdoc />
    public string Name => "Monochromatic";

    /// <inheritdoc />
    public List<string> Generate(string baseColor, int steps, GenerationOptions options)
    {
        if (!ColorUtils.IsValidHexOrCssName(baseColor))
        {
            throw new ArgumentException("Base color must be a valid hex color code.", nameof(baseColor));
        }

        var (h, _, _) = ColorUtils.HexToHsl(baseColor);
        var list = new List<string>(steps);

        for (var i = 0; i < steps; i++)
        {
            var t = steps <= 1 ? 0 : i / (double)(steps - 1);

            var l2 = ColorUtils.Clamp01(
                options.LightnessMin + (options.LightnessMax - options.LightnessMin) * t
            );

            var s2 = ColorUtils.Clamp01(
                options.SaturationMin + (options.SaturationMax - options.SaturationMin) * t
            );

            list.Add(ColorUtils.HslToHex(h, s2, l2));
        }

        if (options.Reverse)
        {
            list.Reverse();
        }

        return list;
    }
}
