namespace FluentUI.Blazor.Community.Components;

internal class SelectionStrokeAdapter : ISurfaceRenderer<SignatureRenderingOptions>
{
    private readonly ISurfaceRenderer<SignatureSelectionRenderingOptions> _inner;

    public SelectionStrokeAdapter(ISurfaceRenderer<SignatureSelectionRenderingOptions> inner)
    {
        _inner = inner;
    }

    public void Render(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.Render(target, options.Selection);

    public ValueTask RenderAsync(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.RenderAsync(target, options.Selection);
}

