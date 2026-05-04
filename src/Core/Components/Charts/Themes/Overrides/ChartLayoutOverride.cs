namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents the layout settings for a chart, including padding, spacing,
///  gridline thickness, and other layout-related properties.
/// </summary>
public sealed record ChartLayoutOverride
{
    /// <summary>
    /// Gets the value of the top padding for the chart.
    /// </summary>
    public double? PaddingTop { get; init; }

    /// <summary>
    /// Gets the value of the bottom padding for the chart.
    /// </summary>
    public double? PaddingBottom { get; init; }

    /// <summary>
    /// Gets the value of the left padding for the chart.
    /// </summary>
    public double? PaddingLeft { get; init; }

    /// <summary>
    /// Gets the value of the right padding for the chart.
    /// </summary>
    public double? PaddingRight { get; init; }

    /// <summary>
    /// Gets the spacing between the title and adjacent elements.
    /// </summary>
    public double? TitleSpacing { get; init; }

    /// <summary>
    /// Gets the spacing between the subtitle and adjacent elements.
    /// </summary>
    public double? SubtitleSpacing { get; init; }

    /// <summary>
    /// Gets the legend spacing, which defines the distance between the legend and adjacent elements.
    /// </summary>
    public double? LegendSpacing { get; init; }

    /// <summary>
    /// Gets the spacing between axes in the layout.
    /// </summary>
    public double? AxisSpacing { get; init; }

    /// <summary>
    /// Gets the thickness of the grid lines.
    /// </summary>
    public double? GridThickness { get; init; }

    /// <summary>
    /// Gets the thickness of the axis lines.
    /// </summary>
    public double? AxisThickness { get; init; }

    /// <summary>
    /// Gets the length of the tick marks on the axes.
    /// </summary>
    public double? TickLength { get; init; }

    /// <summary>
    /// Gets the radius applied to the corners of bars in bar charts.
    /// </summary>
    public double? BarCornerRadius { get; init; }

    /// <summary>
    /// Gets the minimum radius of the bubble.
    /// </summary>
    public double? MinBubbleRadius { get; init; }

    /// <summary>
    /// Gets the maximum radius of the bubble.
    /// </summary>
    public double? MaxBubbleRadius { get; init; }

    /// <summary>
    /// Gets the smoothing factor applied to lines for rendering purposes.
    /// </summary>
    /// <remarks>A higher value results in smoother, more curved lines, while a lower value produces
    /// straighter lines. The value typically ranges from 0 (no smoothing) to 1 (maximum smoothing).</remarks>
    public double? LineSmoothing { get; init; }

    /// <summary>
    /// Gets the gap, in pixels, between adjacent data series in the chart.
    /// </summary>
    public double? SeriesGap { get; init; }

    /// <summary>
    /// Gets the spacing, in pixels, between adjacent categories in the chart.
    /// </summary>
    public double? CategoryGap { get; init; }

    /// <summary>
    /// Gets the left padding applied to the plot area of the chart.
    /// </summary>
    public double PlotPaddingLeft { get; init; } = 4;

    /// <summary>
    /// Gets the right padding for the plot area in pixels.
    /// </summary>
    public double PlotPaddingRight { get; init; } = 4;

    /// <summary>
    /// Gets the top padding for the plot area in pixels.
    /// </summary>
    public double PlotPaddingTop { get; init; } = 4;

    /// <summary>
    /// Gets the bottom padding for the plot area in pixels.
    /// </summary>
    public double PlotPaddingBottom { get; init; } = 4;

}
