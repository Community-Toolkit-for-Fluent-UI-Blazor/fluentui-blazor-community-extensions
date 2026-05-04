namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload data for a hierarchy node in a chart.
/// </summary>
public sealed record HierarchyNodePayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the X-coordinate of the hierarchy node.
    /// </summary>
    public required double X { get; init; }

    /// <summary>
    /// Gets the Y-coordinate of the hierarchy node.
    /// </summary>
    public required double Y { get; init; }

    /// <summary>
    /// Gets the width of the hierarchy node.
    /// </summary>
    public required double Width { get; init; }

    /// <summary>
    /// Gets the height of the hierarchy node.
    /// </summary>
    public required double Height { get; init; }

    /// <summary>
    /// Gets the numeric value of the hierarchy node.
    /// </summary>
    public double Value { get; init; }

    /// <summary>
    /// Gets the label text to display inside the hierarchy node.
    /// </summary>
    public string? Label { get; init; }

    /// <summary>
    /// Gets a value indicating whether the node is a leaf node (i.e., has no children).
    /// </summary>
    public required bool IsLeaf { get; init; }

    /// <summary>
    /// Gets the depth of the hierarchy node in the tree structure, where the root node has a depth of 0.
    /// </summary>
    public required int Depth { get; init; }

    /// <summary>
    /// Gets the start angle of the hierarchy node in a radial layout.
    /// </summary>
    public required double StartAngle { get; init; }

    /// <summary>
    /// Gets tne end angle of the hierarchy node in a radial layout.
    /// </summary>
    public required double EndAngle { get; init; }

    /// <summary>
    /// Gets the inner radius of the hierarchy node in a radial layout.
    /// </summary>
    public required double InnerRadius { get; init; }

    /// <summary>
    /// Gets the outer radius of the hierarchy node in a radial layout.
    /// </summary>
    public required double OuterRadius { get; init; }

    /// <summary>
    /// Gets the index of the parent.
    /// </summary>
    public required int ParentIndex { get; init; }

    /// <summary>
    /// Gets the range of the children of the hierarchy node.
    /// </summary>
    public Range ChildrenRange { get; init; }
}
