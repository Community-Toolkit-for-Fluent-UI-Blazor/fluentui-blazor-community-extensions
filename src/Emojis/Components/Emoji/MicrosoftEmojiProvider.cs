namespace FluentUI.Blazor.Community.Components.Emojis;

/// <summary>
/// Represents an emoji provider for Microsoft platforms.
/// </summary>
public sealed class MicrosoftEmojiProvider : IEmojiFontProvider
{
    /// <inheritdoc />
    public string FontFamily => "Segoe UI Emoji";

    /// <inheritdoc />
    public string FallbackFontFamily => "Segoe UI Symbol";
}
