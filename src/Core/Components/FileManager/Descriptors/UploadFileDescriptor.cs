namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a descriptor for a file that is being uploaded,
///  containing metadata such as the file's identifier, name, size,
/// </summary>
/// <param name="Id">Identifier of the file.</param>
/// <param name="Name">Name of the file.</param>
/// <param name="Size">Size of the file in bytes.</param>
/// <param name="CreatedDate">Creation date and time of the file.</param>
/// <param name="ModifiedDate">Modification date and time of the file.</param>
/// <param name="GetBytesAsync">Function that returns a task which, when awaited,
///  provides the byte array representing the file's content. This allows for
///  asynchronous retrieval of the file data, which is particularly useful for handling
///  large files or performing I/O operations without blocking the main thread.</param>
public sealed record UploadFileDescriptor(
    string Id,
    string Name,
    long Size,
    DateTimeOffset CreatedDate,
    DateTimeOffset ModifiedDate,
    Func<Task<byte[]>> GetBytesAsync)
{
}

