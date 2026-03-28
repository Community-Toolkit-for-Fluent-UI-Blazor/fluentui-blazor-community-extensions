using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Encoders;

public class Ean8EncoderTests
{
    [Fact]
    public void Encode_7Digits_AppendsChecksum()
    {
        var options = new Ean8Options
        {
            ModuleWidth = 1,
            ModuleHeight = 10
        };

        var payload = Ean8Encoder.Instance.Encode("1234567", options);

        Assert.Equal("12345670", payload.Value);
        Assert.Equal(8, payload.Texts.Count);
        Assert.NotEmpty(payload.Bars);
    }

    [Fact]
    public void Encode_InvalidCharacter_Throws()
    {
        var options = new Ean8Options
        {
            ModuleWidth = 1,
            ModuleHeight = 10
        };

        Assert.Throws<ArgumentException>(() => Ean8Encoder.Instance.Encode("1234AB7", options));
    }
}
