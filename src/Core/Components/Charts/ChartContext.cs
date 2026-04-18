using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts;

/// <summary>
/// Provides contextual information for rendering a chart, including its plot area and axes.
/// </summary>
/// <remarks>Use this class to encapsulate the main components required to describe a chart's layout and
/// coordinate system. The context is typically passed to rendering or layout routines that require access to the
/// chart's axes and plot area.</remarks>
internal sealed record ChartContext
{
    /// <summary>
    /// Gets or sets the identifier of the chart.
    /// </summary>
    public string ChartId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the width value.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the height value.
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// Gets or sets the plot area configuration for the chart.
    /// </summary>
    /// <remarks>The plot area defines the region of the chart where data series are rendered. This
    /// property must be set during object initialization and cannot be modified afterwards.</remarks>
    public ChartRect PlotArea { get; set; } = new();

    /// <summary>
    /// Gets or sets the area of the chart in which data is rendered.
    /// </summary>
    public ChartRect ChartArea { get; set; } = new();

    /// <summary>
    /// Gets or sets the axis configuration for the X-axis of the chart.
    /// </summary>
    public ChartAxis? XAxis { get; set; } = new();

    /// <summary>
    /// Gets or sets the axis configuration for the Y-axis of the chart.
    /// </summary>
    public ChartAxis? YAxis { get; set; } = new();

    /// <summary>
    /// Gets a value indicating whether the chart's plot area or data series need to be recalculated and redrawn.
    /// </summary>
    public bool Dirty { get; internal set; }

    /// <summary>
    /// Gets the area of the chart reserved for the title.
    /// </summary>
    public ChartRect TitleArea { get; internal set; } = ChartRect.Empty;

    /// <summary>
    /// Gets the area of the chart reserved for the subtitle.
    /// </summary>
    public ChartRect SubtitleArea { get; internal set; } = ChartRect.Empty;

    /// <summary>
    /// Gets the bounding rectangle that defines the area occupied by the chart legend.
    /// </summary>
    public ChartRect LegendArea { get; internal set; } = ChartRect.Empty;

    /// <summary>
    /// Gets the remaining area of the chart that is not occupied by the title, subtitle, or legend.
    /// </summary>
    public ChartRect RemainingSpaceArea { get; internal set; } = ChartRect.Empty;

    /// <summary>
    /// Gets the margins to apply around the axes of the chart.
    /// </summary>
    public Thickness AxesMargins { get; internal set; } = Thickness.Empty;

    /// <summary>
    /// Gets the number of legend items that are currently visible in the chart's legend area.
    /// </summary>
    public int LegendItemCount { get; internal set; }

    /// <summary>
    /// Gets the unique identifier for the clipping path used in the chart's rendering context, if applicable.
    /// </summary>
    public string? ClipPathId { get; internal set; }
}
