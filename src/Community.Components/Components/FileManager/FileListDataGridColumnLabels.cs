namespace FluentUI.Blazor.Community.Components;

public record FileListDataGridColumnLabels
{
    public static FileListDataGridColumnLabels Default { get; } = new();

    public static FileListDataGridColumnLabels French { get; } = new()
    {
        CreatedDate = "Date de création",
        Name = "Nom",
        Size = "Taille"
    };

    public string Name { get; set; } = "Name";

    public string Size { get; set; } = "Size";

    public string CreatedDate { get; set; } = "Created Date";
}
