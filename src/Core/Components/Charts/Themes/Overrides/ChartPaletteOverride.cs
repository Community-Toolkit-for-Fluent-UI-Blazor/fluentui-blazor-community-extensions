using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a set of color overrides for chart elements, allowing customization of the chart's palette.
/// </summary>
/// <remarks>Use this type to specify custom colors for various chart components, such as the background,
/// foreground, grid, axis, and data series. Any property left unset will use the chart's default palette for that
/// element.</remarks>
public sealed record ChartPaletteOverride
{
    /// <summary>
    /// Gets the background color override for the chart palette.
    /// </summary>
    public Srgb8? Background { get; init; }

    /// <summary>
    /// Gets the foreground color override for the chart palette.
    /// </summary>
    public Srgb8? Foreground { get; init; }

    /// <summary>
    /// Gets the grid color override for the chart palette.
    /// </summary>
    public Srgb8? Grid { get; init; }

    /// <summary>
    /// Gets the axis color override for the chart palette.
    /// </summary>
    public Srgb8? Axis { get; init; }

    /// <summary>
    /// Gets the collection of series color overrides for the chart palette.
    /// </summary>
    public IReadOnlyList<Srgb8>? Series { get; init; }

    /// <summary>
    /// Gets the collection of stroke colors used for rendering each data series.
    /// </summary>
    /// <remarks>The colors in this collection are applied in order to the corresponding data series. If there
    /// are more series than colors, the colors may be reused or default values may be applied, depending on the
    /// component's implementation.</remarks>
    public IReadOnlyList<Srgb8> SeriesStroke { get; init; } = [];
}
