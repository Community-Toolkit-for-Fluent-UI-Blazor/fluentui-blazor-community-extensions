using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a polar grid circle in a radar chart.
/// </summary>
public sealed record PolarGridCirclePayload
{
    /// <summary>
    /// Gets the center point of the grid circle.
    /// </summary>
    public required ChartPoint Center { get; init; }

    /// <summary>
    /// Gets the radius of the grid circle.
    /// </summary>
    public required double Radius { get; init; }
}
