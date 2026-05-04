using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Encoders;

public class CodabarEncoderTests
{
    [Fact]
    public void Encode_UppercasesAndAddsText()
    {
        var options = new CodabarOptions
        {
            ModuleWidth = 1,
            ModuleHeight = 10,
            WideRatio = 2,
            ShowText = true
        };

        var payload = CodabarEncoder.Instance.Encode("a12b", options);

        Assert.Equal("A12B", payload.Value);
        Assert.Single(payload.Texts);
        Assert.NotEmpty(payload.Bars);
    }

    [Fact]
    public void Encode_MissingStartStop_Throws()
    {
        var options = new CodabarOptions
        {
            ModuleWidth = 1,
            ModuleHeight = 10,
            WideRatio = 2
        };

        Assert.Throws<ArgumentException>(() => CodabarEncoder.Instance.Encode("1234", options));
    }
}
