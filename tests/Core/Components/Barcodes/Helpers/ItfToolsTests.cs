using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Helpers;

public class ItfToolsTests
{
    [Fact]
    public void Expand_ReplacesNarrowAndWide()
    {
        var result = ItfTools.Expand("NWW");

        Assert.Equal("1111111", result);
    }

    [Fact]
    public void BuildInterleavedPair_ReturnsBinaryPattern()
    {
        var pattern = ItfTools.BuildInterleavedPair(0, 1);

        Assert.Equal(18, pattern.Length);
        Assert.All(pattern, c => Assert.True(c is '0' or '1'));
    }

    [Fact]
    public void ComputeChecksum_ReturnsExpectedDigit()
    {
        var checksum = ItfTools.ComputeChecksum("1234567890123");

        Assert.Equal(1, checksum);
    }
}
