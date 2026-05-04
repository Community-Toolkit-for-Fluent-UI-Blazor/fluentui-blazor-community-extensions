namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a polar bar chart.
/// </summary>
public sealed record PolarBarPayloadCollection(IReadOnlyList<PolarBarPayload> Bars) : ILayerPayload
{
}
