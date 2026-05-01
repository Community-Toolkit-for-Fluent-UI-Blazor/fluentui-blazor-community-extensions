using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a chart series that displays data in an XY coordinate system.
/// </summary>
public sealed class XYSerie : ChartSerie<XYItem>
{
    /// <inheritdoc />
    public override ChartType ChartType => XYType switch
    {
        XYChartType.Scatter => ChartType.Scatter,
        XYChartType.Bubble => ChartType.Bubble,
        XYChartType.Line => ChartType.XYLine,
        XYChartType.Area => ChartType.XYArea,
        XYChartType.Column => ChartType.XYColumn,
        _ => throw new NotSupportedException($"Unsupported XY chart type: {XYType}")
    };

    /// <summary>
    /// Gets or sets the type of XY chart to render.
    /// </summary>
    public XYChartType XYType { get; internal set; }

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);
}
