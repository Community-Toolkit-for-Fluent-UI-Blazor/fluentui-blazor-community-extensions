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
    public override ChartType ChartType => IsStacked ? ChartType.StackedBar : ChartType.Bar;

    /// <summary>
    /// Gets or sets a value indicating whether the bar series is stacked.
    /// </summary>
    public bool IsStacked { get; internal set; }

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);
}
