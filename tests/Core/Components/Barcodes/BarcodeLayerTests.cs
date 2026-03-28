using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes;

public class BarcodeLayerTests
{
    [Fact]
    public void Constructor_SetsPayloadAndDefaults()
    {
        var payload = new BarcodePayload<Barcode1DPayload>
        {
            Data = new Barcode1DPayload()
        };

        var layer = new BarcodeLayer<Barcode1DPayload>(payload);

        Assert.Equal("barcode", layer.Key);
        Assert.Equal(LayerOrder.Content, layer.Order);
        Assert.Equal(LayerPriority.Normal, layer.Priority);
        Assert.Same(payload, layer.LayerPayload);
    }
}
