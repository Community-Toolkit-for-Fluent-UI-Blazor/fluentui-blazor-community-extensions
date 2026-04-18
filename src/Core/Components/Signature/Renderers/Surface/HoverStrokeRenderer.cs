namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides rendering functionality for displaying a hover effect on a signature stroke within a surface, using a
/// delegate to determine the currently hovered stroke.
/// </summary>
/// <remarks>This renderer is intended for use in scenarios where a visual indication of a hovered stroke is
/// required, such as interactive signature surfaces. The delegate supplied to the constructor should return the stroke
/// to be rendered as hovered, or null if no stroke is currently hovered. Rendering occurs only when hover effects are
/// enabled in the provided rendering options.</remarks>
internal sealed class HoverStrokeRenderer : ISurfaceComposer<SignatureHoverRenderingOptions>
{
    /// <summary>
    /// Represents a delegate that retrieves the current hover stroke, or null if no stroke is being hovered.
    /// </summary>
    private readonly Func<SignatureStroke?> _getHoverStroke;

    /// <summary>
    /// Initializes a new instance of the HoverStrokeRenderer class using the specified delegate to retrieve the current
    /// hover stroke.
    /// </summary>
    /// <param name="getHoverStroke">A delegate that returns the current hover stroke, or null if no stroke is being hovered. Used to determine which
    /// stroke should be rendered as hovered.</param>
    public HoverStrokeRenderer(Func<SignatureStroke?> getHoverStroke)
    {
        _getHoverStroke = getHoverStroke;
    }

    /// <inheritdoc />
    public void Compose(ISurfaceRenderTarget target, SignatureHoverRenderingOptions options)
    {
        if (!options.Enabled)
        {
            return;
        }

        var stroke = _getHoverStroke();
        var payload = PayloadFactory.CreateHover(stroke, options);

        if (payload is null)
        {
            return;
        }

        target.AddLayer(new HoverLayer(payload));
    }

    /// <inheritdoc />
    public ValueTask ComposeAsync(ISurfaceRenderTarget target, SignatureHoverRenderingOptions options)
    {
        Compose(target, options);

        return ValueTask.CompletedTask;
    }
}

