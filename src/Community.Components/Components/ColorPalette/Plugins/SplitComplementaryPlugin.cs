using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a plugin that generates color palettes based on the split-complementary color scheme.
/// </summary>
/// <remarks>The split-complementary color scheme is a variation of the complementary color scheme.  It uses a
/// base color and two colors adjacent to the base color's complement on the color wheel. This plugin generates
/// gradients for each of the three colors in the scheme, combining them into a single palette.</remarks>
public sealed class SplitComplementaryPlugin : IColorPlugin
{
    /// <inheritdoc />
    public string Name => "SplitComplementary";

    /// <inheritdoc />
    public List<string> Generate(string baseColor, int steps, GenerationOptions options)
    {
        if (!ColorUtils.IsValidHexOrCssName(baseColor))
        {
            throw new ArgumentException("Base color must be a valid hex color code.", nameof(baseColor));
        }

        var (h, s, l) = ColorUtils.HexToHsl(baseColor);

        var c1 = baseColor;
        var c2 = ColorUtils.HslToHex(h + 150, s, l);
        var c3 = ColorUtils.HslToHex(h + 210, s, l);

        var part = Math.Max(1, steps / 3);
        var list = new List<string>();

        list.AddRange(ColorUtils.GenerateGradient(c1, part, GradientStrategy.Shades, options));
        list.AddRange(ColorUtils.GenerateGradient(c2, part, GradientStrategy.Shades, options));
        list.AddRange(ColorUtils.GenerateGradient(c3, steps - 2 * part, GradientStrategy.Shades, options));

        return list;
    }
}
