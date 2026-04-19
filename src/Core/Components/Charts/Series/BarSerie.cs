using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a data series for rendering bar charts using category items and bar-specific options.
/// </summary>
public sealed class BarSerie
    : ChartSerie<CategoryItem, BarSerieOptions>
{
    /// <inheritdoc />
    public override ChartType ChartType => IsFull ? ChartType.Stacked100Bar : (IsStacked ? ChartType.StackedBar : ChartType.Bar);

    /// <summary>
    /// Gets a value indicating whether the bar series is stacked.
    /// </summary>
    public bool IsStacked { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether this serie is represented as a percentage of the total value of all series in the chart.
    /// When set to true, the values of this serie will be normalized to represent their percentage contribution to the total
    /// value of all series.
    /// </summary>
    public bool IsFull { get; internal set; }

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);
}
