namespace FluentUI.Blazor.Community.Components;

internal class HoverStrokeAdapter : ISurfaceRenderer<SignatureRenderingOptions>
{
    private readonly ISurfaceRenderer<SignatureHoverRenderingOptions> _inner;

    public HoverStrokeAdapter(ISurfaceRenderer<SignatureHoverRenderingOptions> inner)
    {
        _inner = inner;
    }

    public void Render(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.Render(target, options.Hover);

    public ValueTask RenderAsync(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.RenderAsync(target, options.Hover);
}

