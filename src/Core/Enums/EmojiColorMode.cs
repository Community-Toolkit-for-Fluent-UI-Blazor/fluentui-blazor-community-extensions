namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Represents the color mode of an emoji, which can be full color, flat color, variable color (COLRv1), or text-only (no color).
/// </summary>
public enum EmojiColorMode
{
    /// <summary>
    /// Color mode is determined automatically based on the emoji and the platform's capabilities.
    /// </summary>
    FullColor,

    /// <summary>
    /// Color mode is flat color, which means the emoji is rendered in a single color without gradients or shading.
    /// </summary>
    FlatColor,

    /// <summary>
    /// Color mode is variable color, which means the emoji is rendered using the COLRv1 format with gradients and shading.
    /// </summary>
    VariableColor,

    /// <summary>
    /// Color mode is text-only, which means the emoji is rendered without any color (fallback to text representation).
    /// </summary>  
    TextOnly
}
