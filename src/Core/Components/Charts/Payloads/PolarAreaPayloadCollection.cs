namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the collection of polar area payloads for a polar area chart layer.
/// </summary>
/// <param name="Areas">The list of polar area payloads to be rendered.</param>
public sealed record PolarAreaPayloadCollection(IReadOnlyList<PolarAreaPayload> Areas) : ILayerPayload
{
}
