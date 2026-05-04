namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents a collection of XY area payloads for rendering area series in a chart layer.
/// </summary>
/// <param name="Payloads">The collection of XY area payloads.</param>
public sealed record XYAreaPayloadCollection(IReadOnlyList<XYAreaPayload> Payloads) : ILayerPayload
{
}
