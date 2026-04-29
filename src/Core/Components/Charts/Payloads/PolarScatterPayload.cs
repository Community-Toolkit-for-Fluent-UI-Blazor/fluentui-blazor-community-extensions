namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for the polar scatter chart.
/// </summary>
public sealed record PolarScatterPayload : ILayerPayload
{
    /// <summary>
    /// Gets the collection of points that make up the polar scatter chart.
    /// </summary>
    public required IReadOnlyList<PolarScatterPointPayload> Points { get; init; } = [];
}
