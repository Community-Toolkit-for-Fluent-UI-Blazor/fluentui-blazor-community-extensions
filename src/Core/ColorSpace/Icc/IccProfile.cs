using System.Diagnostics;
using FluentUI.Blazor.Community.Components.ColorSpace.Cie;
using FluentUI.Blazor.Community.Components.Maths;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Icc;

/// <summary>
/// Represents an ICC profile, which is a set of data that characterizes a color input or output device, or a color space, according to standards set by the International Color Consortium (ICC). An ICC profile contains information about the device's color characteristics, including its gamma value, standard illuminant, and a colorimetric matrix. This struct is immutable and can be compared for equality based on its properties.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public readonly struct IccProfile
        : IEquatable<IccProfile>
{
    /// <summary>
    /// Avoid creating a new instance of the <see cref="IccProfile"/> class.
    /// </summary>
    /// <param name="name">Name of the profile.</param>
    /// <param name="gamma">Gamma value of the profile.</param>
    /// <param name="standardIlluminantName">Illuminant of the profile.</param>
    /// <param name="m11">First matrix value of the profile.</param>
    /// <param name="m12">Second matrix value of the profile.</param>
    /// <param name="m13">Third matrix value of the profile.</param>
    /// <param name="m21">Fourth matrix value of the profile.</param>
    /// <param name="m22">Fifth matrix value of the profile.</param>
    /// <param name="m23">Sixth matrix value of the profile.</param>
    /// <param name="m31">Seventh matrix value of the profile.</param>
    /// <param name="m32">Eighth matrix value of the profile.</param>
    /// <param name="m33">Ninth matrix value of the profile.</param>
    internal IccProfile(
        IccProfileName name,
        double gamma,
        IlluminantName standardIlluminantName,
        double m11,
        double m12,
        double m13,
        double m21,
        double m22,
        double m23,
        double m31,
        double m32,
        double m33)
    {
        if (!Enum.IsDefined(standardIlluminantName))
        {
            throw new ArgumentOutOfRangeException(nameof(standardIlluminantName), $"The value '{standardIlluminantName}' is not a valid {nameof(Cie.IlluminantName)}.");
        }

        if (m11 <= 0 ||
            m12 <= 0 ||
            m13 <= 0 ||
            m21 <= 0 ||
            m22 <= 0 ||
            m23 <= 0 ||
            m31 <= 0 ||
            m32 <= 0 ||
            m33 <= 0)
        {
            throw new ArgumentException("Matrix values cannot be zero.");
        }

        if (gamma <= 0)
        {
            throw new ArgumentException("Gamma value must be greater than zero.", nameof(gamma));
        }

        Gamma = gamma;
        IlluminantName = standardIlluminantName;
        Matrix = new Matrix3x3(m11, m12, m13, m21, m22, m23, m31, m32, m33);
        Name = name;
    }

    /// <summary>
    /// Gets the <c>Gamma</c> value of the profile.
    /// </summary>
    public double Gamma { get; }

    /// <summary>
    /// Gets the name of the ICC profile associated with this instance.
    /// </summary>
    public IccProfileName Name { get; }

    /// <summary>
    /// Gets the illuminant of the profile.
    /// </summary>
    public IlluminantName IlluminantName { get; }

    /// <summary>
    /// Gets the colorimetric matrix of the profile.
    /// </summary>
    public Matrix3x3 Matrix { get; }

    #region Methods

    /// <summary>
    /// Checks if the instances <paramref name="left"/> and <paramref name="right"/> are equal.
    /// </summary>
    /// <param name="left">First value to check.</param>
    /// <param name="right">Second value to check.</param>
    /// <returns>Returns <see langword="true"/> if the values are equal, <see langword="false"/> otherwise.</returns>
    public static bool operator ==(IccProfile left, IccProfile right)
    {
        return left.Gamma == right.Gamma &&
               left.IlluminantName == right.IlluminantName &&
               left.Matrix == right.Matrix;
    }

    /// <summary>
    /// Checks if the instances <paramref name="left"/> and <paramref name="right"/> are not equal.
    /// </summary>
    /// <param name="left">First value to check.</param>
    /// <param name="right">Second value to check.</param>
    /// <returns>Returns <see langword="true"/> if the values are not equal, <see langword="false"/> otherwise.</returns>
    public static bool operator !=(IccProfile left, IccProfile right)
    {
        return !(left == right);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is IccProfile profile)
        {
            return this == profile;
        }

        return false;
    }

    /// <inheritdoc />
    public bool Equals(IccProfile other)
    {
        return this == other;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Gamma, IlluminantName, Matrix);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Gamma: {Gamma}, StandardIlluminant: {IlluminantName}, Matrix: {Matrix}";
    }

    #endregion Methods
}
