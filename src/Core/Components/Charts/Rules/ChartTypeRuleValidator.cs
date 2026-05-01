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
        [ChartType.PolarArea] = GetDefault(ChartCategory.Polar),
        [ChartType.PolarRose] = GetDefault(ChartCategory.Polar),
        [ChartType.Pie] = GetDefault(ChartCategory.Circular),
        [ChartType.Donut] = GetDefault(ChartCategory.Circular),
        [ChartType.Bar] = GetDefaultWithTrue(ChartCategory.Category),
        [ChartType.Step] = GetDefaultWithTrue(ChartCategory.Category),
        [ChartType.StackedColumn] = GetDefaultWithTrue(ChartCategory.Category),
        [ChartType.StackedBar] = GetDefaultWithTrue(ChartCategory.Category),
        [ChartType.StackedStep] = GetDefaultWithTrue(ChartCategory.Category),
        [ChartType.Scatter] = GetDefaultWithTrue(ChartCategory.XY),
        [ChartType.Bubble] = GetDefaultWithTrue(ChartCategory.XY),
        [ChartType.Column] = GetDefaultWithTrue(ChartCategory.Category),
        [ChartType.StackedArea] = GetDefaultWithTrue(ChartCategory.Category),
        [ChartType.Stacked100Bar] = GetDefaultWithTrue(ChartCategory.Category),
        [ChartType.Stacked100Area] = GetDefaultWithTrue(ChartCategory.Category),
        [ChartType.Stacked100Column] = GetDefaultWithTrue(ChartCategory.Category),
        [ChartType.Stacked100Step] = GetDefaultWithTrue(ChartCategory.Category),
        [ChartType.Area] = GetDefaultWithTrue(ChartCategory.Category),
        [ChartType.Line] = GetDefaultWithTrue(ChartCategory.Category),
        [ChartType.XYLine] = GetDefaultWithTrue(ChartCategory.XY),
        [ChartType.XYArea] = GetDefaultWithTrue(ChartCategory.XY),
        [ChartType.MultiDonut] = GetDefaultWithTrue(ChartCategory.Circular),
        [ChartType.SemiDonut] = GetDefaultWithTrue(ChartCategory.Circular),
        [ChartType.Histogram] = GetDefault(ChartCategory.XY),
        [ChartType.XYColumn] = GetDefaultWithTrue(ChartCategory.XY)
    };

    private static ChartTypeRule GetDefaultWithTrue(ChartCategory category)
    {
        return new ChartTypeRule
        {
            Category = category,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        };
    }

    private static ChartTypeRule GetDefault(ChartCategory value)
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
            Category = ChartCategory.Polar,
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
