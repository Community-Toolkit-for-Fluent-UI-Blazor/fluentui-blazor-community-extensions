namespace FluentUI.Blazor.Community.Components.Tests.Components.FileManager;

public class FileManagerLabelsTests
{
    [Fact]
    public void DefaultInstance_ShouldHaveEnglishValues()
    {
        var labels = FileManagerLabels.Default;
        Assert.Equal("Close", labels.DialogCloseLabel);
        Assert.Equal("Show details", labels.ShowDetailsLabel);
        Assert.Equal("Search ...", labels.SearchPlaceholder);
        Assert.Equal("New Folder", labels.NewFolderLabel);
        Assert.Equal("Upload", labels.UploadLabel);
        Assert.Equal("Name", labels.SortByNameLabel);
        Assert.Equal("Extension", labels.SortByExtensionLabel);
        Assert.Equal("Size", labels.SortBySizeLabel);
        Assert.Equal("Created date", labels.SortByCreationDate);
        Assert.Equal("Modified date", labels.SortByModifiedDate);
        Assert.Equal("Delete", labels.DeleteLabel);
        Assert.Equal("Download", labels.DownloadLabel);
        Assert.Equal("Rename", labels.RenameLabel);
        Assert.Equal("Sort by", labels.SortLabel);
        Assert.Equal("View", labels.ViewLabel);
        Assert.Equal("List", labels.ListViewLabel);
        Assert.Equal("Ascending", labels.AscendingLabel);
        Assert.Equal("Descending", labels.DescendingLabel);
        Assert.Equal("Create a new folder", labels.FolderDialogTitle);
        Assert.Equal("Rename the folder", labels.RenameFolderDialogTitle);
        Assert.Equal("Rename the file", labels.RenameFileDialogTitle);
        Assert.Equal("Name of the folder", labels.FolderLabel);
        Assert.Equal("Enter the name of the folder", labels.FolderPlaceholder);
        Assert.Equal("Name of the file", labels.FileLabel);
        Assert.Equal("Enter the name of the file", labels.FilePlaceholder);
        Assert.Equal("Are you sur to delete the file or folder ?", labels.DeleteDescriptionLabel);
        Assert.Equal("Yes", labels.DialogYesLabel);
        Assert.Equal("No", labels.DialogNoLabel);
        Assert.Equal("OK", labels.DialogOkLabel);
        Assert.Equal("Cancel", labels.DialogCancelLabel);
        Assert.Equal("Delete the file or the folder", labels.DeleteTitle);
        Assert.Equal("Uploading in progress...", labels.UploadingLabel);
        Assert.Equal("Downloading in progress...", labels.DownloadingLabel);
        Assert.Equal("Deleting in progress...", labels.DeletingLabel);
        Assert.Equal("Moving in progress...", labels.MovingLabel);
        Assert.Equal("Properties", labels.PropertiesLabel);
        Assert.Equal("Hierarchical", labels.HierarchicalLabel);
        Assert.Equal("Flat", labels.FlatLabel);
        Assert.Equal("The maximum number of files that can be selected is {0}", labels.ExceededFileCountMessage);
        Assert.Equal("Maximum number of allowed files exceeded", labels.ExceededFileCountTitle);
        Assert.Equal("Move to", labels.MoveToLabel);
        Assert.Equal("Options", labels.ViewOptionsLabel);
        Assert.Equal("Details", labels.ListWithDetailsLabel);
        Assert.Equal("Mosaic", labels.GridViewMosaicLabel);
        Assert.Equal("Small icons", labels.GridViewSmallIconsLabel);
        Assert.Equal("Medium icons", labels.GridViewMediumIconsLabel);
        Assert.Equal("Large icons", labels.GridViewLargeIconsLabel);
        Assert.Equal("Very large icons", labels.GridViewVeryLargeIconsLabel);
    }

