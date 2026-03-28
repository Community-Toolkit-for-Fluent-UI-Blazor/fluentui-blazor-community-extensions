using System.Globalization;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Encoders;

public class Itf14EncoderTests
{
    [Fact]
    public void Encode_13Digits_AppendsChecksum()
    {
        var options = new Itf14Options
        {
            ModuleWidth = 1,
            ModuleHeight = 10,
            ShowText = true
        };

        var data = "1234567890123";
        var checksum = ItfTools.ComputeChecksum(data);

        var payload = Itf14Encoder.Instance.Encode(data, options);

        Assert.Equal(data + checksum.ToString(CultureInfo.InvariantCulture), payload.Value);
        Assert.Single(payload.Texts);
        Assert.NotEmpty(payload.Bars);
    }

    [Fact]
    public void Encode_InvalidCharacter_Throws()
    {
        var options = new Itf14Options
        {
            ModuleWidth = 1,
            ModuleHeight = 10
        };

        Assert.Throws<ArgumentException>(() => Itf14Encoder.Instance.Encode("123ABC", options));
    }
}
