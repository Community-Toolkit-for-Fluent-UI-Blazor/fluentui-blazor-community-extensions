using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Composers;

public class Barcode2DComposerTests
{
    [Fact]
    public void Compose_AddsModuleShapesAndSetsLabelPosition()
    {
        var data = new Barcode2DPayload
        {
            Rows = 2,
            Columns = 3,
            Modules =
            [
                new BarcodeRectangle(0, 0, 1, 1),
                new BarcodeRectangle(1, 0, 1, 2)
            ],
            Texts =
            [
                new BarcodeText { X = 1, Y = 0, Text = "QR" }
            ]
        };

        var options = new BarcodeRenderingOptions
        {
            Label = new BarcodeLabelOptions
            {
                Enabled = true,
                FontSize = 5,
                OffsetY = 3
            }
        };

        var composer = new Barcode2DLayerComposer<Test2DOptions>();

        var payload = composer.Compose(data, options, new Test2DOptions());

        Assert.Equal(22, payload.Width);
        Assert.Equal(28, payload.Height);
        Assert.Equal(2, payload.Shapes.Count);
        Assert.Equal(8, payload.Shapes[0].X);
        Assert.Equal(8, payload.Shapes[0].Y);
        Assert.Equal(3, payload.Data.Texts[0].Y);
    }

    private sealed class Test2DOptions : IBarcode2DOptions
    {
        public double ModuleSize { get; } = 2;
    }
}
