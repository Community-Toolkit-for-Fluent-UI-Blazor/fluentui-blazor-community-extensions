namespace FluentUI.Blazor.Community.Components.Emojis;

internal sealed class EmojiStore
{
    private readonly EmojiIndexer _indexer = new();
    private readonly EmojiLoader _emojiLoader = new();

    public IReadOnlyList<FluentCxEmoji> All { get; private set; } = [];
    public IReadOnlyDictionary<string, List<FluentCxEmoji>> Categories { get; private set; } = new Dictionary<string, List<FluentCxEmoji>>();

    public IReadOnlyDictionary<string, List<FluentCxEmoji>> Subgroups { get; private set;  } = new Dictionary<string, List<FluentCxEmoji>>();

    public string CurrentLanguage { get; private set; } = "en";

    public async Task LoadLanguageAsync(string lang)
    {
        CurrentLanguage = lang;

        var emojis = await _emojiLoader.GetEmojisForLanguageAsync(lang);
        All = emojis;

        Categories = EmojiCategorizer.BuildCategories(emojis);
        Subgroups = EmojiCategorizer.BuildSubgroups(emojis);

        _indexer.Build(emojis);
    }

    public IEnumerable<FluentCxEmoji> Search(string query) => _indexer.Search(query);

    public FluentCxEmoji? Find(string unicode) => All.FirstOrDefault(e => e.Unicode == unicode);
}

