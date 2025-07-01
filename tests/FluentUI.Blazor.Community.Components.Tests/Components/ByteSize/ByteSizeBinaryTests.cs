using System.Globalization;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ByteSizeBinaryTests
{
    [Fact]
    public void Constants_AreCorrect()
    {
        Assert.Equal(1024, ByteSize.BytesInKibiByte);
        Assert.Equal(1024 * 1024, ByteSize.BytesInMebiByte);
        Assert.Equal(1024 * 1024 * 1024, ByteSize.BytesInGibiByte);
        Assert.Equal(1024L * 1024 * 1024 * 1024, ByteSize.BytesInTebiByte);
        Assert.Equal(1024L * 1024 * 1024 * 1024 * 1024, ByteSize.BytesInPebiByte);
        Assert.Equal(1024L * 1024 * 1024 * 1024 * 1024 * 1024, ByteSize.BytesInExabiByte);
        Assert.Equal("KiB", ByteSize.KibiByteSymbol);
        Assert.Equal("MiB", ByteSize.MebiByteSymbol);
        Assert.Equal("GiB", ByteSize.GibiByteSymbol);
        Assert.Equal("TiB", ByteSize.TebiByteSymbol);
        Assert.Equal("PiB", ByteSize.PebiByteSymbol);
        Assert.Equal("EiB", ByteSize.ExabiByteSymbol);
    }

    [Fact]
    public void Properties_ConvertCorrectly()
    {
        var b = ByteSize.FromBytes(1024 * 1024 * 1024);
        Assert.Equal(1024 * 1024 * 1024 / 1024.0, b.KibiBytes);
        Assert.Equal(1024 * 1024 * 1024 / (1024.0 * 1024), b.MebiBytes);
        Assert.Equal(1024 * 1024 * 1024 / (1024.0 * 1024 * 1024), b.GibiBytes);
    }

    [Fact]
    public void AllBinaryProperties_AreCorrect()
    {
        var bytes = 1_152_921_504_606_846_976L; // 1 EiB
        var b = ByteSize.FromBytes(bytes);
        Assert.Equal(bytes / (double)ByteSize.BytesInKibiByte, b.KibiBytes);
        Assert.Equal(bytes / (double)ByteSize.BytesInMebiByte, b.MebiBytes);
        Assert.Equal(bytes / (double)ByteSize.BytesInGibiByte, b.GibiBytes);
        Assert.Equal(bytes / (double)ByteSize.BytesInTebiByte, b.TebiBytes);
        Assert.Equal(bytes / (double)ByteSize.BytesInPebiByte, b.PebiBytes);
        Assert.Equal(bytes / (double)ByteSize.BytesInExabiByte, b.ExabiBytes);
    }

    [Fact]
    public void AllBinaryFactories_AreCorrect()
    {
        Assert.Equal(2 * ByteSize.BytesInKibiByte, ByteSize.FromKibiBytes(2).Bytes);
        Assert.Equal(2 * ByteSize.BytesInMebiByte, ByteSize.FromMebiBytes(2).Bytes);
        Assert.Equal(2 * ByteSize.BytesInGibiByte, ByteSize.FromGibiBytes(2).Bytes);
        Assert.Equal(2 * ByteSize.BytesInTebiByte, ByteSize.FromTebiBytes(2).Bytes);
        Assert.Equal(2 * ByteSize.BytesInPebiByte, ByteSize.FromPebiBytes(2).Bytes);
        Assert.Equal(2 * ByteSize.BytesInExabiByte, ByteSize.FromExabiBytes(2).Bytes);
    }

    [Fact]
    public void AllBinaryAddMethods_AreCorrect()
    {
        var b = ByteSize.FromBytes(0);
        Assert.Equal(ByteSize.FromKibiBytes(1).Bytes, b.AddKibiBytes(1).Bytes);
        Assert.Equal(ByteSize.FromMebiBytes(1).Bytes, b.AddMebiBytes(1).Bytes);
        Assert.Equal(ByteSize.FromGibiBytes(1).Bytes, b.AddGibiBytes(1).Bytes);
        Assert.Equal(ByteSize.FromTebiBytes(1).Bytes, b.AddTebiBytes(1).Bytes);
        Assert.Equal(ByteSize.FromPebiBytes(1).Bytes, b.AddPebiBytes(1).Bytes);
        Assert.Equal(ByteSize.FromExabiBytes(1).Bytes, b.AddExabiBytes(1).Bytes);
    }

    [Fact]
    public void ToBinaryString_FormatsCorrectly()
    {
        var b = ByteSize.FromGibiBytes(1.5);
        var str = b.ToBinaryString(CultureInfo.InvariantCulture);
        Assert.Contains("1.5 GiB", str);
    }
}
