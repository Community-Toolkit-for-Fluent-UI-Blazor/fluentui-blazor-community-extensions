using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to retrieve metadata for audio files.
/// </summary>
/// <remarks>This class is intended to be used for extracting and managing metadata such as title, artist, album, 
/// and other relevant information from audio files. It supports various audio formats.</remarks>
internal class AudioMetadataProvider(HttpClient httpClient, ILogger<AudioMetadataProvider> logger)
    : IAudioMetadataProvider
{
    /// <summary>
    /// Provides a static instance of <see cref="FileExtensionContentTypeProvider"/> used to map file extensions to MIME
    /// content types.
    /// </summary>
    /// <remarks>This instance can be used to look up MIME types for file extensions. It is initialized with
    /// default mappings provided by the framework.</remarks>
    private static FileExtensionContentTypeProvider _fileExtensionContentTypeProvider = new();

    /// <summary>
    /// Asynchronously retrieves audio metadata from the specified file.
    /// </summary>
    /// <remarks>This method reads the file's content into memory and extracts metadata from the stream. 
    /// Ensure the file size is within the limits supported by the application to avoid memory issues.</remarks>
    /// <param name="browserFile">The file to extract audio metadata from. Must not be <c>null</c>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the extracted  <see
    /// cref="AudioMetadata"/> if the operation is successful; otherwise, <c>null</c>.</returns>
    public async Task<AudioMetadata?> GetFromFileAsync(IBrowserFile browserFile)
    {
        using var stream = browserFile.OpenReadStream();
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        ms.Position = 0;

        return GetFromStream(browserFile.Name, ms);
    }

    /// <summary>
    /// Extracts audio metadata from a given stream.
    /// </summary>
    /// <remarks>This method uses the TagLib library to parse the audio file and extract metadata. The
    /// extracted metadata includes information such as title, album, artists, duration, bitrate, and cover art. If the
    /// audio file is corrupt or in an unsupported format, the method logs the error and returns an empty <see
    /// cref="AudioMetadata"/> object.</remarks>
    /// <param name="name">The name of the audio file, used for logging and identification purposes.</param>
    /// <param name="stream">The input stream containing the audio file data. The stream must be readable and seekable.</param>
    /// <returns>An <see cref="AudioMetadata"/> object containing descriptive, technical, extended, legal, and visual metadata
    /// extracted from the audio file. Returns an empty <see cref="AudioMetadata"/> object if the file is corrupt,
    /// unsupported, or an error occurs during processing.</returns>
    public AudioMetadata GetFromStream(string name, Stream stream)
    {
        try
        {
            var contentType = _fileExtensionContentTypeProvider.TryGetContentType(name, out var mime) ? mime : "application/octet-stream";
            using var internalStream = new MemoryStream();
            stream.CopyTo(internalStream);
            internalStream.Position = 0;
            using var file = TagLib.File.Create(new StreamFileAbstraction(name, internalStream, internalStream), mime, TagLib.ReadStyle.Average);
            var tag = file.Tag;

            return new AudioMetadata()
            {
                Descriptive = new DescriptiveMetadata()
                {
                    Title = tag.Title,
                    Album = tag.Album,
                    AlbumArtists = tag.AlbumArtists,
                    Comment = tag.Comment,
                    Composers = tag.Composers,
                    Conductor = tag.Conductor,
                    Genres = tag.Genres,
                    Performers = tag.Performers,
                    Year = tag.Year,
                },

                Technical = new TechnicalMetadata()
                {
                    AudioBitrate = file.Properties.AudioBitrate,
                    AudioChannels = file.Properties.AudioChannels,
                    Duration = file.Properties.Duration,
                    SampleRate = file.Properties.AudioSampleRate,
                    MediaTypes = file.Properties.MediaTypes.ToString(),
                    FileType = file.MimeType,
                    FileSize = stream.Length,
                    Codec = file.Properties.Codecs.FirstOrDefault()?.Description,
                },

                Extended = new ExtendedMetadata()
                {
                    TrackNumber = tag.Track,
                    TotalTracks = tag.TrackCount,
                    DiscNumber = tag.Disc,
                    TotalDiscs = tag.DiscCount,
                    BeatsPerMinute = tag.BeatsPerMinute,
                    Grouping = tag.Grouping,
                    ISRC = tag.ISRC,
                    Lyrics = tag.Lyrics,
                },

                Legal = new LegalMetadata()
                {
                    Copyright = tag.Copyright,
                    Publisher = tag.Publisher,
                },

                Visual = new VisualMetadata()
                {
                    Description = tag.Description,
                    MimeType = tag.Pictures.FirstOrDefault()?.MimeType,
                    Type = tag.Pictures.FirstOrDefault()?.Type.ToString(),
                    CoverUrl = tag.Pictures.Length > 0 ? $"data:{tag.Pictures[0].MimeType};base64,{Convert.ToBase64String(tag.Pictures[0].Data.Data)}" : null
                }
            };
        }
        catch(TagLib.CorruptFileException ex)
        {
            logger.LogError(ex, "Corrupt audio file: {FileName}", name);
            return new();
        }
        catch (TagLib.UnsupportedFormatException ex)
        {
            logger.LogError(ex, "Unsupported audio format: {FileName}", name);
            return new();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error reading audio metadata from stream.");
            return new();
        }
    }

    /// <summary>
    /// Retrieves audio metadata from a specified URL.
    /// </summary>
    /// <remarks>This method downloads the audio file from the specified URL, processes it in memory,  and
    /// extracts its metadata. Ensure that the URL points to a valid audio file.</remarks>
    /// <param name="name">The name to associate with the audio metadata.</param>
    /// <param name="url">The URL of the audio file. Can be <see langword="null"/> if no URL is provided.</param>
    /// <returns>An <see cref="AudioMetadata"/> object containing the metadata of the audio file,  or <see langword="null"/> if
    /// the metadata could not be retrieved.</returns>
    public async Task<AudioMetadata?> GetFromUrlAsync(string name, string? url)
    {
        var audioData = await httpClient.GetStreamAsync(url);
        using var ms = new MemoryStream();
        await audioData.CopyToAsync(ms);

        return GetFromStream(name, ms);
    }
}
