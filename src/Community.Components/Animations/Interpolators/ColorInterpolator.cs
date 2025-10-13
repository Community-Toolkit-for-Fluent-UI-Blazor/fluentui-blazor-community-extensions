using System.Drawing;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to interpolate between two <see cref="Color"/> values.
/// </summary>
/// <remarks>This class performs linear interpolation (lerp) on the RGBA components of the colors. It is useful
/// for creating smooth transitions between colors, such as in animations or gradients.</remarks>
public sealed class ColorInterpolator
    : IInterpolator<Color>
{
    /// <inheritdoc />
    public Color Lerp(Color start, Color end, double amount)
    {
        var r = (byte)(start.R + (end.R - start.R) * amount);
        var g = (byte)(start.G + (end.G - start.G) * amount);
        var b = (byte)(start.B + (end.B - start.B) * amount);
        var a = (byte)(start.A + (end.A - start.A) * amount);

        return Color.FromArgb(a, r, g, b);
    }
}
