using System.Diagnostics;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

/// <summary>
/// Represents a color in the CMYK (Cyan, Magenta, Yellow, Key/Black) color space, which is commonly used in printing.
/// </summary>

[DebuggerDisplay("{ToString()}")]
public readonly struct Cmyk : IEquatable<Cmyk>
{
    /// <summary>
    /// Gets the cyan component of the color, which ranges from 0 to 1.
    /// </summary>
    /// <remarks>
    /// A value of 0 means no cyan (full red), while a value of 1 means full cyan (no red).
    /// </remarks>
    public double C { get; }

    /// <summary>
    /// Gets the magenta component of the color, which ranges from 0 to 1.
    /// </summary>
    /// <remarks>
    /// A value of 0 means no magenta (full green), while a value of 1 means full magenta (no green).
    /// </remarks>
    public double M { get; }

    /// <summary>
    /// Gets the yellow component of the color, which ranges from 0 to 1.
    /// </summary>
    /// <remarks>
    /// A value of 0 means no yellow (full blue), while a value of 1 means full yellow (no blue).
    /// </remarks>
    public double Y { get; }

    /// <summary>
    /// Gets the alpha component of the color, which ranges from 0 to 1.
    /// </summary>
    public double A { get; }

    /// <summary>
    /// Gets the key (black) component of the color, which ranges from 0 to 1.
    /// </summary>
    public double K { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Cmyk"/> struct with the specified cyan, magenta, yellow, key (black), and alpha components.
    /// </summary>
    /// <param name="c">The cyan component of the color, which ranges from 0 to 1.</param>
    /// <param name="m">The magenta component of the color, which ranges from 0 to 1.</param>
    /// <param name="y">The yellow component of the color, which ranges from 0 to 1.</param>
    /// <param name="k">The key (black) component of the color, which ranges from 0 to 1.</param>
    /// <param name="a">The alpha component of the color, which ranges from 0 to 1.</param>
    public Cmyk(double c, double m, double y, double k, double a = 1.0)
    {
        C = Math.Clamp(c, 0, 1);
        M = Math.Clamp(m, 0, 1);
        Y = Math.Clamp(y, 0, 1);
        K = Math.Clamp(k, 0, 1);
        A = Math.Clamp(a, 0, 1);
    }

    /// <summary>
    /// Creates a new CMYK color from the specified linear RGB color and alpha value.
    /// </summary>
    /// <remarks>The conversion assumes that the input RGB values are in linear color space, not
    /// gamma-corrected. If the input color is pure black, the resulting CMYK color will have a key (K) value of 1 and
    /// cyan, magenta, and yellow values of 0.</remarks>
    /// <param name="rgb">The linear RGB color to convert to the CMYK color space.</param>
    /// <param name="alpha">The alpha (opacity) component of the resulting color. The value must be between 0.0 (fully transparent) and 1.0
    /// (fully opaque). The default is 1.0.</param>
    /// <returns>A CMYK color that represents the equivalent of the specified linear RGB color and alpha value.</returns>
    public static Cmyk FromRgbLinear(RgbLinear rgb, double alpha = 1.0)
    {
        var r = rgb.R;
        var g = rgb.G;
        var b = rgb.B;

        var k = 1 - Math.Max(r, Math.Max(g, b));

        if (k >= 1.0)
        {
            return new Cmyk(0, 0, 0, 1, alpha);
        }

        var c = (1 - r - k) / (1 - k);
        var m = (1 - g - k) / (1 - k);
        var y = (1 - b - k) / (1 - k);

        return new Cmyk(c, m, y, k, alpha);
    }

    /// <summary>
    /// Converts the current color to its equivalent linear RGB representation.
    /// </summary>
    /// <returns>An instance of <see cref="RgbLinear"/> representing the color in linear RGB color space.</returns>
    public RgbLinear ToRgbLinear()
    {
        var r = (1 - C) * (1 - K);
        var g = (1 - M) * (1 - K);
        var b = (1 - Y) * (1 - K);

        return new RgbLinear(r, g, b);
    }

    /// <inheritdoc />
    public bool Equals(Cmyk other) => C == other.C && M == other.M && Y == other.Y && K == other.K && A == other.A;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Cmyk other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(C, M, Y, K, A);

    /// <summary>
    /// Determines if two <see cref="Cmyk"/> instances are equal by comparing their components.
    /// </summary>
    /// <param name="left">The first <see cref="Cmyk"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Cmyk"/> instance to compare.</param>
    /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Cmyk left, Cmyk right) => left.Equals(right);

    /// <summary>
    /// Determines whether two Cmyk instances represent different values.
    /// </summary>
    /// <param name="left">The first Cmyk instance to compare.</param>
    /// <param name="right">The second Cmyk instance to compare.</param>
    /// <returns>true if the values of left and right are not equal; otherwise, false.</returns>
    public static bool operator !=(Cmyk left, Cmyk right) => !left.Equals(right);

    /// <inheritdoc />
    public override string ToString() => $"C={C:0.###}, M={M:0.###}, Y={Y:0.###}, K={K:0.###}, A={A:0.###}";
}
