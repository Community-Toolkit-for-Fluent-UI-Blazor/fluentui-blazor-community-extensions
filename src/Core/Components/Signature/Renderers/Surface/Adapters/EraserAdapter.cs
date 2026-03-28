namespace FluentUI.Blazor.Community.Components;

internal class EraserAdapter : ISurfaceRenderer<SignatureEngineOptions>
{
    private readonly ISurfaceRenderer<SignatureEraserOptions> _inner;

    public EraserAdapter(ISurfaceRenderer<SignatureEraserOptions> inner)
    {
        _inner = inner;
    }

    public void Render(ISurfaceRenderTarget target, SignatureEngineOptions options)
        => _inner.Render(target, options.Eraser);

    public ValueTask RenderAsync(ISurfaceRenderTarget target, SignatureEngineOptions options)
        => _inner.RenderAsync(target, options.Eraser);
}

