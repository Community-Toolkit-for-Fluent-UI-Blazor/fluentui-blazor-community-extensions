using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Encoders;

public class Code93EncoderTests
{
    [Fact]
    public void Encode_StandardMode_BuildsPayload()
    {
        var options = new Code93Options
        {
            ModuleWidth = 1,
            ModuleHeight = 10,
            ShowText = true
        };

        var payload = Code93Encoder.Instance.Encode("ABC", options);

        Assert.Equal("ABC", payload.Value);
        Assert.Single(payload.Texts);
        Assert.NotEmpty(payload.Bars);
    }

    [Fact]
    public void Encode_InvalidCharacter_Throws()
    {
        var options = new Code93Options
        {
            ModuleWidth = 1,
            ModuleHeight = 10
        };

        Assert.Throws<ArgumentException>(() => Code93Encoder.Instance.Encode("a", options));
    }
}
