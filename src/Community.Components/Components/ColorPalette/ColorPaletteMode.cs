namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the mode used for generating color palettes.
/// </summary>
public enum ColorPaletteMode
{
    /// <summary>
    /// Represents no specific palette mode.
    /// </summary>
    None,

    /// <summary>
    /// Represents a palette mode where colors are provided directly.
    /// </summary>
    Provided,

    /// <summary>
    /// Represents a palette mode where colors are generated randomly.
    /// </summary>
    Random,

    /// <summary>
    /// Represents a palette mode where colors are generated in a gradient.
    /// </summary>
    Gradient,

    /// <summary>
    /// Represents a palette mode where colors are generated using a custom gradient.
    /// </summary>
    CustomGradient,

    /// <summary>
    /// Represents a palette mode where colors are generated using complementary colors.
    /// </summary>
    Complementary,

    /// <summary>
    /// Represents a palette mode where colors are generated using analogous colors.
    /// </summary>
    Analogous,

    /// <summary>
    /// Represents a palette mode where colors are generated using triadic colors.
    /// </summary>
    Triadic,

    /// <summary>
    /// Represents a palette mode where colors are generated using tetradic colors.
    /// </summary>
    Tetradic,

    /// <summary>
    /// Represents a palette mode where colors are generated using split-complementary colors.
    /// </summary>
    SplitComplementary,

    /// <summary>
    /// Represents a palette mode where colors are generated in a monochrome scheme.
    /// </summary>
    Monochrome,

    /// <summary>
    /// Represents a palette mode where colors are generated in a warm scheme with variations.
    /// </summary>
    Warm,

    /// <summary>
    /// Represents a palette mode where colors are generated in a cool scheme with cool variations.
    /// </summary>
    Cool,

    /// <summary>
    /// Represents a palette mode where colors are generated in a pastel scheme.
    /// </summary>
    Pastel,

    /// <summary>
    /// Represents a palette mode where colors are generated in a vibrant scheme.
    /// </summary>
    Neon,

    /// <summary>
    /// Represents a palette mode where colors are generated in a greyscale scheme.
    /// </summary>
    Greyscale,

    /// <summary>
    /// Represents a palette mode where colors are generated to be accessibility safe.
    /// </summary>
    AccessibilitySafe,

    /// <summary>
    /// Represents a palette mode where colors are generated based on an image.
    /// </summary>
    FromImage,

    /// <summary>
    /// Represents a palette mode where colors are generated in a desaturated scheme.
    /// </summary>
    Desaturate
}

