using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Generates a tetradic color scheme based on the specified base color, number of steps, and generation options.
/// </summary>
/// <remarks>A tetradic color scheme consists of four colors evenly spaced around the color wheel (90° apart).
/// This method generates gradients for each of the four colors, distributing the specified number of steps across the
/// gradients. The resulting list contains all generated colors in sequence.</remarks>
public class TetradicPlugin : IColorPlugin
{
    /// <inheritdoc />
    public string Name => "Tetradic";

    /// <inheritdoc />
    public List<string> Generate(string baseColor, int steps, GenerationOptions options)
    {
        if (!ColorUtils.IsValidHexOrCssName(baseColor))
        {
            throw new ArgumentException("Base color must be a valid hex color code.", nameof(baseColor));
        }

        var (h, s, l) = ColorUtils.HexToHsl(baseColor);

        var c1 = baseColor;
        var c2 = ColorUtils.HslToHex(h + 90, s, l);
        var c3 = ColorUtils.HslToHex(h + 180, s, l);
        var c4 = ColorUtils.HslToHex(h + 270, s, l);

        var part = Math.Max(1, steps / 4);
        var list = new List<string>();

        list.AddRange(ColorUtils.GenerateGradient(c1, part, GradientStrategy.Shades, options));
        list.AddRange(ColorUtils.GenerateGradient(c2, part, GradientStrategy.Shades, options));
        list.AddRange(ColorUtils.GenerateGradient(c3, part, GradientStrategy.Shades, options));
        list.AddRange(ColorUtils.GenerateGradient(c4, steps - 3 * part, GradientStrategy.Shades, options));

        return list;
    }
}
