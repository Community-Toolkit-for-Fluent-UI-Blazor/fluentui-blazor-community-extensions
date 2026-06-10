using Microsoft.AspNetCore.Components.Forms;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for retrieving video metadata from various sources such as URLs, files, or streams.
/// </summary>
/// <remarks>Implementations of this interface provide methods to extract video metadata, including duration,
/// format, and bitrate, from different input sources. Methods support both asynchronous and synchronous operations,
/// allowing integration with a variety of application scenarios. Callers are responsible for ensuring that input
/// sources, such as streams or files, are valid and accessible as required by each method.</remarks>
public interface IVideoMetadataProvider
{
    /// <summary>
    /// Asynchronously retrieves video metadata from the specified URL.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch and parse metadata from the provided
    /// URL. Ensure the URL points to a valid and accessible video file.</remarks>
    /// <param name="url">The URL of the video file to retrieve metadata from. Can be null or empty, in which case the method will return
    /// <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="VideoMetadata"/>
    /// object with the metadata of the video file, or <see langword="null"/> if the metadata could not be retrieved.</returns>
    Task<VideoMetadata?> GetFromUrlAsync(string? url);

    /// <summary>
    /// Asynchronously retrieves video metadata from the specified file.
    /// </summary>
    /// <param name="browserFile">The file from which to extract video metadata. Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the extracted  <see
    /// cref="VideoMetadata"/> if the operation is successful; otherwise, <see langword="null"/>  if the metadata could
    /// not be retrieved.</returns>
    Task<VideoMetadata?> GetFromFileAsync(IBrowserFile browserFile);

    /// <summary>
    /// Extracts video metadata from the specified stream.
    /// </summary>
    /// <remarks>The caller is responsible for ensuring the stream remains open and readable during the
    /// operation.  The method does not modify the position of the stream.</remarks>
    /// <param name="stream">The input stream containing video data. The stream must be readable and positioned at the start of the video
    /// content.</param>
    /// <returns>An <see cref="VideoMetadata"/> object containing the extracted metadata, such as duration, format, and bitrate.</returns>
    Task<VideoMetadata?> GetFromStreamAsync(Stream stream);
}
