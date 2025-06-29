namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event args for entries which are moved from one place to another.
/// </summary>
/// <typeparam name="TItem">Type of the folder.</typeparam>
/// <param name="DestinationFolder">Represents the destination folder where the entries moved.</param>
/// <param name="MovedEntries">Represents the entries that have been moved.</param>
public record FileManagerEntriesMovedEventArgs<TItem>(
    FileManagerEntry<TItem> DestinationFolder,
    IEnumerable<FileManagerEntry<TItem>> MovedEntries) where TItem : class, new()
{
}
