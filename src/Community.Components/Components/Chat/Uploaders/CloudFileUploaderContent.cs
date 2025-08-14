namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the content for the <see cref="CloudFileManagerDialog{TItem}"/>.
/// </summary>
/// <param name="View">Represents the file manager view.</param>
/// <param name="ColumnLabels">Represents the labels for the datagrid columns of the <see cref="FluentCxFileManager{TItem}"/>.</param>
/// <param name="FileExtensionTypeLabels">Represents the labels for the extension file type of the <see cref="FluentCxFileManager{TItem}"/>.</param>
/// <param name="DetailsLabels">Represents the labels for the details panel of the <see cref="FluentCxFileManager{TItem}"/>.</param>
/// <param name="FileManagerLabels">Represents the labels for the <see cref="FluentCxFileManager{TItem}"/>.</param>
public record CloudFileUploaderContent(
    FileManagerView View,
    FileListDataGridColumnLabels ColumnLabels,
    FileExtensionTypeLabels FileExtensionTypeLabels,
    FileManagerDetailsLabels DetailsLabels,
    FileManagerLabels FileManagerLabels
    ) 
{
}
