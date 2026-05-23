namespace FluentUI.Blazor.Community.Components.Infrastructure;

/// <summary>
/// Represents a client for translating text using an external translation service.
/// </summary>
public interface ITranslationClient
{
    /// <summary>
    /// Gets a value indicating whether the translation client is properly configured and can be used to perform translations.
    /// </summary>
    bool IsConfigurationValid { get; }

    /// <summary>
    /// Translates text from one language to another.
    /// </summary>
    /// <param name="text">The text to translate.</param>
    /// <param name="fromLanguage">The source language code.</param>
    /// <param name="language">The target language code.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the translated text.</returns>
    Task<string> TranslateAsync(
        string? text,
        string? fromLanguage,
        string? language);

    /// <summary>
    /// Translates text from a source language to multiple target languages.
    /// </summary>
    /// <param name="text">The text to translate.</param>
    /// <param name="fromLanguage">The source language code. If null, auto-detection may be attempted.</param>
    /// <param name="languages">The target language codes to translate to.</param>
    /// <returns>A dictionary where each key is a language code and the value is a list of possible translations for that
    /// language.</returns>
    Task<Dictionary<string, List<string>>> TranslateAsync(
        string? text,
        string? fromLanguage,
        IEnumerable<string?> languages);

    /// <summary>
    /// Translates text from a source language to multiple target languages asynchronously.
    /// </summary>
    /// <param name="text">The text items to translate.</param>
    /// <param name="fromLanguage">The source language code, or null to auto-detect.</param>
    /// <param name="languages">The target language codes to translate to.</param>
    /// <returns>A dictionary containing translations grouped by target language code.</returns>
    Task<Dictionary<string, List<string>>> TranslateAsync(
        IEnumerable<string?> text,
        string? fromLanguage,
        IEnumerable<string> languages);
}
