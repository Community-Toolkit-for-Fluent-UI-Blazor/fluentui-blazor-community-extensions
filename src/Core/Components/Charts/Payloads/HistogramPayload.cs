namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a histogram chart item.
/// </summary>
public sealed record HistogramPayload
{
    /// <summary>
    /// Gets the bars of the histogram.
    /// </summary>
    public required IReadOnlyList<HistogramBarPayload> Bars { get; init; }

    /// <summary>
    /// Gets the unique identifier for the histogram payload.
    /// </summary>
    public required string Id { get; init; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the serie index of the histogram.
    /// </summary>
    public required int SerieIndex { get; init; }
}
