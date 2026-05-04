namespace FluentUI.Blazor.Community.Components.Charts.Drawing;

/// <summary>
/// Represents a single bar in a histogram with its geometric boundaries and frequency count.
/// </summary>
internal sealed record HistogramBar
{
    /// <summary>
    /// Gets the X coordinate of the first point.
    /// </summary>
    public double X1 { get; init; }

    /// <summary>
    /// Gets the X coordinate of the last point.
    /// </summary>
    public double X2 { get; init; }

    /// <summary>
    /// Gets the Y coordinate of the first point.
    /// </summary>
    public double Y1 { get; init; }

    /// <summary>
    /// Gets the Y coordinate of the last point.
    /// </summary>
    public double Y2 { get; init; }

    /// <summary>
    /// Gets the count
    /// </summary>
    public int Count { get; init; }
}
