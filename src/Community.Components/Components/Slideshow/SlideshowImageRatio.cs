namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the available ratios for images in the <see cref="FluentCxSlideshow{TItem}" />.
/// </summary>
public enum SlideshowImageRatio
{
    /// <summary>
    /// If the image natural dimensions are greater than the container dimensions,
    ///  the image is shrinked, and the aspect ratio is maintened.
    /// </summary>
    Auto,

    /// <summary>
    /// The image fill the container.
    /// </summary>
    /// <remarks>When the dimensions of the container are not set, the width of the container is set to 100%,
    ///  after that, the first image in the slideshow, when stretched, gives the height of the container,
    ///  and the other images will take that height.</remarks>
    Fill

}
