using System.Diagnostics.CodeAnalysis;
using FluentUI.Blazor.Community.Components.ColorSpace.Converters;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

/// <summary>
/// Represents a color using the Hue, Saturation, Brightness (HSB) color model with an alpha (opacity) component.
/// </summary>
/// <remarks>The HSB color model describes colors in terms of hue (0–360 degrees), saturation (0–1), brightness
/// (0–1), and alpha (0–1). This struct provides methods for converting to and from sRGB color representations. All
/// component values are clamped to their valid ranges.</remarks>
public readonly struct Hsb : IEquatable<Hsb>
{
    /// <summary>
    /// Represents the hash code for the current instance of the <see cref="Hsb"/> struct.
    /// </summary>
    private readonly int _hashCode;

    /// <summary>
    /// Gets the hue component of the color, in degrees.
    /// </summary>
    /// <remarks>The hue is represented as a value in the range 0 through 360, where 0 and 360 both correspond
    /// to red, 120 to green, and 240 to blue.</remarks>
    public double H { get; }

    /// <summary>
    /// Gets the saturation component of the color as a double in the range 0 to 1.
    /// </summary>
    public double S { get; }

    /// <summary>
    /// Gets the brightness component of the color as a double in the range 0 to 1.
    /// </summary>
    public double B { get; }

    /// <summary>
    /// Gets the alpha (opacity) component of the color as a double in the range 0 to 1.
    /// </summary>
    public double A { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Hsb"/> struct with the specified hue, saturation, brightness, and alpha values.
    /// </summary>
    /// <param name="h">The hue component of the color, in degrees.</param>
    /// <param name="s">The saturation component of the color as a double in the range 0 to 1.</param>
    /// <param name="b">The brightness component of the color as a double in the range 0 to 1.</param>
    /// <param name="a">The alpha (opacity) component of the color as a double in the range 0 to 1.</param>
    public Hsb(double h, double s, double b, double a = 1.0)
    {
        H = (h % 360 + 360) % 360;
        S = Math.Clamp(s, 0, 1);
        B = Math.Clamp(b, 0, 1);
        A = Math.Clamp(a, 0, 1);
        _hashCode = HashCode.Combine(H, S, B, A);
    }

    /// <summary>
    /// Creates an HSB color from an sRGB8 color value.
    /// </summary>
    /// <param name="c">The sRGB8 color to convert to HSB.</param>
    /// <returns>An Hsb structure representing the equivalent hue, saturation, brightness, and alpha values of the specified
    /// sRGB8 color.</returns>
    public static Hsb FromSrgb8(Srgb8 c)
    {
        var rgb = RgbLinear.FromSrgb8(c);
        return ColorSpaceConverters.ToHsb(rgb, c.A / 255.0);
    }

    /// <summary>
    /// Converts the current color to its equivalent sRGB 8-bit representation.
    /// </summary>
    /// <remarks>Use this method to obtain a standard sRGB 8-bit color value from the current color instance,
    /// which may be in a different color space. This is useful for rendering or interoperability with APIs that require
    /// sRGB input.</remarks>
    /// <returns>An <see cref="Srgb8"/> structure representing the color in sRGB 8-bit format.</returns>
    public Srgb8 ToSrgb8() => ColorSpaceConverters.FromHsb(this).ToSrgb8();

    /// <inheritdoc/>
    public bool Equals(Hsb other) => H == other.H && S == other.S && B == other.B && A == other.A;

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Hsb other && Equals(other);

    /// <inheritdoc/>
    public override string ToString() => $"H={H:0.##}, S={S:0.##}, B={B:0.##}, A={A:0.##}";

    /// <summary>
    /// Determines whether two Hsb instances are equal.
    /// </summary>
    /// <remarks>This operator compares the values of the two Hsb instances for equality.</remarks>
    /// <param name="left">The first Hsb instance to compare.</param>
    /// <param name="right">The second Hsb instance to compare.</param>
    /// <returns>true if the specified Hsb instances are equal; otherwise, false.</returns>
    public static bool operator ==(Hsb left, Hsb right) => left.Equals(right);

    /// <summary>
    /// Determines whether two Hsb instances are not equal.
    /// </summary>
    /// <remarks>Use this operator to compare two Hsb values for inequality. The result is based on the Equals
    /// method implementation.</remarks>
    /// <param name="left">The first Hsb instance to compare.</param>
    /// <param name="right">The second Hsb instance to compare.</param>
    /// <returns>true if the specified Hsb instances are not equal; otherwise, false.</returns>
    public static bool operator !=(Hsb left, Hsb right) => !left.Equals(right);

    /// <inheritdoc />
    public override int GetHashCode() => _hashCode;
}
