namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a polar line chart.
/// </summary>
public sealed record PolarLinePayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the path of the polar line.
    /// </summary>
    public required PolarPathPayload Path { get; init; }

    /// <summary>
    /// Gets the points of the polar line.
    /// </summary>
    public required IReadOnlyList<LinePointPayload> Points { get; init; }
}

