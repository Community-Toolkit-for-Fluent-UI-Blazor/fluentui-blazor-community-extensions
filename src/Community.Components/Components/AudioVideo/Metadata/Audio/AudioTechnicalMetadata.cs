namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents technical metadata for a media file, including properties such as duration, bitrate, sample rate, and
/// codec information.
/// </summary>
/// <remarks>This class provides a set of properties to describe the technical characteristics of a media file. 
/// All properties are nullable, allowing for scenarios where specific metadata may not be available.</remarks>
public class AudioTechnicalMetadata
{
    /// <summary>
    /// Gets or sets the duration of the operation.
    /// </summary>
    public TimeSpan? Duration { get; set; }

    /// <summary>
    /// Gets or sets the bitrate of the media, in bits per second.
    /// </summary>
    public int? AudioBitrate { get; set; }

    /// <summary>
    /// Gets or sets the sample rate, in hertz, for the audio processing operation.
    /// </summary>
    /// <remarks>The sample rate determines the number of audio samples processed per second. Ensure the value
    /// is compatible with the audio source and processing requirements.</remarks>
    public int? SampleRate { get; set; }

    /// <summary>
    /// Gets or sets the number of channels to be used. 
    /// </summary>
    /// <remarks>A value of <see langword="null"/> indicates that the default number of channels will be
    /// used.</remarks>
    public int? AudioChannels { get; set; }

    /// <summary>
    /// Gets or sets the codec used for encoding or decoding data.
    /// </summary>
    public string? Codec { get; set; }

    /// <summary>
    /// Gets or sets the media types associated with the file.
    /// </summary>
    public string? MediaTypes { get; set; }

    /// <summary>
    /// Gets or sets the file type of the media, such as "audio/mpeg" or "video/mp4".
    /// </summary>
    public string? FileType { get; set; }

    /// <summary>
    /// Gets or sets the size of the file in bytes.
    /// </summary>
    public long FileSize { get; set; }
}
