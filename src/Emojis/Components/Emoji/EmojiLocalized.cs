namespace FluentUI.Blazor.Community.Components.Emojis;

internal sealed class EmojiLocalized
{
    public string Name { get; set; } = string.Empty;

    public string ShortName { get; set; } = string.Empty;

    public IReadOnlyList<string> Keywords { get; set; } = [];
}
