namespace FluentUI.Blazor.Community.Components.Emojis;

/// <summary>
/// Factory for creating <see cref="FluentCxEmoji"/> instances from emoji metadata and localization data.
/// </summary>
internal static class FluentCxEmojiFactory
{
    /// <summary>
    /// Creates a new FluentCxEmoji instance from Unicode, metadata, and localized information.
    /// </summary>
    /// <param name="unicode">The Unicode character(s) representing the emoji.</param>
    /// <param name="meta">The metadata containing technical information about the emoji.</param>
    /// <param name="loc">The localized information containing names, keywords, and categories.</param>
    /// <returns>A new FluentCxEmoji instance initialized with the provided data.</returns>
    public static FluentCxEmoji Create(
        string unicode,
        EmojiMeta meta,
        EmojiLocalized loc)
    {
        return new FluentCxEmoji(
            unicode,
            loc.Name,
            loc.ShortName,
            loc.Keywords,
            meta.Category,
            meta.LocalizedCategory,
            meta.Subgroup,
            meta.IsSequence,
            meta.Codepoints,
            meta.Variants,
            meta.SupportsSkinTone,
            meta.UnicodeVersion,
            meta.EmojiVersion,
            meta.IsZWJ,
            meta.IsKeycap,
            meta.IsFlag);
    }
}
