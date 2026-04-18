namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a single XY point in a Cartesian chart.
/// </summary>
public class XYPoint : ChartItem
{
    /// <summary>
    /// Gets the X coordinate of the point.
    /// </summary>
    public required double X { get; init; }

    /// <summary>
    /// Gets the Y coordinate of the point.
    /// </summary>
    public required double Y { get; init; }
}
