namespace FluentUI.Blazor.Community.Components;

internal sealed class BackgroundAdapter
    : ISurfaceRenderer<SignatureRenderingOptions>
{
    private readonly ISurfaceRenderer<SurfaceBackgroundOptions> _inner;

    public BackgroundAdapter(ISurfaceRenderer<SurfaceBackgroundOptions> inner)
    {
        _inner = inner;
    }

    public void Render(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.Render(target, options.Background);

    public ValueTask RenderAsync(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.RenderAsync(target, options.Background);
}

