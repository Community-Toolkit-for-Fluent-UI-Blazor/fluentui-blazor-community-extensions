namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the collection of polar scatter payloads for a polar scatter chart layer.
/// </summary>
/// <param name="Scatters">The list of polar scatter payloads to be rendered.</param>
public sealed record PolarScatterPayloadCollection(IReadOnlyList<PolarScatterPayload> Scatters) : ILayerPayload
{
    /// <summary>
    /// Gets the unique identifier for this payload collection, generated as a new GUID string.
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();
}
