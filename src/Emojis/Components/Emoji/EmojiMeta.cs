namespace FluentUI.Blazor.Community.Components.Emojis;

internal sealed class EmojiMeta
{
    public IReadOnlyList<string> Codepoints { get; set; } = [];

    public bool IsSequence { get; set; }

    public bool IsZWJ { get; set; }

    public bool IsKeycap { get; set; }

    public bool IsFlag { get; set; }

    public bool SupportsSkinTone { get; set; }

    public string EmojiVersion { get; set; } = string.Empty;

    public string UnicodeVersion { get; set; } = string.Empty;

    public IReadOnlyList<string> Variants { get; set; } = [];

    public string Unicode { get; set;  } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Subgroup { get; set; } = string.Empty;

    public string LocalizedCategory { get; set; } = string.Empty;
}
