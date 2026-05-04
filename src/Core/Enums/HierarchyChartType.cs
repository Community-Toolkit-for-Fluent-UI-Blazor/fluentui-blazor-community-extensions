namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Represents the different types of hierarchy charts available in the FluentUI Blazor Community Components library.
/// </summary>
internal enum HierarchyChartType
{
    /// <summary>
    /// Specifies a tree map chart type, which displays hierarchical data as a set of nested rectangles.
    /// </summary>
    Treemap,

    /// <summary>
    /// Specifies a sunburst chart type, which displays hierarchical data as a set of concentric circles.
    /// </summary>
    Sunburst,

    /// <summary>
    /// Specifies an icicle chart type, which displays hierarchical data as a set of nested rectangles in a vertical layout.
    /// </summary>
    Icicle,

    /// <summary>
    /// Specifies a partition chart type, which displays hierarchical data as a set of nested rectangles in a horizontal layout.
    /// </summary>
    Partition,

    /// <summary>
    /// Specifies a radial tree chart type, which displays hierarchical data as a tree structure with nodes arranged in a circular layout.
    /// </summary>
    RadialTree,

    /// <summary>
    /// Specifies a tree chart type, which displays hierarchical data as a tree structure with nodes arranged in a vertical or horizontal layout.
    /// </summary>
    Tree,

    /// <summary>
    /// Specifies a dendrogram chart type, which displays hierarchical data as a tree structure with nodes connected by lines, often used to represent clustering or phylogenetic relationships.
    /// </summary>
    Dendrogram
}
