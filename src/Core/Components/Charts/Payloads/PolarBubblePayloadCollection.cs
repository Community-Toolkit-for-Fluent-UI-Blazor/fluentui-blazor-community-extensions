namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the collection of polar bubble payloads for a polar bubble chart layer.
/// </summary>
/// <param name="Bubbles">The list of polar bubble payloads to be rendered.</param>
public sealed record PolarBubblePayloadCollection(IReadOnlyList<PolarBubblePayload> Bubbles) : ILayerPayload
{
    /// <summary>
    /// Gets the unique identifier for this payload collection, generated as a new GUID string.
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();
}
