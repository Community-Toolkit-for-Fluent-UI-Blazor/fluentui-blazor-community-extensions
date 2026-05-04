using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a data series for rendering hierarchical charts (treemap, sunburst) using hierarchy nodes.
/// </summary>
public sealed class HierarchySerie
    : ChartSerie<HierarchyItem>
{
    /// <inheritdoc />
    public override ChartType ChartType => HierarchyChartType switch
    {
        HierarchyChartType.Treemap => ChartType.Treemap,
        HierarchyChartType.Tree => ChartType.Tree,
        HierarchyChartType.Sunburst => ChartType.Sunburst,
        HierarchyChartType.Icicle => ChartType.Icicle,
        HierarchyChartType.Partition => ChartType.Partition,
        HierarchyChartType.RadialTree => ChartType.RadialTree,
        HierarchyChartType.Dendrogram => ChartType.Dendrogram,
        _ => throw new ArgumentOutOfRangeException(nameof(HierarchyChartType), HierarchyChartType, "Invalid hierarchy chart type.")
    };

    /// <summary>
    /// Gets or sets the function to extract the hierarchy nodes from the data items for rendering in the hierarchical chart.
    /// </summary>
    public Func<HierarchyItem, string[]>? Group { get; set; }

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);

    /// <summary>
    /// Gets the type of the hierarchy chart to be rendered (e.g., treemap, sunburst).
    /// </summary>
    internal HierarchyChartType HierarchyChartType { get; set; }
}
