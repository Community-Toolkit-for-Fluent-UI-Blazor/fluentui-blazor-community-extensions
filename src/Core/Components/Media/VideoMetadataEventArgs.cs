namespace FluentUI.Blazor.Community.Components.Media;

/// <summary>
/// Event arguments for video metadata-related events in media components, containing the video name, URL, and metadata information.
/// </summary>
/// <param name="Name">The name of the video.</param>
/// <param name="Url">The URL of the video.</param>
public sealed record VideoMetadataEventArgs(string Name, string Url)
{
    /// <summary>
    /// Gets or sets the metadata information for the video, including details such as duration, dimensions, and other relevant properties.
    /// </summary>
    public VideoMetadata? Metadata { get; set; }
}
