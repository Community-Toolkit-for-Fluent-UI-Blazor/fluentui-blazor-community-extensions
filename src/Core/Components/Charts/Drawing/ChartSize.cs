namespace FluentUI.Blazor.Community.Components.Charts;

/// <summary>
/// Represents the dimensions of a chart using width and height values.
/// </summary>
/// <param name="width">The width of the chart, in device-independent units. Must be a non-negative value.</param>
/// <param name="height">The height of the chart, in device-independent units. Must be a non-negative value.</param>
public struct ChartSize(double width, double height)
{
    /// <summary>
    /// Gets the width of the chart.
    /// </summary>
    public double Width { get; set; } = width;

    /// <summary>
    /// Gets the height of the chart.
    /// </summary>
    public double Height { get; set; } = height;
}
