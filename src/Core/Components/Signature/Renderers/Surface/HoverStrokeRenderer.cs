namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides rendering functionality for displaying a hover effect on a signature stroke within a surface, using a
/// delegate to determine the currently hovered stroke.
/// </summary>
/// <remarks>This renderer is intended for use in scenarios where a visual indication of a hovered stroke is
/// required, such as interactive signature surfaces. The delegate supplied to the constructor should return the stroke
/// to be rendered as hovered, or null if no stroke is currently hovered. Rendering occurs only when hover effects are
/// enabled in the provided rendering options.</remarks>
internal sealed class HoverStrokeRenderer : ISignatureSurfaceRenderer
{
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
    public void Render(ISurfaceRenderTarget target, SignatureRenderingOptions options)
    {
        if (!options.Hover.Enabled)
        {
            return;
        }

        var stroke = _getHoverStroke();
        var payload = PayloadFactory.CreateHover(stroke, options.Hover);

        target.Hover.SetHover(payload);
    }

    /// <inheritdoc />
    public ValueTask RenderAsync(ISurfaceRenderTarget target, SignatureRenderingOptions options)
    {
        Render(target, options);

        return ValueTask.CompletedTask;
    }
}

