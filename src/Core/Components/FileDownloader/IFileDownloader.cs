using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for asynchronously downloading files with specified metadata and content.
/// </summary>
/// <remarks>Implementations of this interface initiate file downloads without blocking the calling thread.
/// Callers should ensure that the provided file name, content type, and content accurately represent the file to avoid
/// issues with file handling or client compatibility.</remarks>
public interface IFileDownloader
{
    /// <summary>
    /// Initiates an asynchronous download of a file with the specified name, content type, and data.
    /// </summary>
    /// <remarks>The operation is performed asynchronously to avoid blocking the calling thread. Ensure that
    /// the content and content type accurately represent the file to prevent issues during download or file
    /// handling.</remarks>
    /// <param name="filename">The name of the file to be downloaded, including its extension. This value determines the filename presented to
    /// the user.</param>
    /// <param name="contentType">The MIME type of the file content. This value informs the client how to handle or display the downloaded file.</param>
    /// <param name="content">A byte array containing the data to be downloaded as the file. Must not be null.</param>
    /// <returns>A task that represents the asynchronous file download operation.</returns>
    Task DownloadFileAsync(string filename, string contentType, byte[] content);

    /// <summary>
    /// Requests cancellation of the ongoing asynchronous operation.
    /// </summary>
    /// <remarks>Call this method to attempt to cancel an operation that is currently in progress. After
    /// cancellation, ensure that any necessary cleanup is performed. The exact behavior may depend on the
    /// implementation and the current state of the operation.</remarks>
    /// <returns>A task that represents the asynchronous cancellation request. The task completes when the cancellation process
    /// has finished.</returns>
    Task CancelAsync();

    /// <summary>
    /// Initializes the component with the specified JavaScript module reference.
    /// </summary>
    /// <param name="module">The JavaScript module reference used to set up interop functionality. Cannot be null.</param>
    void Initialize(IJSObjectReference module);
}
