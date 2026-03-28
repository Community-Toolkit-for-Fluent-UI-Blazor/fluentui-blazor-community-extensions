namespace FluentUI.Blazor.Community.Components;

internal class GridAdapter : ISurfaceRenderer<SignatureRenderingOptions>
{
    private readonly ISurfaceRenderer<SurfaceGridOptions> _inner;

    public GridAdapter(ISurfaceRenderer<SurfaceGridOptions> inner)
    {
        _inner = inner;
    }

    public void Render(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.Render(target, options.Grid);

    public ValueTask RenderAsync(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.RenderAsync(target, options.Grid);
}

