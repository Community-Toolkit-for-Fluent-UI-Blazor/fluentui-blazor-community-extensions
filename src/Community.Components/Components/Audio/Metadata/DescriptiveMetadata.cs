namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents descriptive metadata for a musical track, including details such as title, album, performers, and other
/// related information.
/// </summary>
/// <remarks>This class provides a structured way to store and access metadata commonly associated with musical
/// tracks.  It includes properties for identifying the track's title, album, contributors, genres, and other
/// descriptive details. All string array properties are initialized as empty arrays to ensure safe iteration.</remarks>
public class DescriptiveMetadata
{
    /// <summary>
    /// Gets or sets the title associated with the object.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the name of the album associated with the item.
    /// </summary>
    public string? Album { get; set; }

    /// <summary>
    /// Gets or sets the list of performers associated with the event.
    /// </summary>
    public string[] Performers { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of album artists associated with the album.
    /// </summary>
    public string[] AlbumArtists { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of composers associated with the current context.
    /// </summary>
    public string[] Composers { get; set; } = [];

    /// <summary>
    /// Gets or sets the name of the conductor associated with the performance.
    /// </summary>
    public string? Conductor { get; set; }

    /// <summary>
    /// Gets or sets the list of genres associated with the item.
    /// </summary>
    public string[] Genres { get; set; } = [];

    /// <summary>
    /// Gets or sets the year associated with the item.
    /// </summary>
    public uint Year { get; set; }

    /// <summary>
    /// Gets or sets comments associated with the item.
    /// </summary>
    public string? Comment { get; set; }
}
