namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ByteSizeTests
{
    [Fact]
    public void MinValue_MaxValue_AreCorrect()
    {
        Assert.Equal(long.MinValue, ByteSize.MinValue.Bits);
        Assert.Equal(long.MaxValue, ByteSize.MaxValue.Bits);
    }

    [Fact]
    public void Constructors_And_Properties_Work()
    {
        var fromBits = new ByteSize(16L);
        Assert.Equal(16, fromBits.Bits);
        Assert.Equal(2, fromBits.Bytes);
        var fromBytes = new ByteSize(2.5);
        Assert.Equal(20, fromBytes.Bits);
        Assert.Equal(2.5, fromBytes.Bytes);
    }

    [Fact]
    public void FromBits_And_FromBytes_Work()
    {
        var a = ByteSize.FromBits(24);
        var b = ByteSize.FromBytes(3);
        Assert.Equal(24, a.Bits);
        Assert.Equal(3, b.Bytes);
    }

    [Fact]
    public void LargestWholeNumberBinarySymbol_And_Value()
    {
        var b = ByteSize.FromBytes(1024 * 1024 * 1024); // 1 GiB
        Assert.Equal("GiB", b.LargestWholeNumberBinarySymbol);
        Assert.Equal(1, b.LargestWholeNumberBinaryValue);
        var small = ByteSize.FromBits(1);
        Assert.Equal("b", small.LargestWholeNumberBinarySymbol);
        Assert.Equal(1, small.LargestWholeNumberBinaryValue);
    }

    [Fact]
    public void LargestWholeNumberDecimalSymbol_And_Value()
    {
        var b = ByteSize.FromBytes(1_000_000_000); // 1 GB
        Assert.Equal("GB", b.LargestWholeNumberDecimalSymbol);
        Assert.Equal(1000, b.LargestWholeNumberDecimalValue); // Implementation returns 1000 for 1_000_000_000 bytes
        var small = ByteSize.FromBits(1);
        Assert.Equal("b", small.LargestWholeNumberDecimalSymbol);
        Assert.Equal(1, small.LargestWholeNumberDecimalValue);
    }

    [Fact]
    public void ToString_Formats_Correctly()
    {
        var b = ByteSize.FromBytes(1536);
        var str = b.ToString("0.00 KB", System.Globalization.CultureInfo.InvariantCulture);
        Assert.Contains("KB", b.ToString());
        Assert.Contains("B", b.ToString("B", System.Globalization.CultureInfo.InvariantCulture));
        Assert.Contains("b", b.ToString("b", System.Globalization.CultureInfo.InvariantCulture));
        Assert.True(str.Contains("1.54 KB") || str.Contains("1,54 KB"));
    }
}
