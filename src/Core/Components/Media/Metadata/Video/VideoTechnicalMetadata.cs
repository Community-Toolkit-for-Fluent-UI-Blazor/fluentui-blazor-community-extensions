namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents technical metadata for a video file, including properties such as duration, file type, and codec
/// information.
/// </summary>
/// <remarks>This class provides detailed information about the video and audio characteristics of a media file,
/// which can be useful for processing or displaying video content.</remarks>
public class VideoTechnicalMetadata
{
    /// <summary>
    /// Gets or sets the duration of the event, represented as a nullable time interval.
    /// </summary>
    /// <remarks>If set, this property specifies the length of time for which the event occurs. A null value
    /// indicates that the duration is unspecified.</remarks>
    public TimeSpan? Duration { get; set; }

    /// <summary>
    /// Gets or sets the type of the file, which may influence how the file is processed or displayed.
    /// </summary>
    /// <remarks>This property can be null, indicating that the file type is unspecified. It is important to
    /// set this property to a valid file type to ensure correct handling of the file.</remarks>
    public string? FileType { get; set; }

    /// <summary>
    /// Gets or sets the MIME type of the content.
    /// </summary>
    /// <remarks>The MIME type specifies the nature and format of the content, which can affect how it is
    /// processed by clients and servers. Common examples include "video/mp4" for MP4 video files or "image/jpeg" for
    /// JPEG images.</remarks>
    public string? MimeType { get; set; }

    /// <summary>
    /// Gets or sets the size of the file in bytes.
    /// </summary>
    /// <remarks>The value represents the total number of bytes used by the file. This property can be used to
    /// determine the storage requirements for the file.</remarks>
    public long FileSize { get; set; }

    /// <summary>
    /// Gets or sets the video codec used for encoding or decoding video streams.
    /// </summary>
    /// <remarks>This property allows the specification of the video codec to be used, which can affect the
    /// quality and size of the video output. Common codecs include H.264, H.265, and VP9. Ensure that the selected
    /// codec is supported by the playback environment.</remarks>
    public string? VideoCodec { get; set; }

    /// <summary>
    /// Gets or sets the video bitrate, in bits per second, for the media stream.
    /// </summary>
    /// <remarks>A null value indicates that the video bitrate is not specified. Set this property according
    /// to the desired video quality and available bandwidth constraints.</remarks>
    public long? VideoBitrate { get; set; }

    /// <summary>
    /// Gets or sets the frame rate for media playback, measured in frames per second.
    /// </summary>
    /// <remarks>A null value indicates that the frame rate is not specified. The frame rate should be a
    /// positive value to ensure smooth playback.</remarks>
    public double? FrameRate { get; set; }

    /// <summary>
    /// Gets or sets the aspect ratio of the visual element, represented as a string.
    /// </summary>
    /// <remarks>The aspect ratio is typically expressed in the format 'width:height', such as '16:9'. This
    /// property can be used to maintain the correct proportions of the visual element when resizing.</remarks>
    public string? AspectRatio { get; set; }

    /// <summary>
    /// Gets or sets the audio codec used for encoding or decoding audio streams.
    /// </summary>
    /// <remarks>This property allows the specification of the audio codec to be used, which can affect the
    /// quality and size of the audio data. Common codecs include AAC, MP3, and WAV. Ensure that the selected codec is
    /// supported by the audio processing system.</remarks>
    public string? AudioCodec { get; set; }

    /// <summary>
    /// Gets or sets the audio bitrate, in bits per second, for the media stream.
    /// </summary>
    /// <remarks>The bitrate influences the quality and size of the audio stream. Higher bitrates generally
    /// result in better audio quality but larger file sizes. Set this property to null to indicate that no specific
    /// bitrate is defined.</remarks>
    public long? AudioBitrate { get; set; }

    /// <summary>
    /// Gets or sets the number of audio channels for the media.
    /// </summary>
    /// <remarks>Set this property to null if the audio channel configuration is not specified. Typical values
    /// are 1 for mono and 2 for stereo. The value must be a positive integer when specified.</remarks>
    public int? AudioChannels { get; set; }

    /// <summary>
    /// Gets or sets the audio sample rate, in hertz, for the audio stream.
    /// </summary>
    /// <remarks>The sample rate determines the number of audio samples captured or played per second,
    /// directly affecting audio quality and file size. Common values include 44100 Hz for CD-quality audio. A null
    /// value indicates that the sample rate is not specified.</remarks>
    public int? AudioSampleRate { get; set; }
}
