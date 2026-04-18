namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the collection of column payloads for a column chart layer.
/// </summary>
/// <param name="Columns">The list of column payloads to be rendered.</param>
public sealed record ColumnPayloadCollection(IReadOnlyList<ColumnPayload> Columns) : ILayerPayload
{
}
