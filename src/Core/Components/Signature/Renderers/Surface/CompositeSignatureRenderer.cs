namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a composite renderer that delegates signature rendering to multiple surface and stroke renderers.
/// </summary>
/// <remarks>This class enables combining several surface and stroke renderers to produce a complete signature
/// rendering. Surface renderers are invoked before stroke renderers, allowing for layered rendering such as
/// backgrounds, grids, and signature strokes. This is useful for scenarios where rendering responsibilities are split
/// across multiple components or features.</remarks>
internal sealed class CompositeSignatureRenderer
{
    /// <summary>
    /// Represents the collection of surface renderers used for signature rendering operations.
    /// </summary>
    private readonly IReadOnlyList<ISurfaceComposer<SignatureRenderingOptions>> _surfaceRenderers;

    /// <summary>
    /// Represents the collection of surface renderers used for signature engine operations.
    /// </summary>
    private readonly IReadOnlyList<ISurfaceComposer<SignatureEngineOptions>> _surfaceEngineRenderers;

    /// <summary>
    /// Represents the collection of stroke renderers used to render signature strokes.
    /// </summary>
    private readonly IReadOnlyList<ISignatureStrokeRenderer> _strokeRenderers;

    /// <summary>
    /// Initializes a new instance of the CompositeSignatureRenderer class with the specified surface and stroke renderers.
    /// </summary>
    /// <param name="surfaceRenderers">Represents the collection of surface renderers.</param>
    /// <param name="strokeRenderers">Represents the collection of stroke renderers.</param>
    /// <param name="surfaceEngineRenderers">Represents the collection of surface engine renderers</param>
    public CompositeSignatureRenderer(
        IEnumerable<ISurfaceComposer<SignatureRenderingOptions>> surfaceRenderers,
        IEnumerable<ISurfaceComposer<SignatureEngineOptions>> surfaceEngineRenderers,
        IEnumerable<ISignatureStrokeRenderer> strokeRenderers)
    {
        _surfaceRenderers = [.. surfaceRenderers];
        _strokeRenderers = [.. strokeRenderers];
        _surfaceEngineRenderers = [.. surfaceEngineRenderers];
    }

    /// <summary>
    /// Renders the surface and signature strokes onto the specified render target using the provided rendering options.
    /// </summary>
    /// <remarks>This method first renders the surface elements, such as background or grid, followed by the
    /// signature strokes. The rendering order ensures that strokes appear above the surface elements.</remarks>
    /// <param name="target">The render target on which to draw the surface and strokes.</param>
    /// <param name="strokes">The collection of signature strokes to be rendered on the surface.</param>
    /// <param name="options">The rendering options that control the appearance and behavior of the rendering process.</param>
    /// <param name="engineOptions">The engine options that control the appearance and behavior of the rendering process.</param>
    public void Compose(
        ISurfaceRenderTarget target,
        IReadOnlyList<SignatureStroke> strokes,
        SignatureRenderingOptions options,
        SignatureEngineOptions engineOptions)
    {
        foreach (var renderer in _surfaceRenderers)
        {
            renderer.Compose(target, options);
        }

        foreach (var renderer in _strokeRenderers)
        {
            renderer.Compose(target, strokes, options);
        }

        foreach (var renderer in _surfaceEngineRenderers)
        {
            renderer.Compose(target, engineOptions);
        }
    }

    /// <summary>
    /// Asynchronously renders signature strokes onto the specified surface using the provided rendering options.
    /// </summary>
    /// <param name="target">The surface render target on which the signature will be drawn.</param>
    /// <param name="strokes">A read-only list of signature strokes to render onto the target surface.</param>
    /// <param name="options">The rendering options that control how the signature strokes are displayed.</param>
    /// <param name="engineOptions">The engine options that control the appearance and behavior of the rendering process.</param>
    /// <returns>A task that represents the asynchronous rendering operation.</returns>
    public async ValueTask RenderAsync(
        ISurfaceRenderTarget target,
        IReadOnlyList<SignatureStroke> strokes,
        SignatureRenderingOptions options,
        SignatureEngineOptions engineOptions)
    {
        foreach (var renderer in _surfaceRenderers)
        {
            await renderer.ComposeAsync(target, options);
        }

        foreach (var renderer in _strokeRenderers)
        {
            await renderer.ComposeAsync(target, strokes, options);
        }

        foreach (var renderer in _surfaceEngineRenderers)
        {
            await renderer.ComposeAsync(target, engineOptions);
        }
    }
}
