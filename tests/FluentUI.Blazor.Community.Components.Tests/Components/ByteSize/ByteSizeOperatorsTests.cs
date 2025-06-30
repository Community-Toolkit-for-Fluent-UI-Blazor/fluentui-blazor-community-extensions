namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ByteSizeOperatorsTests
{
    [Fact]
    public void Equals_ObjectAndStruct_WorksCorrectly()
    {
        var a = ByteSize.FromBytes(10);
        var b = ByteSize.FromBytes(10);
        object obj = b;
        Assert.True(a.Equals(obj));
        Assert.True(a.Equals(b));
        Assert.False(a.Equals(null));
    }

    [Fact]
    public void GetHashCode_ReturnsSameForEqual()
    {
        var a = ByteSize.FromBytes(5);
        var b = ByteSize.FromBytes(5);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void CompareTo_WorksCorrectly()
    {
        var a = ByteSize.FromBytes(1);
        var b = ByteSize.FromBytes(2);
        Assert.True(a.CompareTo(b) < 0);
        Assert.True(b.CompareTo(a) > 0);
        Assert.Equal(0, a.CompareTo(ByteSize.FromBytes(1)));
    }

    [Fact]
    public void Add_Subtract_Operators_Work()
    {
        var a = ByteSize.FromBytes(3);
        var b = ByteSize.FromBytes(2);
        Assert.Equal(ByteSize.FromBytes(5), a + b);
        Assert.Equal(ByteSize.FromBytes(1), a - b);
    }

    [Fact]
    public void Increment_Decrement_Operators_Work()
    {
        var a = ByteSize.FromBytes(1);
        Assert.Equal(ByteSize.FromBytes(2), ++a);
        Assert.Equal(ByteSize.FromBytes(1), --a);
    }

    [Fact]
    public void Negate_Operator_Works()
    {
        var a = ByteSize.FromBytes(2);
        Assert.Equal(ByteSize.FromBytes(-2), -a);
    }

    [Fact]
    public void Multiply_Divide_Operators_Work()
    {
        var a = ByteSize.FromBytes(4);
        var b = ByteSize.FromBytes(2);
        Assert.Equal(ByteSize.FromBytes(8), a * b);
        Assert.Equal(ByteSize.FromBytes(2), a / b);
    }

    [Fact]
    public void Divide_ByZero_Throws()
    {
        var a = ByteSize.FromBytes(1);
        var b = ByteSize.FromBytes(0);
        Assert.Throws<DivideByZeroException>(() => a / b);
    }

    [Fact]
    public void Equality_Operators_Work()
    {
        var a = ByteSize.FromBytes(1);
        var b = ByteSize.FromBytes(1);
        var c = ByteSize.FromBytes(2);
        Assert.True(a == b);
        Assert.False(a == c);
        Assert.True(a != c);
        Assert.False(a != b);
    }

    [Fact]
    public void Comparison_Operators_Work()
    {
        var a = ByteSize.FromBytes(1);
        var b = ByteSize.FromBytes(2);
        Assert.True(a < b);
        Assert.True(a <= b);
        Assert.True(b > a);
        Assert.True(b >= a);
        Assert.True(a <= a);
        Assert.True(a >= a);
    }

    [Fact]
    public void AddBits_AddsBitsCorrectly()
    {
        var a = ByteSize.FromBytes(1); // 8 bits
        var result = a.AddBits(8); // +8 bits = 2 bytes
        Assert.Equal(ByteSize.FromBytes(2), result);
    }

    [Fact]
    public void AddBytes_AddsBytesCorrectly()
    {
        var a = ByteSize.FromBytes(1);
        var result = a.AddBytes(2.5);
        Assert.Equal(ByteSize.FromBytes(3.5), result);
    }

    [Fact]
    public void Add_AddsByteSizesCorrectly()
    {
        var a = ByteSize.FromBytes(1);
        var b = ByteSize.FromBytes(2);
        var result = a.Add(b);
        Assert.Equal(ByteSize.FromBytes(3), result);
    }

    [Fact]
    public void Subtract_SubtractsByteSizesCorrectly()
    {
        var a = ByteSize.FromBytes(5);
        var b = ByteSize.FromBytes(2);
        var result = a.Subtract(b);
        Assert.Equal(ByteSize.FromBytes(3), result);
    }

    [Fact]
    public void CompareTo_EqualAndNegative()
    {
        var a = ByteSize.FromBytes(2);
        var b = ByteSize.FromBytes(2);
        var c = ByteSize.FromBytes(-2);
        Assert.Equal(0, a.CompareTo(b));
        Assert.True(c.CompareTo(a) < 0);
    }

    [Fact]
    public void Equals_NullObject_ReturnsFalse()
    {
        var a = ByteSize.FromBytes(1);
        object? obj = null;
        Assert.False(a.Equals(obj));
    }

    [Fact]
    public void Equals_DifferentTypeObject_ReturnsFalse()
    {
        var a = ByteSize.FromBytes(1);
        Assert.False(a.Equals("string"));
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        var a = ByteSize.FromBytes(1);
        var b = ByteSize.FromBytes(2);
        Assert.False(a.Equals(b));
    }

    [Fact]
    public void Add_WithZeroAndNegative()
    {
        var a = ByteSize.FromBytes(5);
        var zero = ByteSize.FromBytes(0);
        var neg = ByteSize.FromBytes(-2);
        Assert.Equal(a, a.Add(zero));
        Assert.Equal(ByteSize.FromBytes(3), a.Add(neg));
    }

    [Fact]
    public void AddBits_WithZeroAndNegative()
    {
        var a = ByteSize.FromBytes(2); // 16 bits
        Assert.Equal(a, a.AddBits(0));
        var result = a.AddBits(-8); // -8 bits = -1 byte
        Assert.Equal(ByteSize.FromBytes(1), result);
    }

    [Fact]
    public void AddBytes_WithZeroAndNegative()
    {
        var a = ByteSize.FromBytes(2);
        Assert.Equal(a, a.AddBytes(0));
        var result = a.AddBytes(-1.5);
        Assert.Equal(ByteSize.FromBytes(0.5), result);
    }

    [Fact]
    public void Equals_DifferentType_ReturnsFalse()
    {
        var a = ByteSize.FromBytes(1);
        Assert.False(a.Equals("not a ByteSize"));
    }
}