    [Fact]
    public void FrenchInstance_ShouldHaveFrenchValues()
    {
        var labels = FileManagerLabels.French;
        Assert.Equal("Fermer", labels.DialogCloseLabel);
        Assert.Equal("Afficher détails", labels.ShowDetailsLabel);
        Assert.Equal("Rechercher...", labels.SearchPlaceholder);
        Assert.Equal("Nouveau répertoire", labels.NewFolderLabel);
        Assert.Equal("Transférer", labels.UploadLabel);
        Assert.Equal("Nom", labels.SortByNameLabel);
        Assert.Equal("Extensions", labels.SortByExtensionLabel);
        Assert.Equal("Taille", labels.SortBySizeLabel);
        Assert.Equal("Date de création", labels.SortByCreationDate);
        Assert.Equal("Date de modification", labels.SortByModifiedDate);
        Assert.Equal("Supprimer", labels.DeleteLabel);
        Assert.Equal("Télécharger", labels.DownloadLabel);
        Assert.Equal("Renommer", labels.RenameLabel);
        Assert.Equal("Trier", labels.SortLabel);
        Assert.Equal("Affichage", labels.ViewLabel);
        Assert.Equal("Liste", labels.ListViewLabel);
        Assert.Equal("Croissant", labels.AscendingLabel);
        Assert.Equal("Décroissant", labels.DescendingLabel);
        Assert.Equal("Créer un nouveau répertoire", labels.FolderDialogTitle);
        Assert.Equal("Renommer un répertoire", labels.RenameFolderDialogTitle);
        Assert.Equal("Renommer un fichier", labels.RenameFileDialogTitle);
        Assert.Equal("Nom du répertoire", labels.FolderLabel);
        Assert.Equal("Entrez le nom du répertoire", labels.FolderPlaceholder);
        Assert.Equal("Nom du fichier", labels.FileLabel);
        Assert.Equal("Entrez le nom du fichier", labels.FilePlaceholder);
        Assert.Equal("Êtes-vous sûr de vouloir supprimer le fichier ou le répertoire ?", labels.DeleteDescriptionLabel);
        Assert.Equal("Oui", labels.DialogYesLabel);
        Assert.Equal("Non", labels.DialogNoLabel);
        Assert.Equal("Ok", labels.DialogOkLabel);
        Assert.Equal("Annuler", labels.DialogCancelLabel);
        Assert.Equal("Supprimer le fichier ou le répertoire", labels.DeleteTitle);
        Assert.Equal("Transfert en cours...", labels.UploadingLabel);
        Assert.Equal("Téléchargement en cours...", labels.DownloadingLabel);
        Assert.Equal("Suppression en cours...", labels.DeletingLabel);
        Assert.Equal("Déplacement en cours ...", labels.MovingLabel);
        Assert.Equal("Propriétés", labels.PropertiesLabel);
        Assert.Equal("Hiérarchie", labels.HierarchicalLabel);
        Assert.Equal("Plat", labels.FlatLabel);
        Assert.Equal("Le nombre maximal de fichiers pouvant être sélectionné est de {0}", labels.ExceededFileCountMessage);
        Assert.Equal("Nombre de fichiers maximum autorisés dépassé", labels.ExceededFileCountTitle);
        Assert.Equal("Déplacer vers", labels.MoveToLabel);
        Assert.Equal("Options", labels.ViewOptionsLabel);
        Assert.Equal("Détails", labels.ListWithDetailsLabel);
        Assert.Equal("Mosaïques", labels.GridViewMosaicLabel);
        Assert.Equal("Petites icônes", labels.GridViewSmallIconsLabel);
        Assert.Equal("Icônes moyennes", labels.GridViewMediumIconsLabel);
        Assert.Equal("Grandes icônes", labels.GridViewLargeIconsLabel);
        Assert.Equal("Très grandes icônes", labels.GridViewVeryLargeIconsLabel);
    }

    [Fact]
    public void CanSetProperties()
    {
        var labels = new FileManagerLabels
        {
            DialogCloseLabel = "TestClose",
            ShowDetailsLabel = "TestDetails",
            SearchPlaceholder = "TestSearch",
            NewFolderLabel = "TestNewFolder"
        };

        Assert.Equal("TestClose", labels.DialogCloseLabel);
        Assert.Equal("TestDetails", labels.ShowDetailsLabel);
        Assert.Equal("TestSearch", labels.SearchPlaceholder);
        Assert.Equal("TestNewFolder", labels.NewFolderLabel);
    }
}
