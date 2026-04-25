namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Adapts a surface composer that operates on grid options to work with signature rendering options.
/// </summary>
/// <remarks>This adapter enables the use of an existing surface composer implementation that expects
/// grid-specific options in scenarios where signature rendering options are provided. It delegates composition calls to
/// the underlying composer, extracting the relevant grid options from the signature rendering options.</remarks>
internal class GridAdapter : ISurfaceComposer<SignatureRenderingOptions>
{
    /// <summary>
    /// Provides access to the underlying surface composer implementation for grid options.
    /// </summary>
    private readonly ISurfaceComposer<SurfaceGridOptions> _inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="GridAdapter"/> class.
    /// </summary>
    /// <param name="inner">The inner surface composer to adapt.</param>
    public GridAdapter(ISurfaceComposer<SurfaceGridOptions> inner)
    {
        _inner = inner;
    }

    /// <inheritdoc />
    public bool Compose(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.Compose(target, options.Grid);

    /// <inheritdoc />
    public ValueTask<bool> ComposeAsync(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.ComposeAsync(target, options.Grid);
}

