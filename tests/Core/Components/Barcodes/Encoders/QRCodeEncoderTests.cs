using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Encoders;

public class QRCodeEncoderTests
{
    [Fact]
    public void Encode_BuildsPayload()
    {
        var options = new QRCodeOptions
        {
            ModuleSize = 1
        };

        var payload = QRCodeEncoder.Instance.Encode("HELLO", options);

        Assert.Equal("HELLO", payload.Value);
        Assert.True(payload.Rows > 0);
        Assert.True(payload.Columns > 0);
        Assert.NotEmpty(payload.Modules);
    }
}
