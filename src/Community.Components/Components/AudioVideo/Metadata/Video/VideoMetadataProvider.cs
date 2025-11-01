using System.Drawing;
using FFMpegCore;
using FluentUI.Blazor.Community.Helpers;
using MetadataExtractor;
using MetadataExtractor.Formats.QuickTime;
using MetadataExtractor.Formats.Xmp;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to extract metadata from video files using various input sources such as browser files,
/// streams, or URLs.
/// </summary>
/// <remarks>This provider supports extracting technical, descriptive, visual, extended, and legal metadata from
/// video files. It is intended for internal use and is not thread-safe. Temporary files are created during metadata
/// extraction and are managed internally.</remarks>
/// <param name="httpClient">The HTTP client used to download video files from remote URLs when extracting metadata.</param>
/// <param name="logger">The logger used to record diagnostic and error information during metadata extraction.</param>
internal class VideoMetadataProvider(HttpClient httpClient, ILogger<VideoMetadataProvider> logger)
    : IVideoMetadataProvider
{
    /// <summary>
    /// Represents a static instance of <see cref="FileExtensionContentTypeProvider"/> used to map file extensions to MIME
    /// </summary>
    private static readonly FileExtensionContentTypeProvider _contentTypeProvider = new();

    /// <summary>
    /// Asynchronously extracts video metadata from the specified browser-uploaded file.
    /// </summary>
    /// <remarks>The method creates a temporary file on disk to process the uploaded video. The temporary file
    /// is deleted when the process ends. This method is intended for use with files uploaded via browser-based file
    /// inputs, such as in Blazor applications.</remarks>
    /// <param name="browserFile">The browser file to analyze. Must not be null and should represent a valid video file.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="VideoMetadata"/> object
    /// with the extracted metadata if successful; otherwise, <see langword="null"/> if the file is not a valid or
    /// supported video.</returns>
    public async Task<VideoMetadata?> GetFromFileAsync(IBrowserFile browserFile)
    {
        var tempPath = Path.GetTempFileName();

        using (var fs = File.Create(tempPath))
        {
            await browserFile.OpenReadStream().CopyToAsync(fs);
        }

        return await GetFromPathAsync(browserFile.Name, tempPath);
    }

    /// <summary>
    /// Asynchronously extracts video metadata from the provided stream.
    /// </summary>
    /// <remarks>The method reads the entire stream and may create a temporary file during processing. The
    /// caller is responsible for disposing the provided stream after the operation completes.</remarks>
    /// <param name="stream">The stream containing video data to analyze. The stream must be readable and positioned at the start of the
    /// video data.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="VideoMetadata"/> object
    /// with the extracted metadata, or <see langword="null"/> if the metadata could not be determined.</returns>
    public async Task<VideoMetadata?> GetFromStreamAsync(Stream stream)
    {
        var tempPath = Path.GetTempFileName();
        using (var fs = File.Create(tempPath))
        {
            stream.CopyTo(fs);
        }

        return await GetFromPathAsync(Path.GetFileName(tempPath), tempPath);
    }

    /// <summary>
    /// Asynchronously retrieves video metadata from the specified URL.
    /// </summary>
    /// <param name="url">The URL of the video file to retrieve metadata from. Can be null or empty, in which case the method returns
    /// null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="VideoMetadata"/> object
    /// with the extracted metadata if successful; otherwise, null.</returns>
    public async Task<VideoMetadata?> GetFromUrlAsync(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        var tempPath = Path.GetTempFileName();
        using var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        await using (var fs = File.Create(tempPath))
        {
            await response.Content.CopyToAsync(fs);
        }

        return await GetFromPathAsync(Path.GetFileName(url), tempPath);
    }

    /// <summary>
    /// Asynchronously extracts video metadata from the specified file path and file name.
    /// </summary>
    /// <remarks>If metadata extraction fails due to an error, a new <see cref="VideoMetadata"/> instance with
    /// default values is returned and the error is logged. The returned metadata may be incomplete if the file lacks
    /// certain information.</remarks>
    /// <param name="name">The name of the video file. Used to determine file type and MIME type.</param>
    /// <param name="path">The full file system path to the video file to extract metadata from. Cannot be null or empty.</param>
    /// <returns>A <see cref="VideoMetadata"/> object containing descriptive, technical, visual, extended, and legal metadata for
    /// the video, or a new <see cref="VideoMetadata"/> instance with default values if extraction fails.</returns>
    private async Task<VideoMetadata?> GetFromPathAsync(string name, string path)
    {
        try
        {
            if (!RunningWasmHelper.IsWasm)
            {
                var mediaInfo = await FFProbe.AnalyseAsync(path);
                var videoStream = mediaInfo.VideoStreams.FirstOrDefault();
                var audioStream = mediaInfo.AudioStreams.FirstOrDefault();

                var technical = new VideoTechnicalMetadata
                {
                    Duration = mediaInfo.Duration,
                    FileType = Path.GetExtension(name),
                    MimeType = _contentTypeProvider.TryGetContentType(name, out var mime) ? mime : "application/octet-stream",
                    FileSize = new FileInfo(path).Length,
                    VideoCodec = videoStream?.CodecName,
                    VideoBitrate = videoStream?.BitRate,
                    FrameRate = videoStream?.FrameRate,
                    AspectRatio = videoStream != null ? $"{videoStream.Width}:{videoStream.Height}" : null,
                    AudioCodec = audioStream?.CodecName,
                    AudioBitrate = audioStream?.BitRate,
                    AudioChannels = audioStream?.Channels,
                    AudioSampleRate = audioStream?.SampleRateHz
                };

                var visual = new VideoVisualMetadata
                {
                    Width = videoStream?.Width,
                    Height = videoStream?.Height
                };

                if (videoStream != null)
                {
                    var thumbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");

                    var captureTime = mediaInfo.Duration > TimeSpan.FromSeconds(5)
                        ? TimeSpan.FromSeconds(5)
                        : TimeSpan.FromSeconds(1);

                    await FFMpeg.SnapshotAsync(path, thumbPath, new Size(320, 240), captureTime);

                    if (File.Exists(thumbPath))
                    {
                        var bytes = await File.ReadAllBytesAsync(thumbPath);
                        var base64 = Convert.ToBase64String(bytes);
                        visual.ThumbnailUrl = $"data:image/jpeg;base64,{base64}";
                    }
                }

                var directories = ImageMetadataReader.ReadMetadata(path);

                var descriptive = new VideoDescriptiveMetadata();
                var extended = new VideoExtendedMetadata();
                var legal = new VideoLegalMetadata();

                var qtMeta = directories.OfType<QuickTimeMetadataHeaderDirectory>().FirstOrDefault();

                if (qtMeta is not null)
                {
                    descriptive.Title = qtMeta.GetDescription(QuickTimeMetadataHeaderDirectory.TagDisplayName);
                    descriptive.Description = qtMeta.GetDescription(QuickTimeMetadataHeaderDirectory.TagDescription);
                    descriptive.Comment = qtMeta.GetDescription(QuickTimeMetadataHeaderDirectory.TagComment);
                    legal.Copyright = qtMeta.GetDescription(QuickTimeMetadataHeaderDirectory.TagCopyright);
                    legal.Publisher = qtMeta.GetDescription(QuickTimeMetadataHeaderDirectory.TagPublisher);
                }

                var xmp = directories.OfType<XmpDirectory>().FirstOrDefault();

                if (xmp != null)
                {
                    var props = xmp.GetXmpProperties();
                    if (props.TryGetValue("dc:title", out var title))
                    {
                        descriptive.Title ??= title;
                    }

                    if (props.TryGetValue("dc:description", out var desc))
                    {
                        descriptive.Description ??= desc;
                    }

                    if (props.TryGetValue("dc:creator", out var creator))
                    {
                        descriptive.Comment ??= creator;
                    }
                }

                if (audioStream != null && !string.IsNullOrEmpty(audioStream.Language))
                {
                    extended.Languages = [audioStream.Language];
                }

                return new VideoMetadata
                {
                    Descriptive = descriptive,
                    Extended = extended,
                    Legal = legal,
                    Technical = technical,
                    Visual = visual
                };
            }
            else
            {
                return new();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error extracting video metadata from {FileName}", name);
            return new();
        }
    }
}
