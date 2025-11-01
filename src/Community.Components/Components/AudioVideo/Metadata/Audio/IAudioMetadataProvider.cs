using Microsoft.AspNetCore.Components.Forms;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines methods for retrieving audio metadata from various sources, such as URLs or uploaded files.
/// </summary>
/// <remarks>This interface provides asynchronous methods to extract metadata from audio files, including details
/// such as title, artist, album, and duration. Implementations may support different audio formats and
/// sources.</remarks>
public interface IAudioMetadataProvider
{
    /// <summary>
    /// Asynchronously retrieves audio metadata from the specified URL.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch and parse metadata from the provided
    /// URL. Ensure the URL points to a valid and accessible audio file.</remarks>
    /// <param name="url">The URL of the audio file to retrieve metadata from. Can be null or empty, in which case the method will return
    /// <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="AudioMetadata"/>
    /// object with the metadata of the audio file, or <see langword="null"/> if the metadata could not be retrieved.</returns>
    Task<AudioMetadata?> GetFromUrlAsync(string name, string? url);

    /// <summary>
    /// Asynchronously retrieves audio metadata from the specified file.
    /// </summary>
    /// <param name="browserFile">The file from which to extract audio metadata. Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the extracted  <see
    /// cref="AudioMetadata"/> if the operation is successful; otherwise, <see langword="null"/>  if the metadata could
    /// not be retrieved.</returns>
    Task<AudioMetadata?> GetFromFileAsync(IBrowserFile browserFile);

    /// <summary>
    /// Extracts audio metadata from the specified stream.
    /// </summary>
    /// <remarks>The caller is responsible for ensuring the stream remains open and readable during the
    /// operation.  The method does not modify the position of the stream.</remarks>
    /// <param name="stream">The input stream containing audio data. The stream must be readable and positioned at the start of the audio
    /// content.</param>
    /// <returns>An <see cref="AudioMetadata"/> object containing the extracted metadata, such as duration, format, and bitrate.</returns>
    AudioMetadata GetFromStream(string name, Stream stream);
}
