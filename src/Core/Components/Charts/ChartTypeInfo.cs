using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts;

/// <summary>
/// Represents metadata about chart types, such as their categories.
/// </summary>
internal static class ChartTypeInfo
{
    /// <summary>
    /// Provides a mapping between chart types and their corresponding chart categories.
    /// </summary>
    /// <remarks>This dictionary allows quick lookup of the category for a given chart type, which can be
    /// useful for rendering logic, filtering, or grouping charts by category. The mapping is static and
    /// read-only.</remarks>
    public static readonly Dictionary<ChartType, ChartFamily> Categories = new(EqualityComparer<ChartType>.Default)
    {
        [ChartType.Column] = ChartFamily.Category,
        [ChartType.Bar] = ChartFamily.Category,
        [ChartType.Line] = ChartFamily.Category,
        [ChartType.Area] = ChartFamily.Category,
        [ChartType.Step] = ChartFamily.Category,
        [ChartType.StackedColumn] = ChartFamily.Category,
        [ChartType.StackedBar] = ChartFamily.Category,
        [ChartType.Stacked100Column] = ChartFamily.Category,
        [ChartType.Stacked100Bar] = ChartFamily.Category,
        [ChartType.StackedArea] = ChartFamily.Category,
        [ChartType.Stacked100Area] = ChartFamily.Category,
        [ChartType.StackedStep] = ChartFamily.Category,
        [ChartType.Stacked100Step] = ChartFamily.Category,

        [ChartType.XYLine] = ChartFamily.XY,
        [ChartType.XYArea] = ChartFamily.XY,
        [ChartType.Scatter] = ChartFamily.XY,
        [ChartType.Bubble] = ChartFamily.XY,
        [ChartType.Boxplot] = ChartFamily.XY,
        [ChartType.Violin] = ChartFamily.XY,
        [ChartType.Density] = ChartFamily.XY,
        [ChartType.XYColumn] = ChartFamily.XY,

        [ChartType.Histogram] = ChartFamily.Histogram,

        [ChartType.Candlestick] = ChartFamily.Financial,
        [ChartType.OHLC] = ChartFamily.Financial,

        [ChartType.Pie] = ChartFamily.Circular,
        [ChartType.Donut] = ChartFamily.Circular,
        [ChartType.SemiDonut] = ChartFamily.Circular,
        [ChartType.MultiDonut] = ChartFamily.Circular,

        [ChartType.Radar] = ChartFamily.Polar,
        [ChartType.PolarArea] = ChartFamily.Polar,
        [ChartType.PolarLine] = ChartFamily.Polar,
        [ChartType.PolarBar] = ChartFamily.Polar,
        [ChartType.PolarRose] = ChartFamily.Polar,
        [ChartType.PolarScatter] = ChartFamily.Polar,
        [ChartType.PolarBubble] = ChartFamily.Polar,

        [ChartType.Treemap] = ChartFamily.Hierarchy,
        [ChartType.Sunburst] = ChartFamily.Hierarchy,
        [ChartType.Icicle] = ChartFamily.Hierarchy,
        [ChartType.Partition] = ChartFamily.Hierarchy,
        [ChartType.Tree] = ChartFamily.Hierarchy,
        [ChartType.RadialTree] = ChartFamily.Hierarchy,
        [ChartType.Dendrogram] = ChartFamily.Hierarchy,
    };
}
