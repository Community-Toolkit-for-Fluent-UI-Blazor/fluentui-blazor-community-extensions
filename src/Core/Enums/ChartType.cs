namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Defines all supported chart types. 
/// Only actual chart visualizations appear here.
/// Non-chart layers (axes, grid, background, annotations) are handled separately.
/// </summary>
public enum ChartType
{
    /// <summary>
    /// Vertical bars grouped by category.
    /// </summary>
    Column,

    /// <summary>
    /// Horizontal bars grouped by category.
    /// </summary>
    Bar,

    /// <summary>
    /// Line chart using categorical X-axis.
    /// </summary>
    Line,

    /// <summary>
    /// Area chart using categorical X-axis.
    /// </summary>
    Area,

    /// <summary>
    /// Step line chart using categorical X-axis.
    /// </summary>
    Step,

    /// <summary>
    /// Stacked version of Column chart.
    /// </summary>
    StackedColumn,

    /// <summary>
    /// Stacked version of Bar chart.
    /// </summary>
    StackedBar,

    /// <summary>
    /// Stacked version of Area chart.
    /// </summary>
    StackedArea,

    /// <summary>
    /// Stacked version of Step chart.
    /// </summary>
    StackedStep,

    /// <summary>
    /// 100% stacked Column chart.
    /// </summary>
    Stacked100Column,

    /// <summary>
    /// 100% stacked Bar chart.
    /// </summary>
    Stacked100Bar,

    /// <summary>
    /// 100% stacked Area chart.
    /// </summary>
    Stacked100Area,

    /// <summary>
    /// 100% stacked Step chart.
    /// </summary>
    Stacked100Step,

    /// <summary>
    /// Standard XY line chart.
    /// </summary>
    XYLine,

    /// <summary>
    /// XY area chart.
    /// </summary>
    XYArea,

    /// <summary>
    /// XY column chart (vertical bars with XY coordinates).
    /// </summary>
    XYColumn,

    /// <summary>
    /// Scatter plot.
    /// </summary>
    Scatter,

    /// <summary>
    /// Bubble chart.
    /// </summary>
    Bubble,

    /// <summary>
    /// Candlestick financial chart.
    /// </summary>
    Candlestick,

    /// <summary>
    /// OHLC financial chart.
    /// </summary>
    OHLC,

    /// <summary>
    /// Standard pie chart.
    /// </summary>
    Pie,

    /// <summary>
    /// Donut chart with inner radius.
    /// </summary>
    Donut,

    /// <summary>
    /// Semi-donut chart (half circle).
    /// </summary>
    SemiDonut,

    /// <summary>
    /// Multi-layer donut chart.
    /// </summary>
    MultiDonut,

    /// <summary>
    /// Radar chart (spider chart).
    /// </summary>
    Radar,

    /// <summary>
    /// Polar area chart.
    /// </summary>
    PolarArea,

    /// <summary>
    /// Polar line chart.
    /// </summary>
    PolarLine,

    /// <summary>
    /// Polar bar chart.
    /// </summary>
    PolarBar,

    /// <summary>
    /// Polar scatter chart.
    /// </summary>
    PolarScatter,

    /// <summary>
    /// Polar bubble chart.
    /// </summary>
    PolarBubble,

    /// <summary>
    /// Polar rose chart
    /// </summary>
    PolarRose,

    /// <summary>
    /// Treemap chart.
    /// </summary>
    Treemap,

    /// <summary>
    /// Sunburst chart.
    /// </summary>
    Sunburst,

    /// <summary>
    /// Icicle chart.
    /// </summary>
    Icicle,

    /// <summary>
    /// Histogram chart.
    /// </summary>
    Histogram,

    /// <summary>
    /// Boxplot chart.
    /// </summary>
    Boxplot,

    /// <summary>
    /// Violin plot.
    /// </summary>
    Violin,

    /// <summary>
    /// Density plot.
    /// </summary>
    Density,

    /// <summary>
    /// Partition chart (used for hierarchical data visualization).
    /// </summary>
    Partition,

    /// <summary>
    /// Dendrogram chart.
    /// </summary>
    Dendrogram,

    /// <summary>
    /// Radial tree chart.
    /// </summary>
    RadialTree,

    /// <summary>
    /// Tree chart.
    /// </summary>
    Tree

    /*
    Waterfall,
    StackedWaterfall,
    Stacked100Waterfall,

    Heatmap,
    CalendarHeatmap,

    Sankey,
    Chord,

    Streamgraph,
    Marimekko,
    Bullet,
    Gantt,
    ParallelCoordinates,
    Boxen,*/
}
