namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the labels used by the <see cref="FileManagerEntryDetails{TItem}"/> component.
/// </summary>
public record FileManagerDetailsLabels
{
    /// <summary>
    /// Gets the default labels.
    /// </summary>
    public static FileManagerDetailsLabels Default { get; } = new();

    /// <summary>
    /// Gets the french labels.
    /// </summary>
    public static FileManagerDetailsLabels French { get; } = new()
    {
        ContentType = "Type de contenu",
        CreatedDate = "Date de création",
        DateFormat = "dd/MM/yyyy HH:mm",
        ElementPlural = "éléments",
        ElementSingular = "élément",
        ModifiedDate = "Date de modification",
        NoEntryFoundDescription = "Puisque ce répertoire est vide, aucune information n'a pu être obtenue.",
        SelectedElements = "éléments sélectionnés",
        SelectSingleFileToGetMoreInformation = "Sélectionnez un seul fichier pour obtenir plus d'informations",
        Size = "Taille",
        Type = "Type",
        Details = "Détails"
    };

    /// <summary>
    /// Gets or sets the plural label for elements.
    /// </summary>
    public string ElementPlural { get; set; } = "elements";

    /// <summary>
    /// Gets or sets the singular label for element.
    /// </summary>
    public string ElementSingular { get; set; } = "element";

    /// <summary>
    /// Gets or sets the label when no entry was found.
    /// </summary>
    public string NoEntryFoundDescription { get; set; } = "Since this directory is empty, no information could be obtained.";

    /// <summary>
    /// Gets or sets the label when multiple files are selected.
    /// </summary>
    public string SelectSingleFileToGetMoreInformation { get; set; } = "Select a single file to get more information";

    /// <summary>
    /// Gets or sets the label for size.
    /// </summary>
    public string Size { get; set; } = "Size";

    /// <summary>
    /// Gets or sets the label for type.
    /// </summary>
    public string Type { get; set; } = "Type";

    /// <summary>
    /// Gets or sets the label for content type.
    /// </summary>
    public string ContentType { get; set; } = "Content type";

    /// <summary>
    /// Gets or sets the label for modified date.
    /// </summary>
    public string ModifiedDate { get; set; } = "Modified date";

    /// <summary>
    /// Gets or sets the label for created date.
    /// </summary>
    public string CreatedDate { get; set; } = "Created date";

    /// <summary>
    /// Gets or sets the format of the date.
    /// </summary>
    public string DateFormat { get; set; } = "MM/dd/yyyy HH:mm";

    /// <summary>
    /// Gets or sets the label for selected elements.
    /// </summary>
    public string SelectedElements { get; set; } = "selected elements";

    /// <summary>
    /// Gets or sets the label for details.
    /// </summary>
    public string Details { get; set; } = "Details";
}

