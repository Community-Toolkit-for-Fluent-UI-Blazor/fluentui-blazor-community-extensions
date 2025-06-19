namespace FluentUI.Blazor.Community.Components;

public record FileManagerDetailsLabels
{
    public static FileManagerDetailsLabels Default { get; } = new();

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

    public string ElementPlural { get; set; } = "elements";

    public string ElementSingular { get; set; } = "element";

    public string NoEntryFoundDescription { get; set; } = "Since this directory is empty, no information could be obtained.";

    public string SelectSingleFileToGetMoreInformation { get; set; } = "Select a single file to get more information";

    public string Size { get; set; } = "Size";

    public string Type { get; set; } = "Type";

    public string ContentType { get; set; } = "Content type";

    public string ModifiedDate { get; set; } = "Modified date";

    public string CreatedDate { get; set; } = "Created date";

    public string DateFormat { get; set; } = "MM/dd/yyyy HH:mm";

    public string SelectedElements { get; set; } = "selected elements";

    public string Details { get; set; } = "Details";
}

