using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a plugin that generates a gradient of warm colors.
/// </summary>
/// <remarks>This plugin generates a gradient transitioning from red to gold. It can be used to create warm color
/// palettes for various applications, such as UI design or data visualization.</remarks>
public sealed class WarmPlugin
    : IColorPlugin
{
    /// <inheritdoc />
    public string Name => "Warm";

    /// <inheritdoc />
    public List<string> Generate(string baseColor, int steps, GenerationOptions options)
    {
        return ColorUtils.GenerateCustomGradient("#FF0000", "#FFD700", steps, options);
    }
}
