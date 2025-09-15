using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a plugin that generates analogous color palettes based on a base color.
/// </summary>
public class AnalogousPlugin : IColorPlugin
{
    /// <inheritdoc />
    public string Name => "Analogous";

    /// <inheritdoc />
    public List<string> Generate(string baseColor, int steps, GenerationOptions options)
    {
        if (!ColorUtils.IsValidHexOrCssName(baseColor))
        {
            throw new ArgumentException("Base color must be a valid hex color code.", nameof(baseColor));
        }

        var (h, s, l) = ColorUtils.HexToHsl(baseColor);
        var left = ColorUtils.HslToHex(h - 30, s, l);
        var right = ColorUtils.HslToHex(h + 30, s, l);

        var half = steps / 2;
        var list = ColorUtils.GenerateGradient(left, half, GradientStrategy.Shades, options);
        list.AddRange(ColorUtils.GenerateGradient(right, steps - half, GradientStrategy.Shades, options));

        return list;
    }
}
