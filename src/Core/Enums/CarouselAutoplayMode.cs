namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the autoplay mode for the carousel.
/// </summary>
public enum CarouselAutoplayMode
{
    /// <summary>
    /// No autoplay.
    /// </summary>
    None,

    /// <summary>
    /// The carousel will loop back to the start after reaching the end.
    /// </summary>
    Rewind,

    /// <summary>
    /// The carousel will loop by pushing the first slide at the end of the last slide,
    /// to create a smooth and infinite loop.
    /// </summary>
    Infinite
}
