namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a rose chart.
/// </summary>
public sealed record RosePayload : ILayerPayload
{
    /// <summary>
    /// Gets the unique identifier for the rose chart payload.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the list of rose segments that define the chart's layout.
    /// </summary>
    public required IReadOnlyList<RoseSegmentPayload> Segments { get; init; }
}

