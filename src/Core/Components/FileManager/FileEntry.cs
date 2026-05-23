using Microsoft.AspNetCore.StaticFiles;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a file system entry (file or directory) used by the FluentCxFileManager.
/// This model is intentionally focused on data and hierarchy, not behavior.
/// </summary>
/// <typeparam name="TItem">
/// The domain item associated with this entry (for example a database product, document entity, etc.).
/// </typeparam>
public sealed class FileEntry<TItem>
    where TItem : class, new()
{
    /// <summary>
    /// Represents the collection of child file entries associated with the current item.
    /// </summary>
    private readonly List<FileEntry<TItem>> _children;

    /// <summary>
    /// Provides a mapping between file extensions and MIME content types used to determine the content type of files
    /// based on their extensions.
    /// </summary>
    /// <remarks>This provider is typically used to look up the appropriate MIME type for a given file
    /// extension when serving files in web applications.</remarks>
    private static readonly FileExtensionContentTypeProvider s_contentTypeProvider = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="FileEntry{TItem}"/> class.
    /// </summary>
    /// <param name="id">Unique identifier of the entry.</param>
    /// <param name="name">Name of the entry (including extension for files).</param>
    /// <param name="isDirectory">Value indicating whether the entry is a directory.</param>
    /// <param name="size">Size of the entry in bytes (0 for directories or unknown).</param>
    /// <param name="createdDate">Creation date of the entry.</param>
    /// <param name="modifiedDate">Last modification date of the entry.</param>
    /// <param name="item">Item associated with this entry.</param>
    /// <param name="parent">Parent entry of this entry (null for root entries).</param>
    public FileEntry(
        string id,
        string name,
        bool isDirectory,
        long size,
        DateTimeOffset createdDate,
        DateTimeOffset modifiedDate,
        TItem item,
        FileEntry<TItem>? parent = null)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        IsDirectory = isDirectory;
        Size = size;
        CreatedDate = createdDate;
        ModifiedDate = modifiedDate;
        Item = item ?? throw new ArgumentNullException(nameof(item));

        _children = [];
        Metadata = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

        Parent = parent;
    }

    /// <summary>
    /// Gets the unique identifier of the entry.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets or sets the name of the entry.
    /// For files, this includes the extension.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the file extension of the entry, including the leading dot (e.g. ".pdf"),
    /// or <see langword="null"/> if the entry has no extension.
    /// </summary>
    public string? Extension => Path.GetExtension(Name);

    /// <summary>
    /// Gets a value indicating whether this entry represents a directory.
    /// </summary>
    public bool IsDirectory { get; }

    /// <summary>
    /// Gets the size of the entry in bytes.
    /// For directories, this value is typically 0 or a computed aggregate.
    /// </summary>
    public long Size { get; private set; }

    /// <summary>
    /// Gets the creation date of the entry.
    /// </summary>
    public DateTimeOffset CreatedDate { get; }

    /// <summary>
    /// Gets the last modification date of the entry.
    /// </summary>
    public DateTimeOffset ModifiedDate { get; private set; }

    /// <summary>
    /// Gets the domain item associated with this entry.
    /// This can be any business object (for example a product, document, or database entity).
    /// </summary>
    public TItem Item { get; }

    /// <summary>
    /// Gets the parent entry of this entry, or <see langword="null"/> if this is a root entry.
    /// </summary>
    public FileEntry<TItem>? Parent { get; private set; }

    /// <summary>
    /// Gets the children of this entry.
    /// This collection is empty for file entries.
    /// </summary>
    public List<FileEntry<TItem>> Children => _children;

    /// <summary>
    /// Gets a dictionary of custom metadata associated with this entry.
    /// This can be used to store arbitrary key/value pairs such as tags, labels, or business attributes.
    /// </summary>
    public IDictionary<string, object?> Metadata { get; }

    /// <summary>
    /// Gets or sets a delegate that can asynchronously provide the raw data of the file.
    /// This is typically used for file download or preview scenarios.
    /// For directories, this value is usually <see langword="null"/>.
    /// </summary>
    public Func<Task<byte[]>>? DataProviderAsync { get; set; }

    /// <summary>
    /// Gets or sets a delegate that can synchronously provide the raw data of the file.
    /// This is an optional alternative to <see cref="DataProviderAsync"/>.
    /// </summary>
    public Func<byte[]>? DataProvider { get; set; }

    /// <summary>
    /// Adds a child entry to this entry.
    /// This method is intended to be used by higher-level services that build the hierarchy.
    /// </summary>
    /// <param name="child">The child entry to add.</param>
    public void AddChild(FileEntry<TItem> child)
    {
        ArgumentNullException.ThrowIfNull(child);

        if (!IsDirectory)
        {
            throw new InvalidOperationException("Cannot add a child to a file entry. Only directories can contain children.");
        }

        if (_children.Contains(child, FileEntryComparer<TItem>.Default))
        {
            return;
        }

        child.Parent = this;
        _children.Add(child);
    }

    /// <summary>
    /// Renames the entry while preserving its extension for files.
    /// </summary>
    /// <param name="newNameWithoutExtension">The new name without extension.</param>
    public void Rename(string newNameWithoutExtension)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newNameWithoutExtension, nameof(newNameWithoutExtension));

        if (string.IsNullOrEmpty(Extension))
        {
            Name = newNameWithoutExtension;
        }
        else
        {
            Name = $"{newNameWithoutExtension}{Extension}";
        }
    }

    /// <summary>
    /// Removes the specified child entry from this directory.
    /// </summary>
    /// <param name="child">The child entry to remove.</param>
    /// <returns>
    /// Returns <see langword="true"/> if the child was removed successfully,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public bool RemoveChild(FileEntry<TItem> child)
    {
        ArgumentNullException.ThrowIfNull(child);

        if (!IsDirectory)
        {
            throw new InvalidOperationException("Cannot remove a child from a file entry. Only directories can contain children.");
        }

        var removed = _children.RemoveAll(c => c.Id == child.Id) > 0;

        if (removed)
        {
            child.Parent = null;
        }

        return removed;
    }

    /// <summary>
    /// Updates the size of the entry.
    /// This is typically used by higher-level services when recalculating directory sizes.
    /// </summary>
    /// <param name="size">The new size in bytes.</param>
    public void SetSize(long size)
    {
        if (size < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), "Size cannot be negative.");
        }

        Size = size;
    }

    /// <summary>
    /// Determines whether the current entry is an ancestor of the specified target entry in the file hierarchy.
    /// </summary>
    /// <remarks>An ancestor is any entry that appears in the parent chain of the target entry. If target is
    /// null, the method returns false.</remarks>
    /// <param name="target">The entry to test for ancestry. Can be null.</param>
    /// <returns>true if the current entry is an ancestor of the target entry; otherwise, false.</returns>
    public bool IsAncestorOf(FileEntry<TItem>? target)
    {
        if (target is null)
        {
            return false;
        }

        var current = target.Parent;

        while (current is not null)
        {
            if (string.Equals(current.Id, Id, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            current = current.Parent;
        }

        return false;
    }

    /// <summary>
    /// Returns a string representation of the entry for debugging purposes.
    /// </summary>
    public override string ToString()
    {
        return IsDirectory
            ? $"[Dir] {Name} (Id={Id})"
            : $"[File] {Name} ({Size} bytes, Id={Id})";
    }

    /// <summary>
    /// Sets the modification date to the specified UTC date and time.
    /// </summary>
    /// <param name="utcNow">The UTC date and time to assign as the modification date.</param>
    internal void SetModifiedDate(DateTime utcNow)
    {
        ModifiedDate = utcNow;
    }

    /// <summary>
    /// Calculates and updates the size of the specified directory entry
    ///  by recursively summing the sizes of its child entries.
    /// </summary>
    /// <param name="entry">Entry for which to recalculate the size. Must be a directory entry.</param>
    /// <returns>Returns the total size of the directory after recalculation.</returns>
    private static long ComputeDirectorySize(FileEntry<TItem> entry)
    {
        if (!entry.IsDirectory)
        {
            return entry.Size;
        }

        var total = 0L;

        foreach (var child in entry.Children)
        {
            total += ComputeDirectorySize(child);
        }

        entry.SetSize(total);

        return total;
    }

    /// <summary>
    /// Recalculates the sizes of the specified directory entry and all its ancestor directories.
    /// </summary>
    /// <remarks>This method updates the size information for the provided entry and propagates the
    /// recalculation up the directory hierarchy to ensure that all parent directories reflect the correct total
    /// size.</remarks>
    public void RecalculateSizes()
    {
        ComputeDirectorySize(this);

        var parent = Parent;

        while (parent is not null)
        {
            ComputeDirectorySize(parent);
            parent = parent.Parent;
        }
    }

    /// <summary>
    /// Collect the parent entries of the specified file entries,
    ///  returning a set of unique parent entries that may be affected by
    ///  operations on the provided entries.
    /// </summary>
    /// <param name="entries">Entries for which to capture parent entries.</param>
    /// <returns>Returns a set of unique parent entries associated with the provided file entries. The set will be empty if no parent entries are found.</returns>
    public static HashSet<FileEntry<TItem>> CaptureParents(IEnumerable<FileEntry<TItem>> entries)
    {
        HashSet<FileEntry<TItem>> parents = [];

        foreach (var entry in entries)
        {
            var parent = entry.Parent;

            while (parent is not null)
            {
                parents.Add(parent);
                parent = parent.Parent;
            }
        }

        return parents;
    }

    /// <summary>
    /// Retrieves the content type for a given file name using the content type provider,
    ///  defaulting to "application/octet-stream" if the content type cannot be determined.
    /// </summary>
    /// <returns>Returns the content type as a string if determined; otherwise, returns "application/octet-stream".</returns>
    public string GetContentType()
    {
        return s_contentTypeProvider.TryGetContentType(Name, out var ct)
            ? ct : "application/octet-stream";
    }

    /// <summary>
    /// Retrieves the content of the specified file entry by invoking its data provider.
    /// </summary>
    /// <returns>Returns a byte array containing the content of the file entry.
    ///  If no data provider is set, returns an empty byte array.</returns>
    public async Task<byte[]> GetContentAsync()
    {
        if (DataProviderAsync is not null)
        {
            return await DataProviderAsync();
        }

        if (DataProvider is not null)
        {
            return DataProvider();
        }

        return [];
    }

    /// <summary>
    /// Creates a deep copy of the current file or directory entry, including all child entries.
    /// </summary>
    /// <remarks>The cloned entry and its children are independent of the original instances. Changes to the
    /// clone or its descendants do not affect the original tree, and vice versa.</remarks>
    /// <returns>A new instance of <see cref="FileEntry{TItem}"/> that is a deep copy of the current entry and its children.</returns>
    public FileEntry<TItem> Clone()
    {
        return new FileEntry<TItem>(
            id: Id,
            name: Name,
            isDirectory: IsDirectory,
            size: Size,
            createdDate: CreatedDate,
            modifiedDate: ModifiedDate,
            item: Item,
            parent: Parent);
    }
}
