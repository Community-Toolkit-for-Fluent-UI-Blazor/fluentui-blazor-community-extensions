namespace FluentUI.Blazor.Community.Components.Services;

public interface ITranslationClient
{
    bool IsConfigurationValid { get; }

    Task<string> TranslateAsync(
        string? text,
        string? fromLanguage,
        string? language);

    Task<Dictionary<string, List<string>>> TranslateAsync(
        string? text,
        string? fromLanguage,
        IEnumerable<string?> languages);

    Task<Dictionary<string, List<string>>> TranslateAsync(
        IEnumerable<string?> text,
        string? fromLanguage,
        IEnumerable<string> languages);
}
