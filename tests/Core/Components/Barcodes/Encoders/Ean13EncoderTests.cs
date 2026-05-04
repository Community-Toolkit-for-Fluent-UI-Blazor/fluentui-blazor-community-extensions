using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Encoders;

public class Ean13EncoderTests
{
    [Fact]
    public void Encode_12Digits_AppendsChecksum()
    {
        var options = new Ean13Options
        {
            ModuleWidth = 1,
            ModuleHeight = 10
        };

        var payload = Ean13Encoder.Instance.Encode("590123412345", options);

        Assert.Equal("5901234123457", payload.Value);
        Assert.Equal(13, payload.Texts.Count);
        Assert.NotEmpty(payload.Bars);
    }

    [Fact]
    public void Encode_InvalidLength_Throws()
    {
        var options = new Ean13Options
        {
            ModuleWidth = 1,
            ModuleHeight = 10
        };

        Assert.Throws<ArgumentException>(() => Ean13Encoder.Instance.Encode("123", options));
    }
}
