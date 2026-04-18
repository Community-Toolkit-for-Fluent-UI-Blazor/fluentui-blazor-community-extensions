using System.Diagnostics.CodeAnalysis;
using FluentUI.Blazor.Community.Components.ColorSpace.Converters;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

/// <summary>
/// Represents a color in the RYB (Red, Yellow, Blue) color space with an optional alpha (transparency) channel.
/// </summary>
public readonly struct Ryb : IEquatable<Ryb>
{
    /// <summary>
    /// Represents the hash code for the current instance of the <see cref="Ryb"/> struct.
    /// </summary>
    private readonly int _hashCode;

    /// <summary>
    /// Gets the value of the Red component in the RYB color space, normalized to the range [0, 1].
    /// </summary>
    public double R { get; }

    /// <summary>
    /// Gets the value of the Yellow component in the RYB color space, normalized to the range [0, 1].
    /// </summary>
    public double Y { get; }

    /// <summary>
    /// Gets the value of the Blue component in the RYB color space, normalized to the range [0, 1].
    /// </summary>
    public double B { get; }

    /// <summary>
    /// Gets the value of the alpha (opacity) component in the RYB color space, normalized to the range [0, 1].
    /// </summary>
    public double A { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Ryb"/> struct with the specified red, yellow, blue, and alpha values.
    /// </summary>
    /// <param name="r">The red component of the color, normalized to the range [0, 1].</param>
    /// <param name="y">The yellow component of the color, normalized to the range [0, 1].</param>
    /// <param name="b">The blue component of the color, normalized to the range [0, 1].</param>
    /// <param name="a">The alpha (opacity) component of the color, normalized to the range [0, 1].</param>
    public Ryb(double r, double y, double b, double a = 1.0)
    {
        R = Math.Clamp(r, 0, 1);
        Y = Math.Clamp(y, 0, 1);
        B = Math.Clamp(b, 0, 1);
        A = Math.Clamp(a, 0, 1);
        _hashCode = HashCode.Combine(R, Y, B, A);
    }

    /// <summary>
    /// Creates a new Ryb color from the specified sRGB color with 8-bit channel precision.
    /// </summary>
    /// <remarks>This method converts the input sRGB color to a linear RGB representation before mapping it to
    /// the Ryb color space. The alpha channel is normalized to the range 0.0 to 1.0.</remarks>
    /// <param name="c">The sRGB color with 8-bit red, green, blue, and alpha channels to convert to the Ryb color space.</param>
    /// <returns>A Ryb color equivalent to the specified sRGB color, preserving the alpha channel as a normalized value.</returns>
    public static Ryb FromSrgb8(Srgb8 c) => ColorSpaceConverters.ToRyb(RgbLinear.FromSrgb8(c), c.A / 255.0);

    /// <summary>
    /// Converts the current color to its sRGB 8-bit representation.
    /// </summary>
    /// <returns>An <see cref="Srgb8"/> structure representing the color in sRGB 8-bit format.</returns>
    public Srgb8 ToSrgb8() => ColorSpaceConverters.FromRyb(this).ToSrgb8();

    /// <summary>
    /// Determines whether the specified <see cref="Ryb"/> instance is equal to the current instance by comparing their component values.
    /// </summary>
    /// <param name="other">The <see cref="Ryb"/> instance to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the specified <see cref="Ryb"/> instance is equal to the current instance; otherwise, <see langword="false"/>.</returns>
    public bool Equals(Ryb other) => R == other.R && Y == other.Y && B == other.B && A == other.A;

    /// <inheritdoc />
    public override int GetHashCode() => _hashCode;

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Ryb other && Equals(other);
}
