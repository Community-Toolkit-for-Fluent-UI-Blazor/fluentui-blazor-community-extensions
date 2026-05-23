namespace FluentUI.Blazor.Community.Components.Components.FileManager.Services;

/// <summary>
/// Defines a service for creating a ZIP archive from a collection of file entries of a specified type.
/// </summary>
/// <remarks>Implementations of this interface provide functionality to compress multiple file entries into a
/// single ZIP archive. The generic type parameter allows the service to be used with different file entry content
/// types.</remarks>
/// <typeparam name="TItem">The type of the file entry content. Must be a reference type.</typeparam>
public interface IFileEntryZipService<TItem>
    where TItem : class, new()
{
    /// <summary>
    /// Creates a compressed archive containing the specified file entries asynchronously.
    /// </summary>
    /// <remarks>The returned archive will contain all provided entries. The operation is performed
    /// asynchronously and may be awaited.</remarks>
    /// <param name="entries">The collection of file entries to include in the resulting archive. Cannot be null or contain null elements.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a file entry representing the
    /// created archive.</returns>
    ValueTask<FileEntry<TItem>> ZipAsync(IEnumerable<FileEntry<TItem>> entries);
}

