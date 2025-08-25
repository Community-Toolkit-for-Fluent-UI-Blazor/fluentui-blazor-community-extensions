namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the size of the slideshow when it was resized from the browser.
/// </summary>
public struct SlideshowResize
{
    /// <summary>
    /// Gets or sets a value indicating whether the width is fixed.
    /// </summary>
    public bool FixedWidth { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the height is fixed.
    /// </summary>
    public bool FixedHeight { get; set; }

    /// <summary>
    /// Gets or sets the width of the slideshow.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the slideshow.
    /// </summary>
    public int Height { get; set; }
}
