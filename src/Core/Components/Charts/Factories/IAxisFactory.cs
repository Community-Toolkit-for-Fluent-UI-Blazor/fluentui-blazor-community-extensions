using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts.Factories;

/// <summary>
/// Represents a factory for creating chart axes based on the provided chart options, series data, and plot area configuration.
/// </summary>
/// <typeparam name="TOptions"></typeparam>
internal interface IAxisFactory<TOptions>
{
    /// <summary>
    /// Creates and configures the X and Y axes for a chart based on the specified options, data series, and plot area.
    /// </summary>
    /// <param name="sort">Value indicating whether the categories should be sorted.</param>
    /// <param name="series">The collection of data series to be plotted, which may influence axis scaling and labels.</param>
    /// <param name="plotArea">The plot area that defines the bounds within which the axes will be rendered.</param>
    /// <returns>A tuple containing the configured X-axis and Y-axis for the chart.</returns>
    (ChartAxis XAxis, ChartAxis YAxis) CreateAxes(
        bool sort,
        IEnumerable<ChartSerie> series,
        ChartRect plotArea);
}
