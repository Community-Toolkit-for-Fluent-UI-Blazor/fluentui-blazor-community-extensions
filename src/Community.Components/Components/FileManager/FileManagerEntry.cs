// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.StaticFiles;

namespace FluentUI.Blazor.Community.Components;

public sealed class FileManagerEntry<TItem>
    : IEqualityComparer<FileManagerEntry<TItem>> where TItem : class, new()
{
    private readonly List<FileManagerEntry<TItem>> _files = [];
    private readonly List<FileManagerEntry<TItem>> _directories = [];

    /// <summary>
    /// Do not use it ! Use <see cref="Home"/> property, or the static create methods.
    /// </summary>
    public FileManagerEntry()
    {
        Data = default!;
        Name = string.Empty;
    }

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

    public static FileManagerEntry<TItem> Home => CreateHomeDirectory();

    private static readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider = new();

    private readonly List<FileManagerEntry<TItem>> _merged = [];

    public int TotalEntriesCount => _files.Count + _directories.Count;

    private Func<Task<byte[]>>? DataBytesAsyncFunc { get; }

    private byte[]? DataBytes { get; }

    public long Size { get; private set; }

    public TItem Data { get; }

    public DateTime CreatedDate { get; }

    public DateTime ModifiedDate { get; }

    public string ContentType => GetContentType();

    public string Name { get; private set; }

    internal string NameWithoutExtension => Path.GetFileNameWithoutExtension(Name);

    public int Count { get; set; }

    public bool HasExtension => !string.IsNullOrEmpty(Path.GetExtension(Name));

    public string? Extension => Path.GetExtension(Name);

    public bool IsDirectory { get; }

    internal bool HasDirectory => _directories is not null && _directories.Count > 0;

    internal bool HasFiles => _files is not null && _files.Count > 0;

    public string Id { get; set; } = Guid.NewGuid().ToString();

    internal string? RelativePath { get; private set; }

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

    public FileManagerEntry<TItem> CreateDirectory(
       string directoryName)
    {
        return CreateDirectory(DateTime.Now, DateTime.Now, directoryName, 0L);
    }

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

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is FileManagerEntry<TItem> m)
        {
            return this == m;
        }

        return false;
    }

    public static bool operator ==(FileManagerEntry<TItem>? x, FileManagerEntry<TItem>? y)
    {
        if (x is null || y is null)
        {
            return false;
        }

        return string.Equals(x.Id, y.Id, StringComparison.OrdinalIgnoreCase);
    }

    public static bool operator !=(FileManagerEntry<TItem>? x, FileManagerEntry<TItem>? y)
    {
        return !(x == y);
    }

    public bool Equals(FileManagerEntry<TItem>? x, FileManagerEntry<TItem>? y)
    {
        if (x is null || y is null)
        {
            return false;
        }

        return x == y;
    }

    public int GetHashCode([DisallowNull] FileManagerEntry<TItem> obj)
    {
        return HashCode.Combine(obj.Name, obj.CreatedDate, obj.ModifiedDate, obj.Size);
    }

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

    public override int GetHashCode()
    {
        return GetHashCode(this);
    }

    public void Add(FileManagerEntry<TItem> entry)
    {
        if (!entry.IsDirectory)
        {
            _files.Add(entry);
        }
        else
        {
            entry.RelativePath = $"{RelativePath}\\{entry.Name}";
            _directories.Add(entry);
        }

        InvalidateMerge();
        UpdateSize();
    }

    public void AddRange(IEnumerable<FileManagerEntry<TItem>> entries)
    {
        foreach (var entry in entries)
        {
            if (!entry.IsDirectory)
            {
                _files.Add(entry);
            }
            else
            {
                _directories.Add(entry);
            }
        }

        InvalidateMerge();
        UpdateSize();
    }

    public void Clear()
    {
        _files.Clear();
        _directories.Clear();

        InvalidateMerge();
        UpdateSize();
    }

    public IEnumerable<FileManagerEntry<TItem>> GetDirectories()
    {
        return _directories;
    }

    public IEnumerable<FileManagerEntry<TItem>> GetFiles()
    {
        return _files;
    }

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
            InvalidateMerge();
            UpdateSize();
        }
    }

    public void Remove(IEnumerable<FileManagerEntry<TItem>> entries)
    {
        var removed = false;

        foreach (var entry in entries)
        {
            if (entry.IsDirectory)
            {
                removed |= _directories.Remove(entry);
            }
            else
            {
                removed |= _files.Remove(entry);
            }
        }

        if (removed)
        {
            InvalidateMerge();
            UpdateSize();
        }
    }

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

    internal void InvalidateMerge()
    {
        _merged.Clear();
        _merged.AddRange(GetMerged());
    }

    internal IList<FileManagerEntry<TItem>> Enumerate()
    {
        return _merged;
    }

    internal void Sort(FileManagerEntryComparer<TItem> comparer)
    {
        _merged.Sort(comparer);
    }

    public static FileManagerEntry<TItem>? Find(FileManagerEntry<TItem> root, string id)
    {
        if (string.Equals(root.Id, id, StringComparison.OrdinalIgnoreCase))
        {
            return root;
        }

        return Find(root.Enumerate(), id);
    }

    public static FileManagerEntry<TItem>? Find(FileManagerEntry<TItem> root, Func<FileManagerEntry<TItem>, bool> predicate)
    {
        if (predicate(root))
        {
            return root;
        }

        return Find(root.Enumerate(), predicate);
    }

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

    public FileManagerEntry<TItem> CreateDefaultFileEntry(
        byte[] data,
        string name,
        int length)
    {
        var entry = new FileManagerEntry<TItem>(Activator.CreateInstance<TItem>(), data, name, false, length, DateTime.Now, DateTime.Now);
        Add(entry);

        return entry;
    }

    public FileManagerEntry<TItem> CreateDefaultFileEntry(
        Func<Task<byte[]>> value,
        string name,
        long length)
    {
        var entry = new FileManagerEntry<TItem>(Activator.CreateInstance<TItem>(), value, name, false, length, DateTime.Now, DateTime.Now);
        Add(entry);

        return entry;
    }

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

    public static FileManagerEntry<TItem> CreateEntry(
       Func<Task<byte[]>> value,
       string name,
       long length,
       DateTime creationDate,
       DateTime lastModificationDate)
    {
        return new FileManagerEntry<TItem>(default!, value, name, false, length, creationDate, lastModificationDate);
    }

    public static FileManagerEntry<TItem> CreateEntry(
       Func<Task<byte[]>> value,
       string name,
       long length)
    {
        return new FileManagerEntry<TItem>(default!, value, name, false, length, DateTime.Now, DateTime.Now);
    }

    public static FileManagerEntry<TItem> CreateEntry(
       byte[] value,
       string name,
       long length)
    {
        return new FileManagerEntry<TItem>(default!, value, name, false, length, DateTime.Now, DateTime.Now);
    }

    private void UpdateSize()
    {
        Size = 0L;
        Size += _directories.Sum(x => x.Size);
        Size += _files.Sum(x => x.Size);
    }
}

