namespace FluentUI.Blazor.Community.Components;

public record FileManagerEntryEventArgs<TItem>(FileManagerEntry<TItem> Entry) where TItem : class, new()
{
}

public record CreateFileManagerEntryEventArgs<TItem>(
    FileManagerEntry<TItem> Parent,
    FileManagerEntry<TItem> Entry) where TItem : class, new()
{
}

public record DeleteFileManagerEntryEventArgs<TItem>(
    FileManagerEntry<TItem> Parent,
    IEnumerable<FileManagerEntry<TItem>> Entries) where TItem : class, new()
{
}
