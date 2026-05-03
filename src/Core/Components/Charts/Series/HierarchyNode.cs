namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a node in a hierarchical chart (treemap, sunburst).
/// </summary>
internal sealed class HierarchyNode
{
    public string Name { get; set; } = "";
    public double Value { get; internal set; }

    public List<HierarchyNode> Children { get; } = [];

    public HierarchyNode? Parent { get; internal set; }

    public int Depth { get; internal set; }

    public HierarchyItem? SourceItem { get; internal set; }

    public int ParentIndex { get; internal set; }
    public Range ChildrenRange { get; internal set; }
}
