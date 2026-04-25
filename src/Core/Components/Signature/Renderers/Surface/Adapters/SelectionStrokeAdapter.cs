namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an adapter that allows a surface composer for signature selection rendering options to be used as a surface composer for signature rendering options.
/// </summary>
internal class SelectionStrokeAdapter : ISurfaceComposer<SignatureRenderingOptions>
{
    /// <summary>
    /// Represents the inner surface composer that handles selection rendering options.
    /// </summary>
    private readonly ISurfaceComposer<SignatureSelectionRenderingOptions> _inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectionStrokeAdapter"/> class.
    /// </summary>
    /// <param name="inner">The inner surface composer that handles selection rendering options.</param>
    public SelectionStrokeAdapter(ISurfaceComposer<SignatureSelectionRenderingOptions> inner)
    {
        _inner = inner;
    }

    /// <inheritdoc />
    public bool Compose(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.Compose(target, options.Selection);

    /// <inheritdoc />
    public ValueTask<bool> ComposeAsync(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.ComposeAsync(target, options.Selection);
}

