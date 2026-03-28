namespace FluentUI.Blazor.Community.Components;

internal class WatermarkAdapter : ISurfaceRenderer<SignatureRenderingOptions>
{
    private readonly ISurfaceRenderer<SurfaceWatermarkOptions> _inner;

    public WatermarkAdapter(ISurfaceRenderer<SurfaceWatermarkOptions> inner)
    {
        _inner = inner;
    }

    public void Render(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.Render(target, options.Watermark);

    public ValueTask RenderAsync(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.RenderAsync(target, options.Watermark);
}

