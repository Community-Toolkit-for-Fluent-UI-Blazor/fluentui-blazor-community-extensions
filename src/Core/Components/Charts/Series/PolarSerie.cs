using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a chart series that displays data in a polar coordinate system.
/// </summary>
public sealed class PolarSerie
    : ChartSerie<PolarItem, PolarSerieOptions>
{
    /// <inheritdoc />
    public override ChartType ChartType => PolarType switch
    {
        PolarChartType.PolarBar => ChartType.PolarBar,
        PolarChartType.PolarArea => ChartType.PolarArea,
        PolarChartType.PolarLine => ChartType.PolarLine,
        _ => ChartType.Radar
    };

    /// <summary>
    /// Gets or sets the type of polar chart to render.
    /// </summary>
    public PolarChartType PolarType { get; internal set; }

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);
}
