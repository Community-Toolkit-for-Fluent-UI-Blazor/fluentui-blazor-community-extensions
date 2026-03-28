using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Composers;

public class Barcode1DStackedComposerTests
{
    [Fact]
    public void Compose_AddsModuleShapesAndUpdatesLabelPosition()
    {
        var data = new Barcode1DStackedPayload
        {
            Rows = 2,
            Columns = 3,
            Modules =
            [
                new BarcodeRectangle(0, 0, 1, 1),
                new BarcodeRectangle(1, 0, 2, 1)
            ],
            Texts =
            [
                new BarcodeText { X = 1.5, Y = 0, Text = "STACK", Anchor = SvgTextAnchor.Middle }
            ]
        };

        var options = new BarcodeRenderingOptions
        {
            QuietZone = new BarcodeQuietZoneOptions
            {
                Padding = new Thickness(1, 2, 3, 4)
            },
            Label = new BarcodeLabelOptions
            {
                Enabled = true,
                FontSize = 10,
                OffsetY = 5
            }
        };

        var composer = new Barcode1DStackedComposer<TestStackedOptions>();

        var payload = composer.Compose(data, options, new TestStackedOptions());

        Assert.Equal(10, payload.Width);
        Assert.Equal(13, payload.Height);
        Assert.Equal(2, payload.Shapes.Count);
        Assert.Equal(1, payload.Shapes[0].X);
        Assert.Equal(2, payload.Shapes[0].Y);
        Assert.Equal(2, payload.Shapes[0].Width);
        Assert.Equal(3, payload.Shapes[0].Height);
        Assert.Equal(7, payload.Data.Texts[0].Y);
    }

    private sealed class TestStackedOptions : IBarcode1DStackedOptions
    {
        public double ModuleWidth { get; } = 2;
        public double ModuleHeight { get; } = 3;
    }
}
