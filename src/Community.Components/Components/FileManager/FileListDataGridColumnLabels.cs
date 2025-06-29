namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the labels for the DataGrid column inside the <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
public record FileListDataGridColumnLabels
{
    /// <summary>
    /// Gets the default labels.
    /// </summary>
    public static FileListDataGridColumnLabels Default { get; } = new();

    /// <summary>
    /// Gets the french labels.
    /// </summary>
    public static FileListDataGridColumnLabels French { get; } = new()
    {
        CreatedDate = "Date de création",
        Name = "Nom",
        Size = "Taille"
    };

    /// <summary>
    /// Gets or sets the label for the name column.
    /// </summary>
    public string Name { get; set; } = "Name";

    /// <summary>
    /// Gets or sets the label for the size column.
    /// </summary>
    public string Size { get; set; } = "Size";

    /// <summary>
    /// Gets or sets the label for the created date column. 
    /// </summary>
    public string CreatedDate { get; set; } = "Created Date";
}
