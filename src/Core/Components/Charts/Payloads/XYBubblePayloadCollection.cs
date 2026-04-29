namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents a collection of XY scatter payloads for rendering scatter series in a chart layer.
/// </summary>
/// <param name="Payloads">The collection of XY bubble payloads.</param>
public sealed record XYBubblePayloadCollection(IReadOnlyList<XYBubblePayload> Payloads) : ILayerPayload
{
}
