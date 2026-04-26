using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Components.Charts;

internal static class ChartTypeRuleValidator
{
    internal static readonly Dictionary<ChartType, ChartTypeRule> Rules =
    new(EqualityComparer<ChartType>.Default)
    {
        [ChartType.Radar] = new ChartTypeRule
        {
            Category = ChartCategory.Polar,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        },

        [ChartType.PolarLine] = new ChartTypeRule
        {
            Category = ChartCategory.Polar,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        },

        [ChartType.PolarBar] = new ChartTypeRule
        {
            Category = ChartCategory.Polar,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        },

        [ChartType.PolarArea] = new ChartTypeRule
        {
            Category = ChartCategory.Polar,
            AllowMultipleSeries = false,
            RequireSameCategoryCount = false,
            RequireSameCategoryLabels = false
        },

        [ChartType.PolarRose] = new ChartTypeRule
        {
            Category = ChartCategory.Polar,
            AllowMultipleSeries = false,
            RequireSameCategoryCount = false,
            RequireSameCategoryLabels = false
        },

        [ChartType.Pie] = new ChartTypeRule
        {
            Category = ChartCategory.Category,
            AllowMultipleSeries = false
        },

        [ChartType.Donut] = new ChartTypeRule
        {
            Category = ChartCategory.Category,
            AllowMultipleSeries = false,
        },

        [ChartType.Bar] = new ChartTypeRule
        {
            Category = ChartCategory.Category,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        },

        [ChartType.Column] = new ChartTypeRule
        {
            Category = ChartCategory.Category,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        },

        [ChartType.Stacked100Bar] = new ChartTypeRule
        {
            Category = ChartCategory.Category,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        },

        [ChartType.Stacked100Area] = new ChartTypeRule
        {
            Category = ChartCategory.Category,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        },

        [ChartType.Stacked100Column] = new ChartTypeRule
        {
            Category = ChartCategory.Category,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        },

        [ChartType.Stacked100Step] = new ChartTypeRule
        {
            Category = ChartCategory.Category,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        },

        [ChartType.Area] = new ChartTypeRule
        {
            Category = ChartCategory.Category,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        },

        [ChartType.Line] = new ChartTypeRule
        {
            Category = ChartCategory.Category,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        },

        [ChartType.MultiDonut] = new ChartTypeRule
        {
            Category = ChartCategory.Category,
            AllowMultipleSeries = true,
            RequireSameCategoryCount = true,
            RequireSameCategoryLabels = true
        }
    };

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
