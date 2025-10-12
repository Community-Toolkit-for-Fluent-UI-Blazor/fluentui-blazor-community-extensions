namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a video media source with a specified quality and source URL.
/// </summary>
/// <param name="Quality">The quality level of the video stream. Higher values typically indicate better video resolution or bitrate.</param>
/// <param name="SourceUrl">The URL from which the video can be accessed or streamed. Cannot be null.</param>
public record VideoMediaSource(int Quality, string SourceUrl)
{
    /// <summary>
    /// Gets a value indicating whether the video is in high definition.
    /// </summary>
    public bool IsHighDefinition => Quality >= 720;
}
