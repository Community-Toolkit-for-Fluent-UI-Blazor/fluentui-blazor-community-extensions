using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Represents a layout rectangle assigned to a hierarchy node in a treemap chart.
/// </summary>
internal sealed class TreemapLayoutNode
{
    /// <summary>
    /// Gets the hierarchy node associated with this layout rectangle.
    /// </summary>
    public required HierarchyNode Node { get; init; }

    /// <summary>
    /// Gets the rectangle assigned to this node by the treemap layout algorithm.
    /// </summary>
    public required ChartRect Rect { get; init; }
}

