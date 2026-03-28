using System.Globalization;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Encoders;

public class UpcAEncoderTests
{
    [Fact]
    public void Encode_11Digits_AppendsChecksum()
    {
        var options = new UpcAOptions
        {
            ModuleWidth = 1,
            ModuleHeight = 10
        };

        var data = "03600029145";
        var checksum = UpcAEncoder.ComputeChecksum(data);

        var payload = UpcAEncoder.Instance.Encode(data, options);

        Assert.Equal(data + checksum.ToString(CultureInfo.InvariantCulture), payload.Value);
        Assert.Equal(12, payload.Texts.Count);
        Assert.NotEmpty(payload.Bars);
    }

    [Fact]
    public void Encode_InvalidLength_Throws()
    {
        var options = new UpcAOptions
        {
            ModuleWidth = 1,
            ModuleHeight = 10
        };

        Assert.Throws<ArgumentException>(() => UpcAEncoder.Instance.Encode("1234567890", options));
    }
}
