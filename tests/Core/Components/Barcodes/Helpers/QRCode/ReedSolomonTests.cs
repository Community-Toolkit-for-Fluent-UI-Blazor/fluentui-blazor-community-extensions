using FluentUI.Blazor.Community.Components.Helpers;
using Xunit;

namespace Components.Tests.Components.Barcodes.Helpers.QRCode;

public class ReedSolomonTests
{
    [Fact]
    public void GenerateECC_ReturnsExpectedLength()
    {
        var ecc = ReedSolomon.GenerateECC([1, 2, 3], 7);

        Assert.Equal(7, ecc.Length);
    }

    [Fact]
    public void GenerateECC_InvalidCount_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ReedSolomon.GenerateECC([1], 0));
    }
}
