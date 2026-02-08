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
    /// Asynchronously initializes the component and prepares it for use.
    /// </summary>
    /// <remarks>Await this method to ensure that initialization completes before performing further
    /// operations on the component.</remarks>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    Task InitializeAsync();

    /// <summary>
    /// Asynchronously releases the resources used by the object.
    /// </summary>
    /// <remarks>This method should be called to free unmanaged resources and perform other cleanup operations
    /// asynchronously. It is recommended to await this method to ensure proper resource management.</remarks>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    ValueTask DisposeAsync();
}
