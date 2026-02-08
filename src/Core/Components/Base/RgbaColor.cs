using System.Globalization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an RGBA color value for CSS.
/// </summary>
public readonly struct RgbaColor
{
    /// <summary>
    /// Represents the red component of the color (0-255).
    /// </summary>
    private readonly byte _r;

    /// <summary>
    /// Represents the green component of the color (0-255).
    /// </summary>
    private readonly byte _g;

    /// <summary>
    /// Represents the blue component of the color (0-255).
    /// </summary>
    private readonly byte _b;

    /// <summary>
    /// Represents the alpha (opacity) component of the color (0.0-1.0).
    /// </summary>
    private readonly double _a;

    /// <summary>
    /// Represents the culture info for formatting.
    /// </summary>
    private static readonly CultureInfo _culture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Initializes a new instance of the <see cref="RgbaColor"/> class with the specified red, green, blue, and alpha
    /// channel values.
    /// </summary>
    /// <param name="r">The red component of the color, ranging from 0 to 255.</param>
    /// <param name="g">The green component of the color, ranging from 0 to 255.</param>
    /// <param name="b">The blue component of the color, ranging from 0 to 255.</param>
    /// <param name="a">The alpha (transparency) component of the color, ranging from 0.0 (completely transparent) to 1.0 (completely
    /// opaque). Defaults to 1.0.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="a"/> is less than 0, greater than 1, or is not a finite number.</exception>
    public RgbaColor(byte r, byte g, byte b, double a = 1.0)
    {
        if (a < 0 || a > 1 || double.IsNaN(a) || double.IsInfinity(a))
        {
            throw new ArgumentException("Alpha must be between 0 and 1.", nameof(a));
        }

        _r = r;
        _g = g;
        _b = b;
        _a = a;
    }

    /// <inheritdoc />
    public override string? ToString() => $"rgba({_r.ToString(_culture)},{_g.ToString(_culture)},{_b.ToString(_culture)},{_a.ToString("0.##", _culture)})";
}
