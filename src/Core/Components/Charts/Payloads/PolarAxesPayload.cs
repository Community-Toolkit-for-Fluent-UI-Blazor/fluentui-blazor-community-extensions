using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for polar axes in a radar chart.
/// </summary>
public sealed record PolarAxesPayload : ILayerPayload
{
    /// <summary>
    /// Gets the collection of radial axes that extend from the center of the polar chart to the outer edge.
    /// </summary>
    public required IReadOnlyList<PolarAxisLinePayload> RadialAxes { get; init; }

    /// <summary>
    /// Gets the collection of concentric circles that define the polar grid structure.
    /// </summary>
    public required IReadOnlyList<PolarGridCirclePayload> ConcentricGrid { get; init; }

    /// <summary>
    /// Gets the collection of labels displayed along the angular axis of the polar chart.
    /// </summary>
    public required IReadOnlyList<PolarLabelPayload> AngleLabels { get; init; }

    /// <summary>
    /// Gets the collection of labels for the radial axis in a polar chart.
    /// </summary>
    public required IReadOnlyList<PolarLabelPayload> RadiusLabels { get; init; }

    /// <summary>
    /// Gets or sets the type of grid used in the radar chart.
    /// </summary>
    public required RadarGridType Grid { get; init; }
}
