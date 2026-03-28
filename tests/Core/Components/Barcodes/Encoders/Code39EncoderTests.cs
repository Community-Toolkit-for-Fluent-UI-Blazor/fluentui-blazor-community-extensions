using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Encoders;

public class Code39EncoderTests
{
    [Fact]
    public void Encode_WithChecksum_AppendsChecksumAndStartStop()
    {
        var options = new Code39Options
        {
            EnableChecksum = true,
            ModuleWidth = 1,
            ModuleHeight = 10,
            WideRatio = 3
        };

        var payload = Code39Encoder.Instance.Encode("ABC", options);

        Assert.Equal("*ABCX*", payload.Value);
        Assert.Single(payload.Texts);
        Assert.NotEmpty(payload.Bars);
    }

    [Fact]
    public void Encode_InvalidCharacter_Throws()
    {
        var options = new Code39Options
        {
            ModuleWidth = 1,
            ModuleHeight = 10
        };

        Assert.Throws<ArgumentException>(() => Code39Encoder.Instance.Encode("abc", options));
    }
}
