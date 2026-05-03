using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts;

internal sealed class HierarchyLayoutNode
{
    public required HierarchyNode Source { get; set; }

    public required int Depth { get; set; }
    public required ChartRect Rect { get; set; }

    public List<HierarchyLayoutNode> Children { get; } = [];

    public HierarchyLayoutNode? Parent { get; set; }

    public double Value { get; set; }

    public double StartAngle { get; set; }
    public double EndAngle { get; set; }
    public double InnerRadius { get; set; }
    public double OuterRadius { get; set; }
    public int LeafColorIndex { get; set; } = -1;
    public double SubtreeWidth { get; internal set; }
}

