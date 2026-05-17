namespace FluentUI.Blazor.Community.Components.Emojis;

/// <summary>
/// Represents a provider of emoji font information for Fluent UI Blazor components.
/// </summary>
public interface IEmojiFontProvider
{
    /// <summary>
    /// Gets the font family.
    /// </summary>
    string FontFamily { get; }

    /// <summary>
    /// Gets the fallback font family to use when the primary font is unavailable.
    /// </summary>
    /// <returns>The name of the fallback font family.</returns>
    string FallbackFontFamily { get; }
}
