using FluentUI.Blazor.Community.Components.Helpers.Pdf417;
using Xunit;

namespace Components.Tests.Components.Barcodes.Helpers.Pdf417;

public class Pdf417ReedSolomonTests
{
    [Fact]
    public void GetECCWordCount_ReturnsExpectedValues()
    {
        Assert.Equal(2, Pdf417ReedSolomon.GetECCWordCount(FluentUI.Blazor.Community.Components.Enums.PDF417ErrorCorrectionLevel.Level0));
    }

    [Fact]
    public void GenerateECC_ReturnsExpectedLength()
    {
        var data = new[] { 3, 1, 2, 3 };

        var ecc = Pdf417ReedSolomon.GenerateECC(data, 2, 0);

        Assert.Equal(2, ecc.Length);
        Assert.All(ecc, value => Assert.InRange(value, 0, 928));
    }
}
