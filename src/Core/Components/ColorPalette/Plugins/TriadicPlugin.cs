using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Generates a triadic color palette based on the specified base color.
/// </summary>
/// <remarks>A triadic color scheme is created by selecting three colors that are evenly spaced around the color
/// wheel (120 degrees apart in hue). This method generates gradients for each of the three colors and combines them
/// into a single palette.</remarks>
public class TriadicPlugin : IColorPlugin
{
    /// <inheritdoc />
    public string Name => "Triadic";

    /// <inheritdoc />
    public List<string> Generate(string baseColor, int steps, GenerationOptions options)
    {
        if (!ColorUtils.IsValidHexOrCssName(baseColor))
        {
            throw new ArgumentException("Base color must be a valid hex color code.", nameof(baseColor));
        }

        var (h, s, l) = ColorUtils.HexToHsl(baseColor);
        var c2 = ColorUtils.HslToHex(h + 120, s, l);
        var c3 = ColorUtils.HslToHex(h + 240, s, l);

        var part = steps / 3;
        var list = ColorUtils.GenerateGradient(baseColor, part, GradientStrategy.Shades, options);
        list.AddRange(ColorUtils.GenerateGradient(c2, part, GradientStrategy.Shades, options));
        list.AddRange(ColorUtils.GenerateGradient(c3, steps - 2 * part, GradientStrategy.Shades, options));

        return list;
    }
}

