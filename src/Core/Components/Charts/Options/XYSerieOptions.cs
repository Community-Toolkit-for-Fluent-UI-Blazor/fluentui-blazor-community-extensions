namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents the options for an XY chart serie.
/// </summary>
public sealed class XYSeriesOptions : IChartSerieOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether line smoothing is enabled for rendering operations.
    /// </summary>
    public bool SmoothLines { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether markers are displayed at data points on the chart.
    /// </summary>
    public bool ShowMarkers { get; set; } = true;

    /// <summary>
    /// Gets or sets the size of the markers displayed at data points on the chart.
    /// </summary>
    public double MarkerSize { get; set; } = 4;

    /// <summary>
    /// Gets or sets a value indicating whether lines are displayed connecting the data points on the chart.
    /// </summary>
    public bool ShowLine { get; set; } = true;

    /// <inheritdoc />
    public ChartAnimationOptions? Animation { get; set; }
}

