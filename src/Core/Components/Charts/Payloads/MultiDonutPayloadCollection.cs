using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents a collection of concentric donut chart rings and their associated data, centered at a specific point.
/// </summary>
/// <remarks>Use this type to encapsulate the data required to render multiple donut chart rings sharing a common
/// center. Each ring is represented by a DonutPayloadCollection, allowing for layered visualizations such as
/// multi-level donut or radial charts.</remarks>
public sealed record MultiDonutPayloadCollection : ILayerPayload
{
    /// <summary>
    /// Gets the center point of the chart element.
    /// </summary>
    public required ChartPoint Center { get; init; }

    /// <summary>
    /// Gets the collection of data series to be rendered as concentric rings in the donut chart.
    /// </summary>
    /// <remarks>Each element in the collection represents a separate ring, allowing for the visualization of
    /// multiple datasets within a single chart. The order of the collection determines the rendering order from the
    /// innermost to the outermost ring.</remarks>
    public required IReadOnlyList<DonutPayloadCollection> Rings { get; init; }
}

