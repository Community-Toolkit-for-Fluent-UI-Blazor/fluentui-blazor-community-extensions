using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a color plugin that generates a gradient between two hues.
/// </summary>
/// <remarks>This plugin creates a gradient by blending a base color with a secondary color. The secondary color
/// is specified during the initialization of the plugin.</remarks>
/// <param name="secondaryHex"></param>
public class DualHuePlugin(string secondaryHex) : IColorPlugin
{
    /// <inheritdoc />
    public string Name => "DualHue";

    /// <inheritdoc />
    public List<string> Generate(string baseColor, int steps, GenerationOptions options)
    {
        if (!ColorUtils.IsValidHexOrCssName(baseColor))
        {
            throw new ArgumentException("Base color must be a valid hex color code.", nameof(baseColor));
        }

        return ColorUtils.GenerateCustomGradient(baseColor, secondaryHex, steps, options);
    }
}
