namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the labels used by the <see cref="FluentCxFileManager{TItem}"/> component.
/// </summary>
public record FileManagerLabels
{
    /// <summary>
    /// Gets the default labels.
    /// </summary>
    public static FileManagerLabels Default { get; } = new();

    /// <summary>
    /// Gets the french labels.
    /// </summary>
    public static FileManagerLabels French { get; } = new()
    {
        AscendingLabel = "Croissant",
        DeleteDescriptionLabel = "Êtes-vous sûr de vouloir supprimer le fichier ou le répertoire ?",
        DeleteLabel = "Supprimer",
        DeleteTitle = "Supprimer le fichier ou le répertoire",
        DeletingLabel = "Suppression en cours...",
        DescendingLabel = "Décroissant",
        DialogCancelLabel = "Annuler",
        DialogNoLabel = "Non",
        DialogOkLabel = "Ok",
        DialogYesLabel = "Oui",
        DialogCloseLabel = "Fermer",
        DownloadingLabel = "Téléchargement en cours...",
        DownloadLabel = "Télécharger",
        FileLabel = "Nom du fichier",
        FilePlaceholder = "Entrez le nom du fichier",
        FolderDialogTitle = "Créer un nouveau répertoire",
        FolderLabel = "Nom du répertoire",
        FolderPlaceholder = "Entrez le nom du répertoire",
        ListViewLabel = "Liste",
        NewFolderLabel = "Nouveau répertoire",
        PropertiesLabel = "Propriétés",
        RenameFileDialogTitle = "Renommer un fichier",
        RenameFolderDialogTitle = "Renommer un répertoire",
        RenameLabel = "Renommer",
        SearchPlaceholder = "Rechercher...",
        ShowDetailsLabel = "Afficher détails",
        SortByCreationDate = "Date de création",
        SortByExtensionLabel = "Extensions",
        SortByModifiedDate = "Date de modification",
        SortByNameLabel = "Nom",
        SortBySizeLabel = "Taille",
        SortLabel = "Trier",
        UploadingLabel = "Transfert en cours...",
        UploadLabel = "Transférer",
        ViewLabel = "Affichage",
        HierarchicalLabel = "Hiérarchie",
        FlatLabel = "Plat",
        ExceededFileCountMessage = "Le nombre maximal de fichiers pouvant être sélectionné est de {0}",
        ExceededFileCountTitle = "Nombre de fichiers maximum autorisés dépassé",
        MoveToLabel = "Déplacer vers",
        MovingLabel = "Déplacement en cours ...",
        ViewOptionsLabel = "Options",
        ListWithDetailsLabel = "Détails",
        GridViewMosaicLabel = "Mosaïques",
        GridViewLargeIconsLabel = "Grandes icônes",
        GridViewMediumIconsLabel = "Icônes moyennes",
        GridViewSmallIconsLabel = "Petites icônes",
        GridViewVeryLargeIconsLabel = "Très grandes icônes"
    };

    /// <summary>
    /// Gets or sets the label for the close button in a dialog.
    /// </summary>
    public string DialogCloseLabel { get; set; } = "Close";

    /// <summary>
    /// Gets or sets the label for the show details button.
    /// </summary>
    public string ShowDetailsLabel { get; set; } = "Show details";

    /// <summary>
    /// Gets or sets the placeholder label for the search text box.
    /// </summary>
    public string SearchPlaceholder { get; set; } = "Search ...";

    /// <summary>
    /// Gets or sets the label for the new folder button.
    /// </summary>
    public string NewFolderLabel { get; set; } = "New Folder";

    /// <summary>
    /// Gets or sets the label for the upload button.
    /// </summary>
    public string UploadLabel { get; set; } = "Upload";

    /// <summary>
    /// Gets or sets the label for the sort by name menu item.
    /// </summary>
    public string SortByNameLabel { get; set; } = "Name";

    /// <summary>
    /// Gets or sets the label for the sort by extension menu item.
    /// </summary>
    public string SortByExtensionLabel { get; set; } = "Extension";

    /// <summary>
    /// Gets or sets the label for the sort by size menu item.
    /// </summary>
    public string SortBySizeLabel { get; set; } = "Size";

    /// <summary>
    /// Gets or sets the label for the sort by created date menu item.
    /// </summary>
    public string SortByCreationDate { get; set; } = "Created date";

    /// <summary>
    /// Gets or sets the label for the sort by modified date menu item.
    /// </summary>
    public string SortByModifiedDate { get; set; } = "Modified date";

    /// <summary>
    /// Gets or sets the label for the sort by delete menu item.
    /// </summary>
    public string DeleteLabel { get; set; } = "Delete";

    /// <summary>
    /// Gets or sets the label for the download button.
    /// </summary>
    public string DownloadLabel { get; set; } = "Download";

    /// <summary>
    /// Gets or sets the label for the rename button.
    /// </summary>
    public string RenameLabel { get; set; } = "Rename";

    /// <summary>
    /// Gets or sets the label for the sort button.
    /// </summary>
    public string SortLabel { get; set; } = "Sort by";

    /// <summary>
    /// Gets or sets the label for the view button.
    /// </summary>
    public string ViewLabel { get; set; } = "View";

    /// <summary>
    /// Gets or sets the label for the list menu item.
    /// </summary>
    public string ListViewLabel { get; set; } = "List";

