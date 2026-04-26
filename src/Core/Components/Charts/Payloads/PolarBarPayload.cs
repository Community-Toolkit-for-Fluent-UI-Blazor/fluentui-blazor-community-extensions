namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a polar area chart.
/// </summary>
public sealed record PolarBarPayload : ILayerPayload
{
    /// <summary>
    /// Gets the unique identifier for the polar area chart payload.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the list of polar bar segments that define the chart's layout.
    /// </summary>
    public required IReadOnlyList<PolarBarSegmentPayload> Segments { get; init; }
}
