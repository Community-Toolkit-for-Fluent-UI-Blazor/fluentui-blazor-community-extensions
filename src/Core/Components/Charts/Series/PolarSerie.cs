using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a chart series that displays data in a polar coordinate system.
/// </summary>
public sealed class PolarSerie
    : ChartSerie<PolarItem>
{
    /// <inheritdoc />
    public override ChartType ChartType => PolarType switch
    {
        PolarChartType.Bar => ChartType.PolarBar,
        PolarChartType.Area => ChartType.PolarArea,
        PolarChartType.Line => ChartType.PolarLine,
        PolarChartType.Scatter => ChartType.PolarScatter,
        PolarChartType.Bubble => ChartType.PolarBubble,
        PolarChartType.Rose => ChartType.PolarRose,
        _ => ChartType.Radar
    };

    /// <summary>
    /// Gets or sets the type of polar chart to render.
    /// </summary>
    public PolarChartType PolarType { get; internal set; }

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);
}
