namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for an XY scatter chart, containing a collection of XY points to be rendered.
/// </summary>
public sealed record XYScatterPayload : ILayerPayload
{
    /// <summary>
    /// Gets the collection of scatter points.
    /// </summary>
    public required IReadOnlyList<XYPointPayload> Points { get; init; }

    /// <summary>
    /// Gets the identifier of the payload.
    /// </summary>
    public required string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the index of the series to which this scatter payload belongs, used for styling and identification purposes.
    /// </summary>
    public required int SerieIndex { get; init; }
}
