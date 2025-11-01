namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a collection of metadata describing various aspects of a video, including descriptive, technical, legal,
/// visual, and extended information.
/// </summary>
/// <remarks>Use this class to access and organize all major categories of video metadata in a single object. Each
/// property provides access to a specific facet of the video's metadata, enabling comprehensive management and
/// retrieval of video-related information.</remarks>
public class VideoMetadata
{
    /// <summary>
    /// Gets or sets the descriptive metadata associated with the video.
    /// </summary>
    public VideoDescriptiveMetadata Descriptive { get; set; } = new();

    /// <summary>
    /// Gets or sets the extended metadata associated with the video.
    /// </summary>
    public VideoExtendedMetadata Extended { get; set; } = new();

    /// <summary>
    /// Gets or sets the legal metadata associated with the video.
    /// </summary>
    public VideoLegalMetadata Legal { get; set; } = new();

    /// <summary>
    /// Gets or sets the technical metadata associated with the video.
    /// </summary>
    public VideoTechnicalMetadata Technical { get; set; } = new();

    /// <summary>
    /// Gets or sets the visual metadata associated with the video content.
    /// </summary>
    public VideoVisualMetadata Visual { get; set; } = new();
}
