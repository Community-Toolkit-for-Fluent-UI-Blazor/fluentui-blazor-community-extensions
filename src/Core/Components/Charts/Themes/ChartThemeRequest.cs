using FluentUI.Blazor.Community.Components.ColorSpace.Icc;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a request to generate a chart theme with specified palette, color vision, and working space settings.
/// </summary>
/// <remarks>Use this record to specify the parameters required for generating a chart theme, including the number
/// of series, categories, color vision accessibility, and palette style. This type is immutable and intended for use as
/// a data transfer object when creating or customizing chart themes.</remarks>
public sealed record ChartThemeRequest
{
    /// <summary>
    /// Gets the name associated with the instance.
    /// </summary>
    public string Name { get; init; } = "Generated";

    /// <summary>
    /// Gets the number of colors for the request.
    /// </summary>
    public int ColorCount { get; init; }

    /// <summary>
    /// Gets the type of color vision to simulate.
    /// </summary>
    public ColorVisionType Vision { get; init; }

    /// <summary>
    /// Gets the RGB working space used for color calculations.
    /// </summary>
    /// <remarks>The working space defines the color profile and characteristics for RGB color operations.
    /// Changing the working space may affect color conversions and rendering results.</remarks>
    public RgbWorkingSpace WorkingSpace { get; init; } = RgbWorkingSpace.Create(IccProfileName.Srgb);

    /// <summary>
    /// Gets the palette style used to determine the color scheme for the chart elements.
    /// </summary>
    /// <remarks>Use this property to specify how colors are applied to chart series and data points. The
    /// selected palette style affects the overall appearance and visual consistency of the chart.</remarks>
    public ChartPaletteStyle PaletteStyle { get; init; }

    /// <summary>
    /// Gets the chart theme options used to configure the appearance and behavior of the chart.
    /// </summary>
    /// <remarks>Use this property to customize various aspects of the chart's theme, such as colors, fonts,
    /// and layout settings. The options are initialized with default values and can be set during object
    /// initialization.</remarks>
    public ChartThemeOptions Options { get; init; } = new();

    /// <summary>
    /// Gets the name of the custom palette provider to use for generating the chart palette.
    /// </summary>
    public string? CustomPaletteProvider { get; init; }
}
