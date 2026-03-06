namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a service for searching files within a hierarchical file structure using a specified query and search
/// options.
/// </summary>
/// <remarks>Implementations of this interface should support asynchronous enumeration of search results, enabling
/// efficient processing of large file structures. The search behavior and supported query syntax are determined by the
/// implementation and the provided options.</remarks>
/// <typeparam name="TItem">The type of the data associated with each file entry. Must be a reference type.</typeparam>
public interface IFileSearchService<TItem>
    where TItem : class, new()
{
    /// <summary>
    /// Asynchronously searches the file tree starting from the specified root entry for items matching the given query
    /// and search options.
    /// </summary>
    /// <param name="root">The root file or directory entry from which the search operation begins. Cannot be null.</param>
    /// <param name="query">The search query used to match file or directory names. The interpretation of the query depends on the provided
    /// options.</param>
    /// <param name="metadataExtractor">A function that extracts a string representation of the metadata from an item of type <typeparamref name="TItem"/>.</param>
    /// <param name="options">The options that control the search behavior, such as case sensitivity, search depth, or file type filters.
    /// Cannot be null.</param>
    /// <returns>An asynchronous stream of file entries that match the search criteria. The stream may be empty if no entries
    /// match.</returns>
    IAsyncEnumerable<FileEntry<TItem>> SearchAsync(
        FileEntry<TItem> root,
        string query,
        Func<TItem, string> metadataExtractor,
        FileSearchOptions options);
}
