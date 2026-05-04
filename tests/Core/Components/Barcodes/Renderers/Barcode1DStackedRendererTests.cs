using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Renderers;

public class Barcode1DStackedRendererTests
{
    [Fact]
    public void Render_AddsBarcodeLayer()
    {
        var encoder = new TestStackedEncoder();
        var composer = new TestStackedComposer();
        var renderer = new Barcode1DStackedComposer<TestStackedOptions>(encoder, () => "DATA", new TestStackedOptions(), composer);
        var target = new TestSurfaceRenderTarget();

        renderer.Compose(target, new BarcodeRenderingOptions());

        Assert.Single(target.Layers);
        Assert.IsType<BarcodeLayer<Barcode1DStackedPayload>>(target.Layers[0]);
        Assert.True(composer.WasCalled);
    }

    [Fact]
    public void RenderAsync_Completes()
    {
        var renderer = new Barcode1DStackedComposer<TestStackedOptions>(new TestStackedEncoder(), () => "DATA", new TestStackedOptions());
        var target = new TestSurfaceRenderTarget();

        var task = renderer.ComposeAsync(target, new BarcodeRenderingOptions());

        Assert.True(task.IsCompleted);
        Assert.Single(target.Layers);
    }

    private sealed class TestStackedOptions : IBarcode1DStackedOptions
    {
        public double ModuleWidth => 1;
        public double ModuleHeight => 1;
    }

    private sealed class TestStackedEncoder : IBarcodeEncoder<Barcode1DStackedPayload, TestStackedOptions>
    {
        public Barcode1DStackedPayload Encode(string data, TestStackedOptions options)
        {
            return new Barcode1DStackedPayload
            {
                Value = data,
                Rows = 1,
                Columns = 1,
                Modules = [new BarcodeRectangle(0, 0, 1, 1)]
            };
        }
    }

    private sealed class TestStackedComposer : Barcode1DStackedLayerComposer<TestStackedOptions>
    {
        public bool WasCalled { get; private set; }

        protected override void AfterCompose(
            BarcodePayload<Barcode1DStackedPayload> payload,
            Barcode1DStackedPayload data,
            BarcodeRenderingOptions options,
            TestStackedOptions barcodeOptions)
        {
            WasCalled = true;
        }
    }

    private sealed class TestSurfaceRenderTarget : ISurfaceRenderTarget
    {
        public List<ILayer> Layers { get; } = [];

        public ViewPayload View { get; } = new();

        public void SetView(ViewPayload view)
        {
        }

        public ValueTask FlushAsync() => ValueTask.CompletedTask;

        public object? GetNativeHandle() => null;

        public ISurfaceRenderTarget AddLayer(ILayer layer)
        {
            Layers.Add(layer);
            return this;
        }
    }
}
