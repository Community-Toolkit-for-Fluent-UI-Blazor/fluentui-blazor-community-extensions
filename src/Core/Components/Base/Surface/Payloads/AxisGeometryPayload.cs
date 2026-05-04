using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Surface.Payloads;

/// <summary>
/// Represents the payload for axis geometry.
/// </summary>
public class AxisGeometryPayload
{
    /// <summary>
    /// Gets the starting point of the axis.
    /// </summary>
    public required ChartPoint StartPoint { get; init; }

    /// <summary>
    /// Gets the ending point of the axis.
    /// </summary>
    public required ChartPoint EndPoint { get; init; }

    /// <summary>
    /// Gets the length of each tick mark on the axis.
    /// </summary>
    public double TickLength { get; init; } = 4.0;

    /// <summary>
    /// Gets the collection of tick mark geometries for the axis.
    /// </summary>
    /// <remarks>The collection defines the positions and appearance of each tick mark rendered on the axis.
    /// The order of the items in the list corresponds to their visual order on the axis.</remarks>
    public required IReadOnlyList<ChartPoint> Ticks { get; init; } = [];

    /// <summary>
    /// Gets the collection of axis label geometry payloads to be rendered.
    /// </summary>
    public required IReadOnlyList<AxisLabelPayload> Labels { get; init; } = [];
}
