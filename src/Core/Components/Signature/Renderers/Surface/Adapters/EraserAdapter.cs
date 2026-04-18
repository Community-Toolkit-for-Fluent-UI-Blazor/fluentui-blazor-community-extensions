namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Adapts a surface composer that operates on eraser options to work with signature engine options.
/// </summary>
/// <remarks>This adapter enables the use of an ISurfaceComposer implementation designed for
/// SignatureEraserOptions in contexts where SignatureEngineOptions are required. It extracts the eraser-specific
/// options from the engine options and delegates composition operations to the inner composer.</remarks>
internal class EraserAdapter : ISurfaceComposer<SignatureEngineOptions>
{
    /// <summary>
    /// Represents the inner surface composer.
    /// </summary>
    private readonly ISurfaceComposer<SignatureEraserOptions> _inner;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="EraserAdapter"/> class.
    /// </summary>
    /// <param name="inner">The inner surface composer to adapt.</param>
    public EraserAdapter(ISurfaceComposer<SignatureEraserOptions> inner)
    {
        _inner = inner;
    }

    /// <inheritdoc />
    public void Compose(ISurfaceRenderTarget target, SignatureEngineOptions options)
        => _inner.Compose(target, options.Eraser);

    /// <inheritdoc />
    public ValueTask ComposeAsync(ISurfaceRenderTarget target, SignatureEngineOptions options)
        => _inner.ComposeAsync(target, options.Eraser);
}

