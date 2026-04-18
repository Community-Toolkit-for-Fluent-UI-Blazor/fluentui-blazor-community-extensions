using System.Diagnostics.CodeAnalysis;
using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.Maths;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

/// <summary>
/// Represents a color in the CIE 1931 XYZ color space, which is a device-independent color representation based on human vision. The X, Y, and Z components correspond to the tristimulus values that define a color in this space. This struct provides a way to represent colors in the XYZ color space, which can be used for various color conversions and manipulations.
/// </summary>
public readonly struct Xyz : IEquatable<Xyz>
{
    /// <summary>
    /// Represents the hash code for the current instance of the <see cref="Xyz"/> struct.
    /// </summary>
    private readonly int _hashcode;

    /// <summary>
    /// Initializes a new instance of the <see cref="Xyz"/> struct with the specified X, Y, and Z component values. The values of X, Y, and Z can be any non-negative double values, where the Y component typically represents the luminance of the color.
    /// </summary>
    /// <param name="x">The X component of the color, representing the red-green axis of the color space.</param>
    /// <param name="y">The Y component of the color, representing the luminance (brightness) of the color.</param>
    /// <param name="z">The Z component of the color, representing the blue-yellow axis of the color space.</param>
    public Xyz(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
        _hashcode = HashCode.Combine(X, Y, Z);
    }

    /// <summary>
    /// Gets the X component of the color in the XYZ color space, representing the red-green axis of the color space.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Gets the Y component of the color in the XYZ color space, representing the luminance (brightness) of the color. The Y component is often used as a measure of the perceived brightness of a color, with higher values indicating brighter colors.
    /// </summary>
    public double Y { get; }

    /// <summary>
    /// Gets the Z component of the color in the XYZ color space, representing the blue-yellow axis of the color space. The Z component contributes to the overall color appearance and is used in conjunction with the X and Y components to define a specific color in the XYZ color space.
    /// </summary>
    public double Z { get; }

    /// <inheritdoc />
    public bool Equals(Xyz other) => X == other.X && Y == other.Y && Z == other.Z;

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Xyz other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => _hashcode;

    /// <inheritdoc />
    public override string ToString() => $"X={X:0.###}, Y={Y:0.###}, Z={Z:0.###}";

    /// <summary>
    /// Converts a color from the sRGB color space (represented as an <see cref="Srgb8"/> struct) to the XYZ color space using the provided RGB to XYZ conversion matrix, gamma value, and a flag indicating whether the input color is in sRGB format. The method applies the appropriate gamma correction to the RGB values before performing the matrix multiplication to obtain the corresponding XYZ values. The resulting XYZ color is returned as a new instance of the <see cref="Xyz"/> struct.
    /// </summary>
    /// <param name="color">The color in the sRGB color space to be converted.</param>
    /// <param name="rgbToXyzMatrix">The matrix used to convert RGB values to XYZ values.</param>
    /// <param name="gamma">The gamma value to use for non-sRGB colors.</param>
    /// <param name="isSrgb">Indicates whether the input color is in sRGB format.</param>
    /// <returns></returns>
    public static Xyz ToXYZ(
        Srgb8 color,
        Matrix3x3 rgbToXyzMatrix,
        double gamma,
        bool isSrgb)
    {
        var m = rgbToXyzMatrix;
        var r = color.R / 255.0;
        var g = color.G / 255.0;
        var b = color.B / 255.0;

        r = GetXYZValue(r, gamma, isSrgb);
        g = GetXYZValue(g, gamma, isSrgb);
        b = GetXYZValue(b, gamma, isSrgb);

        return new Xyz(r * m.M11 + g * m.M21 + b * m.M31,
                       r * m.M12 + g * m.M22 + b * m.M32,
                       r * m.M13 + g * m.M22 + b * m.M33);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="gamma"></param>
    /// <param name="isSrgb"></param>
    /// <returns></returns>
    private static double GetXYZValue(double value, double gamma, bool isSrgb)
    {
        if (isSrgb)
        {
            return ColorSpaceConverters.SrgbToLinear(value);
        }

        return Math.Pow(value, gamma);
    }
}
