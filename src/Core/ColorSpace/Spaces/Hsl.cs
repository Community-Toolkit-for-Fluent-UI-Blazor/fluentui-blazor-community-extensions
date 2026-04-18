using System.Diagnostics.CodeAnalysis;
using FluentUI.Blazor.Community.Components.ColorSpace.Converters;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

/// <summary>
/// Represents a color in the HSL (Hue, Saturation, Lightness) color space with an optional alpha (transparency)
/// channel.
/// </summary>
/// <remarks>The HSL color model is commonly used for color selection and manipulation, as it separates color hue
/// from its saturation and lightness. The alpha channel specifies the opacity of the color, where 1.0 is fully opaque
/// and 0.0 is fully transparent. This struct provides methods for converting to and from sRGB color
/// representations.</remarks>
public readonly struct Hsl : IEquatable<Hsl>
{
    /// <summary>
    /// Represents the hash code for the current instance of the <see cref="Hsl"/> struct.
    /// </summary>
    private readonly int _hashcode;

    /// <summary>
    /// Gets the value of H.
    /// </summary>
    public double H { get; }

    /// <summary>
    /// Gets the value of S.
    /// </summary>
    public double S { get; }

    /// <summary>
    /// Gets the L component value in the color space.
    /// </summary>
    public double L { get; }

    /// <summary>
    /// Gets the value of A.
    /// </summary>
    public double A { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Hsl"/> struct with the specified hue, saturation, lightness, and alpha values.
    /// </summary>
    /// <param name="h">The hue component of the color, in degrees.</param>
    /// <param name="s">The saturation component of the color as a double in the range 0 to 1.</param>
    /// <param name="l">The lightness component of the color as a double in the range 0 to 1.</param>
    /// <param name="a">The alpha (opacity) component of the color as a double in the range 0 to 1.</param>
    public Hsl(double h, double s, double l, double a = 1.0)
    {
        H = (h % 360 + 360) % 360;
        S = Math.Clamp(s, 0, 1);
        L = Math.Clamp(l, 0, 1);
        A = Math.Clamp(a, 0, 1);
        _hashcode = HashCode.Combine(H, S, L, A);
    }

    /// <summary>
    /// Creates an HSL color from the specified sRGB color with 8-bit channel values.
    /// </summary>
    /// <remarks>The alpha channel is normalized from the 8-bit value to the range 0.0 to 1.0.</remarks>
    /// <param name="c">The sRGB color with 8-bit red, green, blue, and alpha components to convert.</param>
    /// <returns>An Hsl structure representing the equivalent color and alpha value.</returns>
    public static Hsl FromSrgb8(Srgb8 c) => ColorSpaceConverters.ToHsl(RgbLinear.FromSrgb8(c), c.A / 255.0);

    /// <summary>
    /// Converts the current HSL color to its equivalent sRGB representation.
    /// </summary>
    /// <returns>An Srgb8 structure representing the sRGB equivalent of the current HSL color.</returns>
    public Srgb8 ToSrgb8() => ColorSpaceConverters.ToSrgb8(this);

    /// <inheritdoc />
    public bool Equals(Hsl other) => H == other.H && S == other.S && L == other.L && A == other.A;

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Hsl other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => _hashcode;

    /// <summary>
    /// Determines whether two Hsl instances are equal by comparing their component values.
    /// </summary>
    /// <param name="left">The first Hsl instance to compare.</param>
    /// <param name="right">The second Hsl instance to compare.</param>
    /// <returns>true if the specified Hsl instances are equal; otherwise, false.</returns>
    public static bool operator ==(Hsl left, Hsl right) => left.Equals(right);

    /// <summary>
    /// Determines whether two Hsl instances are not equal.
    /// </summary>
    /// <param name="left">The first Hsl instance to compare.</param>
    /// <param name="right">The second Hsl instance to compare.</param>
    /// <returns>true if the specified Hsl instances are not equal; otherwise, false.</returns>
    public static bool operator !=(Hsl left, Hsl right) => !left.Equals(right);
}
