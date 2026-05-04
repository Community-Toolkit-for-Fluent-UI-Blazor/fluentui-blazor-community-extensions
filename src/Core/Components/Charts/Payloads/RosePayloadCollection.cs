namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a rose chart.
/// </summary>
public sealed record RosePayloadCollection(IReadOnlyList<RosePayload> Roses) : ILayerPayload
{
}
