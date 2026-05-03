namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a base class for items in a hierarchical chart (treemap, sunburst).
/// </summary>
public class HierarchyItem : ChartItem
{
    /// <summary>
    /// Gets the numeric value of the node.
    /// </summary>
    public double Value { get; init; }
}
