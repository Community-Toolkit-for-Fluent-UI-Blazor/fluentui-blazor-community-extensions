namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for an XY line chart, containing a collection of XY points to be rendered.
/// </summary>
public sealed record XYLinePayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the collection of line points.
    /// </summary>
    public required IReadOnlyList<XYPointPayload> Points { get; init; }
}
