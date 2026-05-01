namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents a collection of histogram payloads for rendering histogram series in a chart layer.
/// </summary>
/// <param name="Payloads">The collection of histogram payloads.</param>
public sealed record HistogramPayloadCollection(IReadOnlyList<HistogramPayload> Payloads) : ILayerPayload
{
}
