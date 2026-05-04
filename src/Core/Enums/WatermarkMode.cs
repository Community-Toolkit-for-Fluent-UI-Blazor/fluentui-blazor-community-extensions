namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the available modes for displaying a watermark, indicating whether text, an image, or both are used.
/// </summary>
/// <remarks>Use this enumeration to select the type of watermark to apply. The mode determines if the watermark
/// consists of text, an image, or a combination of both. This can be useful for customizing visual overlays in user
/// interface components.</remarks>
public enum WatermarkMode
{
    /// <summary>
    /// Represents a watermark that consists solely of text.
    /// </summary>
    Text,

    /// <summary>
    /// Represents a watermark that consists solely of an image.
    /// </summary>
    Image,

    /// <summary>
    /// Represents a watermark that consists of both text and an image.
    /// </summary>
    Both
}
