using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a plugin that generates a gradient of colors based on a specified base color and options.
/// </summary>
/// <remarks>This plugin is designed to generate a gradient starting from a predefined base color of blue. The
/// gradient generation process can be customized using the specified number of steps and additional options.</remarks>
public sealed class CoolPlugin : IColorPlugin
{
    /// <inheritdoc />
    public string Name => "Cool";

    /// <inheritdoc />
    public List<string> Generate(string baseColor, int steps, GenerationOptions options)
    {
        if (!ColorUtils.IsValidHexOrCssName(baseColor))
        {
            throw new ArgumentException("Base color must be a valid hex color code.", nameof(baseColor));
        }

        return ColorUtils.GenerateCustomGradient("#0000FF", "#00FFFF", steps, options);
    }
}
