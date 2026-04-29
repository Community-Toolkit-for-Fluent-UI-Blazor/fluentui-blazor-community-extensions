using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a chart series that displays data as a line connecting category items.
/// </summary>
/// <remarks>Use this class to visualize category-based data with a line chart, where each point corresponds to a
/// category and is connected by straight lines. This series type is suitable for showing trends across discrete
/// categories.</remarks>
public sealed class LineSerie
    : ChartSerie<CategoryItem>
{
    /// <inheritdoc />
    public override ChartType ChartType => LineType switch
    {
        LineChartType.Line => ChartType.Line,
        LineChartType.Area => ChartType.Area,
        LineChartType.StackedArea => ChartType.StackedArea,
        LineChartType.Stacked100Area => ChartType.Stacked100Area,
        LineChartType.Step => ChartType.Step,
        LineChartType.StackedStep => ChartType.StackedStep,
        LineChartType.Stacked100Step => ChartType.Stacked100Step,
        _ => ChartType.Line
    };

    /// <summary>
    /// Gets or sets the type of line chart to render.
    /// </summary>
    public LineChartType LineType { get; internal set; }

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);
}
