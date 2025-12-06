namespace FluentUI.Blazor.Community.Components;

public class VideoTechnicalMetadata
{
    public TimeSpan? Duration { get; set; }
    public string? FileType { get; set; }
    public string? MimeType { get; set; }
    public long FileSize { get; set; }

    public string? VideoCodec { get; set; }
    public long? VideoBitrate { get; set; }
    public double? FrameRate { get; set; }
    public string? AspectRatio { get; set; }

    public string? AudioCodec { get; set; }
    public long? AudioBitrate { get; set; }
    public int? AudioChannels { get; set; }
    public int? AudioSampleRate { get; set; }
}
