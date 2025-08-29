namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the looping mode of a slideshow.
/// </summary>
public enum SlideshowLoopingMode
{
    /// <summary>
    /// No looping.
    /// </summary>
    None,

    /// <summary>
    /// The slideshow will loop back to the start after reaching the end.
    /// </summary>
    Rewind,

    /// <summary>
    /// The slideshow will loop by pushing the first slide at the end of the last slide,
    ///  to create a smooth and infinity loop.
    /// </summary>
    Infinite
}
