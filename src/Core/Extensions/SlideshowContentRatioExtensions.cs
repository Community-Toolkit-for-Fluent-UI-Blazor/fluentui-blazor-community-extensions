namespace FluentUI.Blazor.Community.Components.Extensions;

/// <summary>
/// Provides extension methods for converting slideshow content ratios to their corresponding CSS aspect ratio string
/// representations.
/// </summary>
/// <remarks>This class enables mapping between predefined slideshow content ratios and standard CSS aspect ratio
/// formats. The extension methods are intended for use when generating CSS styles for responsive slideshow layouts. If
/// a ratio does not have a defined CSS aspect ratio, the method returns null.</remarks>
internal static class SlideshowContentRatioExtensions
{
    /// <summary>
    /// Represents the value of a square aspect ratio (1:1).
    /// </summary>
    private const double Square = 1.0 / 1.0;

    /// <summary>
    /// Represents the aspect ratio for landscape orientation, calculated as 4:3.
    /// </summary>
    private const double Landscape = 4.0 / 3.0;

    /// <summary>
    /// Represents the aspect ratio for widescreen orientation, calculated as 16:9. 
    /// </summary>
    private const double Widescreen = 16.0 / 9.0;

    /// <summary>
    /// Represents the aspect ratio for ultra-widescreen displays, calculated as 21:9.
    /// </summary>
    private const double UltraWidescreen = 21.0 / 9.0;

    /// <summary>
    /// Represents the aspect ratio for portrait orientation, calculated as 3:4.
    /// </summary>
    private const double Portrait = 3.0 / 4.0;

    /// <summary>
    /// Represents the vertical aspect ratio of a display, calculated as 9 divided by 16.
    /// </summary>
    private const double Vertical = 9.0 / 16.0;

    /// <summary>
    /// Represents the aspect ratio for portrait-oriented social media images, defined as 4:5.
    /// </summary>
    private const double PortraitSocial = 4.0 / 5.0;

    /// <summary>
    /// Represents the standard aspect ratio of a story, defined as the ratio of its height to its width.
    /// </summary>
    private const double Story = 9.0 / 16.0;

    /// <summary>
    /// Represents the aspect ratio for cover photos, calculated as 16:7.
    /// </summary>
    private const double CoverPhoto = 16.0 / 7.0;

    /// <summary>
    /// Represents the aspect ratio based on the golden ratio, approximately 1.618:1.
    /// </summary>
    private const double GoldenRatio = 1.618 / 1.0;

    /// <summary>
    /// Represents the aspect ratio for A4 paper in portrait orientation, calculated as 1:√2.
    /// </summary>
    private const double A4Portrait = 1.0 / 1.414;

    /// <summary>
    /// Represents the aspect ratio of A4 paper in landscape orientation, calculated as √2:1.
    /// </summary>
    private const double A4Landscape = 1.414 / 1.0;

    /// <summary>
    /// Converts a specified slideshow content ratio to its corresponding CSS aspect-ratio value as a string.
    /// </summary>
    /// <remarks>This method supports several common aspect ratios, including square, landscape, widescreen,
    /// portrait, and others. The returned string is suitable for use with the CSS 'aspect-ratio' property.</remarks>
    /// <param name="ratio">The slideshow content ratio to convert. Determines the aspect ratio string to be returned.</param>
    /// <returns>A string representing the CSS aspect-ratio value for the specified slideshow content ratio, or null if the ratio
    /// is 'Original', 'FullContainer', or an unrecognized value.</returns>
    internal static double ToFormula(this SlideshowContentRatio ratio)
    {
        return ratio switch
        {
            SlideshowContentRatio.Square => Square,
            SlideshowContentRatio.Landscape => Landscape,
            SlideshowContentRatio.Widescreen => Widescreen,
            SlideshowContentRatio.UltraWidescreen => UltraWidescreen,
            SlideshowContentRatio.Portrait => Portrait,
            SlideshowContentRatio.Vertical => Vertical,
            SlideshowContentRatio.PortraitSocial => PortraitSocial,
            SlideshowContentRatio.Story => Story,
            SlideshowContentRatio.CoverPhoto => CoverPhoto,
            SlideshowContentRatio.GoldenRatio => GoldenRatio,
            SlideshowContentRatio.A4Portrait => A4Portrait,
            SlideshowContentRatio.A4Landscape => A4Landscape,
            _ => double.NaN
        };
    }
}
