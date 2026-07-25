namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies how the aspect ratio of images is determined when displayed in a slideshow.
/// </summary>
/// <remarks>Use this enumeration to control whether images in a slideshow retain their original aspect ratio or
/// are adjusted to fit the aspect ratio of the container. The BasedOnImage mode preserves the original proportions of
/// each image, while the BasedOnContainer mode scales images to match the container's aspect ratio, which can be useful
/// for responsive layouts.</remarks>
public enum SlideshowRatioMode
{
    /// <summary>
    /// Represents the aspect ratio based on the image's original dimensions.
    /// </summary>
    Image,

    /// <summary>
    /// Represents the aspect ratio based on the container's dimensions.
    /// </summary>
    Container
}
