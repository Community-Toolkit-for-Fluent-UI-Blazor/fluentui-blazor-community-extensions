using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to generate a complementary color gradient based on a specified base color.
/// </summary>
/// <remarks>This plugin calculates the complementary color of the given base color and generates a gradient using
/// the complementary color as the starting point. The gradient is created based on the specified number of steps and
/// generation options.</remarks>
public class ComplementaryPlugin : IColorPlugin
{
    /// <inheritdoc />
    public string Name => "Complementary";

    /// <inheritdoc />
    public List<string> Generate(string baseColor, int steps, GenerationOptions options)
    {
        if (!ColorUtils.IsValidHexOrCssName(baseColor))
        {
            throw new ArgumentException("Base color must be a valid hex color code.", nameof(baseColor));
        }

        var (h, s, l) = ColorUtils.HexToHsl(baseColor);
        var comp = ColorUtils.HslToHex(h + 180, s, l);

        return ColorUtils.GenerateGradient(comp, steps, GradientStrategy.Shades, options);
    }
}

