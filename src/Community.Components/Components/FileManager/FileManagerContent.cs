namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the content of an item of the <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
/// <param name="Label">Label of the content.</param>
/// <param name="Placeholder">Placeholder of the content.</param>
/// <param name="Name">Name of the file or folder.</param>
/// <param name="IsDirectory">Value indicating if the item is a directory.</param>
/// <param name="IsRenaming">Value indicating if the item will be renamed.</param>
public record FileManagerContent(
    string? Label,
    string? Placeholder,
    string? Name,
    bool IsDirectory,
    bool IsRenaming
    )
{
    /// <summary>
    /// Gets or sets the name of the file or the folder.
    /// </summary>
    public string? Name { get; set; } = Name;
}
