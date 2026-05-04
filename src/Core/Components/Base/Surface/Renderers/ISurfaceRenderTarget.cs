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
    /// Asynchronously flushes any buffered data to render engine.
    /// </summary>
    /// <returns>A ValueTask that represents the asynchronous flush operation.</returns>
    ValueTask FlushAsync();

    /// <summary>
    /// Gets a platform-specific handle or reference to the underlying native resource, if available.
    /// </summary>
    /// <remarks>The type and meaning of the returned handle depend on the platform and implementation.
    /// Callers should cast and use the handle according to the expected native API. If the resource does not have a
    /// native representation, this method returns null.</remarks>
    /// <returns>An object representing the native handle associated with the current instance, or null if no native handle is
    /// available.</returns>
    object? GetNativeHandle();

    /// <summary>
    /// Adds a new rendering layer to the surface and returns a render target for drawing on that layer.
    /// </summary>
    /// <param name="layer">The layer to add to the surface. Cannot be null.</param>
    /// <returns>A render target associated with the added layer, which can be used to perform drawing operations on that layer.</returns>
    ISurfaceRenderTarget AddLayer(ILayer layer);
}
