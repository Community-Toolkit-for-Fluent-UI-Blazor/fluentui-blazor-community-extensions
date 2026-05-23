namespace FluentUI.Blazor.Community.Components.Emojis;

/// <summary>
/// Represents the settings for the emoji dialog.
/// </summary>
public sealed record EmojiDialogSettings
{
    /// <summary>
    /// Gets or sets the number of emojis to display per row in the emoji picker dialog.
    /// </summary>
    public int EmojisPerRow { get; init; } = 8;

    /// <summary>
    /// Gets or sets the emoji font provider.
    /// </summary>
    public IEmojiFontProvider FontProvider { get; init; } = new MicrosoftEmojiProvider();
}
