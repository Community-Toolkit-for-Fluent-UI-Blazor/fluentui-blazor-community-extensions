namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for an XY area chart, containing a collection of XY points to be rendered.
/// </summary>
public sealed record XYAreaPayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the collection of area points.
    /// </summary>
    public required IReadOnlyList<XYPointPayload> Points { get; init; }

    /// <summary>
    /// Gets the baseline value for the area chart.
    /// </summary>
    public required double Baseline { get; init; }
}
