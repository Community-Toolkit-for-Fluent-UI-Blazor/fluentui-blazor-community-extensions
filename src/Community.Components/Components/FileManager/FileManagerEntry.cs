using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an entry for the <see cref="FluentCxFileManager{TItem}"/> component.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public sealed class FileManagerEntry<TItem>
    : IEqualityComparer<FileManagerEntry<TItem>> where TItem : class, new()
{
    /// <summary>
    /// Represents the files inside the current instance.
    /// </summary>
    private readonly List<FileManagerEntry<TItem>> _files = [];

    /// <summary>
    /// Represents the directories inside the current instance.
    /// </summary>
    private readonly List<FileManagerEntry<TItem>> _directories = [];

    /// <summary>
    /// Gets the provider of content type from a file extension.
    /// </summary>
    private static readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider = new();

    /// <summary>
    /// Represents the list of files and directories of the current instance.
    /// </summary>
    private readonly List<FileManagerEntry<TItem>> _merged = [];

    /// <summary>
    /// Prevents the creation of an instance of the class <see cref="FileManagerEntry{TItem}"/>.
    /// </summary>
    private FileManagerEntry()
    {
        Data = default!;
        Name = string.Empty;
    }

    /// <summary>
    /// Prevents the creation of an instance of the class <see cref="FileManagerEntry{TItem}"/>.
    /// </summary>
    /// <param name="itemData">Represents the data inside the current instance.</param>
    /// <param name="dataBytesAsyncFunc">Represents the function to get the data of the entry.</param>
    /// <param name="name">Name of the entry.</param>
    /// <param name="isDirectory">Value indicating if the entry is a directory or a file.</param>
    /// <param name="size">Size of the entry.</param>
    /// <param name="createdDate">Creation date of the item of the entry.</param>
    /// <param name="modifiedDate">Modification date of the item of the entry.</param>
    private FileManagerEntry(
       TItem itemData,
       Func<Task<byte[]>>? dataBytesAsyncFunc,
       string name,
       bool isDirectory,
       long size,
       DateTime createdDate,
       DateTime modifiedDate)
    {
        DataBytesAsyncFunc = dataBytesAsyncFunc;
        Size = size;
        CreatedDate = createdDate;
        ModifiedDate = modifiedDate;
        Name = name;
        IsDirectory = isDirectory;
        Data = itemData;
    }

    /// <summary>
    /// Prevents the creation of an instance of the class <see cref="FileManagerEntry{TItem}"/>.
    /// </summary>
    /// <param name="itemData">Represents the data inside the current instance.</param>
    /// <param name="dataBytes">Represents the data of the entry.</param>
    /// <param name="name">Name of the entry.</param>
    /// <param name="isDirectory">Value indicating if the entry is a directory or a file.</param>
    /// <param name="size">Size of the entry.</param>
    /// <param name="createdDate">Creation date of the item of the entry.</param>
    /// <param name="modifiedDate">Modification date of the item of the entry.</param>
    private FileManagerEntry(
        TItem itemData,
        byte[]? dataBytes,
        string name,
        bool isDirectory,
        long size,
        DateTime createdDate,
        DateTime modifiedDate)
    {
        DataBytes = dataBytes;
        Size = size;
        CreatedDate = createdDate;
        ModifiedDate = modifiedDate;
        Name = name;
        IsDirectory = isDirectory;
        Data = itemData;
    }

    /// <summary>
    /// Gets the home entry.
    /// </summary>
    public static FileManagerEntry<TItem> Home => CreateHomeDirectory();

    /// <summary>
    /// Gets the parent of this entry.
    /// </summary>
    public FileManagerEntry<TItem>? Parent { get; private set; }

    /// <summary>
    /// Gets the total number of entries.
    /// </summary>
    public int TotalEntriesCount => _files.Count + _directories.Count;

    /// <summary>
    /// Gets the function which retrieves the raw data of the entry asynchronously.
    /// </summary>
    private Func<Task<byte[]>>? DataBytesAsyncFunc { get; }

    /// <summary>
    /// Gets the data of the entry.
    /// </summary>
    private byte[]? DataBytes { get; }

    /// <summary>
    /// Gets the size of the entry.
    /// </summary>
    public long Size { get; private set; }

    /// <summary>
    /// Gets the data instance of the entry.
    /// </summary>
    public TItem Data { get; }

    /// <summary>
    /// Gets the creation date of the entry.
    /// </summary>
    public DateTime CreatedDate { get; }

    /// <summary>
    /// Gets the modified date of the entry.
    /// </summary>
    public DateTime ModifiedDate { get; }

    /// <summary>
    /// Gets the content type of the entry.
    /// </summary>
    public string ContentType => GetContentType();

    /// <summary>
    /// Gets the name of the entry.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the name of the file without its extension.
    /// </summary>
    internal string NameWithoutExtension => Path.GetFileNameWithoutExtension(Name);

    /// <summary>
    /// Gets or sets the number of file which contains the same name.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Gets a value indicating if the entry has an extension.
    /// </summary>
    public bool HasExtension => !string.IsNullOrEmpty(Path.GetExtension(Name));

    /// <summary>
    /// Gets the extension of the entry.
    /// </summary>
    public string? Extension => Path.GetExtension(Name);

    /// <summary>
    /// Gets a value indicating if the entry is a directory or not.
    /// </summary>
    public bool IsDirectory { get; }

    /// <summary>
    /// Gets a value indicating if the entry has directories.
    /// </summary>
    internal bool HasDirectory => _directories is not null && _directories.Count > 0;

    /// <summary>
    /// Gets a value indicating if the entry has files.
    /// </summary>
    internal bool HasFiles => _files is not null && _files.Count > 0;

    /// <summary>
    /// Gets or sets the identifier of the entry.
    /// </summary>
    public string Id { get; set; } = Identifier.NewId();

    /// <summary>
    /// Gets the relative path of the entry.
    /// </summary>
    internal string? RelativePath { get; private set; }

    /// <summary>
    /// Gets the content type of the entry.
    /// </summary>
    /// <returns>Returns the content type of the entry.</returns>
    private string GetContentType()
    {
        if (string.IsNullOrEmpty(Extension))
        {
            return string.Empty;
        }

        if (_fileExtensionContentTypeProvider.TryGetContentType(Extension, out var contentType))
        {
            return contentType;
        }

        return "application/octet-stream";
    }

    /// <summary>
    /// Gets the merged list of entries inside the current entry.
    /// </summary>
    /// <returns>Returns the merged entries.</returns>
    private IEnumerable<FileManagerEntry<TItem>> GetMerged()
    {
        if (!HasDirectory)
        {
            return HasFiles ? _files : [];
        }

        if (!HasFiles)
        {
            return HasDirectory ? _directories : [];
        }

        return _files.Union(_directories);
    }

    /// <summary>
    /// Remove the <paramref name="entry"/> from specified <paramref name="root"/>;
    /// </summary>
    /// <param name="root">Root where the entry to remove is.</param>
    /// <param name="entry">Entry to remove.</param>
    /// <returns>Returns <see langword="true"/> if the entry was successfully removed, <see langword="false" /> sinon.</returns>
    private static bool RemoveEntry(
        FileManagerEntry<TItem> root,
        FileManagerEntry<TItem> entry)
    {
        static bool StringEquals(string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

        if (StringEquals(root.Id, entry.Id))
        {
            root.Clear();
            root.InvalidateSize();
            return true;
        }

        for (var i = root._files.Count - 1; i >= 0; i--)
        {
            var f = root._files[i];

            if (StringEquals(f.Id, entry.Id))
            {
                var removed = root._files.Remove(f);
                f.Parent = null;
                root.InvalidateSize();

                return removed;
            }
        }

        for (var i = root._directories.Count - 1; i >= 0; i--)
        {
            var d = root._directories[i];

            if (StringEquals(d.Id, entry.Id))
            {
                var removed = root._directories.Remove(d);
                d.Parent = null;
                root.InvalidateSize();

                return removed;
            }

            var childRemoved = RemoveEntry(d, entry);

            if (childRemoved)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Creates a directory inside the current entry.
    /// </summary>
    /// <param name="creationDate">Date of creation of the directory.</param>
    /// <param name="modificationDate">Modification date of the directory.</param>
    /// <param name="directoryName">Name of the directory.</param>
    /// <param name="size">Size of the directory.</param>
    /// <returns>Returns the newly created entry.</returns>
    public FileManagerEntry<TItem> CreateDirectory(
        DateTime creationDate,
        DateTime modificationDate,
        string directoryName,
        long size = 0L)
    {
        var entry = new FileManagerEntry<TItem>(
            Activator.CreateInstance<TItem>(),
            (byte[]?)null,
            directoryName,
            true,
            size,
            creationDate,
            modificationDate);

        Add(entry);

        return entry;
    }

    /// <summary>
    /// Creates a directory inside the current entry.
    /// </summary>
    /// <param name="directoryName">Name of the directory.</param>
    /// <returns>Returns the newly created entry.</returns>
    public FileManagerEntry<TItem> CreateDirectory(
       string directoryName)
    {
        return CreateDirectory(DateTime.Now, DateTime.Now, directoryName, 0L);
    }

    /// <summary>
    /// Creates the home directory. 
    /// </summary>
    /// <returns></returns>
    private static FileManagerEntry<TItem> CreateHomeDirectory()
    {
        return new FileManagerEntry<TItem>(
            Activator.CreateInstance<TItem>(),
            (byte[]?)null,
            "Home",
            true,
            0L,
            DateTime.Now,
            DateTime.Now)
        {
            RelativePath = "Home"
        };
    }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is FileManagerEntry<TItem> m)
        {
            return this == m;
        }

        return false;
    }

    /// <summary>
    /// Check if <paramref name="x"/> is equal to <paramref name="y"/>.
    /// </summary>
    /// <param name="x">Left entry to compare with.</param>
    /// <param name="y">Right entry to compare with.</param>
    /// <returns>Returns <see langword="true"/> if the entries are equal, <see langword="false"/> otherwise.</returns>
    public static bool operator ==(FileManagerEntry<TItem>? x, FileManagerEntry<TItem>? y)
    {
        if (x is null || y is null)
        {
            return false;
        }

        return string.Equals(x.Id, y.Id, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Check if <paramref name="x"/> is inequal to <paramref name="y"/>.
    /// </summary>
    /// <param name="x">Left entry to compare with.</param>
    /// <param name="y">Right entry to compare with.</param>
    /// <returns>Returns <see langword="true"/> if the entries are not equal, <see langword="false"/> otherwise.</returns>

    public static bool operator !=(FileManagerEntry<TItem>? x, FileManagerEntry<TItem>? y)
    {
        return !(x == y);
    }

    /// <inheritdoc />
    public bool Equals(FileManagerEntry<TItem>? x, FileManagerEntry<TItem>? y)
    {
        if (x is null || y is null)
        {
            return false;
        }

        return x == y;
    }

    /// <inheritdoc />
    public int GetHashCode([DisallowNull] FileManagerEntry<TItem> obj)
    {
        return HashCode.Combine(obj.Name, obj.CreatedDate, obj.ModifiedDate, obj.Size);
    }

    /// <summary>
    /// Gets the raw data of the entry in an asynchronous way.
    /// </summary>
    /// <returns>Returns the raw data of the entry.</returns>
    public async Task<byte[]> GetBytesAsync()
    {
        if (DataBytesAsyncFunc is not null)
        {
            return await DataBytesAsyncFunc();
        }

        if (DataBytes is not null)
        {
            return DataBytes;
        }

        return [];
    }

    /// <summary>
    /// Sets the name of the entry.
    /// </summary>
    /// <param name="newName">Name of the entry.</param>
    internal void SetName(string newName)
    {
        if (!string.IsNullOrEmpty(Extension))
        {
            Name = $"{newName}{Extension}";
        }
        else
        {
            Name = newName;
        }
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return GetHashCode(this);
    }

    /// <summary>
    /// Adds an entry in the current instance.
    /// </summary>
    /// <param name="entry">Entry to add.</param>
    private void Add(FileManagerEntry<TItem> entry)
    {
        entry.Parent = this;

        if (!entry.IsDirectory)
        {
            _files.Add(entry);
        }
        else
        {
            entry.RelativePath = $"{RelativePath}{Path.DirectorySeparatorChar}{entry.Name}";
            _directories.Add(entry);
        }
    }

    /// <summary>
    /// Adds a range of entries in the current instance.
    /// </summary>
    /// <param name="entries">Entries to add.</param>
    public void AddRange(params FileManagerEntry<TItem>[]? entries)
    {
        if (entries is null ||
            entries.Length == 0)
        {
            return;
        }

        foreach (var entry in entries)
        {
            entry.Parent = this;
            Add(entry);
        }

        InvalidateMerge();
        InvalidateSize();
    }

    /// <summary>
    /// Clears the current entry.
    /// </summary>
    public void Clear()
    {
        _files.Clear();
        _directories.Clear();

        InvalidateMerge();
        InvalidateSize();
    }

    /// <summary>
    /// Gets the directories of the current entry.
    /// </summary>
    /// <returns>Returns the directories of the current entry.</returns>
    public IEnumerable<FileManagerEntry<TItem>> GetDirectories()
    {
        return _directories;
    }

    /// <summary>
    /// Gets the files of the current entry.
    /// </summary>
    /// <returns>Returns the files of the current entry.</returns>
    public IEnumerable<FileManagerEntry<TItem>> GetFiles()
    {
        return _files;
    }

    /// <summary>
    /// Removes the entry from the current entry.
    /// </summary>
    /// <param name="entry">Entry to remove.</param>
    public void Remove(FileManagerEntry<TItem> entry)
    {
        bool removed;

        if (entry.IsDirectory)
        {
            removed = _directories.Remove(entry);
        }
        else
        {
            removed = _files.Remove(entry);
        }

        if (removed)
        {
            entry.Parent = null;
            InvalidateMerge();
            InvalidateSize();
        }
    }

    /// <summary>
    /// Removes all specified entries from the current entry.
    /// </summary>
    /// <param name="entries">Entries to removed.</param>
    public void Remove(IEnumerable<FileManagerEntry<TItem>> entries)
    {
        var removed = false;

        foreach (var entry in entries)
        {
            removed |= RemoveEntry(this, entry);
        }

        if (removed)
        {
            InvalidateMerge();
            InvalidateSize();
        }
    }

    /// <summary>
    /// Invalidate the merged entries.
    /// </summary>
    internal void InvalidateMerge()
    {
        _merged.Clear();
        _merged.AddRange(GetMerged());
    }

    /// <summary>
    /// Enumerates all the entries in the current entries.
    /// </summary>
    /// <returns></returns>
    internal IList<FileManagerEntry<TItem>> Enumerate()
    {
        return _merged;
    }

    /// <summary>
    /// Sorts the entries.
    /// </summary>
    /// <param name="sortBy">Type of sort.</param>
    /// <param name="sortMode">Ascending or descending sort.</param>
    internal void Sort(FileSortMode sortMode, FileSortBy sortBy)
    {
        _merged.Sort(FileManagerEntryComparer<TItem>.Default.WithSortBy(sortBy).WithSortMode(sortMode));
    }

    /// <summary>
    /// Finds the entry specified <paramref name="id"/> by starting from <paramref name="root"/>.
    /// </summary>
    /// <param name="root">Entry to use as a search entry.</param>
    /// <param name="id">Identifier to find.</param>
    /// <returns>Returns the entry if found, <see langword="null" /> otherwise.</returns>
    public static FileManagerEntry<TItem>? Find(FileManagerEntry<TItem> root, string id)
    {
        if (string.Equals(root.Id, id, StringComparison.OrdinalIgnoreCase))
        {
            return root;
        }

        return Find(root.Enumerate(), id);
    }

    /// <summary>
    /// Finds an entry from the <paramref name="root"/>.
    /// </summary>
    /// <param name="root">Entry to use as a search entry.</param>
    /// <param name="predicate">Predicate to apply to find the entry.</param>
    /// <returns>Returns the entry if found, <see langword="null" /> otherwise.</returns>
    public static FileManagerEntry<TItem>? Find(FileManagerEntry<TItem> root, Func<FileManagerEntry<TItem>, bool> predicate)
    {
        if (predicate(root))
        {
            return root;
        }

        return Find(root.Enumerate(), predicate);
    }

    /// <summary>
    /// Finds the entry specified with <paramref name="id"/> inside the specified <paramref name="items"/>.
    /// </summary>
    /// <param name="items">List of items where the search begins.</param>
    /// <param name="id">Identifier of the entry to find.</param>
    /// <returns>Returns the entry if found, <see langword="null" /> otherwise.</returns>
    public static FileManagerEntry<TItem>? Find(IEnumerable<FileManagerEntry<TItem>> items, string id)
    {
        foreach (var item in items)
        {
            if (string.Equals(item.Id, id, StringComparison.OrdinalIgnoreCase))
            {
                return item;
            }

            foreach (var subItem in item._files)
            {
                if (string.Equals(subItem.Id, id, StringComparison.OrdinalIgnoreCase))
                {
                    return subItem;
                }
            }

            var entry = Find(item._directories, id);

            if (entry is not null)
            {
                return entry;
            }
        }

        return null;
    }

    /// <summary>
    /// Finds an entry from the <paramref name="items"/>.
    /// </summary>
    /// <param name="items">List of entries to use.</param>
    /// <param name="predicate">Predicate to apply to find the entry.</param>
    /// <returns>Returns the entry if found, <see langword="null" /> otherwise.</returns>
    public static FileManagerEntry<TItem>? Find(
        IEnumerable<FileManagerEntry<TItem>> items,
        Func<FileManagerEntry<TItem>, bool> predicate)
    {
        foreach (var item in items)
        {
            if (predicate(item))
            {
                return item;
            }

            foreach (var subItem in item._files)
            {
                if (predicate(item))
                {
                    return subItem;
                }
            }

            var entry = Find(item._directories, predicate);

            if (entry is not null)
            {
                return entry;
            }
        }

        return null;
    }

    /// <summary>
    /// Finds an entry from its name.
    /// </summary>
    /// <param name="entry">Entry to use as search entry.</param>
    /// <param name="name">Name of the entry.</param>
    /// <returns>Returns the entry if found, <see langword="null" /> otherwise.</returns>
    public static IEnumerable<FileManagerEntry<TItem>> FindByName(FileManagerEntry<TItem>? entry, string name)
    {
        if (entry is null)
        {
            return [];
        }

        return Find(entry._files, name).Union(Find(entry._directories, name));

        static IEnumerable<FileManagerEntry<TItem>> Find(IEnumerable<FileManagerEntry<TItem>> items, string name)
        {
            foreach (var item in items)
            {
                if (item.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase))
                {
                    yield return item;
                }
            }
        }
    }

    /// <summary>
    /// Creates a default file entry.
    /// </summary>
    /// <param name="data">Data of the entry.</param>
    /// <param name="name">Name of the entry.</param>
    /// <param name="length">Size of the entry.</param>
    /// <returns>Returns the newly created entry.</returns>
    public FileManagerEntry<TItem> CreateDefaultFileEntry(
        byte[] data,
        string name,
        int length)
    {
        var entry = new FileManagerEntry<TItem>(Activator.CreateInstance<TItem>(), data, name, false, length, DateTime.Now, DateTime.Now);
        Add(entry);

        return entry;
    }

    /// <summary>
    /// Creates a default file entry.
    /// </summary>
    /// <param name="value">Function to retrieve the data of the entry.</param>
    /// <param name="name">Name of the entry.</param>
    /// <param name="length">Size of the entry.</param>
    /// <returns>Returns the newly created entry.</returns>
    public FileManagerEntry<TItem> CreateDefaultFileEntry(
        Func<Task<byte[]>> value,
        string name,
        long length)
    {
        var entry = new FileManagerEntry<TItem>(Activator.CreateInstance<TItem>(), value, name, false, length, DateTime.Now, DateTime.Now);
        Add(entry);

        return entry;
    }

    /// <summary>
    /// Creates a default file entry.
    /// </summary>
    /// <param name="value">Function to retrieve the data of the entry.</param>
    /// <param name="name">Name of the entry.</param>
    /// <param name="length">Size of the entry.</param>
    /// <param name="creationDate">Date of the creation of the entry.</param>
    /// <param name="lastModificationDate">Date of the last modification of the entry.</param>
    /// <returns>Returns the newly created entry.</returns>
    public FileManagerEntry<TItem> CreateDefaultFileEntry(
        Func<Task<byte[]>> value,
        string name,
        long length,
        DateTime creationDate,
        DateTime lastModificationDate)
    {
        var entry = new FileManagerEntry<TItem>(Activator.CreateInstance<TItem>(), value, name, false, length, creationDate, lastModificationDate);
        Add(entry);

        return entry;
    }

    /// <summary>
    /// Creates a default file entry.
    /// </summary>
    /// <param name="data">Instance of the item.</param>
    /// <param name="value">Function to retrieve the data of the entry.</param>
    /// <param name="name">Name of the entry.</param>
    /// <param name="length">Size of the entry.</param>
    /// <param name="creationDate">Date of the creation of the entry.</param>
    /// <param name="lastModificationDate">Date of the last modification of the entry.</param>
    /// <returns>Returns the newly created entry.</returns>
    public FileManagerEntry<TItem> CreateDefaultFileEntryWithData(
        TItem data,
       Func<Task<byte[]>> value,
       string name,
       long length,
       DateTime creationDate,
       DateTime lastModificationDate)
    {
        var entry = new FileManagerEntry<TItem>(data, value, name, false, length, creationDate, lastModificationDate);
        Add(entry);

        return entry;
    }

    /// <summary>
    /// Creates a default file entry.
    /// </summary>
    /// <param name="data">Instance of the item.</param>
    /// <param name="value">Raw data of the entry.</param>
    /// <param name="name">Name of the entry.</param>
    /// <param name="length">Size of the entry.</param>
    /// <param name="creationDate">Date of the creation of the entry.</param>
    /// <param name="lastModificationDate">Date of the last modification of the entry.</param>
    /// <returns>Returns the newly created entry.</returns>
    public FileManagerEntry<TItem> CreateDefaultFileEntryWithData(
        TItem data,
       byte[] value,
       string name,
       long length,
       DateTime creationDate,
       DateTime lastModificationDate)
    {
        var entry = new FileManagerEntry<TItem>(data, value, name, false, length, creationDate, lastModificationDate);
        Add(entry);

        return entry;
    }

    /// <summary>
    /// Creates a default file entry.
    /// </summary>
    /// <param name="data">Instance of the item.</param>
    /// <param name="value">Function to retrieve the data of the entry.</param>
    /// <param name="name">Name of the entry.</param>
    /// <param name="length">Size of the entry.</param>
    /// <param name="creationDate">Date of the creation of the entry.</param>
    /// <param name="lastModificationDate">Date of the last modification of the entry.</param>
    /// <returns>Returns the newly created entry.</returns>
    public static FileManagerEntry<TItem> CreateEntryWithData(
        TItem data,
       Func<Task<byte[]>> value,
       string name,
       long length,
       DateTime creationDate,
       DateTime lastModificationDate)
    {
        return new FileManagerEntry<TItem>(data, value, name, false, length, creationDate, lastModificationDate);
    }

    /// <summary>
    /// Creates a default file entry.
    /// </summary>
    /// <param name="data">Instance of the item.</param>
    /// <param name="value">Function to retrieve the data of the entry.</param>
    /// <param name="name">Name of the entry.</param>
    /// <param name="length">Size of the entry.</param>
    /// <param name="creationDate">Date of the creation of the entry.</param>
    /// <param name="lastModificationDate">Date of the last modification of the entry.</param>
    /// <returns>Returns the newly created entry.</returns>
    public static FileManagerEntry<TItem> CreateEntryWithData(
        TItem data,
       byte[] value,
       string name,
       long length,
       DateTime creationDate,
       DateTime lastModificationDate)
    {
        return new FileManagerEntry<TItem>(data, value, name, false, length, creationDate, lastModificationDate);
    }

    /// <summary>
    /// Creates a default file entry.
    /// </summary>
    /// <param name="value">Function to retrieve the data of the entry.</param>
    /// <param name="name">Name of the entry.</param>
    /// <param name="length">Size of the entry.</param>
    /// <param name="creationDate">Date of the creation of the entry.</param>
    /// <param name="lastModificationDate">Date of the last modification of the entry.</param>
    /// <returns>Returns the newly created entry.</returns>
    public static FileManagerEntry<TItem> CreateEntry(
       Func<Task<byte[]>> value,
       string name,
       long length,
       DateTime creationDate,
       DateTime lastModificationDate)
    {
        return new FileManagerEntry<TItem>(Activator.CreateInstance<TItem>(), value, name, false, length, creationDate, lastModificationDate);
    }

    /// <summary>
    /// Creates a default file entry.
    /// </summary>
    /// <param name="value">Function to retrieve the data of the entry.</param>
    /// <param name="name">Name of the entry.</param>
    /// <param name="length">Size of the entry.</param>
    /// <returns>Returns the newly created entry.</returns>
    public static FileManagerEntry<TItem> CreateEntry(
       Func<Task<byte[]>> value,
       string name,
       long length)
    {
        return new FileManagerEntry<TItem>(Activator.CreateInstance<TItem>(), value, name, false, length, DateTime.Now, DateTime.Now);
    }

    /// <summary>
    /// Creates a default file entry.
    /// </summary>
    /// <param name="value">Raw data of the entry.</param>
    /// <param name="name">Name of the entry.</param>
    /// <param name="length">Size of the entry.</param>
    /// <returns>Returns the newly created entry.</returns>
    public static FileManagerEntry<TItem> CreateEntry(
       byte[] value,
       string name,
       long length)
    {
        return new FileManagerEntry<TItem>(Activator.CreateInstance<TItem>(), value, name, false, length, DateTime.Now, DateTime.Now);
    }

    /// <summary>
    /// Invalidates the size of the entry.
    /// </summary>
    internal void InvalidateSize()
    {
        Size = 0L;
        Size += _directories.Sum(x => x.Size);
        Size += _files.Sum(x => x.Size);

        while (Parent is not null)
        {
            Parent.InvalidateSize();
            Parent = Parent.Parent;
        }
    }
}

