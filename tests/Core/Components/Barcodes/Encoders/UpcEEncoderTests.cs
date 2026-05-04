using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Encoders;

public class UpcEEncoderTests
{
    [Fact]
    public void Encode_ValidData_BuildsPayload()
    {
        var options = new UpcEOptions
        {
            ModuleWidth = 1,
            ModuleHeight = 10,
            NumberSystem = 0
        };

        var payload = UpcEEncoder.Instance.Encode("123456", options);

        Assert.StartsWith("0", payload.Value);
        Assert.Equal(8, payload.Value.Length);
        Assert.Contains("123456", payload.Value);
        Assert.Equal(8, payload.Texts.Count);
        Assert.NotEmpty(payload.Bars);
    }

    [Fact]
    public void Encode_InvalidNumberSystem_Throws()
    {
        var options = new UpcEOptions
        {
            ModuleWidth = 1,
            ModuleHeight = 10,
            NumberSystem = 2
        };

        Assert.Throws<ArgumentException>(() => UpcEEncoder.Instance.Encode("123456", options));
    }
}
