using System.Diagnostics;
using FluentUI.Blazor.Community.Components.ColorSpace.Converters;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

/// <summary>
/// Represents a color in the linear RGB color space, where each component (R, G, B) is a double-precision value in the range [0, 1].
/// </summary>
[DebuggerDisplay("{ToString()}")]
public readonly struct RgbLinear : IEquatable<RgbLinear>
{
    /// <summary>
    /// Gets the red component of the color, which ranges from 0 to 1.
    /// </summary>
    public double R { get; }

    /// <summary>
    /// Gets the green component of the color, which ranges from 0 to 1.
    /// </summary>
    public double G { get; }

    /// <summary>
    /// Gets the blue component of the color, which ranges from 0 to 1.
    /// </summary>
    public double B { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RgbLinear"/> struct with the specified red, green, and blue components.
    /// </summary>
    /// <param name="r">The red component of the color, which ranges from 0 to 1.</param>
    /// <param name="g">The green component of the color, which ranges from 0 to 1.</param>
    /// <param name="b">The blue component of the color, which ranges from 0 to 1.</param>
    public RgbLinear(double r, double g, double b)
    {
        R = Math.Clamp(r, 0, 1);
        G = Math.Clamp(g, 0, 1);
        B = Math.Clamp(b, 0, 1);
    }

    /// <summary>
    /// Converts an sRGB color with 8-bit channel values to its linear RGB representation.
    /// </summary>
    /// <remarks>This method applies the standard sRGB transfer function to convert each 8-bit channel value
    /// to its corresponding linear value. Use this conversion when accurate color computations or blending are required
    /// in a linear color space.</remarks>
    /// <param name="srgb">The sRGB color to convert. Each channel value should be in the range 0 to 255.</param>
    /// <returns>A new RgbLinear instance representing the linear RGB equivalent of the specified sRGB color.</returns>
    public static RgbLinear FromSrgb8(Srgb8 srgb)
    {
        return new RgbLinear(
            ColorSpaceConverters.SrgbToLinear(srgb.R / 255.0),
            ColorSpaceConverters.SrgbToLinear(srgb.G / 255.0),
            ColorSpaceConverters.SrgbToLinear(srgb.B / 255.0)
        );
    }

    /// <summary>
    /// Converts the current color to its sRGB 8-bit representation.
    /// </summary>
    /// <returns>An instance of the Srgb8 struct representing the color in sRGB 8-bit format.</returns>
    public Srgb8 ToSrgb8()
    {
        return new Srgb8(
            ColorSpaceConverters.LinearToSrgb(R),
            ColorSpaceConverters.LinearToSrgb(G),
            ColorSpaceConverters.LinearToSrgb(B)
        );
    }

    /// <summary>
    /// Adds the corresponding color channel values of two RgbLinear instances.
    /// </summary>
    /// <remarks>Channel values are added component-wise. The resulting channel values are not clamped and may
    /// exceed the typical range for color channels.</remarks>
    /// <param name="a">The first RgbLinear color to add.</param>
    /// <param name="b">The second RgbLinear color to add.</param>
    /// <returns>A new RgbLinear instance whose channel values are the sums of the corresponding channels of the input colors.</returns>
    public static RgbLinear operator +(RgbLinear a, RgbLinear b) => new(a.R + b.R, a.G + b.G, a.B + b.B);

    /// <summary>
    /// Multiplies the components of an RgbLinear color by the specified scalar value.
    /// </summary>
    /// <param name="a">The RgbLinear color to be scaled.</param>
    /// <param name="s">The scalar value by which to multiply each color component.</param>
    /// <returns>A new RgbLinear instance with each component multiplied by the specified scalar value.</returns>
    public static RgbLinear operator *(RgbLinear a, double s)  => new(a.R * s, a.G * s, a.B * s);

    /// <summary>
    /// Multiplies each component of the specified RgbLinear color by the given scalar value.
    /// </summary>
    /// <remarks>This operator enables scaling of color intensity by multiplying all color channels by the
    /// same factor.</remarks>
    /// <param name="s">The scalar value by which to multiply each color component.</param>
    /// <param name="a">The RgbLinear color whose components are to be multiplied.</param>
    /// <returns>A new RgbLinear color with each component multiplied by the specified scalar value.</returns>
    public static RgbLinear operator *(double s, RgbLinear a) => new(a.R * s, a.G * s, a.B * s);

    /// <inheritdoc />
    public bool Equals(RgbLinear other) => R == other.R && G == other.G && B == other.B;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is RgbLinear other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(R, G, B);

    /// <summary>
    /// Determines whether two RgbLinear instances are equal by comparing their component values.
    /// </summary>
    /// <param name="left">The first RgbLinear instance to compare.</param>
    /// <param name="right">The second RgbLinear instance to compare.</param>
    /// <returns>true if the R, G, and B components of both instances are equal; otherwise, false.</returns>
    public static bool operator ==(RgbLinear left, RgbLinear right) => left.Equals(right);

    /// <summary>
    /// Determines whether two RgbLinear instances are not equal.
    /// </summary>
    /// <param name="left">The first RgbLinear instance to compare.</param>
    /// <param name="right">The second RgbLinear instance to compare.</param>
    /// <returns>true if the specified RgbLinear instances are not equal; otherwise, false.</returns>
    public static bool operator !=(RgbLinear left, RgbLinear right) => !left.Equals(right);

    /// <inheritdoc />
    public override string ToString() => $"Linear RGB: ({R:0.###}, {G:0.###}, {B:0.###})";
}
