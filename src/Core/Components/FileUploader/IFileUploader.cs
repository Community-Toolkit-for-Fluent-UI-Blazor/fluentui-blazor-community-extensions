using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for asynchronously uploading files with specified metadata and content.
/// </summary>
/// <remarks>Implementations of this interface initiate file uploads without blocking the calling thread.
/// Callers should ensure that the provided file name, content type, and content accurately represent the file to avoid
/// issues with file handling or client compatibility.</remarks>
public interface IFileUploader
{
    /// <summary>
    /// Initiates an asynchronous download of a file with the specified name, content type, and data.
    /// </summary>
    /// <remarks>The operation is performed asynchronously to avoid blocking the calling thread. Ensure that
    /// the content and content type accurately represent the file to prevent issues during download or file
    /// handling.</remarks>
    /// <param name="id">The name or identifier for the file being uploaded. This value is used to identify the file on the client side and may be displayed to the user during the download process.</param>
    /// <param name="contentType">The MIME type of the file content. This value informs the client how to handle or display the downloaded file.</param>
    /// <param name="content">A byte array containing the data to be uploaded as the file. Must not be null.</param>
    /// <returns>A task that represents the asynchronous file upload operation. The task result contains the identifier of the uploaded file.</returns>
    Task<string?> UploadFileAsync(string id, byte[] content, string contentType);

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

    /// <summary>
    /// Revokes the specified URL, which may be used to invalidate or remove access to a previously uploaded file. 
    /// </summary>
    /// <param name="url">The URL of the file to revoke. Can be null.</param>
    /// <returns>A task that represents the asynchronous revoke operation. The task completes when the URL has been revoked.</returns>
    Task RevokeUrlAsync(string? url);
}
