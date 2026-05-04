using System.Diagnostics.CodeAnalysis;
using FluentUI.Blazor.Community.Components.ColorSpace.Converters;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

/// <summary>
/// Represents a color in the HSV (Hue, Saturation, Value) color space with an optional alpha (transparency) channel.
/// </summary>
public readonly struct Hsv : IEquatable<Hsv>
{
    /// <summary>
    /// Represents the hash code for the current instance of the <see cref="Hsv"/> struct.
    /// </summary>
    private readonly int _hashcode;

    /// <summary>
    /// Gets the value of the Hue component in the color space.
    /// </summary>
    public double H { get; }

    /// <summary>
    /// Gets the value of the Saturation component in the color space.
    /// </summary>
    public double S { get; }

    /// <summary>
    /// Gets the Value component value in the color space.
    /// </summary>
    public double V { get; }

    /// <summary>
    /// Gets the value of the alpha (opacity) component in the color space.
    /// </summary>
    public double A { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Hsv"/> struct with the specified hue, saturation, value, and alpha values.
    /// </summary>
    /// <param name="h">The hue component of the color, in degrees.</param>
    /// <param name="s">The saturation component of the color as a double in the range 0 to 1.</param>
    /// <param name="v">The value component of the color as a double in the range 0 to 1.</param>
    /// <param name="a">The alpha (opacity) component of the color as a double in the range 0 to 1.</param>
    public Hsv(double h, double s, double v, double a = 1.0)
    {
        H = (h % 360 + 360) % 360;
        S = Math.Clamp(s, 0, 1);
        V = Math.Clamp(v, 0, 1);
        A = Math.Clamp(a, 0, 1);
        _hashcode = HashCode.Combine(H, S, V, A);
    }

    /// <summary>
    /// Creates an HSV color from the specified sRGB color with 8-bit channels.
    /// </summary>
    /// <remarks>The alpha channel of the input color is normalized to the range [0, 1] in the resulting HSV
    /// color.</remarks>
    /// <param name="c">The sRGB color with 8-bit red, green, blue, and alpha channels to convert to HSV.</param>
    /// <returns>An HSV color equivalent to the specified sRGB color.</returns>
    public static Hsv FromSrgb8(Srgb8 c) => ColorSpaceConverters.ToHsv(RgbLinear.FromSrgb8(c), c.A / 255.0);

    /// <summary>
    /// Converts the current HSV color to its equivalent sRGB representation.
    /// </summary>
    /// <remarks>Use this method to obtain an sRGB color that corresponds to the current HSV color values. The
    /// conversion follows standard color space transformation rules.</remarks>
    /// <returns>An Srgb8 structure representing the sRGB color equivalent of this HSV color.</returns>
    public Srgb8 ToSrgb8() => ColorSpaceConverters.FromHsv(this).ToSrgb8();

    /// <inheritdoc />
    public bool Equals(Hsv other) => H == other.H && S == other.S && V == other.V && A == other.A;

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Hsv other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => _hashcode;

    /// <summary>
    /// Determines whether two Hsv instances are equal.
    /// </summary>
    /// <param name="left">The first Hsv instance to compare.</param>
    /// <param name="right">The second Hsv instance to compare.</param>
    /// <returns>true if the specified Hsv instances are equal; otherwise, false.</returns>
    public static bool operator ==(Hsv left, Hsv right) => left.Equals(right);

    /// <summary>
    /// Determines whether two Hsv instances are not equal.
    /// </summary>
    /// <param name="left">The first Hsv instance to compare.</param>
    /// <param name="right">The second Hsv instance to compare.</param>
    /// <returns>true if the specified Hsv instances are not equal; otherwise, false.</returns>
    public static bool operator !=(Hsv left, Hsv right) => !left.Equals(right);
}

