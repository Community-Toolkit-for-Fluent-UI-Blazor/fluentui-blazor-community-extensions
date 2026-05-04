namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for the polar bubble chart.
/// </summary>
public sealed record PolarBubblePayload : ILayerPayload
{
    /// <summary>
    /// Gets the collection of points that make up the polar bubble chart.
    /// </summary>
    public required IReadOnlyList<PolarBubblePointPayload> Points { get; init; } = [];
}
