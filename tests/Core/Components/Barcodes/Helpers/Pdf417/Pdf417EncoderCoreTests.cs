using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Helpers.Pdf417;
using Xunit;

namespace Components.Tests.Components.Barcodes.Helpers.Pdf417;

public class Pdf417EncoderCoreTests
{
    [Fact]
    public void EncodeData_ReturnsCodewords()
    {
        var codewords = Pdf417EncoderCore.EncodeData("1234567890123");

        Assert.True(codewords.Count > 1);
        Assert.Equal(0, codewords[0]);
    }

    [Fact]
    public void BuildFullCodewordArray_SetsLengthAndDataCount()
    {
        var data = new List<int> { 0, 100 };

        var full = Pdf417EncoderCore.BuildFullCodewordArray(
            data,
            rows: 3,
            columns: 3,
            level: PDF417ErrorCorrectionLevel.Level0);

        Assert.Equal(9, full.Length);
        Assert.Equal(7, full[0]);
    }
}
