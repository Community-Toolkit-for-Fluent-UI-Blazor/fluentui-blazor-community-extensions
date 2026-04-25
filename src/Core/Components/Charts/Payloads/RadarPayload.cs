using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a radar chart.
/// </summary>
public sealed record RadarPayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the path of the radar.
    /// </summary>
    public required PolarPathPayload Path { get; init; }

    /// <summary>
    /// Gets the points of the radar.
    /// </summary>
    public required IReadOnlyList<LinePointPayload> Points { get; init; }

    /// <summary>
    /// Gets a value indicating if the area is filled or not.
    /// </summary>
    public bool FillArea { get; init; }

    /// <summary>
    /// Gets or sets the type of grid to be used in the radar chart.
    /// </summary>
    public RadarGridType Grid { get; init; }
}

