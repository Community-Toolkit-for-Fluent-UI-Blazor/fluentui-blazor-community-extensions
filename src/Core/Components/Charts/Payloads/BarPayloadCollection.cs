namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the collection of bar payloads for a bar chart layer. 
/// </summary>
/// <param name="Id">The unique identifier for the bar payload collection.</param>
/// <param name="Bars">The list of bar payloads to be rendered.</param>
public sealed record BarPayloadCollection(string Id, IReadOnlyList<BarPayload> Bars) : ILayerPayload
{
}
