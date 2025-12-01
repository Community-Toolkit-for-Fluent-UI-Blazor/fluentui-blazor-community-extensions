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
    /// Asynchronously retrieves audio metadata from the specified URL and associates it with the given name.
    /// </summary>
    /// <param name="name">The name to assign to the retrieved audio metadata. Cannot be null or empty.</param>
    /// <param name="url">The URL from which to fetch the audio metadata. If null, the method will attempt to resolve the URL based on the
    /// provided name.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="AudioMetadata"/>
    /// instance with the retrieved metadata, or <see langword="null"/> if the metadata could not be obtained.</returns>
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
    /// Retrieves audio metadata from the specified stream and associates it with the given name.
    /// </summary>
    /// <param name="name">The name to associate with the extracted audio metadata. Cannot be null or empty.</param>
    /// <param name="stream">The stream containing audio data from which metadata will be extracted. Must be readable and positioned at the
    /// start of the audio content.</param>
    /// <returns>An AudioMetadata instance containing information extracted from the provided stream. Returns null if the stream
    /// does not contain valid audio metadata.</returns>
    AudioMetadata GetFromStream(string name, Stream stream);
}
