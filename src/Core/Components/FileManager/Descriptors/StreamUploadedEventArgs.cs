namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for an event that occurs when a file stream has been uploaded, including information about the parent
/// context and the uploaded file.
/// </summary>
/// <param name="ParentId">The identifier of the parent entity or context to which the uploaded file is associated.</param>
/// <param name="File">The candidate file that was uploaded, containing details about the file stream and its properties.</param>
public sealed record StreamUploadedEventArgs(
    string ParentId,
    UploadFileCandidate File)
{
    /// <summary>
    /// Gets the descriptor that provides metadata and configuration for the file upload operation.
    /// </summary>
    public UploadFileDescriptor? Descriptor { get; set; }
}
