using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a chart series that displays data as vertical columns.
/// </summary>
public sealed class ColumnSerie
    : ChartSerie<CategoryItem, ColumnSerieOptions>
{
    /// <inheritdoc />
    public override ChartType ChartType => ChartType.Column;

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);
}
