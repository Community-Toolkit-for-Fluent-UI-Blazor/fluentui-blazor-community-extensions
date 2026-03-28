namespace FluentUI.Blazor.Community.Components;

internal class AxesAdapter : ISurfaceRenderer<SignatureRenderingOptions>
{
    private readonly ISurfaceRenderer<SurfaceAxesOptions> _inner;

    public AxesAdapter(ISurfaceRenderer<SurfaceAxesOptions> inner)
    {
        _inner = inner;
    }

    public void Render(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.Render(target, options.Axes);

    public ValueTask RenderAsync(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.RenderAsync(target, options.Axes);
}

