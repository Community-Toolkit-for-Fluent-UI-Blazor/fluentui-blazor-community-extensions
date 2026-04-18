using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Composers;

public class Barcode1DComposerTests
{
    [Fact]
    public void Compose_UpdatesDimensionsAndOffsetsWithShapesAndLabel()
    {
        var data = new Barcode1DPayload
        {
            Bars =
            [
                new Barcode1DBar { X = -2, Y = 0, Width = 2, Height = 10 },
                new Barcode1DBar { X = 2, Y = 0, Width = 3, Height = 8 }
            ],
            Texts =
            [
                new BarcodeText { X = 1, Y = 0, Text = "123", Anchor = SvgTextAnchor.Middle }
            ]
        };

        var options = new BarcodeRenderingOptions
        {
            QuietZone = new BarcodeQuietZoneOptions
            {
                Padding = new Thickness(1)
            },
            Label = new BarcodeLabelOptions
            {
                Enabled = true,
                FontSize = 10,
                OffsetY = 4
            }
        };

        var composer = new TestBarcode1DComposer();

        var payload = composer.Compose(data, options, new TestBarcodeOptions());

        Assert.Equal(10, payload.Width);
        Assert.Equal(17, payload.Height);
        Assert.Equal(1, payload.Data.Bars[0].X);
        Assert.Equal(1, payload.Data.Bars[0].Y);
        Assert.Equal(5, payload.Data.Bars[1].X);
        Assert.Equal(1, payload.Data.Bars[1].Y);
        Assert.Equal(0, payload.Shapes[0].X);
        Assert.Equal(0, payload.Shapes[0].Y);
        Assert.Equal(4, payload.Data.Texts[0].X);
        Assert.Equal(15, payload.Data.Texts[0].Y);
    }

    private sealed class TestBarcodeOptions
    {
    }

    private sealed class TestBarcode1DComposer : Barcode1DLayerComposer<TestBarcodeOptions>
    {
        protected override void AfterCompose(
            BarcodePayload<Barcode1DPayload> payload,
            Barcode1DPayload data,
            BarcodeRenderingOptions options,
            TestBarcodeOptions barcodeOptions)
        {
            payload.Shapes.Add(new BarcodeRectangle(-3, -1, 1, 5));
        }
    }
}
