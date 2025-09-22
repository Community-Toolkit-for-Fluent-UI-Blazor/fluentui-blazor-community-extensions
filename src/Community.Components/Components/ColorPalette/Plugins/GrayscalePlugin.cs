using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a plugin that generates a series of greyscale colors.
/// </summary>
/// <remarks>This plugin implements the <see cref="IColorPlugin"/> interface and generates greyscale colors by
/// varying the lightness component while keeping hue and saturation fixed at zero.</remarks>
public class GrayscalePlugin : IColorPlugin
{
    /// <inheritdoc />
    public string Name => "Greyscale";

    /// <inheritdoc />
    public List<string> Generate(string baseColor, int steps, GenerationOptions options)
    {
        var list = new List<string>();

        for (var i = 0; i < steps; i++)
        {
            var l = i / (double)(steps - 1);
            list.Add(ColorUtils.HslToHex(0, 0, l));
        }

        return list;
    }
}
