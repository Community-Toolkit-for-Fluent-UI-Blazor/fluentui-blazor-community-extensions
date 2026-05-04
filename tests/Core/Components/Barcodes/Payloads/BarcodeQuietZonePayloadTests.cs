using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Payloads;

public class BarcodeQuietZonePayloadTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var payload = new BarcodeQuietZonePayload();

        Assert.Equal(0, payload.X);
        Assert.Equal(0, payload.Y);
        Assert.Equal(0, payload.Width);
        Assert.Equal(0, payload.Height);
    }

    [Fact]
    public void InitProperties_PreservesAssignedValues()
    {
        var payload = new BarcodeQuietZonePayload
        {
            X = 1,
            Y = 2,
            Width = 3,
            Height = 4
        };

        Assert.Equal(1, payload.X);
        Assert.Equal(2, payload.Y);
        Assert.Equal(3, payload.Width);
        Assert.Equal(4, payload.Height);
    }
}
