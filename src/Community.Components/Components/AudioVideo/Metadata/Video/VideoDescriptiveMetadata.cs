namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents descriptive metadata information for a video, including title, series, directors, cast, genres, and
/// </summary>
public class VideoDescriptiveMetadata
{
    /// <summary>
    /// Gets or sets the title associated with the video.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the series identifier or name associated with the video.
    /// </summary>
    public string? Series { get; set; }

    /// <summary>
    /// Gets or sets the names of the directors associated with the video.
    /// </summary>
    public string[] Directors { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of writer names associated with the video.
    /// </summary>
    public string[] Writers { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of cast members associated with the video.
    /// </summary>
    public string[] Cast { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of producers associated with the video.
    /// </summary>
    public string[] Producers { get; set; } = [];

    /// <summary>
    /// Gets or sets the genres associated with the video.
    /// </summary>
    public string[] Genres { get; set; } = [];

    /// <summary>
    /// Gets or sets the year associated with the video.
    /// </summary>
    public uint? Year { get; set; }

    /// <summary>
    /// Gets or sets the description associated with the video.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the optional comment associated with this video.
    /// </summary>
    public string? Comment { get; set; }
}
