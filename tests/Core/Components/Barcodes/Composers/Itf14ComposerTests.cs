using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Composers;

public class Itf14ComposerTests
{
    [Fact]
    public void Compose_AddsBearerBarsAndUpdatesDimensions()
    {
        var data = new Barcode1DPayload
        {
            Bars =
            [
                new Barcode1DBar { X = 0, Y = 0, Width = 2, Height = 30 },
                new Barcode1DBar { X = 2, Y = 0, Width = 2, Height = 30 }
            ]
        };

        var options = new BarcodeRenderingOptions
        {
            Label = new BarcodeLabelOptions { Enabled = false },
            QuietZone = new BarcodeQuietZoneOptions { Enabled = false }
        };

        var composer = new Itf14Composer();

        var payload = composer.Compose(data, options, new Itf14Options());

        Assert.Equal(24, payload.Width);
        Assert.Equal(40, payload.Height);
        Assert.Equal(4, payload.Shapes.Count);
        Assert.Equal(0, payload.Shapes[0].X);
        Assert.Equal(0, payload.Shapes[0].Y);
        Assert.Equal(24, payload.Shapes[0].Width);
        Assert.Equal(5, payload.Shapes[0].Height);
        Assert.Equal(19, payload.Shapes[3].X);
        Assert.Equal(40, payload.Shapes[3].Height);
    }
}
