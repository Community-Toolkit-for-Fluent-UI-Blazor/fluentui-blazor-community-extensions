namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents extended metadata for an audio file.
/// </summary>
public class AudioExtendedMetadata
{
    /// <summary>
    /// Gets or sets the track number associated with the item.
    /// </summary>
    public uint? TrackNumber { get; set; }

    /// <summary>
    /// Gets or sets the disc number associated with the item. 
    /// </summary>
    public uint? DiscNumber { get; set; }

    /// <summary>
    /// Gets or sets the total number of tracks in the collection.
    /// </summary>
    public uint? TotalTracks { get; set; }

    /// <summary>
    /// Gets or sets the total number of discs in a collection or set.
    /// </summary>
    public uint? TotalDiscs { get; set; }

    /// <summary>
    /// Gets or sets the lyrics of the song.
    /// </summary>
    public string? Lyrics { get; set; }

    /// <summary>
    /// Gets or sets the International Standard Recording Code (ISRC) associated with the recording.
    /// </summary>
    public string? ISRC { get; set; }

    /// <summary>
    /// Gets or sets the grouping identifier used to categorize related items.
    /// </summary>
    public string? Grouping { get; set; }

    /// <summary>
    /// Gets or sets the tempo of the music in beats per minute (BPM).
    /// </summary>
    /// <remarks>The value must be a positive integer. Common tempos range from 40 BPM (slow) to 200 BPM
    /// (fast),  but the property does not enforce specific limits.</remarks>
    public uint BeatsPerMinute { get; set; }
}