    /// <summary>
    /// Gets or sets the label for the ascending menu item.
    /// </summary>
    public string AscendingLabel { get; set; } = "Ascending";

    /// <summary>
    /// Gets or sets the label for the descending menu item.
    /// </summary>
    public string DescendingLabel { get; set; } = "Descending";

    /// <summary>
    /// Gets or sets the title for the new folder dialog.
    /// </summary>
    public string FolderDialogTitle { get; set; } = "Create a new folder";

    /// <summary>
    /// Gets or sets the title for the rename folder dialog.
    /// </summary>
    public string RenameFolderDialogTitle { get; set; } = "Rename the folder";

    /// <summary>
    /// Gets or sets the title for the rename file dialog.
    /// </summary>
    public string RenameFileDialogTitle { get; set; } = "Rename the file";

    /// <summary>
    /// Gets or sets the label for the folder name.
    /// </summary>
    public string FolderLabel { get; set; } = "Name of the folder";

    /// <summary>
    /// Gets or sets the placeholder label for the folder name.
    /// </summary>
    public string FolderPlaceholder { get; set; } = "Enter the name of the folder";

    /// <summary>
    /// Gets or sets the label for the file name.
    /// </summary>
    public string FileLabel { get; set; } = "Name of the file";

    /// <summary>
    /// Gets or sets the placeholder label for the file name.
    /// </summary>
    public string FilePlaceholder { get; set; } = "Enter the name of the file";

    /// <summary>
    /// Gets or sets the description for the delete confirmation dialog.
    /// </summary>
    public string DeleteDescriptionLabel { get; set; } = "Are you sur to delete the file or folder ?";

    /// <summary>
    /// Gets or sets the label for the yes button in a dialog.
    /// </summary>
    public string DialogYesLabel { get; set; } = "Yes";

    /// <summary>
    /// Gets or sets the label for the no button in a dialog.
    /// </summary>
    public string DialogNoLabel { get; set; } = "No";

    /// <summary>
    /// Gets or sets the label for the OK button in a dialog.
    /// </summary>
    public string DialogOkLabel { get; set; } = "OK";

    /// <summary>
    /// Gets or sets the label for the cancel button in a dialog.
    /// </summary>
    public string DialogCancelLabel { get; set; } = "Cancel";

    /// <summary>
    /// Gets or sets the delete dialog title.
    /// </summary>
    public string DeleteTitle { get; set; } = "Delete the file or the folder";

    /// <summary>
    /// Gets or sets the label for an uploading process.
    /// </summary>
    public string UploadingLabel { get; set; } = "Uploading in progress...";

    /// <summary>
    /// Gets or sets the label for a download process.
    /// </summary>
    public string DownloadingLabel { get; set; } = "Downloading in progress...";

    /// <summary>
    /// Gets or sets the label for a delete process.
    /// </summary>
    public string DeletingLabel { get; set; } = "Deleting in progress...";

    /// <summary>
    /// Gets or sets the label for a move process.
    /// </summary>
    public string MovingLabel { get; set; } = "Moving in progress...";

    /// <summary>
    /// Gets or sets the label for the properties button.
    /// </summary>
    public string PropertiesLabel { get; set; } = "Properties";

    /// <summary>
    /// Gets or sets the label for the hierarchical menu item.
    /// </summary>
    public string HierarchicalLabel { get; set; } = "Hierarchical";

    /// <summary>
    /// Gets or sets the label for the flat menu item.
    /// </summary>
    public string FlatLabel { get; set; } = "Flat";

    /// <summary>
    /// Gets or sets the message when the number of files exceeded the maximum allowed.
    /// </summary>
    public string ExceededFileCountMessage { get; set; } = "The maximum number of files that can be selected is {0}";

    /// <summary>
    /// Gets or sets the title when the number of files exceeded the maximum allowed.
    /// </summary>
    public string ExceededFileCountTitle { get; set; } = "Maximum number of allowed files exceeded";

    /// <summary>
    /// Gets or sets the label for the move button.
    /// </summary>
    public string MoveToLabel { get; set; } = "Move to";

    /// <summary>
    /// Gets or sets the label for the options button.
    /// </summary>
    public string ViewOptionsLabel { get; set; } = "Options";

    /// <summary>
    /// Gets or sets the label for the details menu item.
    /// </summary>
    public string ListWithDetailsLabel { get; set; } = "Details";

    /// <summary>
    /// Gets or sets the label for the mosaic menu item.
    /// </summary>
    public string GridViewMosaicLabel { get; set; } = "Mosaic";

    /// <summary>
    /// Gets or sets the label for the small icons menu item.
    /// </summary>
    public string GridViewSmallIconsLabel { get; set; } = "Small icons";

    /// <summary>
    /// Gets or sets the label for the medium menu item.
    /// </summary>
    public string GridViewMediumIconsLabel { get; set; } = "Medium icons";

    /// <summary>
    /// Gets or sets the label for the large icons menu item.
    /// </summary>
    public string GridViewLargeIconsLabel { get; set; } = "Large icons";

    /// <summary>
    /// Gets or sets the label for the very large icons menu item.
    /// </summary>
    public string GridViewVeryLargeIconsLabel { get; set; } = "Very large icons";
}
