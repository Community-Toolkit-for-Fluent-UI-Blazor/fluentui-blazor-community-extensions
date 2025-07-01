namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the content for the FileManagerDetailsDialog.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
/// <param name="FileExtensionTypeLabels">Labels for the files.</param>
/// <param name="Entries">Represents the selected entries.</param>
public record FileManagerDetailsDialogContent<TItem>(
    FileExtensionTypeLabels FileExtensionTypeLabels,
    IEnumerable<FileManagerEntry<TItem>> Entries) where TItem : class, new()
{
}
