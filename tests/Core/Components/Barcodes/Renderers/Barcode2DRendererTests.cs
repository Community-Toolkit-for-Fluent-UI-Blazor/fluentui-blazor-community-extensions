using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Renderers;

public class Barcode2DRendererTests
{
    [Fact]
    public void Render_AddsBarcodeLayer()
    {
        var encoder = new Test2DEncoder();
        var composer = new Test2DComposer();
        var renderer = new Barcode2DRenderer<Test2DOptions>(encoder, () => "DATA", new Test2DOptions(), composer);
        var target = new TestSurfaceRenderTarget();

        renderer.Render(target, new BarcodeRenderingOptions());

        Assert.Single(target.Layers);
        Assert.IsType<BarcodeLayer<Barcode2DPayload>>(target.Layers[0]);
        Assert.True(composer.WasCalled);
    }

    [Fact]
    public void RenderAsync_Completes()
    {
        var renderer = new Barcode2DRenderer<Test2DOptions>(new Test2DEncoder(), () => "DATA", new Test2DOptions());
        var target = new TestSurfaceRenderTarget();

        var task = renderer.RenderAsync(target, new BarcodeRenderingOptions());

        Assert.True(task.IsCompleted);
        Assert.Single(target.Layers);
    }

    private sealed class Test2DOptions : IBarcode2DOptions
    {
        public double ModuleSize => 1;
    }

    private sealed class Test2DEncoder : IBarcodeEncoder<Barcode2DPayload, Test2DOptions>
    {
        public Barcode2DPayload Encode(string data, Test2DOptions options)
        {
            return new Barcode2DPayload
            {
                Value = data,
                Rows = 1,
                Columns = 1,
                Modules = [new BarcodeRectangle(0, 0, 1, 1)]
            };
        }
    }

    private sealed class Test2DComposer : Barcode2DComposer<Test2DOptions>
    {
        public bool WasCalled { get; private set; }

        protected override void AfterCompose(
            BarcodePayload<Barcode2DPayload> payload,
            Barcode2DPayload data,
            BarcodeRenderingOptions options,
            Test2DOptions barcodeOptions)
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
