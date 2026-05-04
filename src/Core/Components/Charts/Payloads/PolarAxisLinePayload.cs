using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a polar axis line in a radar chart.
/// </summary>
public sealed record PolarAxisLinePayload
{
    /// <summary>
    /// Gets the starting point of the axis.
    /// </summary>
    public required ChartPoint Start { get; init; }

    /// <summary>
    /// Gets the ending point of the axis.
    /// </summary>
    public required ChartPoint End { get; init; }
}
