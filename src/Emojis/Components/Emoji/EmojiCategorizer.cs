namespace FluentUI.Blazor.Community.Components.Emojis;

internal static class EmojiCategorizer
{
    public static IReadOnlyDictionary<string, List<FluentCxEmoji>> BuildCategories(IEnumerable<FluentCxEmoji> emojis)
    {
        return emojis
            .GroupBy(e => e.Category)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    public static IReadOnlyDictionary<string, List<FluentCxEmoji>> BuildSubgroups(IEnumerable<FluentCxEmoji> emojis)
    {
        return emojis
            .GroupBy(e => e.Subgroup)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}
