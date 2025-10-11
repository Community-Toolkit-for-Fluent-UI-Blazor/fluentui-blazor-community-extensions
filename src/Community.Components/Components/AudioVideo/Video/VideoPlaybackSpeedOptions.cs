namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents options for configuring the playback speed of a video.
/// </summary>
public record VideoPlaybackSpeedOptions
{
    /// <summary>
    /// Gets or sets the speed value for the video playback.
    /// </summary>
    public double Speed { get; set; } = 1.0;
}
