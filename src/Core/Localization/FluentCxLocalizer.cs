using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Localization;

/// <summary>
/// Provides translations for Fluent UI Community Extensions components.
/// </summary>
public class FluentCxLocalizer : IFluentLocalizer
{
    /// <summary>
    /// Gets the prefix used for community extension localization keys.
    /// </summary>
    public const string COMMUNITY_EXTENSIONS_PREFIX = "CX";

    /// <summary>
    /// Gets the default Fluent UI Community extension translation for the specified key and arguments.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public virtual string GetFluentUICxTranslation(string key, params object[] arguments)
    {
        var localizedString = LanguageResource.ResourceManager.GetString(key, System.Globalization.CultureInfo.InvariantCulture);

        if (localizedString == null)
        {
            throw new ArgumentException($"Key '{key}' not found in LanguageResource.", paramName: nameof(key));
        }

        return arguments.Length > 0
            ? string.Format(System.Globalization.CultureInfo.InvariantCulture, localizedString, arguments)
            : localizedString;
    }

    /// <summary>
    /// Gets the default Fluent UI translation for the specified key and arguments.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public virtual string GetFluentUITranslation(string key, params object[] arguments)
    {
        return IFluentLocalizer.GetDefault(key, arguments);
    }

    /// <summary>
    /// Gets the localized string associated with the specified key, optionally formatting it with the provided
    /// arguments.
    /// </summary>
    /// <remarks>If the key starts with the community extensions prefix, the method retrieves the string from
    /// the language resource and applies formatting if arguments are provided. Otherwise, it falls back to the default
    /// localization mechanism. This indexer supports extensibility for custom translation sources.</remarks>
    /// <param name="key">The resource key that identifies the localized string to retrieve.</param>
    /// <param name="arguments">An array of objects to format the localized string, if it contains format placeholders. Can be empty if no
    /// formatting is required.</param>
    /// <returns>The localized string corresponding to the specified key, formatted with the provided arguments if applicable.</returns>
    /// <exception cref="ArgumentException">Thrown if the specified key is not found in the language resources.</exception>
    public string this[string key, params object[] arguments]
    {
        get
        {
            if (key.StartsWith(COMMUNITY_EXTENSIONS_PREFIX))
            {
                return GetFluentUICxTranslation(key, arguments);
            }

            // Fallback to default FluentUI localization
            return GetFluentUITranslation(key, arguments);
        }
    }
}
