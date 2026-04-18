using System.Diagnostics;

namespace FluentUI.Blazor.Community.Components.Maths;

/// <summary>
/// Represents a three-dimensional vector with double-precision components.
/// </summary>
/// <remarks>The Vector3 struct provides basic arithmetic and equality operations for 3D vectors, making it
/// suitable for mathematical, graphical, or geometric computations. Instances of Vector3 are immutable.</remarks>
/// <param name="x">The X-coordinate value of the vector.</param>
/// <param name="y">The Y-coordinate value of the vector.</param>
/// <param name="z">The Z-coordinate value of the vector.</param>
[DebuggerDisplay("{ToString()}")]
public readonly struct Vector3(double x, double y, double z) : IEquatable<Vector3>
{
    /// <summary>
    /// Gets the X-coordinate value.
    /// </summary>
    public double X { get; } = x;

    /// <summary>
    /// Gets the Y-coordinate value.
    /// </summary>
    public double Y { get; } = y;

    /// <summary>
    /// Gets the Z-coordinate value.
    /// </summary>
    public double Z { get; } = z;

    /// <summary>
    /// Adds two Vector3 instances component-wise.
    /// </summary>
    /// <param name="a">The first vector to add.</param>
    /// <param name="b">The second vector to add.</param>
    /// <returns>A new Vector3 whose components are the sum of the corresponding components of the input vectors.</returns>
    public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    /// <summary>
    /// Subtracts the corresponding components of two vectors and returns the resulting vector.
    /// </summary>
    /// <param name="a">The vector to subtract from.</param>
    /// <param name="b">The vector to subtract.</param>
    /// <returns>A new vector whose components are the result of subtracting the components of <paramref name="b"/> from
    /// <paramref name="a"/>.</returns>
    public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    /// <summary>
    /// Multiplies each component of the specified vector by the given scalar value.
    /// </summary>
    /// <param name="v">The vector whose components are to be multiplied.</param>
    /// <param name="s">The scalar value by which to multiply each component of the vector.</param>
    /// <returns>A new Vector3 whose components are the products of the corresponding components of the input vector and the
    /// scalar value.</returns>
    public static Vector3 operator *(Vector3 v, double s) => new(v.X * s, v.Y * s, v.Z * s);

    /// <summary>
    /// Multiplies each component of the specified vector by the given scalar value.
    /// </summary>
    /// <param name="s">The scalar value by which to multiply each component of the vector.</param>
    /// <param name="v">The vector whose components are to be multiplied by the scalar.</param>
    /// <returns>A new Vector3 whose components are the result of multiplying the corresponding components of the input vector by
    /// the scalar value.</returns>
    public static Vector3 operator *(double s, Vector3 v) => new(v.X * s, v.Y * s, v.Z * s);

    /// <summary>
    /// Divides each component of a specified vector by a scalar value.
    /// </summary>
    /// <remarks>If the scalar value is zero, the result will contain components with infinite or undefined
    /// values, depending on the platform's floating-point behavior.</remarks>
    /// <param name="v">The vector whose components are to be divided.</param>
    /// <param name="s">The scalar value by which to divide each component of the vector.</param>
    /// <returns>A new Vector3 whose components are the result of dividing the corresponding components of the input vector by
    /// the scalar value.</returns>
    public static Vector3 operator /(Vector3 v, double s) => new(v.X / s, v.Y / s, v.Z / s);

    /// <inheritdoc />
    public bool Equals(Vector3 other) => X == other.X && Y == other.Y && Z == other.Z;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Vector3 other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    /// <summary>
    /// Determines whether two Vector3 instances are equal.
    /// </summary>
    /// <param name="left">The first Vector3 instance to compare.</param>
    /// <param name="right">The second Vector3 instance to compare.</param>
    /// <returns>true if the two Vector3 instances are equal; otherwise, false.</returns>
    public static bool operator ==(Vector3 left, Vector3 right) => left.Equals(right);

    /// <summary>
    /// Determines whether two Vector3 instances are not equal.
    /// </summary>
    /// <param name="left">The first Vector3 instance to compare.</param>
    /// <param name="right">The second Vector3 instance to compare.</param>
    /// <returns>true if the two Vector3 instances are not equal; otherwise, false.</returns>
    public static bool operator !=(Vector3 left, Vector3 right) => !left.Equals(right);

    /// <inheritdoc />
    public override string ToString() => $"({X:0.###}, {Y:0.###}, {Z:0.###})";
}
