namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents visual metadata, including cover image URL, MIME type, description, and type.
/// </summary>
public class AudioVisualMetadata
{
    /// <summary>
    /// Gets or sets the URL of the cover image.
    /// </summary>
    public string? CoverUrl { get; set; }

    /// <summary>
    /// Gets or sets the MIME type of the cover image.
    /// </summary>
    public string? MimeType { get; set; }

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the type of the entity or object.
    /// </summary>
    public string? Type { get; set; }
}
