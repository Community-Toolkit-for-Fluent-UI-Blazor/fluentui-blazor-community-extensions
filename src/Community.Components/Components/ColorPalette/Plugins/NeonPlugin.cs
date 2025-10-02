using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a plugin that generates a sequence of neon-like colors based on a specified base color.
/// </summary>
/// <remarks>This plugin implements the <see cref="IColorPlugin"/> interface and generates colors by varying the
/// hue of the base color across the HSL color space. The resulting colors are designed to have maximum saturation and a
/// fixed lightness, creating a vibrant neon effect.</remarks>
public class NeonPlugin : IColorPlugin
{
    /// <inheritdoc />
    public string Name => "Neon";

    /// <inheritdoc />
    public List<string> Generate(string baseColor, int steps, GenerationOptions options)
    {
        if (!ColorUtils.IsValidHexOrCssName(baseColor))
        {
            throw new ArgumentException("Base color must be a valid hex color code.", nameof(baseColor));
        }

        var (h, _, _) = ColorUtils.HexToHsl(baseColor);
        var list = new List<string>();

        for (var i = 0; i < steps; i++)
        {
            var t = i / (double)(steps - 1);
            list.Add(ColorUtils.HslToHex(h + t * 360, 1.0, 0.5));
        }

        return list;
    }
}
