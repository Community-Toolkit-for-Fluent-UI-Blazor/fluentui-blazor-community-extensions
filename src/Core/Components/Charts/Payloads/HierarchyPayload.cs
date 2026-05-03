using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a hierarchy chart.
/// </summary>
public sealed class HierarchyPayload : ILayerPayload
{
    /// <summary>
    /// Gets the collection of hierarchy nodes to be rendered in the hierarchy chart.
    /// </summary>
    public required IReadOnlyList<HierarchyNodePayload> Nodes { get; init; }

    /// <summary>
    /// Gets the unique identifier of the hierarchy chart.
    /// </summary>
    public required string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the type of the hierarchy chart (e.g., Tree, Sunburst, etc.).
    /// </summary>
    public required ChartType Type { get; init; }
}
