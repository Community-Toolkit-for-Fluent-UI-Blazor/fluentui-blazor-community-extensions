namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents visual metadata information for a video, including dimensions, color characteristics, and thumbnail
/// details.
/// </summary>
/// <remarks>This class provides properties describing the visual aspects of a video, such as its width, height,
/// color space, and pixel format. It can be used to convey video characteristics for display, processing, or cataloging
/// purposes. All properties are nullable to accommodate cases where specific metadata may be unavailable.</remarks>
public class VideoVisualMetadata
{
    /// <summary>
    /// Gets or sets the width value.
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the height value.
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// Gets or sets the color space associated with the image or graphic content.
    /// </summary>
    public string? ColorSpace { get; set; }

    /// <summary>
    /// Gets or sets the pixel format used for image processing or rendering.
    /// </summary>
    public string? PixelFormat { get; set; }

    /// <summary>
    /// Gets or sets the URL of the thumbnail image associated with the item.
    /// </summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether High Dynamic Range (HDR) is enabled.
    /// </summary>
    public bool? IsHdr { get; set; }
}

