namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a single item in an XY chart, with X and Y coordinates.
/// </summary>
public sealed class XYItem : ChartItem
{
    /// <summary>
    /// Gets the X coordinate of this item.
    /// </summary>
    public required double X { get; init; }

    /// <summary>
    /// Gets the Y coordinate of this item.
    /// </summary>
    public required double Y { get; init; }

    /// <summary>
    /// Gets the value associated with this item.
    /// </summary>
    public required double Value { get; init; }
}
