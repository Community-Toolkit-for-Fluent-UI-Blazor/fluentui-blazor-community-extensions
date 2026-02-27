namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the different modes available for the eraser tool,
///  determining how it interacts with the canvas when erasing content.
/// </summary>
public enum EraserMode
{
    /// <summary>
    /// Erases content on a pixel-by-pixel basis, allowing for precise control over the erasure process.
    /// </summary>
    Pixel,

    /// <summary>
    /// Erases entire strokes or paths.
    /// </summary>
    Stroke,

    /// <summary>
    /// Erases content based on a combination of pixel-based and stroke-based approaches.
    /// The eraser will remove pixels, but if the amount of erased pixels exceeds a certain threshold,
    ///  it will consider the entire stroke for deletion.
    /// </summary>
    Hybrid
}
