namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for the XY Column.
/// </summary>
public sealed record XYColumnPayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the X coordinate of the column's position in the chart.
    /// </summary>
    public required double X { get; init; }

    /// <summary>
    /// Gets the Y coordinate of the column's position in the chart.
    /// </summary>
    public required double Y { get; init; }

    /// <summary>
    /// Gets the width of the column.
    /// </summary>
    public required double Width { get; init; }

    /// <summary>
    /// Gets the height of the column.
    /// </summary>
    public required double Height { get; init; }

    /// <summary>
    /// Gets the value of the column.
    /// </summary>
    public required double Value { get; init; }
}
