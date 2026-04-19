using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a chart series that displays data as a line connecting category items.
/// </summary>
/// <remarks>Use this class to visualize category-based data with a line chart, where each point corresponds to a
/// category and is connected by straight lines. This series type is suitable for showing trends across discrete
/// categories.</remarks>
public sealed class CategoryLineSerie
    : ChartSerie<CategoryItem, CategoryLineOptions>
{
    /// <inheritdoc />
    public override ChartType ChartType => IsArea ? ChartType.CategoryArea : ChartType.CategoryLine;

    /// <summary>
    /// Gets or sets if the category line is an area.
    /// </summary>
    public bool IsArea { get; internal set; }

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);
}
