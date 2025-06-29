namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event args for a <see cref="FileManagerEntry{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
/// <param name="Entry">Represents the entry to use</param>
public record FileManagerEntryEventArgs<TItem>(FileManagerEntry<TItem> Entry) where TItem : class, new()
{
}

/// <summary>
/// Represents the event args when an entry is created.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
/// <param name="Parent">Entry which contains the <paramref name="Entry"/>.</param>
/// <param name="Entry">Represents the created entry.</param>
public record CreateFileManagerEntryEventArgs<TItem>(
    FileManagerEntry<TItem> Parent,
    FileManagerEntry<TItem> Entry) where TItem : class, new()
{
}

/// <summary>
/// Represents the event args when some entries are deleted.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
/// <param name="Parent">Entry which contains the <paramref name="Entries"/> to delete.</param>
/// <param name="Entries">Represents the entries to delete.</param>
public record DeleteFileManagerEntryEventArgs<TItem>(
    FileManagerEntry<TItem> Parent,
    IEnumerable<FileManagerEntry<TItem>> Entries) where TItem : class, new()
{
}
