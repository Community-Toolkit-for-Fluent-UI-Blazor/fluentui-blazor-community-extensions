namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a service for asynchronously enumerating all file entries within a subtree rooted at a specified entry.
/// </summary>
/// <typeparam name="TItem">The type of the item associated with each file entry. Must be a reference type.</typeparam>
public interface IFileEntryFlattenService<TItem>
    where TItem : class, new()
{
    /// <summary>
    /// Enumerates all entries in the subtree rooted at <paramref name="root"/>.
    /// </summary>
    IAsyncEnumerable<FileEntry<TItem>> EnumerateAsync(FileEntry<TItem> root);
}
