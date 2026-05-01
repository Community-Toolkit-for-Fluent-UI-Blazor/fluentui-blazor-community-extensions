using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a data series for rendering histogram charts using value items and histogram-specific options.
/// </summary>
public sealed class HistogramSerie
    : ChartSerie<ValueItem>
{
    /// <inheritdoc />
    public override ChartType ChartType => ChartType.Histogram;

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);
}
