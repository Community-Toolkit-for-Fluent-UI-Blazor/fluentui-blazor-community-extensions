using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a plugin that desaturates a base color by reducing its saturation level.
/// </summary>
/// <remarks>The desaturation amount is specified as a value between 0 and 1, where 0 leaves the color unchanged
/// and 1 completely removes saturation, resulting in a grayscale color. This plugin generates a gradient of desaturated
/// colors based on the specified number of steps and generation options.</remarks>
/// <param name="amount"></param>
public class DesaturatePlugin(double amount = 0.3) : IColorPlugin
{
    /// <inheritdoc />
    public string Name => "Desaturate";

    /// <inheritdoc />
    public List<string> Generate(string baseColor, int steps, GenerationOptions options)
    {
        if (!ColorUtils.IsValidHexOrCssName(baseColor))
        {
            throw new ArgumentException("Base color must be a valid hex color code.", nameof(baseColor));
        }

        amount = Math.Clamp(amount, 0, 1);
        var (h, s, l) = ColorUtils.HexToHsl(baseColor);
        s *= (1 - amount);

        return ColorUtils.GenerateGradient(ColorUtils.HslToHex(h, s, l), steps, GradientStrategy.Shades, options);
    }
}

