using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Encoders;

public class Gs1_128EncoderTests
{
    [Fact]
    public void Encode_PermissiveValue_BuildsPayload()
    {
        var options = new Gs1_128Options
        {
            ModuleWidth = 1,
            ModuleHeight = 10,
            ShowText = true,
            ValidationMode = Gs1ValidationMode.Permissive
        };

        var payload = Gs1_128Encoder.Instance.Encode("ABC123", options);

        Assert.Equal("ABC123", payload.Value);
        Assert.Single(payload.Texts);
        Assert.NotEmpty(payload.Bars);
    }

    [Fact]
    public void Encode_EmptyData_Throws()
    {
        var options = new Gs1_128Options
        {
            ModuleWidth = 1,
            ModuleHeight = 10,
            ValidationMode = Gs1ValidationMode.Permissive
        };

        Assert.Throws<ArgumentException>(() => Gs1_128Encoder.Instance.Encode(" ", options));
    }
}
