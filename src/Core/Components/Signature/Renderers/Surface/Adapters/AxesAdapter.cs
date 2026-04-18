namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an adapter that allows a surface composer designed for <see cref="SurfaceAxesOptions"/> to be
///  used in the context of <see cref="SignatureRenderingOptions"/> by extracting the relevant axes
///  options from the signature rendering options and delegating the composition to the inner composer.
/// </summary>
internal class AxesAdapter : ISurfaceComposer<SignatureRenderingOptions>
{
    /// <summary>
    /// Represents the inner surface composer.
    /// </summary>
    private readonly ISurfaceComposer<SurfaceAxesOptions> _inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="AxesAdapter"/> class with the specified inner surface composer.
    /// </summary>
    /// <param name="inner">The inner surface composer.</param>
    public AxesAdapter(ISurfaceComposer<SurfaceAxesOptions> inner)
    {
        _inner = inner;
    }

    /// <inheritdoc />
    public void Compose(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.Compose(target, options.Axes);

    /// <inheritdoc />
    public ValueTask ComposeAsync(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.ComposeAsync(target, options.Axes);
}

