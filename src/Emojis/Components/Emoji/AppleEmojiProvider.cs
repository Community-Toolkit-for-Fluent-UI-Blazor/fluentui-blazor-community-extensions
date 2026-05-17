namespace FluentUI.Blazor.Community.Components.Emojis;

/// <summary>
/// Represents an emoji provider for Apple platforms.
/// </summary>
public sealed class AppleEmojiProvider : IEmojiFontProvider
{
    /// <inheritdoc />
    public string FontFamily => "Apple Color Emoji";

    /// <inheritdoc />
    public string FallbackFontFamily => "Segoe UI Emoji";
}
