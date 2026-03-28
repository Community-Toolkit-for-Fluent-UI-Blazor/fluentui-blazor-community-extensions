using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Encoders;

public class Code128EncoderTests
{
    [Fact]
    public void Encode_AutoSubsetC_BuildsPayload()
    {
        var options = new Code128Options
        {
            Subset = Code128Subset.Auto,
            ModuleWidth = 1,
            ModuleHeight = 10,
            ShowText = true
        };

        var payload = Code128Encoder.Instance.Encode("1234", options);

        Assert.Equal("1234", payload.Value);
        Assert.Single(payload.Texts);
        Assert.NotEmpty(payload.Bars);
    }

    [Fact]
    public void Encode_SubsetCWithOddLength_Throws()
    {
        var options = new Code128Options
        {
            Subset = Code128Subset.C,
            ModuleWidth = 1,
            ModuleHeight = 10
        };

        Assert.Throws<ArgumentException>(() => Code128Encoder.Instance.Encode("123", options));
    }
}
