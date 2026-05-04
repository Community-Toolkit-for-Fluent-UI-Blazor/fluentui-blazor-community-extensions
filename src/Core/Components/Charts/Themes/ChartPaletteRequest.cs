using FluentUI.Blazor.Community.Components.ColorSpace.Icc;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a request for generating a chart palette based on specific parameters.
/// </summary>
public sealed record ChartPaletteRequest
{
    /// <summary>
    /// Gets the number of colors in the palette.
    /// </summary>
    public int ColorCount { get; init; }

    /// <summary>
    /// Gets the type of color vision to use.
    /// </summary>
    public ColorVisionType Vision { get; init; }

    /// <summary>
    /// Gets the RGB working space to use for color calculations.
    /// </summary>
    public RgbWorkingSpace WorkingSpace { get; init; } = RgbWorkingSpace.Create(IccProfileName.Srgb);

    /// <summary>
    /// Gets the palette style used to render the chart elements.
    /// </summary>
    public ChartPaletteStyle Style { get; init; }

    /// <summary>
    /// Gets the chart theme options used to configure the appearance and behavior of the chart.
    /// </summary>
    /// <remarks>Use this property to customize various aspects of the chart's theme, such as colors, fonts,
    /// and layout settings. The options are initialized with default values and can be set during object
    /// initialization.</remarks>
    public ChartThemeOptions Options { get; init; } = new();

    /// <summary>
    /// Gets the base sRGB color used for color calculations or rendering operations.
    /// </summary>
    public Srgb8? BaseColor { get; init; }
}
