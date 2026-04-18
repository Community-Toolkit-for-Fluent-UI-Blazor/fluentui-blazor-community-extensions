using FluentUI.Blazor.Community.Components.ColorSpace.Icc;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a chart theme that encapsulates various aspects of chart styling.
/// </summary>
public sealed record ChartTheme
{
    /// <summary>
    /// Gets the metadata associated with the chart theme, including information about the theme's name, description, and other relevant details.
    /// </summary>
    public ChartThemeMetadata Metadata { get; init; } = new();

    /// <summary>
    /// Gets the palette of colors used in the chart theme, which defines the color scheme for various chart elements such as bars, lines, and backgrounds.
    /// </summary>
    public ChartPalette Palette { get; init; } = new();

    /// <summary>
    /// Gets the collection of palette providers used to supply color palettes for chart elements.
    /// </summary>
    /// <remarks>Use this property to customize or extend the color schemes applied to charts by providing
    /// additional or alternative palette providers. The collection is initialized by default and can be configured
    /// during object initialization.</remarks>
    public ChartPaletteProviders PaletteProviders { get; init; } = new();

    /// <summary>
    /// Gets the typography settings for the chart theme, which define the font styles, sizes,
    ///  and other typographic properties used in chart elements such as titles, labels, and legends.
    /// </summary>
    public ChartTypography Typography { get; init; } = new();

    /// <summary>
    /// Gets the layout configuration for the chart.
    /// </summary>
    public ChartLayout Layout { get; init; } = new();

    /// <summary>
    /// Gets the collection of chart strategies used to configure chart behavior.
    /// </summary>
    public ChartAnimationStrategies Strategies { get; init; } = new();

    /// <summary>
    /// Gets the RGB working space used for color calculations.
    /// </summary>
    public RgbWorkingSpace WorkingSpace { get; init; } = RgbWorkingSpace.Create(IccProfileName.Srgb);

    /// <summary>
    /// Gets the palette style used for the chart theme.
    /// </summary>
    public ChartPaletteStyle PaletteStyle { get; init; }
}
