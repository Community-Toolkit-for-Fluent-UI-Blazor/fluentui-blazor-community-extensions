namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event arguments for a slideshow measured event.
/// </summary>
public class SlideshowMeasuredEventArgs
{
    /// <summary>
    /// Gets or sets the identifier of the slideshow item that was measured.
    /// </summary>
    public string? Id { get; init; }

    /// <summary>
    /// Gets or sets the width of the slideshow item that was measured.
    /// </summary>
    public double Width { get; init; }

    /// <summary>
    /// Gets or sets the height of the slideshow item that was measured.
    /// </summary>
    public double Height { get; init; }
}
