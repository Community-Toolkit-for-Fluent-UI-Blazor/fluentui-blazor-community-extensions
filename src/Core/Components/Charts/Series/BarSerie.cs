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
    public override ChartType ChartType => ChartType.Bar;

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);
}
