namespace FluentUI.Blazor.Community.Components;

public record FileManagerDetailsDialogContent<TItem>(
    FileExtensionTypeLabels FileExtensionTypeLabels,
    IEnumerable<FileManagerEntry<TItem>> Entries) where TItem : class, new()
{
}
