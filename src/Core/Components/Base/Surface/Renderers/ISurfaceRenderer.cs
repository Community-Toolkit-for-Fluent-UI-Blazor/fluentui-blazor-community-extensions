namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for rendering a signature surface onto a canvas context asynchronously.
/// </summary>
/// <remarks>Implementations of this interface are responsible for drawing signature data using the provided
/// rendering options and canvas context. The rendering operation is performed asynchronously to support non-blocking UI
/// updates. Implementers should ensure that rendering respects the specified dimensions and DPI for accurate
/// display.</remarks>
public interface ISurfaceComposer<T>
{
    /// <summary>
    /// Renders a signature onto the specified render target using the provided rendering options.
    /// </summary>
    /// <param name="target">The rendering target that provides methods to set various visual elements for the signature surface. Cannot be null.</param>
    /// <param name="options">The rendering options that control the appearance and behavior of the signature rendering. Cannot be null.</param>
    bool Compose(
        ISurfaceRenderTarget target,
        T options);

    /// <summary>
    /// Asynchronously renders a signature onto the specified render target using the provided rendering options. This method is a wrapper around the synchronous Render method, allowing for asynchronous execution without blocking the calling thread. Implementations should ensure that any necessary asynchronous operations are properly handled within the Render method to maintain responsiveness in the UI.
    /// </summary>
    /// <param name="target">The rendering target that provides methods to set various visual elements for the signature surface. Cannot be null.</param>
    /// <param name="options">The rendering options that control the appearance and behavior of the signature rendering. Cannot be null.</param>
    /// <returns>A ValueTask that represents the asynchronous rendering operation.</returns>
    ValueTask<bool> ComposeAsync(
        ISurfaceRenderTarget target,
        T options);
}
