namespace FluentUI.Blazor.Community;

/// <summary>
/// Provides operator overloads and comparison logic for the <see cref="ByteSize"/> struct.
/// </summary>
public partial struct ByteSize
{
    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="ByteSize"/> instance.
    /// </summary>
    /// <param name="value">The object to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified object is a <see cref="ByteSize"/> and has the same value; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? value)
    {
        if (value == null)
        {
            return false;
        }

        if (value is ByteSize size)
        {
            return Equals(size);
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified <see cref="ByteSize"/> is equal to the current instance.
    /// </summary>
    /// <param name="value">The <see cref="ByteSize"/> to compare.</param>
    /// <returns><c>true</c> if both instances represent the same number of bits; otherwise, <c>false</c>.</returns>
    public bool Equals(ByteSize value)
    {
        return Bits == value.Bits;
    }

    /// <summary>
    /// Returns the hash code for this <see cref="ByteSize"/> instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
        return Bits.GetHashCode();
    }

    /// <summary>
    /// Compares the current instance with another <see cref="ByteSize"/> and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order.
    /// </summary>
    /// <param name="other">A <see cref="ByteSize"/> to compare with this instance.</param>
    /// <returns>
    /// A value less than zero if this instance is less than <paramref name="other"/>; zero if equal; greater than zero if this instance is greater.
    /// </returns>
    public int CompareTo(ByteSize other)
    {
        return Bits.CompareTo(other.Bits);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> that is the sum of this instance and the specified <see cref="ByteSize"/>.
    /// </summary>
    /// <param name="bs">The <see cref="ByteSize"/> to add.</param>
    /// <returns>The sum as a new <see cref="ByteSize"/>.</returns>
    public ByteSize Add(ByteSize bs)
    {
        return new ByteSize(Bytes + bs.Bytes);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> by adding the specified number of bits to this instance.
    /// </summary>
    /// <param name="value">The number of bits to add.</param>
    /// <returns>The result as a new <see cref="ByteSize"/>.</returns>
    public ByteSize AddBits(long value)
    {
        return this + FromBits(value);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> by adding the specified number of bytes to this instance.
    /// </summary>
    /// <param name="value">The number of bytes to add.</param>
    /// <returns>The result as a new <see cref="ByteSize"/>.</returns>
    public ByteSize AddBytes(double value)
    {
        return this + FromBytes(value);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> that is the difference between this instance and the specified <see cref="ByteSize"/>.
    /// </summary>
    /// <param name="bs">The <see cref="ByteSize"/> to subtract.</param>
    /// <returns>The difference as a new <see cref="ByteSize"/>.</returns>
    public ByteSize Subtract(ByteSize bs)
    {
        return new ByteSize(Bytes - bs.Bytes);
    }

    /// <summary>
    /// Adds two <see cref="ByteSize"/> values.
    /// </summary>
    public static ByteSize operator +(ByteSize b1, ByteSize b2)
    {
        return new ByteSize(b1.Bytes + b2.Bytes);
    }

    /// <summary>
    /// Increments the <see cref="ByteSize"/> by one byte.
    /// </summary>
    public static ByteSize operator ++(ByteSize b)
    {
        return new ByteSize(b.Bytes + 1);
    }

    /// <summary>
    /// Negates the <see cref="ByteSize"/> value.
    /// </summary>
    public static ByteSize operator -(ByteSize b)
    {
        return new ByteSize(-b.Bytes);
    }

    /// <summary>
    /// Subtracts one <see cref="ByteSize"/> from another.
    /// </summary>
    public static ByteSize operator -(ByteSize b1, ByteSize b2)
    {
        return new ByteSize(b1.Bytes - b2.Bytes);
    }

    /// <summary>
    /// Decrements the <see cref="ByteSize"/> by one byte.
    /// </summary>
    public static ByteSize operator --(ByteSize b)
    {
        return new ByteSize(b.Bytes - 1);
    }

    /// <summary>
    /// Multiplies two <see cref="ByteSize"/> values.
    /// </summary>
    public static ByteSize operator *(ByteSize a, ByteSize b)
    {
        return new ByteSize(a.Bytes * b.Bytes);
    }

    /// <summary>
    /// Divides one <see cref="ByteSize"/> by another.
    /// </summary>
    /// <exception cref="DivideByZeroException">Thrown when <paramref name="b"/> is zero.</exception>
    public static ByteSize operator /(ByteSize a, ByteSize b)
    {
        return b.Bytes switch
        {
            not 0 => new ByteSize(a.Bytes / b.Bytes),
            0 => throw new DivideByZeroException(),
        };
    }

    /// <summary>
    /// Determines whether two <see cref="ByteSize"/> values are equal.
    /// </summary>
    public static bool operator ==(ByteSize b1, ByteSize b2)
    {
        return b1.Bits == b2.Bits;
    }

    /// <summary>
    /// Determines whether two <see cref="ByteSize"/> values are not equal.
    /// </summary>
    public static bool operator !=(ByteSize b1, ByteSize b2)
    {
        return b1.Bits != b2.Bits;
    }

    /// <summary>
    /// Determines whether one <see cref="ByteSize"/> is less than another.
    /// </summary>
    public static bool operator <(ByteSize b1, ByteSize b2)
    {
        return b1.Bits < b2.Bits;
    }

    /// <summary>
    /// Determines whether one <see cref="ByteSize"/> is less than or equal to another.
    /// </summary>
    public static bool operator <=(ByteSize b1, ByteSize b2)
    {
        return b1.Bits <= b2.Bits;
    }

    /// <summary>
    /// Determines whether one <see cref="ByteSize"/> is greater than another.
    /// </summary>
    public static bool operator >(ByteSize b1, ByteSize b2)
    {
        return b1.Bits > b2.Bits;
    }

    /// <summary>
    /// Determines whether one <see cref="ByteSize"/> is greater than or equal to another.
    /// </summary>
    public static bool operator >=(ByteSize b1, ByteSize b2)
    {
        return b1.Bits >= b2.Bits;
    }
}
