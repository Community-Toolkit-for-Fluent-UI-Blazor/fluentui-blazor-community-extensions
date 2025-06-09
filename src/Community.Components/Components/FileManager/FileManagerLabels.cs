// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

public record FileManagerLabels
{
    public static FileManagerLabels Default { get; } = new();

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

    public string DialogCloseLabel { get; set; } = "Close";

    public string ShowDetailsLabel { get; set; } = "Show details";

    public string SearchPlaceholder { get; set; } = "Search ...";

    public string NewFolderLabel { get; set; } = "New Folder";

    public string UploadLabel { get; set; } = "Upload";

    public string SortByNameLabel { get; set; } = "Name";

    public string SortByExtensionLabel { get; set; } = "Extension";

    public string SortBySizeLabel { get; set; } = "Size";

    public string SortByCreationDate { get; set; } = "Created date";

    public string SortByModifiedDate { get; set; } = "Modified date";

    public string DeleteLabel { get; set; } = "Delete";

    public string DownloadLabel { get; set; } = "Download";

    public string RenameLabel { get; set; } = "Rename";

    public string SortLabel { get; set; } = "Sort by";

    public string ViewLabel { get; set; } = "View";

    public string ListViewLabel { get; set; } = "List";

    public string AscendingLabel { get; set; } = "Ascending";

    public string DescendingLabel { get; set; } = "Descending";

    public string FolderDialogTitle { get; set; } = "Create a new folder";

    public string RenameFolderDialogTitle { get; set; } = "Rename the folder";

    public string RenameFileDialogTitle { get; set; } = "Rename the file";

    public string FolderLabel { get; set; } = "Name of the folder";

    public string FolderPlaceholder { get; set; } = "Enter the name of the folder";

    public string FileLabel { get; set; } = "Name of the file";

    public string FilePlaceholder { get; set; } = "Enter the name of the file";

    public string DeleteDescriptionLabel { get; set; } = "Are you sur to delete the file or folder ?";

    public string DialogYesLabel { get; set; } = "Yes";

    public string DialogNoLabel { get; set; } = "No";

    public string DialogOkLabel { get; set; } = "OK";

    public string DialogCancelLabel { get; set; } = "Cancel";

    public string DeleteTitle { get; set; } = "Delete the file or the folder";

    public string UploadingLabel { get; set; } = "Uploading in progress...";

    public string DownloadingLabel { get; set; } = "Downloading in progress...";

    public string DeletingLabel { get; set; } = "Deleting in progress...";

    public string MovingLabel { get; set; } = "Moving in progress...";

    public string PropertiesLabel { get; set; } = "Properties";

    public string HierarchicalLabel { get; set; } = "Hierarchical";

    public string FlatLabel { get; set; } = "Flat";

    public string ExceededFileCountMessage { get; set; } = "The maximum number of files that can be selected is {0}";

    public string ExceededFileCountTitle { get; set; } = "Maximum number of allowed files exceeded";

    public string MoveToLabel { get; set; } = "Move to";

    public string ViewOptionsLabel { get; set; } = "Options";

    public string ListWithDetailsLabel { get; set; } = "Details";

    public string GridViewMosaicLabel { get; set; } = "Mosaics";

    public string GridViewSmallIconsLabel { get; set; } = "Small icons";

    public string GridViewMediumIconsLabel { get; set; } = "Medium icons";

    public string GridViewLargeIconsLabel { get; set; } = "Large icons";

    public string GridViewVeryLargeIconsLabel { get; set; } = "Very large icons";
}
