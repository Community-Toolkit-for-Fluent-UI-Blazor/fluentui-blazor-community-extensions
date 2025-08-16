using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components.Services;

internal sealed class TranslationClient
    : ITranslationClient
{
    private class TranslationArray
    {
        [JsonPropertyName("translations")]
        public List<Translation> Translations { get; set; } = default!;
    }

    private class Translation
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = default!;

        [JsonPropertyName("to")]
        public string To { get; set; } = default!;
    }

    private readonly string _location;
    private readonly string _secretKey;
    private readonly string _url;

    public TranslationClient(IConfiguration configuration)
    {
        IsConfigurationValid = CheckConfig(configuration, out _location, out _secretKey, out _url);
    }

    /// <inheritdoc />
    public bool IsConfigurationValid { get; }

    private static bool CheckConfig(
        IConfiguration configuration,
        out string location,
        out string secretKey,
        out string url)
    {
        var section = configuration.GetSection("Application")
                                   .GetSection("Azure")
                                   .GetSection("TextTranslator");

        location = section.GetValue("Region", string.Empty);
        secretKey = section.GetValue("SecretKey", string.Empty);
        url = section.GetValue("Url", string.Empty);

        return !string.IsNullOrEmpty(location) &&
               !string.IsNullOrEmpty(secretKey) &&
               !string.IsNullOrEmpty(url);
    }

    /// <inheritdoc />
    public async Task<string> TranslateAsync(
        string? text,
        string? fromLanguage,
        string? toLanguage)
    {
        ArgumentException.ThrowIfNullOrEmpty(text, nameof(text));
        ArgumentException.ThrowIfNullOrEmpty(fromLanguage, nameof(fromLanguage));
        ArgumentException.ThrowIfNullOrEmpty(toLanguage, nameof(toLanguage));

        var result = await TranslateAsync(text, fromLanguage, [toLanguage]);

        return result[toLanguage][0];
    }

    /// <inheritdoc />
    public async Task<Dictionary<string, List<string>>> TranslateAsync(
        string? text,
        string? fromLanguage,
        IEnumerable<string?> toLanguages)
    {
        ArgumentException.ThrowIfNullOrEmpty(text, nameof(text));
        ArgumentException.ThrowIfNullOrEmpty(fromLanguage, nameof(fromLanguage));
        ArgumentNullException.ThrowIfNull(toLanguages, nameof(toLanguages));
        ArgumentOutOfRangeException.ThrowIfZero(toLanguages.Count(), nameof(toLanguages));

        return await TranslateAsync([text], fromLanguage, toLanguages.Where(x => !string.IsNullOrEmpty(x)));
    }

    /// <inheritdoc />
    public async Task<Dictionary<string, List<string>>> TranslateAsync(
        IEnumerable<string?> text,
        string? fromLanguage,
        IEnumerable<string?> language)
    {
        ArgumentNullException.ThrowIfNull(text, nameof(text));
        ArgumentException.ThrowIfNullOrEmpty(fromLanguage, nameof(fromLanguage));
        ArgumentNullException.ThrowIfNull(language, nameof(language));

        var texts = text is null ? [] : text.Where(text => !string.IsNullOrEmpty(text)).ToArray();
        var languages = language is null ? [] : language.Where(x => !string.IsNullOrEmpty(x)).ToArray();

        if (texts.Length == 0)
        {
            return [];
        }

        if (languages.Length == 0)
        {
            return [];
        }

        StringBuilder sb = new(_url);
        sb.Append(fromLanguage);
        sb.Append("&to=");
        sb.Append(string.Join("&to=", languages));

        Dictionary<string, List<string>> translatedResources = [];
        using HttpClient httpClient = new();

        foreach (var item in texts)
        {
            object[] body = [new { Text = item }];
            var requestBody = JsonSerializer.Serialize(body);

            using HttpRequestMessage request = new();
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(sb.ToString());
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", _secretKey);

            if (!string.Equals(_location, "global", StringComparison.OrdinalIgnoreCase))
            {
                request.Headers.Add("Ocp-Apim-Subscription-Region", _location);
            }

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            result = result[1..^1];
            var translationArray = JsonSerializer.Deserialize<TranslationArray>(result);

            if (translationArray is not null &&
                translationArray.Translations is not null)
            {
                var group = translationArray.Translations.GroupBy(x => x.To);

                foreach (var subGroup in group)
                {
                    if (!translatedResources.TryGetValue(subGroup.Key, out var resource))
                    {
                        translatedResources[subGroup.Key] = [.. subGroup.Select(x => x.Text)];
                    }
                    else
                    {
                        resource.AddRange([.. subGroup.Select(x => x.Text)]);
                    }
                }
            }
        }

        return translatedResources;
    }
}
