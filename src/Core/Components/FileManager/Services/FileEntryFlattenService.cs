namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a service for recursively enumerating file entries in a hierarchical structure, flattening directory trees
/// into a single sequence.
/// </summary>
/// <remarks>This service uses an injected file provider to retrieve child entries for directories. It yields each
/// file and directory in a depth-first traversal, starting from the specified root entry. The service is typically used
/// to process or display all files and directories in a tree structure as a flat sequence.</remarks>
/// <typeparam name="TItem">The type of the item associated with each file entry. Must be a reference type.</typeparam>
internal sealed class FileEntryFlattenService<TItem> : IFileEntryFlattenService<TItem>
    where TItem : class, new()
{
    /// <summary>
    /// Provides access to the file provider used for operations involving items of type TItem.
    /// </summary>
    private readonly IFileProvider<TItem> _provider;

    /// <summary>
    /// Initializes a new instance of the FileEntryFlattenService class using the specified file provider.
    /// </summary>
    /// <param name="provider">The file provider used to retrieve file entries of type TItem. Cannot be null.</param>
    public FileEntryFlattenService(IFileProvider<TItem> provider)
    {
        _provider = provider;
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<FileEntry<TItem>> EnumerateAsync(FileEntry<TItem> root)
    {
        if (root is null)
        {
            yield break;
        }

        yield return root;

        if (!root.IsDirectory)
        {
            yield break;
        }

        var descriptors = await _provider.GetChildrenAsync(root.Id);

        foreach (var desc in descriptors)
        {
            var child = FileManagerEngine<TItem>.Map(desc, root);
            root.AddChild(child);

            yield return child;

            if (child.IsDirectory)
            {
                await foreach (var sub in EnumerateAsync(child))
                {
                    yield return sub;
                }
            }
        }
    }
}
