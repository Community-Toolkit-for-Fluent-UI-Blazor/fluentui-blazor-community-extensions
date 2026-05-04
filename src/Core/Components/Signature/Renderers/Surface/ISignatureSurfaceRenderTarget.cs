namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for a render target capable of drawing signature stroke segments asynchronously within a given
/// view context.
/// </summary>
/// <remarks>Implementations of this interface are responsible for rendering signature input, such as pen or touch
/// strokes, onto a surface. The interface extends ISurfaceRenderTarget to provide additional functionality specific to
/// signature scenarios.</remarks>
public interface ISignatureSurfaceRenderTarget: ISurfaceRenderTarget
{
    /// <summary>
    /// Asynchronously draws a segment of a stroke using the specified payload and view context.
    /// </summary>
    /// <param name="payload">An object containing the data required to render the stroke segment. The expected structure and required
    /// properties depend on the implementation.</param>
    /// <param name="view">The view context in which the stroke segment will be drawn. Provides information about the rendering
    /// environment.</param>
    /// <returns>A value task that represents the asynchronous drawing operation.</returns>
    ValueTask DrawStrokeSegmentAsync(object payload, ViewPayload view);
}
