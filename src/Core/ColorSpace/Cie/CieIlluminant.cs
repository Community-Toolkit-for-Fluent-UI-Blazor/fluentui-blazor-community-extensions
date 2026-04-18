using System.Diagnostics;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Cie;

/// <summary>
/// Represents a CIE standard illuminant, providing chromaticity coordinates and color temperature for both 2-degree and
/// 10-degree fields of view as defined by the CIE colorimetry standards.
/// </summary>
/// <remarks>A CIE illuminant defines the spectral power distribution of a theoretical light source used as a
/// reference in color science. This struct encapsulates the X and Y chromaticity coordinates for both 2-degree and
/// 10-degree standard observer fields, as well as the correlated color temperature in Kelvin. Instances of this struct
/// are immutable and can be compared for equality.</remarks>
[DebuggerDisplay("{ToString()}")]
public readonly struct CieIlluminant : IEquatable<CieIlluminant>
{
    /// <summary>
    /// Gets the value of the X coordinate for a 2-degree field of view.
    /// </summary>
    public double X2 { get; }

    /// <summary>
    /// Gets the value of the Y coordinate for a 2-degree field of view.
    /// </summary>
    public double Y2 { get; }

    /// <summary>
    /// Gets the value of the X coordinate for a 10-degree field of view.
    /// </summary>
    public double X10 { get; }

    /// <summary>
    /// Gets the value of the Y coordinate for a 10-degree field of view.
    /// </summary>
    public double Y10 { get; }

    /// <summary>
    /// Gets the color temperature in Kelvin.
    /// </summary>
    public double Kelvin { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CieIlluminant"/> struct with the specified values.
    /// </summary>
    /// <param name="x2">The value of the X coordinate for a 2-degree field of view.</param>
    /// <param name="y2">The value of the Y coordinate for a 2-degree field of view.</param>
    /// <param name="x10">The value of the X coordinate for a 10-degree field of view.</param>
    /// <param name="y10">The value of the Y coordinate for a 10-degree field of view.</param>
    /// <param name="kelvin">The color temperature in Kelvin.</param>
    public CieIlluminant(
           double x2,
           double y2,
           double x10,
           double y10,
           double kelvin)
    {
        X2 = x2;
        Y2 = y2;
        X10 = x10;
        Y10 = y10;
        Kelvin = kelvin;
    }

    /// <summary>
    /// Checks if the two instances are equal by comparing their properties.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>Returns <see langword="true"/> if the instances are equal, <see langword="false"/> otherwise.</returns>
    public static bool operator ==(CieIlluminant left, CieIlluminant right)
    {
        return left.Kelvin == right.Kelvin &&
               left.X10 == right.X10 &&
               left.X2 == right.X2 &&
               left.Y10 == right.Y10 &&
               left.Y2 == right.Y2;
    }

    /// <summary>
    /// Checks if the two instances are not equal by comparing their properties.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>Returns <see langword="true"/> if the instances are not equal, <see langword="false"/> otherwise.</returns>
    public static bool operator !=(CieIlluminant left, CieIlluminant right)
    {
        return !(left == right);
    }

    /// <inheritdoc />
    public override readonly bool Equals(object? obj)
    {
        if (obj is CieIlluminant si)
        {
            return si == this;
        }

        return false;
    }

    /// <inheritdoc />
    public readonly bool Equals(CieIlluminant other)
    {
        return this == other;
    }

    /// <inheritdoc />
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(X2, Y2, X10, Y10, Kelvin);
    }

    /// <inheritdoc />
    public override readonly string ToString()
    {
        return $"X2: {X2}, Y2: {Y2}, X10: {X10}, Y10: {Y10}, Kelvin: {Kelvin}";
    }

    /// <summary>
    /// Converts the current color to its CIE XYZ representation using the specified illuminant point of view and
    /// luminance value.
    /// </summary>
    /// <param name="pov">The illuminant point of view to use for the conversion. Determines the observer angle applied in the
    /// transformation.</param>
    /// <param name="Y">The luminance value (Y component) to use in the XYZ color space. Must be a positive number. The default is 1.0.</param>
    /// <returns>An Xyz structure representing the color in the CIE XYZ color space, calculated according to the specified
    /// illuminant point of view and luminance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified <paramref name="pov"/> value is not a valid member of the IlluminantPointOfView
    /// enumeration.</exception>
    public Xyz ToXyz(IlluminantPointOfView pov, double Y = 1.0)
    {
        return pov switch
        {
            IlluminantPointOfView.TwoDegrees => ToXyz2(Y),
            IlluminantPointOfView.TenDegrees => ToXyz10(Y),
            _ => throw new ArgumentOutOfRangeException(nameof(pov))
        };
    }

    /// <summary>
    /// Converts the chromaticity coordinates of the illuminant to the XYZ color space using the 2-degree field of view values.
    /// </summary>
    /// <param name="Y">The Y value to use for the conversion. Defaults to 1.0.</param>
    /// <returns>An <see cref="Xyz"/> instance representing the converted color.</returns>
    private Xyz ToXyz2(double Y = 1.0)
    {
        var X = X2 * (Y / Y2);
        var Z = (1 - X2 - Y2) * (Y / Y2);

        return new Xyz(X, Y, Z);
    }

    /// <summary>
    /// Converts the current chromaticity coordinates to CIE 1931 XYZ tristimulus values using the specified Y
    /// luminance.
    /// </summary>
    /// <remarks>Use this method to obtain absolute XYZ values from chromaticity coordinates by specifying the
    /// desired luminance. The calculation assumes the current instance represents valid chromaticity values.</remarks>
    /// <param name="Y">The Y tristimulus value representing luminance. The default is 1.0. Must be positive.</param>
    /// <returns>An Xyz structure containing the calculated X, Y, and Z tristimulus values corresponding to the current
    /// chromaticity and specified luminance.</returns>
    private Xyz ToXyz10(double Y = 1.0)
    {
        var X = X10 * (Y / Y10);
        var Z = (1 - X10 - Y10) * (Y / Y10);

        return new Xyz(X, Y, Z);
    }
}
