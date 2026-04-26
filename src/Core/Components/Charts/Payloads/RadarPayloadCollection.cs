namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the collection of column payloads for a column chart layer.
/// </summary>
/// <param name="Radars">The list of radar payloads to be rendered.</param>
public sealed record RadarPayloadCollection(IReadOnlyList<RadarPayload> Radars) : ILayerPayload
{
}
