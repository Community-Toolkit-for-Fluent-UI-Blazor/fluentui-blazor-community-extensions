namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Adapts a surface composer that operates on background options to work with signature rendering options.
/// </summary>
/// <remarks>This adapter enables the use of an existing surface composer that requires background-specific
/// options in scenarios where signature rendering options are provided. It extracts the background options from the
/// signature rendering options and delegates composition to the inner composer.</remarks>
internal sealed class BackgroundAdapter
    : ISurfaceComposer<SignatureRenderingOptions>
{
    /// <summary>
    /// Represents the inner surface composer.
    /// </summary>
    private readonly ISurfaceComposer<SurfaceBackgroundOptions> _inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundAdapter"/> class.
    /// </summary>
    /// <param name="inner">The inner surface composer to adapt.</param>
    public BackgroundAdapter(ISurfaceComposer<SurfaceBackgroundOptions> inner)
    {
        _inner = inner;
    }

    /// <inheritdoc />
    public void Compose(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.Compose(target, options.Background);

    /// <inheritdoc />
    public ValueTask ComposeAsync(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.ComposeAsync(target, options.Background);
}

