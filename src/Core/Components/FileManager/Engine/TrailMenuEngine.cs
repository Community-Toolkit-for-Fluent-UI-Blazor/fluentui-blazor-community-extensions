namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to build a hierarchical trail menu structure representing the path from the root to a
/// specified file or directory entry using a file provider.
/// </summary>
/// <remarks>This class is typically used to generate breadcrumb navigation for file system-like structures. It
/// retrieves file and directory information through the specified file provider and constructs a menu structure that
/// includes each ancestor of the specified entry, allowing directories to be expanded to show their immediate
/// children.</remarks>
/// <typeparam name="TItem">The type of the file or directory entry. Must be a reference type with a parameterless constructor.</typeparam>
internal sealed class TrailMenuEngine<TItem>
    where TItem : class, new()
{
    /// <summary>
    /// Represents the file provider used to retrieve file and directory information.
    /// </summary>
    private readonly IFileProvider<TItem> _provider;

    /// <summary>
    /// Initialize a new instance of the <see cref="TrailMenuEngine{TItem}"/> class with the specified file provider.
    /// </summary>
    /// <param name="provider">The file provider used to retrieve file and directory information. Cannot be null.</param>
    public TrailMenuEngine(IFileProvider<TItem> provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Asynchronously builds a hierarchical trail menu structure representing the path from the root to the specified
    /// file or directory entry.
    /// </summary>
    /// <remarks>The returned menu structure includes each ancestor of the specified entry as a menu item,
    /// with the ability to expand directories to show their immediate children. This method is typically used to
    /// generate breadcrumb navigation for file system-like structures.</remarks>
    /// <param name="entry">The file or directory entry for which to build the trail menu. If null, the method returns null.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the root menu item of the
    /// constructed trail, or null if the entry is null.</returns>
    public async Task<ITrailMenuItem?> BuildAsync(FileEntry<TItem>? entry, CancellationToken cancellationToken)
    {
        if (entry is null)
        {
            return null;
        }

        var segments = new List<FileEntry<TItem>>();
        var current = entry;

        while (current is not null)
        {
            segments.Insert(0, current);
            current = current.Parent;
        }

        InternalTrailMenuItem? root = null;
        InternalTrailMenuItem? previous = null;

        foreach (var seg in segments)
        {
            var item = new InternalTrailMenuItem
            {
                Label = seg.Name,
                Id = seg.Id
            };

            if (seg.IsDirectory)
            {
                var descriptors = await _provider.GetChildrenAsync(seg.Id, cancellationToken);

                item.Items = descriptors
                    .Where(d => d.IsDirectory)
                    .Select(d => new InternalTrailMenuItem
                    {
                        Label = d.Name,
                        Id = d.Id,
                        Parent = item
                    })
                    .ToList();
            }

            if (previous is not null)
            {
                previous.Next = item;
                item.Parent = previous;
            }
            else
            {
                root = item;
            }

            previous = item;
        }

        return root!;
    }
}
