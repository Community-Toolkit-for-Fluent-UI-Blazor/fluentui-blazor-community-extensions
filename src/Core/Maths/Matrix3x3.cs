using System.Diagnostics;

namespace FluentUI.Blazor.Community.Components.Maths;

/// <summary>
/// Represents a 3x3 matrix.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public readonly struct Matrix3x3
    : IEquatable<Matrix3x3>
{
    /// <summary>
    /// Represents the data of the first column of the first line of the matrix.
    /// </summary>
    private readonly double m11;

    /// <summary>
    /// Represents the data of the second column of the first line of the matrix.
    /// </summary>
    private readonly double m12;

    /// <summary>
    /// Represents the data of the third column of the first line of the matrix.
    /// </summary>
    private readonly double m13;

    /// <summary>
    /// Represents the data of the first column of the second line of the matrix.
    /// </summary>
    private readonly double m21;

    /// <summary>
    /// Represents the data of the second column of the second line of the matrix.
    /// </summary>
    private readonly double m22;

    /// <summary>
    /// Represents the data of the third column of the second line of the matrix.
    /// </summary>
    private readonly double m23;

    /// <summary>
    /// Represents the data of the first column of the third line of the matrix.
    /// </summary>
    private readonly double m31;

    /// <summary>
    /// Represents the data of the second column of the third line of the matrix.
    /// </summary>
    private readonly double m32;

    /// <summary>
    /// Represents the data of the third column of the third line of the matrix.
    /// </summary>
    private readonly double m33;

    /// <summary>
    /// Gets the identity matrix.
    /// </summary>
    public static Matrix3x3 Identity { get; } = new Matrix3x3(1, 0, 0,
                                                              0, 1, 0,
                                                              0, 0, 1);

    /// <summary>
    /// Gets the data of the first column of the first line of the matrix.
    /// </summary>
    public double M11 => m11;

    /// <summary>
    /// Gets the data of the second column of the first line of the matrix.
    /// </summary>
    public double M12 => m12;

    /// <summary>
    /// Gets the data of the third column of the first line of the matrix.
    /// </summary>
    public double M13 => m13;

    /// <summary>
    /// Gets the data of the first column of the second line of the matrix.
    /// </summary>
    public double M21 => m21;

    /// <summary>
    /// Gets the data of the second column of the second line of the matrix.
    /// </summary>
    public double M22 => m22;

    /// <summary>
    /// Gets the data of the third column of the second line of the matrix.
    /// </summary>
    public double M23 => m23;

    /// <summary>
    /// Gets the data of the first column of the third line of the matrix.
    /// </summary>
    public double M31 => m31;

    /// <summary>
    /// Gets the data of the second column of the third line of the matrix.
    /// </summary>
    public double M32 => m32;

    /// <summary>
    /// Gets the data of the third column of the third line of the matrix.
    /// </summary>
    public double M33 => m33;

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix3x3"/> class.
    /// </summary>
    /// <param name="m11">Data of the first column of the first line of the matrix.</param>
    /// <param name="m12">Data of the second column of the first line of the matrix.</param>
    /// <param name="m13">Data of the third column of the first line of the matrix.</param>
    /// <param name="m21">Data of the first column of the second line of the matrix.</param>
    /// <param name="m22">Data of the second column of the second line of the matrix.</param>
    /// <param name="m23">Data of the third column of the second line of the matrix.</param>
    /// <param name="m31">Data of the first column of the third line of the matrix.</param>
    /// <param name="m32">Data of the second column of the third line of the matrix.</param>
    /// <param name="m33">Data of the third column of the third line of the matrix.</param>
    public Matrix3x3(
        double m11 = 0.0,
        double m12 = 0.0,
        double m13 = 0.0,
        double m21 = 0.0,
        double m22 = 0.0,
        double m23 = 0.0,
        double m31 = 0.0,
        double m32 = 0.0,
        double m33 = 0.0)
    {
        this.m11 = m11;
        this.m12 = m12;
        this.m13 = m13;
        this.m21 = m21;
        this.m22 = m22;
        this.m23 = m23;
        this.m31 = m31;
        this.m32 = m32;
        this.m33 = m33;
    }

    /// <summary>
    /// Multiplies the matrices.
    /// </summary>
    /// <param name="value1">First matrix to multiply.</param>
    /// <param name="value2">Second matrix to multiply.</param>
    /// <returns>Returns the resulting matrix from the multiplication.</returns>
    internal static Matrix3x3 Multiply(
        Matrix3x3 value1,
        Matrix3x3 value2)
    {
        return new Matrix3x3(
            value1.m11 * value2.m11 + value1.m12 * value2.m21 + value1.m13 * value2.m31,
            value1.m11 * value2.m12 + value1.m12 * value2.m22 + value1.m13 * value2.m32,
            value1.m11 * value2.m13 + value1.m12 * value2.m23 + value1.m13 * value2.m33,
            value1.m21 * value2.m11 + value1.m22 * value2.m21 + value1.m23 * value2.m31,
            value1.m21 * value2.m12 + value1.m22 * value2.m22 + value1.m23 * value2.m32,
            value1.m21 * value2.m13 + value1.m22 * value2.m23 + value1.m23 * value2.m33,
            value1.m31 * value2.m11 + value1.m32 * value2.m21 + value1.m33 * value2.m31,
            value1.m31 * value2.m12 + value1.m32 * value2.m22 + value1.m33 * value2.m32,
            value1.m31 * value2.m13 + value1.m32 * value2.m23 + value1.m33 * value2.m33
            );
    }

    /// <summary>
    /// Gets the determinant of the matrix.
    /// </summary>
    /// <returns>Returns the determinant of the matrix.</returns>
    internal double GetDeterminant()
    {
        return m11 * (m22 * m33 - m23 * m32) -
               m12 * (m21 * m33 - m23 * m31) +
               m13 * (m21 * m32 - m22 * m31);
    }

    /// <summary>
    /// Gets the inverse of the specified matrix.
    /// </summary>
    /// <param name="value">Matrix to invert.</param>
    /// <returns>Returns an instance of <see cref="Matrix3x3"/> that is the inverse of the specified matrix.</returns>
    internal static Matrix3x3 Inverse(Matrix3x3 value)
    {
        var d = 1.0 / value.GetDeterminant();
        var m = value;

        return new Matrix3x3
        (
            d * (m.m22 * m.m33 - m.m23 * m.m32),
            d * (-1 * (m.m12 * m.m33 - m.m13 * m.m32)),
            d * (m.m12 * m.m23 - m.m13 * m.m22),
            d * (-1 * (m.m21 * m.m33 - m.m23 * m.m31)),
            d * (m.m11 * m.m33 - m.m13 * m.m31),
            d * (-1 * (m.m11 * m.m23 - m.m13 * m.m21)),
            d * (m.m21 * m.m32 - m.m22 * m.m31),
            d * (-1 * (m.m11 * m.m32 - m.m12 * m.m31)),
            d * (m.m11 * m.m22 - m.m12 * m.m21)
        );
    }

    /// <inheritdoc />
    public bool Equals(Matrix3x3 matrix)
    {
        return m11 == matrix.m11 &&
               m12 == matrix.m12 &&
               m13 == matrix.m13 &&
               m21 == matrix.m21 &&
               m22 == matrix.m22 &&
               m23 == matrix.m23 &&
               m31 == matrix.m31 &&
               m32 == matrix.m32 &&
               m33 == matrix.m33;
    }

    /// <summary>
    /// Checks if the two instances are equal.
    /// </summary>
    /// <param name="left">First instance to compare.</param>
    /// <param name="right">Second instance to compare.</param>
    /// <returns>Returns <see langword="true"/> if the instances are equal, <see langword="false"/> otherwise.</returns>
    public static bool operator ==(Matrix3x3 left, Matrix3x3 right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks if the two instances are not equal.
    /// </summary>
    /// <param name="left">First instance to compare.</param>
    /// <param name="right">Second instance to compare.</param>
    /// <returns>Returns <see langword="true"/> if the instances are not equal, <see langword="false"/> otherwise.</returns>
    public static bool operator !=(Matrix3x3 left, Matrix3x3 right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Multiplies the two matrices.
    /// </summary>
    /// <param name="value1">The first matrix to multiply.</param>
    /// <param name="value2">The second matrix to multiply.</param>
    /// <returns>Returns a new <see cref="Matrix3x3"/> instance that is the result of multiplying the two matrices.</returns>
    public static Matrix3x3 operator *(Matrix3x3 value1, Matrix3x3 value2)
    {
        return Multiply(value1, value2);
    }

    /// <summary>
    /// Transforms the specified vector by the given 3x3 matrix using matrix multiplication.
    /// </summary>
    /// <remarks>This operator performs a standard matrix-vector multiplication, applying the transformation
    /// defined by the matrix to the vector.</remarks>
    /// <param name="value">The matrix to use for transforming the vector.</param>
    /// <param name="vector">The vector to be transformed by the matrix.</param>
    /// <returns>A new vector that is the result of multiplying the matrix by the vector.</returns>
    public static Vector3 operator *(Matrix3x3 value, Vector3 vector)
    {
        return value.Transform(vector);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var code = new HashCode();
        code.Add(m11);
        code.Add(m12);
        code.Add(m13);
        code.Add(m21);
        code.Add(m22);
        code.Add(m23);
        code.Add(m31);
        code.Add(m32);
        code.Add(m33);

        return code.ToHashCode();
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is Matrix3x3 mat)
        {
            return mat == this;
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector3 Transform(Vector3 v)
    {
        return new Vector3(
            m11 * v.X + m12 * v.Y + m13 * v.Z,
            m21 * v.X + m22 * v.Y + m23 * v.Z,
            m31 * v.X + m32 * v.Y + m33 * v.Z
        );
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"[{m11}, {m12}, {m13}]{Environment.NewLine}[{m21}, {m22}, {m23}]{Environment.NewLine}[{m31}, {m32}, {m33}]";
    }
}
