using System.Diagnostics;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

/// <summary>
/// Represents a color in the CIE xyy chromaticity color space, including chrominance coordinates and luminance.
/// </summary>
/// <remarks>The Xyy struct provides properties for the horizontal chrominance (X), vertical chrominance (Y), and
/// luminance (Y2) components. It supports equality comparison, hash code generation, and conversion to and from the Xyz
/// color space. This struct is immutable and suitable for use in color calculations and conversions where CIE xyy
/// representation is required.</remarks>
[DebuggerDisplay("{ToString()}")]
public readonly struct Xyy
    : IEquatable<Xyy>
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="Xyy"/>
    /// </summary>
    /// <param name="x">Valeur de la chrominance horizontale.</param>
    /// <param name="y">Valeur de la chrominance verticale.</param>
    /// <param name="y2">Valeur de la luminance.</param>
    public Xyy(double x, double y, double y2)
    {
        X = x;
        Y = y;
        Y2 = y2;
    }

    /// <summary>
    /// Gets the chrominance on the horizontal axis.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Gets the chrominance on the vertical axis.
    /// </summary>
    public double Y { get; }

    /// <summary>
    /// Gets the luminance.
    /// </summary>
    public double Y2 { get; }

    /// <summary>
    /// Checks if the instances <paramref name="left"/> and <paramref name="right"/> are equal.
    /// </summary>
    /// <param name="left">First value to check.</param>
    /// <param name="right">Second value to check.</param>
    /// <returns>Returns <see langword="true"/> if the values are equal, <see langword="false"/> otherwise.</returns>
    public static bool operator ==(Xyy left, Xyy right)
    {
        return left.X == right.X &&
               left.Y == right.Y &&
               left.Y2 == right.Y2;
    }

    /// <summary>
    /// Checks if the instances <paramref name="left"/> and <paramref name="right"/> are not equal.
    /// </summary>
    /// <param name="left">First value to check.</param>
    /// <param name="right">Second value to check.</param>
    /// <returns>Returns <see langword="true"/> if the values are not equal, <see langword="false"/> otherwise.</returns>
    public static bool operator !=(Xyy left, Xyy right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Gets the hash code for this instance of <see cref="Xyy"/>.
    /// </summary>
    /// <returns>Returns the hash code for this instance.</returns>
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(X, Y, Y2);
    }

    /// <summary>
    /// Checks if the current instance is equal to the specified instance.
    /// </summary>
    /// <param name="other">The value to compare with.</param>
    /// <returns>Returns <see langword="true"/> if the values are equal, <see langword="false"/> otherwise.</returns>
    public bool Equals(Xyy other)
    {
        return this == other;
    }

    /// <summary>
    /// Checks if the current instance is equal to the specified object.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns>Returns <see langword="true"/> if the values are equal, <see langword="false"/> otherwise  .</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Xyy other)
        {
            return this == other;
        }

        return false;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"x: {X}, y: {Y}, Y: {Y2}";
    }

    /// <summary>
    /// Converts a color from the CIE XYZ color space to the CIE xyy chromaticity color space.
    /// </summary>
    /// <param name="value">The color in the CIE XYZ color space to be converted.</param>
    /// <returns>A new <see cref="Xyy"/> instance representing the color in the CIE xyy chromaticity color space.</returns>
    public static Xyy ToXyy(Xyz value)
    {
        var n = value.X + value.Y + value.Z;

        if (n == 0)
        {
            return new Xyy(0, 0, value.Y);
        }

        return new Xyy(value.X / n,
                       value.Y / n,
                       value.Y);
    }
}
