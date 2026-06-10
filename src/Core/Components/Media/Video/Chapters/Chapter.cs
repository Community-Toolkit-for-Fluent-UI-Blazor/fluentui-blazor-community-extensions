namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a chapter within a media timeline, including its title, start and optional end times, and status.
/// </summary>
/// <remarks>Use this record to describe logical segments or chapters in audio, video, or other time-based media.
/// The combination of start and end times defines the chapter's duration. If EndTime is null, the chapter is considered
/// to extend to the end of the media.</remarks>
/// <param name="Title">The title of the chapter. Cannot be null or empty.</param>
/// <param name="StartTime">The time at which the chapter begins within the media.</param>
/// <param name="EndTime">The time at which the chapter ends within the media.</param>
/// <param name="Status">The status of the chapter. Defaults to ChapterStatus.Unspecified if not specified.</param>
public record Chapter(
    string Title,
    TimeSpan StartTime,
    TimeSpan EndTime,
    ChapterStatus Status = ChapterStatus.Unspecified)
{
    /// <summary>
    /// Gets or sets the URL of the thumbnail image representing the chapter.
    /// </summary>
    public string? ThumbnailUrl { get; set; }
}
