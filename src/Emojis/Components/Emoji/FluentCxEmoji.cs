namespace FluentUI.Blazor.Community.Components.Emojis;

/// <summary>
/// Represents a comprehensive model of an emoji, encapsulating all relevant properties and metadata for accurate rendering and categorization within Fluent UI Blazor components.
/// </summary>
public sealed class FluentCxEmoji
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxEmoji"/> class with the specified emoji metadata.
    /// </summary>
    /// <param name="unicode">The Unicode character representation of the emoji.</param>
    /// <param name="name">The full name of the emoji.</param>
    /// <param name="shortName">The short name or identifier for the emoji.</param>
    /// <param name="keywords">A collection of keywords associated with the emoji for search and discovery.</param>
    /// <param name="category">The category to which the emoji belongs.</param>
    /// <param name="subgroup">The subgroup classification of the emoji.</param>
    /// <param name="isSequence">A value indicating whether the emoji is composed of multiple codepoint sequences.</param>
    /// <param name="codepoints">A collection of Unicode codepoints that make up the emoji.</param>
    /// <param name="variants">A collection of variant forms of the emoji.</param>
    /// <param name="supportsSkinTone">A value indicating whether the emoji supports skin tone modifiers.</param>
    /// <param name="unicodeVersion">The Unicode version in which the emoji was introduced.</param>
    /// <param name="emojiVersion">The Emoji version in which the emoji was introduced.</param>
    /// <param name="isZWJ">A value indicating whether the emoji uses Zero Width Joiner (ZWJ) sequences.</param>
    /// <param name="isKeycap">A value indicating whether the emoji is a keycap emoji.</param>
    /// <param name="isFlag">A value indicating whether the emoji represents a flag.</param>
    public FluentCxEmoji(
        string unicode,
        string name,
        string shortName,
        IEnumerable<string> keywords,
        string category,
        string subgroup,
        bool isSequence,
        IEnumerable<string> codepoints,
        IEnumerable<string> variants,
        bool supportsSkinTone,
        string unicodeVersion,
        string emojiVersion,
        bool isZWJ,
        bool isKeycap,
        bool isFlag)
    {
        Unicode = unicode;
        Name = name;
        ShortName = shortName;
        DisplayName = shortName.Replace('_', ' ');
        Keywords = [.. keywords];
        Category = category;
        Subgroup = subgroup;
        IsSequence = isSequence;
        Codepoints = [.. codepoints];
        Variants = [.. variants];
        SupportsSkinTone = supportsSkinTone;
        UnicodeVersion = unicodeVersion;
        EmojiVersion = emojiVersion;
        IsZWJ = isZWJ;
        IsKeycap = isKeycap;
        IsFlag = isFlag;
    }

    /// <summary>
    /// Gets the Unicode representation of the emoji.
    /// </summary>
    public string Unicode { get; }

    /// <summary>
    /// Gets the name of the emoji.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the short name of the emoji.
    /// </summary>
    public string ShortName { get; }

    /// <summary>
    /// Gets tje display name of the emoji.
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// Gets the list of keywords associated with the emoji.
    /// </summary>
    public IReadOnlyList<string> Keywords { get; }

    /// <summary>
    /// Gets the category of the emoji, such as "Smileys &amp; Emotion", "People &amp; Body", "Animals &amp; Nature", etc.
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Gets the subgroup of the emoji, which provides a more specific classification within the main category, such as "face-smiling", "hand-fingers-open", "animal-mammal", etc.
    /// </summary>
    public string Subgroup { get; }

    /// <summary>
    /// Gets a value indicating whether the emoji is a sequence of multiple code points (e.g., family emojis, flag emojis) rather than a single code point.
    /// </summary>
    public bool IsSequence { get; }

    /// <summary>
    /// Gets the list of Unicode code points that make up the emoji.
    /// </summary>
    public IReadOnlyList<string> Codepoints { get; }

    /// <summary>
    /// Gets the list of variant forms of the emoji, such as skin tone.
    /// </summary>
    public IReadOnlyList<string> Variants { get; }

    /// <summary>
    /// Gets a value indicating whether the emoji supports skin tone modifiers.
    /// </summary>
    public bool SupportsSkinTone { get; }

    /// <summary>
    /// Gets a value indicating whether the emoji supports gender.
    /// </summary>
    public bool SupportsGender { get; }

    /// <summary>
    /// Gets the Unicode version in which the emoji was introduced.
    /// </summary>
    public string UnicodeVersion { get; }

    /// <summary>
    /// Gets the emoji version in which the emoji was introduced, which may differ from the Unicode version for certain emojis that were added in later updates to the emoji standard.
    /// </summary>
    public string EmojiVersion { get; }

    /// <summary>
    /// Gets a value indicating whether the emoji is a zero-width joiner (ZWJ) sequence.
    /// </summary>
    public bool IsZWJ { get; }

    /// <summary>
    /// Gets a value indicating whether the emoji is a keycap.
    /// </summary>
    public bool IsKeycap { get; }

    /// <summary>
    /// Gets a value indicating whether the emoji is a flag.
    /// </summary>
    public bool IsFlag { get; }
}
