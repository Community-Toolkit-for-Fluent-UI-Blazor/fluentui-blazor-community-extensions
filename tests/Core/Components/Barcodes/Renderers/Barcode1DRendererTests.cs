using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Renderers;

public class Barcode1DRendererTests
{
    [Fact]
    public void Render_AddsBarcodeLayer()
    {
        var encoder = new Test1DEncoder();
        var composer = new Test1DComposer();
        var renderer = new Barcode1DComposer<TestOptions>(encoder, () => "DATA", new TestOptions(), composer);
        var target = new TestSurfaceRenderTarget();

        renderer.Compose(target, new BarcodeRenderingOptions());

        Assert.Single(target.Layers);
        Assert.IsType<BarcodeLayer<Barcode1DPayload>>(target.Layers[0]);
        Assert.True(composer.WasCalled);
    }

    [Fact]
    public void Render_NullTarget_Throws()
    {
        var renderer = new Barcode1DComposer<TestOptions>(new Test1DEncoder(), () => "DATA", new TestOptions());

        Assert.Throws<ArgumentNullException>(() => renderer.Compose(null!, new BarcodeRenderingOptions()));
    }

    [Fact]
    public void RenderAsync_Completes()
    {
        var renderer = new Barcode1DComposer<TestOptions>(new Test1DEncoder(), () => "DATA", new TestOptions());
        var target = new TestSurfaceRenderTarget();

        var task = renderer.ComposeAsync(target, new BarcodeRenderingOptions());

        Assert.True(task.IsCompleted);
        Assert.Single(target.Layers);
    }

    private sealed class TestOptions
    {
    }

    private sealed class Test1DEncoder : IBarcodeEncoder<Barcode1DPayload, TestOptions>
    {
        public Barcode1DPayload Encode(string data, TestOptions options)
        {
            return new Barcode1DPayload
            {
                Value = data,
                Bars = [new Barcode1DBar { X = 0, Y = 0, Width = 1, Height = 2 }]
            };
        }
    }

    private sealed class Test1DComposer : Barcode1DLayerComposer<TestOptions>
    {
        public bool WasCalled { get; private set; }

        protected override void AfterCompose(
            BarcodePayload<Barcode1DPayload> payload,
            Barcode1DPayload data,
            BarcodeRenderingOptions options,
            TestOptions barcodeOptions)
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
