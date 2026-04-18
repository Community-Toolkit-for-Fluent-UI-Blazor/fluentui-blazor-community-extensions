namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an adapter that allows a surface composer designed for signature hover rendering options to be used in contexts where signature rendering options are expected.
/// </summary>
internal class HoverStrokeAdapter : ISurfaceComposer<SignatureRenderingOptions>
{
    /// <summary>
    /// Represents the inner surface composer that this adapter wraps.
    /// </summary>
    private readonly ISurfaceComposer<SignatureHoverRenderingOptions> _inner;

    /// <summary>
    /// initializes a new instance of the <see cref="HoverStrokeAdapter"/> class.
    /// </summary>
    /// <param name="inner">The inner surface composer to adapt.</param>
    public HoverStrokeAdapter(ISurfaceComposer<SignatureHoverRenderingOptions> inner)
    {
        _inner = inner;
    }

    /// <inheritdoc />
    public void Compose(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.Compose(target, options.Hover);

    /// <inheritdoc />
    public ValueTask ComposeAsync(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.ComposeAsync(target, options.Hover);
}

