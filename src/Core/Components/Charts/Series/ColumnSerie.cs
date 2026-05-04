using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a chart series that displays data as vertical columns.
/// </summary>
public sealed class ColumnSerie
    : ChartSerie<CategoryItem>
{
    /// <inheritdoc />
    public override ChartType ChartType => IsFull ? ChartType.Stacked100Column : (IsStacked ? ChartType.StackedColumn : ChartType.Column);

    /// <summary>
    /// Gets or sets a value indicating whether the column series is stacked.
    /// </summary>
    internal bool IsStacked { get; set; }

    /// <summary>
    /// Gets a value indicating whether this serie is represented as a percentage of the total value of all series in the chart.
    /// When set to true, the values of this serie will be normalized to represent their percentage contribution to the total
    /// value of all series.
    /// </summary>
    internal bool IsFull { get; set; }

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);
}
