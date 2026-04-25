namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the adapter that allows a surface composer designed for <see cref="SurfaceWatermarkOptions"/> to be used with <see cref="SignatureRenderingOptions"/> by extracting the watermark options from the signature rendering options and delegating the composition to the inner surface composer.
/// </summary>
internal class WatermarkAdapter : ISurfaceComposer<SignatureRenderingOptions>
{
    /// <summary>
    /// Represents the inner surface composer that handles the actual composition of the watermark.
    /// </summary>
    private readonly ISurfaceComposer<SurfaceWatermarkOptions> _inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="WatermarkAdapter"/> class.
    /// </summary>
    /// <param name="inner">The inner surface composer to adapt.</param>
    public WatermarkAdapter(ISurfaceComposer<SurfaceWatermarkOptions> inner)
    {
        _inner = inner;
    }

    /// <inheritdoc />
    public bool Compose(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.Compose(target, options.Watermark);

    /// <inheritdoc />
    public ValueTask<bool> ComposeAsync(ISurfaceRenderTarget target, SignatureRenderingOptions options)
        => _inner.ComposeAsync(target, options.Watermark);
}

