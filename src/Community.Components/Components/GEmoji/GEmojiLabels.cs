namespace FluentUI.Blazor.Community.Components;

public record GEmojiLabels
{
    public static GEmojiLabels Default { get; } = new();

    public static GEmojiLabels French { get; } = new()
    {
        SearchLabel = "Rechercher un émoji",
        SearchPlaceholder = "Entrez un émoji à rechercher"
    };

    public string SearchLabel { get; set; } = "Search an emoji";

    public string SearchPlaceholder { get; set; } = "Enter an emoji to search...";
}
