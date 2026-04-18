using System.Diagnostics;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

/// <summary>
/// Represents a color in the CMY (Cyan, Magenta, Yellow) color space, which is commonly used in printing
///  and subtractive color mixing.
///  The alpha component (A) represents the opacity of the color, where 0 is fully transparent and 1 is fully opaque.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public readonly struct Cmy : IEquatable<Cmy>
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
    /// Initializes a new instance of the <see cref="Cmy"/> struct with the specified cyan, magenta, yellow, and alpha components.
    /// </summary>
    /// <param name="c">The cyan component of the color, which ranges from 0 to 1.</param>
    /// <param name="m">The magenta component of the color, which ranges from 0 to 1.</param>
    /// <param name="y">The yellow component of the color, which ranges from 0 to 1.</param>
    /// <param name="a">The alpha component of the color, which ranges from 0 to 1.</param>
    public Cmy(double c, double m, double y, double a = 1.0)
    {
        C = Math.Clamp(c, 0, 1);
        M = Math.Clamp(m, 0, 1);
        Y = Math.Clamp(y, 0, 1);
        A = Math.Clamp(a, 0, 1);
    }

    /// <summary>
    /// Creates a new CMY color from the specified linear RGB color and alpha value.
    /// </summary>
    /// <param name="rgb">The linear RGB color to convert to the CMY color space.</param>
    /// <param name="alpha">The alpha (opacity) component of the resulting color. The value must be between 0.0 (fully transparent) and 1.0
    /// (fully opaque). The default is 1.0.</param>
    /// <returns>A new Cmy instance representing the equivalent color in the CMY color space with the specified alpha value.</returns>
    public static Cmy FromRgbLinear(RgbLinear rgb, double alpha = 1.0)
    {
        return new Cmy(
            c: 1 - rgb.R,
            m: 1 - rgb.G,
            y: 1 - rgb.B,
            a: alpha);
    }

    /// <summary>
    /// Converts the current color to its equivalent in the linear RGB color space.
    /// </summary>
    /// <returns>A new RgbLinear instance representing the color in linear RGB space.</returns>
    public RgbLinear ToRgbLinear()
    {
        return new RgbLinear(
            r: 1 - C,
            g: 1 - M,
            b: 1 - Y);
    }

    /// <inheritdoc />
    public bool Equals(Cmy other) => C == other.C && M == other.M && Y == other.Y && A == other.A;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Cmy other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(C, M, Y, A);

    /// <summary>
    /// Determines whether two Cmy instances are equal.
    /// </summary>
    /// <remarks>This operator compares the values of the two Cmy instances for equality.</remarks>
    /// <param name="left">The first Cmy instance to compare.</param>
    /// <param name="right">The second Cmy instance to compare.</param>
    /// <returns>true if the specified Cmy instances are equal; otherwise, false.</returns>
    public static bool operator ==(Cmy left, Cmy right) => left.Equals(right);

    /// <summary>
    /// Determines whether two Cmy instances are not equal.
    /// </summary>
    /// <remarks>Use this operator to compare two Cmy values for inequality. The result is based on the Equals
    /// method implementation.</remarks>
    /// <param name="left">The first Cmy instance to compare.</param>
    /// <param name="right">The second Cmy instance to compare.</param>
    /// <returns>true if the specified Cmy instances are not equal; otherwise, false.</returns>
    public static bool operator !=(Cmy left, Cmy right) => !left.Equals(right);

    /// <inheritdoc />
    public override string ToString() => $"C={C:0.###}, M={M:0.###}, Y={Y:0.###}, A={A:0.###}";
}
