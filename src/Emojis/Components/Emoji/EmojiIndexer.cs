namespace FluentUI.Blazor.Community.Components.Emojis;

internal sealed class EmojiIndexer
{
    private readonly Dictionary<string, List<FluentCxEmoji>> _index = [];

    public void Build(IEnumerable<FluentCxEmoji> emojis)
    {
        foreach (var e in emojis)
        {
            Index(e.Name, e);
            Index(e.ShortName, e);

            foreach (var kw in e.Keywords)
            {
                Index(kw, e);
            }

            Index(e.Unicode, e);
        }
    }

    private void Index(string text, FluentCxEmoji emoji)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        var key = text.ToLowerInvariant();

        if (!_index.TryGetValue(key, out var list))
        {
            list = _index[key] = [];
        }

        list.Add(emoji);
    }

    public IEnumerable<FluentCxEmoji> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        query = query.ToLowerInvariant();

        if (_index.TryGetValue(query, out var list))
        {
            return list;
        }

        return [];
    }
}
