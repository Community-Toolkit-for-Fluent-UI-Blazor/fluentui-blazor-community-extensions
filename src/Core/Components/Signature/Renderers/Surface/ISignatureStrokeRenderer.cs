namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for rendering signature grids within a user interface.
/// </summary>
/// <remarks>Implementations of this interface are responsible for providing rendering logic for signature grid
/// components. This interface is intended to be used as an extension point for custom rendering strategies.</remarks>
public interface ISignatureStrokeRenderer
{
    /// <summary>
    /// Renders a signature grid onto the specified canvas context asynchronously using the provided options and
    /// dimensions.
    /// </summary>
    /// <remarks>This method does not block the calling thread. Ensure that the canvas context is valid and
    /// available for the duration of the rendering operation.</remarks>
    /// <param name="target">The render target where the signature will be rendered. Must not be null.</param>
    /// <param name="strokes">The collection of signature strokes to be rendered on the grid.</param>
    /// <param name="options">The configuration options for rendering the signature. Determines appearance and behavior. Must not be
    /// null.</param>
    /// <returns>A ValueTask representing the asynchronous rendering operation. The task completes when the grid has been fully
    /// rendered.</returns>
    ValueTask ComposeAsync(
        ISurfaceRenderTarget target,
        IReadOnlyList<SignatureStroke> strokes,
        SignatureRenderingOptions options);

    /// <summary>
    /// Renders a signature grid onto the specified canvas context synchronously using the provided options and dimensions.
    /// </summary>
    /// <param name="target">The render target where the signature will be rendered. Must not be null.</param>
    /// <param name="strokes">The collection of signature strokes to be rendered on the grid.</param>
    /// <param name="options">The configuration options for rendering the signature. Determines appearance and behavior. Must not be
    /// null.</param>
    void Compose(
        ISurfaceRenderTarget target,
        IReadOnlyList<SignatureStroke> strokes,
        SignatureRenderingOptions options);
}

