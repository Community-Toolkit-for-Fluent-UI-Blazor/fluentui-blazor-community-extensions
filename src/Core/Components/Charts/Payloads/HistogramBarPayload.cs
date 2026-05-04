namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload data for a histogram bar in a chart.
/// </summary>
public sealed record HistogramBarPayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the X coordinate of the histogram bar.
    /// </summary>
    public required double X { get; init; }

    /// <summary>
    /// Gets the Y coordinate of the histogram bar.
    /// </summary>
    public required double Y { get; init; }

    /// <summary>
    /// Gets the width of the histogram bar.
    /// </summary>
    public required double Width { get; init; }

    /// <summary>
    /// Gets the height of the histogram bar.
    /// </summary>
    public required double Height { get; init; }

    /// <summary>
    /// Gets the number of bins.
    /// </summary>
    public required int Count { get; init; }
}
