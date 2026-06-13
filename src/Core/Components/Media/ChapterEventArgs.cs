namespace FluentUI.Blazor.Community.Components.Media;

/// <summary>
/// Event arguments for chapter-related events in media components, containing the chapter name, URL, and a list of chapters.
/// </summary>
/// <param name="Name">The name of the video.</param>
/// <param name="Url">The URL of the video.</param>
public sealed record ChapterEventArgs(string Name, string Url)
{
    /// <summary>
    /// Gets or sets the list of chapters for the video.
    /// </summary>
    public IReadOnlyList<Chapter> Chapters { get; set; } = [];
}
