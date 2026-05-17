namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Representrs the rendering mode of an emoji, which can be either color, monochrome, or variable (COLRv1).
/// </summary>
public enum EmojiRenderMode
{
    /// <summary>
    /// Rendering mode is determined automatically based on the emoji and the platform's capabilities.
    /// </summary>
    Auto,

    /// <summary>
    /// Emoji is rendered in color.
    /// </summary>
    Color,

    /// <summary>
    /// Emoji is rendered in monochrome.
    /// </summary>
    Monochrome,

    /// <summary>
    /// Emoji is rendered using the COLRv1 format.
    /// </summary>
    Variable
}
