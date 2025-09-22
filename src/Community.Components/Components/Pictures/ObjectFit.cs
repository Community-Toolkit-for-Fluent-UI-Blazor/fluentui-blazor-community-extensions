namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the object fit options for a picture component.
/// </summary>
public enum ObjectFit
{
    /// <summary>
    /// The default value, which means no specific object fit behavior is applied.
    /// </summary>
    None,

    /// <summary>
    /// Fills the entire container, potentially cropping the image if its aspect ratio differs from that of the container.
    /// </summary>
    Fill,

    /// <summary>
    /// Covers the entire container while maintaining the image's aspect ratio. This may result in some parts of the image being cropped.
    /// </summary>
    Cover,

    /// <summary>
    /// Contains the image within the container while maintaining its aspect ratio. This may result in empty space (letterboxing) if the aspect ratios differ.
    /// </summary>
    Contain,

    /// <summary>
    /// Reduces the size or scale of the object by a specified factor.
    /// </summary>
    ScaleDown
}
