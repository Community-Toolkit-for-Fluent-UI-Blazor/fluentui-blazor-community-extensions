namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the background style to use for displaying subtitles.
/// </summary>
/// <remarks>Use this enumeration to control the opacity and appearance of subtitle backgrounds, which can improve
/// readability depending on the video content and user preferences.</remarks>
public enum SubtitleBackground
{
    /// <summary>
    /// Represents a solid fill style.
    /// </summary>
    /// <remarks>
    /// Opacity is set to 100%, meaning the background completely obscures any content behind it.
    /// </remarks>
    Solid,

    /// <summary>
    /// Represents a color or state that is opaque.
    /// </summary>
    /// <remarks>
    /// Opacity is set to 85% to ensure readability while still allowing some background visibility.
    /// </remarks>
    Opaque,

    /// <summary>
    /// Represents a color or state that is partially transparent, allowing some background to show through.
    /// </summary>
    /// <remarks>
    /// Opacity is set to 50%, balancing readability with background visibility.
    /// </remarks>
    SemiTransparent,

    /// <summary>
    /// Specifies that the element is fully transparent and does not obscure underlying content.
    /// </summary>
    /// <remarks>
    /// Opacity is set to 0%, making the background completely see-through.
    /// </remarks>
    Transparent
}
