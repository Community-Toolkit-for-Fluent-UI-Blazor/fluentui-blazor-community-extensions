namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a node in a hierarchical chart (treemap, sunburst).
/// </summary>
public sealed class HierarchyNode : ChartItem
{
    /// <summary>
    /// Gets the numeric value of this node.
    /// </summary>
    public required double Value { get; init; }

    /// <summary>
    /// Gets the children nodes (empty for leaf nodes).
    /// </summary>
    public IReadOnlyList<HierarchyNode> Children { get; init; } = [];
}
