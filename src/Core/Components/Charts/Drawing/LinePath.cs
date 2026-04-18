namespace FluentUI.Blazor.Community.Components.Charts.Drawing;

/// <summary>
/// Represents the line path for a category line series in a chart.
/// </summary>
internal sealed class LinePath
{
    /// <summary>
    /// Gets the unique identifier for this instance.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the collection of data points to be displayed in the chart.
    /// </summary>
    /// <remarks>The order of points in the collection determines their sequence in the chart. The collection
    /// must not be null and should contain at least one point for the chart to render meaningful data.</remarks>
    public required IReadOnlyList<ChartPoint> Points { get; init; }

    /// <summary>
    /// Gets a value indicating whether smooth transitions or animations are enabled.
    /// </summary>
    public required bool Smooth { get; init; }
}
