namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines the contract for a render target that manages and updates surface visualization elements such as axes,
/// background, grid, strokes, selection, and watermark. Provides methods to set rendering payloads and flush changes
/// asynchronously.
/// </summary>
/// <remarks>Implementations of this interface are responsible for updating the visual state of a surface based on
/// the provided payloads. The interface allows granular control over individual rendering aspects, enabling dynamic
/// updates to the visualization. The asynchronous flush operation ensures that pending changes are committed
/// efficiently. This interface is typically used in scenarios where interactive or real-time surface rendering is
/// required.</remarks>
public interface ISurfaceRenderTarget
{
    /// <summary>
    /// Gets the payload representing the current view state.
    /// </summary>
    ViewPayload View { get; }

    /// <summary>
    /// Sets the current view using the specified payload.
    /// </summary>
    /// <param name="view">The payload containing the view configuration to apply. Cannot be null.</param>
    void SetView(ViewPayload view);

    /// <summary>
    /// Gets the static back layer used for rendering non-interactive surfaces behind the main content.
    /// </summary>
    IStaticSurfaceRender StaticBackLayer { get; }

    /// <summary>
    /// Gets the dynamic surface rendering layer associated with this instance.
    /// </summary>
    IDynamicSurfaceRender DynamicLayer { get; }

    /// <summary>
    /// Gets the static front layer render target for the surface.
    /// </summary>
    IStaticSurfaceRender StaticFrontLayer { get; }

    /// <summary>
    /// Gets the dynamic surface render layer used for debugging visual output.
    /// </summary>
    IDynamicSurfaceRender DebugLayer { get; }

    /// <summary>
    /// Gets the static surface rendering the watermark.
    /// </summary>
    IStaticSurfaceRender WaterMark { get; }

    /// <summary>
    /// Gets the hover surface rendering the hover.
    /// </summary>
    IStaticSurfaceRender Hover { get; }

    /// <summary>
    /// Asynchronously flushes any buffered data to render engine.
    /// </summary>
    /// <returns>A ValueTask that represents the asynchronous flush operation.</returns>
    ValueTask FlushAsync();

    /// <summary>
    /// Asynchronously renders a segment of a stroke using the specified payload and view context.
    /// </summary>
    /// <param name="payload">An object containing the data required to define the stroke segment. The payload must provide all necessary
    /// information for rendering the segment.</param>
    /// <param name="view">The view context in which the stroke segment will be drawn. Specifies rendering parameters and visual state.</param>
    /// <returns>A ValueTask that represents the asynchronous rendering operation.</returns>
    ValueTask DrawStrokeSegmentAsync(object payload, ViewPayload view);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    object? GetNativeHandle();
}
