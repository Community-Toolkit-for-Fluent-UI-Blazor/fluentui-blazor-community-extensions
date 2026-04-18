namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents the layout settings for a chart, including padding, spacing,
///  gridline thickness, and other layout-related properties.
/// </summary>
public sealed record ChartLayout
{
    /// <summary>
    /// Gets the value of the top padding for the chart.
    /// </summary>
    public double PaddingTop { get; init; } = 16;

    /// <summary>
    /// Gets the value of the bottom padding for the chart.
    /// </summary>
    public double PaddingBottom { get; init; } = 16;

    /// <summary>
    /// Gets the value of the left padding for the chart.
    /// </summary>
    public double PaddingLeft { get; init; } = 16;

    /// <summary>
    /// Gets the value of the right padding for the chart.
    /// </summary>
    public double PaddingRight { get; init; } = 16;

    /// <summary>
    /// Gets the spacing between the title and adjacent elements.
    /// </summary>
    public double TitleSpacing { get; init; } = 12;

    /// <summary>
    /// Gets the spacing between the subtitle and adjacent elements.
    /// </summary>
    public double SubtitleSpacing { get; init; } = 8;

    /// <summary>
    /// Gets the legend spacing, which defines the distance between the legend and adjacent elements.
    /// </summary>
    public double LegendSpacing { get; init; } = 12;

    /// <summary>
    /// Gets the spacing between axes in the layout.
    /// </summary>
    public double AxisSpacing { get; init; } = 8;

    /// <summary>
    /// Gets the thickness of the grid lines.
    /// </summary>
    public double GridThickness { get; init; } = 1;

    /// <summary>
    /// Gets the thickness of the axis lines.
    /// </summary>
    public double AxisThickness { get; init; } = 1.5;

    /// <summary>
    /// Gets the length of the tick marks on the axes.
    /// </summary>
    public double TickLength { get; init; } = 4;

    /// <summary>
    /// Gets the radius applied to the corners of bars in bar charts.
    /// </summary>
    public double BarCornerRadius { get; init; } = 4;

    /// <summary>
    /// Gets the minimum radius of the bubble.
    /// </summary>
    public double MinBubbleRadius { get; init; } = 6;

    /// <summary>
    /// Gets the maximum radius of the bubble.
    /// </summary>
    public double MaxBubbleRadius { get; init; } = 60;

    /// <summary>
    /// Gets the smoothing factor applied to lines for rendering purposes.
    /// </summary>
    /// <remarks>A higher value results in smoother, more curved lines, while a lower value produces
    /// straighter lines. The value typically ranges from 0 (no smoothing) to 1 (maximum smoothing).</remarks>
    public double LineSmoothing { get; init; } = 0.25;

    /// <summary>
    /// Gets the gap, in pixels, between adjacent data series in the chart.
    /// </summary>
    public double SeriesGap { get; init; } = 8;

    /// <summary>
    /// Gets the spacing, in pixels, between adjacent categories in the chart.
    /// </summary>
    public double CategoryGap { get; init; } = 4;

    /// <summary>
    /// Gets the maximum number of legend items that can be displayed.
    /// </summary>
    public int MaxLegendItems { get; init; } = 8;

    /// <summary>
    /// Gets the spacing, in pixels, between legend items in the chart legend.
    /// </summary>
    public double LegendItemSpacing { get; init; } = 4;

    /// <summary>
    /// Gets the size, of the shape used to represent legend items in the chart legend.
    /// </summary>
    public double LegendShapeSize { get; init; } = 12;

    /// <summary>
    /// Gets the spacing,between the shape and text of legend items in the chart legend.
    /// </summary>
    public double LegendShapeTextSpacing { get; init; } = 8;

    /// <summary>
    /// Gets the horizontal padding, in pixels, applied to the content of the chart legend.
    /// </summary>
    public double LegendHorizontalPadding { get; init; } = 12;

    /// <summary>
    /// Gets the number of characters to display for legend labels before truncating with an ellipsis.
    /// </summary>
    public int LegendMaxLabelCharacters { get; init; } = 12;

    /// <summary>
    /// Gets the minimum width, in pixels, of the chart legend.
    /// </summary>
    public int LegendMinWidth { get; init; } = 100;

    /// <summary>
    /// Gets the vertical padding, in pixels, applied to the content of the chart legend.
    /// </summary>
    public double LegendVerticalPadding { get; init; } = 4;

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
