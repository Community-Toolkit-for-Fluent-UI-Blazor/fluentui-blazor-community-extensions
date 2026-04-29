namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for an XY bubble chart, containing a collection of XY points to be rendered.
/// </summary>
public sealed record XYBubblePayload : ILayerPayload
{
    /// <summary>
    /// Gets the collection of bubble point.
    /// </summary>
    public required IReadOnlyList<XYPointPayload> Points { get; init; }

    /// <summary>
    /// Gets the unique identifier for the bubble payload.
    /// </summary>
    public required string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the index of the series to which this bubble payload belongs, used for styling and identification purposes.
    /// </summary>
    public required int SerieIndex { get; init; }
}
