using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Encoders;

public class Pdf417EncoderTests
{
    [Fact]
    public void Encode_BuildsStackedPayload()
    {
        var options = new Pdf417Options
        {
            ModuleWidth = 1,
            ModuleHeight = 2
        };

        var payload = Pdf417Encoder.Instance.Encode("PDF417", options);

        Assert.Equal("PDF417", payload.Value);
        Assert.True(payload.Rows > 0);
        Assert.True(payload.Columns > 0);
        Assert.NotEmpty(payload.Modules);
    }
}
