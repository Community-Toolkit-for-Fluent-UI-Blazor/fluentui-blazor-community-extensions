using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts;

/// <summary>
/// Represents a validator for chart type rules.
/// </summary>
internal static class ChartTypeRuleValidator
{
    internal static readonly Dictionary<ChartType, ChartTypeRule> Rules = new(EqualityComparer<ChartType>.Default)
    {
        [ChartType.Radar] = GetDefaultPolar(),
        [ChartType.PolarLine] = GetDefaultPolar(),
        [ChartType.PolarScatter] = GetDefaultPolar(),
        [ChartType.PolarBubble] = GetDefaultPolar(),
        [ChartType.PolarBar] = GetDefaultPolar(),
        [ChartType.PolarArea] = GetDefault(ChartFamily.Polar),
        [ChartType.PolarRose] = GetDefault(ChartFamily.Polar),
        [ChartType.Pie] = GetDefault(ChartFamily.Circular),
        [ChartType.Donut] = GetDefault(ChartFamily.Circular),
        [ChartType.Bar] = GetDefaultWithTrue(ChartFamily.Category),
        [ChartType.Step] = GetDefaultWithTrue(ChartFamily.Category),
        [ChartType.StackedColumn] = GetDefaultWithTrue(ChartFamily.Category),
        [ChartType.StackedBar] = GetDefaultWithTrue(ChartFamily.Category),
        [ChartType.StackedStep] = GetDefaultWithTrue(ChartFamily.Category),
        [ChartType.Scatter] = GetDefaultWithTrue(ChartFamily.XY),
        [ChartType.Bubble] = GetDefaultWithTrue(ChartFamily.XY),
        [ChartType.Column] = GetDefaultWithTrue(ChartFamily.Category),
        [ChartType.StackedArea] = GetDefaultWithTrue(ChartFamily.Category),
        [ChartType.Stacked100Bar] = GetDefaultWithTrue(ChartFamily.Category),
        [ChartType.Stacked100Area] = GetDefaultWithTrue(ChartFamily.Category),
        [ChartType.Stacked100Column] = GetDefaultWithTrue(ChartFamily.Category),
        [ChartType.Stacked100Step] = GetDefaultWithTrue(ChartFamily.Category),
        [ChartType.Area] = GetDefaultWithTrue(ChartFamily.Category),
        [ChartType.Line] = GetDefaultWithTrue(ChartFamily.Category),
        [ChartType.XYLine] = GetDefaultWithTrue(ChartFamily.XY),
        [ChartType.XYArea] = GetDefaultWithTrue(ChartFamily.XY),
        [ChartType.MultiDonut] = GetDefaultWithTrue(ChartFamily.Circular),
        [ChartType.SemiDonut] = GetDefaultWithTrue(ChartFamily.Circular),
        [ChartType.Histogram] = GetDefault(ChartFamily.XY),
        [ChartType.XYColumn] = GetDefaultWithTrue(ChartFamily.XY),
        [ChartType.Tree] = GetDefault(ChartFamily.Hierarchy),
        [ChartType.Treemap] = GetDefault(ChartFamily.Hierarchy),
        [ChartType.Dendrogram] = GetDefault(ChartFamily.Hierarchy),
        [ChartType.Partition] = GetDefault(ChartFamily.Hierarchy),
        [ChartType.Sunburst] = GetDefault(ChartFamily.Hierarchy),
        [ChartType.Icicle] = GetDefault(ChartFamily.Hierarchy),
        [ChartType.RadialTree] = GetDefault(ChartFamily.Hierarchy),
    };

    private static ChartTypeRule GetDefaultWithTrue(ChartFamily category)
    {
        return new ChartTypeRule
        {
            Category = category,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        };
    }

    private static ChartTypeRule GetDefault(ChartFamily value)
    {
        return new ChartTypeRule
        {
            Category = value
        };
    }

    private static ChartTypeRule GetDefaultPolar()
    {
        return new ChartTypeRule
        {
            Category = ChartFamily.Polar,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        };
    }

    internal static void ValidateCategory(ChartTypeRule left, ChartTypeRule right)
    {
        if (left.Category != right.Category)
        {
            throw new InvalidOperationException($"Chart types must be of the same category. Left: {left.Category}, Right: {right.Category}");
        }
    }

    internal static void ValidateMultipleSeries(ChartTypeRule value, Func<bool> condition)
    {
        if (!value.AllowMultipleSeries && condition())
        {
            throw new InvalidOperationException($"Chart type {value.Category} does not allow multiple series.");
        }
    }

    internal static void ValidateSameCategoryCount(ChartTypeRule left, Func<bool> condition)
    {
        if (left.RequireSameCategoryCount && !condition())
        {
            throw new InvalidOperationException($"Chart types must have the same category count. Left: {left.Category}");
        }
    }

    internal static void ValidateSameCategoryLabels(ChartTypeRule value, Func<bool> condition)
    {
        if (value.RequireSameCategoryLabels && !condition())
        {
            throw new InvalidOperationException($"Chart type {value.Category} requires the same category labels.");
        }
    }
}
