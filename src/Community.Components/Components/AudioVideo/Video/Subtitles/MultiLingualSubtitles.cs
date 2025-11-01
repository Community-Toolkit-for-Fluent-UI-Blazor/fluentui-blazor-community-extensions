namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a collection of subtitles organized by language, allowing management of multiple language-specific
/// subtitle tracks within a single object.
/// </summary>
public sealed class MultilingualSubtitles
{
    /// <summary>
    /// Represents a collection of subtitles organized by language.
    /// </summary>
    private readonly Dictionary<SubtitleLanguage, List<SubtitleEntry>> _multiLingualSubtitles = new(SubtitleLanguageComparer.Default);

    /// <summary>
    /// Gets the collection of subtitle languages available in the current set.
    /// </summary>
    public IEnumerable<SubtitleLanguage> Keys => _multiLingualSubtitles.Keys;

    /// <summary>
    /// Gets the list of subtitle entries for the specified language.
    /// </summary>
    /// <param name="language">The language for which to retrieve subtitle entries.</param>
    /// <returns>A list of <see cref="SubtitleEntry"/> objects for the specified language. Returns an empty list if no subtitles
    /// are available for the language.</returns>
    public List<SubtitleEntry> this[SubtitleLanguage? language] => language is null ? [] : _multiLingualSubtitles.TryGetValue(language, out var value) ? value : [];

    /// <summary>
    /// Adds a collection of subtitles for the specified language. If subtitles for the language already exist, the new
    /// entries are appended.
    /// </summary>
    /// <param name="language">The language for which the subtitles are to be added.</param>
    /// <param name="subtitles">The list of subtitle entries to add. Cannot be null.</param>
    public void Add(SubtitleLanguage language, List<SubtitleEntry> subtitles)
    {
        if (_multiLingualSubtitles.TryGetValue(language, out var value))
        {
            value.AddRange(subtitles);
        }
        else
        {
            _multiLingualSubtitles[language] = [.. subtitles];
        }
    }

    /// <summary>
    /// Adds multiple collections of subtitles, each associated with a specific language, to the current set.
    /// </summary>
    /// <remarks>If any of the provided subtitle lists are empty, no subtitles will be added for that
    /// language. This method is useful for efficiently adding subtitles in multiple languages in a single
    /// operation.</remarks>
    /// <param name="items">An array of tuples, each containing a subtitle language and a list of subtitle entries to add. Cannot be null.
    /// Each tuple's language specifies the language for the subtitles, and the subtitles list contains the entries to
    /// add for that language.</param>
    public void AddRange(params (SubtitleLanguage Language, List<SubtitleEntry> Subtitles)[] items)
    {
        foreach (var (language, subtitles) in items)
        {
            Add(language, subtitles);
        }
    }
}
