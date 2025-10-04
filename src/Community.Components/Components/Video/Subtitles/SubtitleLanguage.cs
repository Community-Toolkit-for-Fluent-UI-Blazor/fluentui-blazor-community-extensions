namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a subtitle language, including its standardized code and display name.
/// </summary>
/// <remarks>Use this record to specify or identify the language of subtitles in media applications. The language
/// code typically follows ISO 639-1 or ISO 639-2 standards, while the language name provides a human-readable
/// description.</remarks>
public record SubtitleLanguage
{
    /// <summary>
    /// Gets the language code that identifies the language of the content or resource.
    /// </summary>
    /// <remarks>The language code should conform to standard formats such as ISO 639-1 (e.g., "en" for
    /// English, "fr" for French). This property is immutable and set during object initialization.</remarks>
    public string LanguageCode { get; init; } = string.Empty;

    /// <summary>
    /// Gets the name of the language associated with this instance.
    /// </summary>
    public string LanguageName { get; init; } = string.Empty;
}
