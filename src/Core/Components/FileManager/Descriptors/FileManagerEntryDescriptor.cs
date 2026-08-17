namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a descriptor for a file or directory entry.
/// </summary>
/// <param name="Id">Identifies the entry. Cannot be null.</param>
/// <param name="Name">Name of the entry to be displayed. Cannot be null.</param>
/// <param name="ParentId">Parent identifier of the entry. Can be null for root entries.</param>
/// <param name="IsDirectory">Value indicating whether the entry represents a directory (true) or a file (false).</param>
/// <param name="Size">Size of the file or a directory in bytes.</param>
/// <param name="CreatedDate">Date and time when the entry was created.</param>
/// <param name="ModifiedDate">Date and time when the entry was last modified.</param>
/// <param name="GetBytesAsync">A function that asynchronously retrieves the contents of the file as a byte array. This should be null for directory entries.</param>
/// <param name="Value">An optional value of type TItem associated with the entry, which can hold additional metadata or information relevant to the file or directory.</param>
public sealed record EntryDescriptor<TItem>(
    string Id,
    string Name,
    string? ParentId,
    bool IsDirectory,
    long? Size,
    DateTime CreatedDate,
    DateTime ModifiedDate,
    Func<CancellationToken, Task<byte[]>>? GetBytesAsync,
    TItem? Value = default)
{
    /// <summary>
    /// Creates a new descriptor representing a directory entry with the specified properties.
    /// </summary>
    /// <param name="id">The unique identifier for the directory entry.</param>
    /// <param name="name">The display name of the directory.</param>
    /// <param name="parentId">The unique identifier of the parent directory. Can be null if the directory is at the root level.</param>
    /// <param name="created">The date and time when the directory was created.</param>
    /// <param name="modified">The date and time when the directory was last modified.</param>
    /// <param name="size">The size of the directory in bytes, if known; otherwise, null.</param>
    /// <param name="value">An optional value of type TItem associated with the directory entry, which can hold additional metadata or information relevant to the directory.</param>
    /// <returns>An EntryDescriptor instance representing the directory with the specified attributes.</returns>
    public static EntryDescriptor<TItem> Directory(
        string id,
        string name,
        string? parentId,
        DateTime created,
        DateTime modified,
        long? size = null,
        TItem? value = default)
        => new(id, name, parentId, true, size, created, modified, null, value);

    /// <summary>
    /// Creates a new descriptor representing a directory entry with the specified properties.
    /// </summary>
    /// <param name="id">The unique identifier for the directory entry.</param>
    /// <param name="name">The display name of the directory.</param>
    /// <param name="parentId">The unique identifier of the parent directory. Can be null if the directory is at the root level.</param>
    /// <param name="created">The date and time when the directory was created.</param>
    /// <param name="modified">The date and time when the directory was last modified.</param>
    /// <param name="size">The size of the directory in bytes, if known; otherwise, null.</param>
    /// <param name="getBytesAsync">A function that asynchronously retrieves the contents of the file as a byte array. This should be null for directory entries.</param>
    /// <param name="value">An optional value of type TItem associated with the directory entry, which can hold additional metadata or information relevant to the directory.</param>
    /// <returns>An EntryDescriptor instance representing the directory with the specified attributes.</returns>
    public static EntryDescriptor<TItem> File(
        string id,
        string name,
        string? parentId,
        long size,
        DateTime created,
        DateTime modified,
        Func<CancellationToken, Task<byte[]>> getBytesAsync,
        TItem? value = default)
        => new(id, name, parentId, false, size, created, modified, getBytesAsync, value);
}
