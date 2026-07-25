namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the available aspect ratios for displaying slideshow content.
/// </summary>
/// <remarks>Use this enumeration to select a predefined aspect ratio when formatting images or media within a
/// slideshow. Each value corresponds to a commonly used ratio, suitable for different types of content and display
/// scenarios. Selecting an appropriate ratio can help ensure that content is presented optimally across various devices
/// and layouts.</remarks>
public enum SlideshowContentRatio
{
    /// <summary>
    /// Represents the original ratio of the content.
    /// </summary>
    Original,

    /// <summary>
    /// Represents a square aspect ratio (1:1).
    /// </summary>
    Square,

    /// <summary>
    /// Represents a landscape aspect ratio (4:3).
    /// </summary>
    Landscape,

    /// <summary>
    /// Represents a widescreen aspect ratio (16:9).
    /// </summary>
    Widescreen,

    /// <summary>
    /// Represents an ultra-widescreen aspect ratio (21:9).
    /// </summary>
    UltraWidescreen,

    /// <summary>
    /// Represents a portrait aspect ratio (3:4).
    /// </summary>
    Portrait,

    /// <summary>
    /// Represents a vertical aspect ratio (9:16).
    /// </summary>
    Vertical,

    /// <summary>
    /// Represents a portrait aspect ratio commonly used for social media (4:5).
    /// </summary>
    PortraitSocial,

    /// <summary>
    /// Represents a story aspect ratio commonly used for social media stories (9:16).
    /// </summary>
    Story,

    /// <summary>
    /// Represents a cover photo aspect ratio (16:7).
    /// </summary>
    CoverPhoto,

    /// <summary>
    /// Represents a golden ratio aspect ratio (approximately 1.618:1).
    /// </summary>
    GoldenRatio,

    /// <summary>
    /// Represents an A4 paper size in portrait orientation (approximately 1:1.414).
    /// </summary>
    A4Portrait,

    /// <summary>
    /// Represents an A4 paper size in landscape orientation (approximately 1.414:1).
    /// </summary>
    A4Landscape,

    /// <summary>
    /// Represents a full container aspect ratio, filling the available space.
    /// </summary>
    /// <remarks>This option scales the content to completely fill its container, which may involve cropping or stretching.</remarks>
    FullContainer
}
