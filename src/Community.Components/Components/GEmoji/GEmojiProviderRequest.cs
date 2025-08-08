using GEmojiSharp;

namespace FluentUI.Blazor.Community.Components;

public record GEmojiProviderRequest(string Text)
{
    public IEnumerable<GEmoji> Items { get; set; } = [];
}
