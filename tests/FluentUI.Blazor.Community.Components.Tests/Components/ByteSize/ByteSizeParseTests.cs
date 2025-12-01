using System.Globalization;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ByteSizeParseTests
{
    [Fact]
    public void Parse_ValidSIUnits()
    {
        Assert.Equal(ByteSize.FromBytes(1_000), ByteSize.Parse("1 KB", NumberFormatInfo.CurrentInfo));
        Assert.Equal(ByteSize.FromBytes(2_000_000), ByteSize.Parse("2 MB", NumberFormatInfo.CurrentInfo));
        Assert.Equal(ByteSize.FromBytes(3_000_000_000), ByteSize.Parse("3 GB", NumberFormatInfo.CurrentInfo));
        Assert.Equal(ByteSize.FromBytes(4_000_000_000_000), ByteSize.Parse("4 TB", NumberFormatInfo.CurrentInfo));
        Assert.Equal(ByteSize.FromBytes(5_000_000_000_000_000), ByteSize.Parse("5 PB", NumberFormatInfo.CurrentInfo));
    }

    [Fact]
    public void Parse_ValidBytesAndBits()
    {
        Assert.Equal(ByteSize.FromBytes(123), ByteSize.Parse("123 B", CultureInfo.InvariantCulture));
        Assert.Equal(ByteSize.FromBits(16), ByteSize.Parse("16 b", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Parse_Invalid_Throws()
    {
        Assert.Throws<FormatException>(() => ByteSize.Parse("not a size", CultureInfo.InvariantCulture));
        Assert.Throws<FormatException>(() => ByteSize.Parse("123 XB", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Parse_ValidWithCulture()
    {
        var de = new CultureInfo("de-DE");
        Assert.Equal(ByteSize.FromBytes(1_500), ByteSize.Parse("1,5 KB", de));
    }

    [Fact]
    public void TryParse_Works()
    {
        Assert.True(ByteSize.TryParse("1 KB", out var result));
        Assert.Equal(ByteSize.FromBytes(1_000), result);
        Assert.False(ByteSize.TryParse("bad", out _));
    }
}
