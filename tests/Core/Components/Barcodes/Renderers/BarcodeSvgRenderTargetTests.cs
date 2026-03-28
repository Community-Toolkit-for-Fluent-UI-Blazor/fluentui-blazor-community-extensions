using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Renderers;

public class BarcodeSvgRenderTargetTests
{
    [Fact]
    public void AddLayer_SecondLayer_Throws()
    {
        var target = new BarcodeSvgRenderTarget();
        var payload = Build1DPayload();

        target.AddLayer(new BarcodeLayer<Barcode1DPayload>(payload));

        Assert.Throws<InvalidOperationException>(() => target.AddLayer(new BarcodeLayer<Barcode1DPayload>(payload)));
    }

    [Fact]
    public void AddLayer_UnsupportedLayer_Throws()
    {
        var target = new BarcodeSvgRenderTarget();

        Assert.Throws<InvalidOperationException>(() => target.AddLayer(new FakeLayer()));
    }

    [Fact]
    public async Task FlushAsync_RendersSvgWithViewBox()
    {
        var target = new BarcodeSvgRenderTarget();
        var payload = Build1DPayload();

        target.AddLayer(new BarcodeLayer<Barcode1DPayload>(payload));

        await target.FlushAsync();

        Assert.Contains("viewBox=\"0 0 10 4\"", target.Svg.Value);
    }

    [Fact]
    public async Task FlushAsync_UsesWidthFor2DHeight()
    {
        var target = new BarcodeSvgRenderTarget();
        var payload = new BarcodePayload<Barcode2DPayload>
        {
            Width = 10,
            Height = 20,
            Data = new Barcode2DPayload
            {
                Rows = 1,
                Columns = 1
            }
        };
        payload.Shapes.Add(new BarcodeRectangle(0, 0, 1, 1));

        target.AddLayer(new BarcodeLayer<Barcode2DPayload>(payload));

        await target.FlushAsync();

        Assert.Contains("viewBox=\"0 0 10 10\"", target.Svg.Value);
    }

    private static BarcodePayload<Barcode1DPayload> Build1DPayload()
    {
        return new BarcodePayload<Barcode1DPayload>
        {
            Width = 10,
            Height = 4,
            Data = new Barcode1DPayload
            {
                Bars = [new Barcode1DBar { X = 0, Y = 0, Width = 1, Height = 2 }]
            }
        };
    }

    private sealed class FakeLayer : ILayer
    {
        public string Key => "fake";

        public LayerOrder Order => LayerOrder.Content;

        public LayerPriority Priority => LayerPriority.Normal;

        public ILayerPayload LayerPayload { get; } = new FakePayload();
    }

    private sealed class FakePayload : ILayerPayload
    {
    }
}
