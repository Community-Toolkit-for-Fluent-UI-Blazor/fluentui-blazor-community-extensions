namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the collection of polar line payloads for a polar line chart layer.
/// </summary>
/// <param name="Lines">The list of polar line payloads to be rendered.</param>
public sealed record PolarLinePayloadCollection(IReadOnlyList<PolarLinePayload> Lines) : ILayerPayload
{
}
