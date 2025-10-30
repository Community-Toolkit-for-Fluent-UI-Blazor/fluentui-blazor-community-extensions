namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a chapter within a media file, defined by its start and end times, title, and status.
/// </summary>
/// <remarks>This record is commonly used to represent segments of a media file, such as chapters in an audiobook
/// or scenes in a video. The <see cref="Start"/> and <see cref="End"/> properties define the time range of the chapter,
/// while the <see cref="Title"/> provides a descriptive name. The <see cref="Status"/> indicates the current state of
/// the chapter, such as whether it is active or inactive.</remarks>
/// <param name="Start">The start time of the chapter, in seconds. Must be greater than or equal to 0.</param>
/// <param name="End">The end time of the chapter, in seconds. Must be greater than <paramref name="Start"/>.</param>
/// <param name="Title">The title of the chapter. Cannot be null or empty.</param>
/// <param name="Status">The status of the chapter, indicating its current state.</param>
public record Chapter(double Start, double End, string Title, ChapterStatus Status)
{
}
