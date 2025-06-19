



namespace FluentUI.Blazor.Community;

public partial struct ByteSize
{
    /// <inheritdoc />
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

    public bool Equals(ByteSize value)
    {
        return Bits == value.Bits;
    }

    public override int GetHashCode()
    {
        return Bits.GetHashCode();
    }

    public int CompareTo(ByteSize other)
    {
        return Bits.CompareTo(other.Bits);
    }

    public ByteSize Add(ByteSize bs)
    {
        return new ByteSize(Bytes + bs.Bytes);
    }

    public ByteSize AddBits(long value)
    {
        return this + FromBits(value);
    }

    public ByteSize AddBytes(double value)
    {
        return this + FromBytes(value);
    }

    public ByteSize Subtract(ByteSize bs)
    {
        return new ByteSize(Bytes - bs.Bytes);
    }

    public static ByteSize operator +(ByteSize b1, ByteSize b2)
    {
        return new ByteSize(b1.Bytes + b2.Bytes);
    }

    public static ByteSize operator ++(ByteSize b)
    {
        return new ByteSize(b.Bytes + 1);
    }

    public static ByteSize operator -(ByteSize b)
    {
        return new ByteSize(-b.Bytes);
    }

    public static ByteSize operator -(ByteSize b1, ByteSize b2)
    {
        return new ByteSize(b1.Bytes - b2.Bytes);
    }

    public static ByteSize operator --(ByteSize b)
    {
        return new ByteSize(b.Bytes - 1);
    }

    public static ByteSize operator *(ByteSize a, ByteSize b)
    {
        return new ByteSize(a.Bytes * b.Bytes);
    }

    public static ByteSize operator /(ByteSize a, ByteSize b)
    {
        return b.Bytes switch
        {
            not 0 => new ByteSize(a.Bytes / b.Bytes),
            0 => throw new DivideByZeroException(),
        };
    }

    public static bool operator ==(ByteSize b1, ByteSize b2)
    {
        return b1.Bits == b2.Bits;
    }

    public static bool operator !=(ByteSize b1, ByteSize b2)
    {
        return b1.Bits != b2.Bits;
    }

    public static bool operator <(ByteSize b1, ByteSize b2)
    {
        return b1.Bits < b2.Bits;
    }

    public static bool operator <=(ByteSize b1, ByteSize b2)
    {
        return b1.Bits <= b2.Bits;
    }

    public static bool operator >(ByteSize b1, ByteSize b2)
    {
        return b1.Bits > b2.Bits;
    }

    public static bool operator >=(ByteSize b1, ByteSize b2)
    {
        return b1.Bits >= b2.Bits;
    }
}
