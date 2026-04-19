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
    public static readonly Dictionary<ChartType, ChartCategory> Categories = new(EqualityComparer<ChartType>.Default)
    {
        { ChartType.Column, ChartCategory.Category },
        { ChartType.Bar, ChartCategory.Category },
        { ChartType.CategoryLine, ChartCategory.Category },
        { ChartType.CategoryArea, ChartCategory.Category },
        { ChartType.CategoryStep, ChartCategory.Category },
        { ChartType.StackedColumn, ChartCategory.Category },
        { ChartType.StackedBar, ChartCategory.Category },
        { ChartType.Stacked100Column, ChartCategory.Category },
        { ChartType.Stacked100Bar, ChartCategory.Category },
        { ChartType.StackedArea, ChartCategory.Category  },
        { ChartType.Stacked100Area, ChartCategory.Category },

        { ChartType.XYLine, ChartCategory.XY },
        { ChartType.XYArea, ChartCategory.XY },
        { ChartType.Scatter, ChartCategory.XY },
        { ChartType.Bubble, ChartCategory.XY },
        { ChartType.Histogram, ChartCategory.XY },
        { ChartType.Boxplot, ChartCategory.XY },
        { ChartType.Violin, ChartCategory.XY },
        { ChartType.Density, ChartCategory.XY },

        { ChartType.Candlestick, ChartCategory.Financial },
        { ChartType.OHLC, ChartCategory.Financial },

        { ChartType.Pie, ChartCategory.Polar },
        { ChartType.Donut, ChartCategory.Polar },
        { ChartType.SemiDonut, ChartCategory.Polar },
        { ChartType.MultiDonut, ChartCategory.Polar },
        { ChartType.Radar, ChartCategory.Polar },
        { ChartType.PolarArea, ChartCategory.Polar },
        { ChartType.PolarLine, ChartCategory.Polar },
        { ChartType.PolarBar, ChartCategory.Polar },

        { ChartType.Treemap, ChartCategory.Hierarchy },
        { ChartType.Sunburst, ChartCategory.Hierarchy },
        { ChartType.Icicle, ChartCategory.Hierarchy },
    };
}
