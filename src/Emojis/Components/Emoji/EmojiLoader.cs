using System.IO.Compression;
using System.Reflection;
using System.Text.Json;

namespace FluentUI.Blazor.Community.Components.Emojis;

internal sealed class EmojiLoader
{
    private readonly Dictionary<string, EmojiMeta> _metaCache = [];
    private readonly Dictionary<string, Dictionary<string, EmojiLocalized>> _localizedCache = [];
    private readonly Dictionary<string, Dictionary<string, string>> _localizedCategoryCache = [];
    private readonly Dictionary<string, List<FluentCxEmoji>> _emojiCache = [];
    private readonly Assembly _assembly;

    public EmojiLoader()
    {
        _assembly = Assembly.GetExecutingAssembly();
    }

    private async Task EnsureMetaLoadedAsync()
    {
        if (_metaCache.Count > 0)
        {
            return;
        }

        var metaDict = await LoadZipJsonAsync<Dictionary<string, EmojiMeta>>("emoji-meta.zip");

        foreach (var kv in metaDict)
        {
            kv.Value.Unicode = kv.Key;
        }

        foreach (var item in metaDict)
        {
            _metaCache.Add(item.Key, item.Value);
        }
    }

    private async Task<Dictionary<string, EmojiLocalized>> EnsureLocalizedLoadedAsync(string lang)
    {
        if (_localizedCache.TryGetValue(lang, out var cached))
        {
            return cached;
        }

        var localized = await LoadZipJsonAsync<Dictionary<string, EmojiLocalized>>($"emoji-{lang}.zip");
        _localizedCache[lang] = localized;

        return localized;
    }

    private async Task<Dictionary<string, string>> EnsureLocalizedCategoryLoadedAsync(string lang)
    {
        if (_localizedCategoryCache.TryGetValue(lang, out var cached))
        {
            return cached;
        }

        var localized = await LoadCategoryJsonAsync($"emoji-categories-{lang}.json");
        _localizedCategoryCache[lang] = localized;

        return localized;
    }

    public async Task<List<FluentCxEmoji>> GetEmojisForLanguageAsync(string lang)
    {
        await EnsureMetaLoadedAsync();

        if (_emojiCache.TryGetValue(lang, out var cached))
        {
            return cached;
        }

        var localized = await EnsureLocalizedLoadedAsync(lang);
        var localizedCategory = await EnsureLocalizedCategoryLoadedAsync(lang);
        var list = new List<FluentCxEmoji>();

        foreach (var kv in _metaCache)
        {
            if (!localized.TryGetValue(kv.Key, out var loc))
            {
                continue;
            }

            if (!localizedCategory.TryGetValue(kv.Value.Category, out var categoryLoc))
            {
                continue;
            }

            kv.Value.LocalizedCategory = categoryLoc;
            var emoji = FluentCxEmojiFactory.Create(kv.Key, kv.Value, loc);
            list.Add(emoji);
        }

        _emojiCache[lang] = list;
        return list;
    }

    private async Task<Dictionary<string, string>> LoadCategoryJsonAsync(string path)
    {
        using var stream = _assembly.GetManifestResourceStream($"FluentUI.Blazor.Community.Components.Emojis.wwwroot.emojis.{path}") ?? throw new FileNotFoundException($"Resource '{path}' not found.");
        using var reader = new StreamReader(stream);

        var json = await reader.ReadToEndAsync();
        return JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;
    }

    private async Task<T> LoadZipJsonAsync<T>(string zipFileName)
    {
        using var stream = _assembly.GetManifestResourceStream($"FluentUI.Blazor.Community.Components.Emojis.wwwroot.emojis.{zipFileName}") ?? throw new FileNotFoundException($"Resource '{zipFileName}' not found.");
        using var zip = new ZipArchive(stream, ZipArchiveMode.Read);

        var entry = zip.Entries.First();
        using var entryStream = entry.Open();
        using var reader = new StreamReader(entryStream);

        var json = await reader.ReadToEndAsync();
        return JsonSerializer.Deserialize<T>(json)!;
    }
}
