using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a polar grid polygon, containing the points that define the polygon's shape in a polar coordinate system.
/// </summary>
public sealed class PolarGridPolygonPayload
{
    /// <summary>
    /// Gets the collection of points that define the shape of the polar grid polygon.
    /// </summary>
    public required IReadOnlyList<ChartPoint> Points { get; init; }
}
