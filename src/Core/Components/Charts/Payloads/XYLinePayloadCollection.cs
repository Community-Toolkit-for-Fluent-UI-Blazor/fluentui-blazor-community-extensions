namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents a collection of XY line payloads for rendering line series in a chart layer.
/// </summary>
/// <param name="Payloads">The collection of XY line payloads.</param>
public sealed record XYLinePayloadCollection(IReadOnlyList<XYLinePayload> Payloads) : ILayerPayload
{
}
