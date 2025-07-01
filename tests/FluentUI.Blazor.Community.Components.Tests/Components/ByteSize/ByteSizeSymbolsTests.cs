namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ByteSizeSymbolsTests
{
    [Fact]
    public void Constants_AreCorrect()
    {
        Assert.Equal(1_000, ByteSize.BytesInKiloByte);
        Assert.Equal(1_000_000, ByteSize.BytesInMegaByte);
        Assert.Equal(1_000_000_000, ByteSize.BytesInGigaByte);
        Assert.Equal(1_000_000_000_000, ByteSize.BytesInTeraByte);
        Assert.Equal(1_000_000_000_000_000, ByteSize.BytesInPetaByte);
        Assert.Equal(1_000_000_000_000_000_000, ByteSize.BytesInExaByte);
        Assert.Equal("KB", ByteSize.KiloByteSymbol);
        Assert.Equal("MB", ByteSize.MegaByteSymbol);
        Assert.Equal("GB", ByteSize.GigaByteSymbol);
        Assert.Equal("TB", ByteSize.TeraByteSymbol);
        Assert.Equal("PB", ByteSize.PetaByteSymbol);
        Assert.Equal("EB", ByteSize.ExaByteSymbol);
    }

    [Fact]
    public void Properties_ConvertCorrectly()
    {
        var b = ByteSize.FromBytes(1_000_000_000_000);
        Assert.Equal(1_000_000_000_000 / 1_000.0, b.KiloBytes);
        Assert.Equal(1_000_000_000_000 / 1_000_000.0, b.MegaBytes);
        Assert.Equal(1_000_000_000_000 / 1_000_000_000.0, b.GigaBytes);
    }

    [Fact]
    public void AllDecimalProperties_AreCorrect()
    {
        var bytes = 1_000_000_000_000_000_000L; // 1 EB
        var b = ByteSize.FromBytes(bytes);
        Assert.Equal(bytes / (double)ByteSize.BytesInKiloByte, b.KiloBytes);
        Assert.Equal(bytes / (double)ByteSize.BytesInMegaByte, b.MegaBytes);
        Assert.Equal(bytes / (double)ByteSize.BytesInGigaByte, b.GigaBytes);
        Assert.Equal(bytes / (double)ByteSize.BytesInTeraByte, b.TeraBytes);
        Assert.Equal(bytes / (double)ByteSize.BytesInPetaByte, b.PetaBytes);
        Assert.Equal(bytes / (double)ByteSize.BytesInExaByte, b.ExaBytes);
    }

    [Fact]
    public void AllDecimalFactories_AreCorrect()
    {
        Assert.Equal(2 * ByteSize.BytesInKiloByte, ByteSize.FromKiloBytes(2).Bytes);
        Assert.Equal(2 * ByteSize.BytesInMegaByte, ByteSize.FromMegaBytes(2).Bytes);
        Assert.Equal(2 * ByteSize.BytesInGigaByte, ByteSize.FromGigaBytes(2).Bytes);
        Assert.Equal(2 * ByteSize.BytesInTeraByte, ByteSize.FromTeraBytes(2).Bytes);
        Assert.Equal(2 * ByteSize.BytesInPetaByte, ByteSize.FromPetaBytes(2).Bytes);
        Assert.Equal(2 * ByteSize.BytesInExaByte, ByteSize.FromExaBytes(2).Bytes);
    }

    [Fact]
    public void AllDecimalAddMethods_AreCorrect()
    {
        var b = ByteSize.FromBytes(0);
        Assert.Equal(ByteSize.FromKiloBytes(1).Bytes, b.AddKiloBytes(1).Bytes);
        Assert.Equal(ByteSize.FromMegaBytes(1).Bytes, b.AddMegaBytes(1).Bytes);
        Assert.Equal(ByteSize.FromGigaBytes(1).Bytes, b.AddGigaBytes(1).Bytes);
        Assert.Equal(ByteSize.FromTeraBytes(1).Bytes, b.AddTeraBytes(1).Bytes);
        Assert.Equal(ByteSize.FromPetaBytes(1).Bytes, b.AddPetaBytes(1).Bytes);
        Assert.Equal(ByteSize.FromExaBytes(1).Bytes, b.AddExaBytes(1).Bytes);
    }
}
