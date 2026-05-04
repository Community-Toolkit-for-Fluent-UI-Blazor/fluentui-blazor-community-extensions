namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for an XY column chart, containing a collection of XY columns to be rendered.
/// </summary>
public sealed record XYColumnPayloadCollection
{
    /// <summary>
    /// Gets the collection of XY columns.
    /// </summary>
    public required IReadOnlyList<XYColumnPayload> Columns { get; init; }

    /// <summary>
    /// Gets the identifier of the payload.
    /// </summary>
    public required string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the index of the series to which this column payload belongs, used for styling and identification purposes.
    /// </summary>
    public required int SerieIndex { get; init; }
}
