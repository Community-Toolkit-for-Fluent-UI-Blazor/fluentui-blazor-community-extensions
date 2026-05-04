namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload containing the path, data points, and baseline for rendering an area in a chart component.
/// </summary>
public sealed record AreaPayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the payload that defines the line path for the area.
    /// Equivalent to LinePayload.Path.
    /// </summary>
    public required LinePathPayload Path { get; init; }

    /// <summary>
    /// Gets the collection of data points used to draw intersection circles.
    /// Equivalent to LinePayload.Points.
    /// </summary>
    public required IReadOnlyList<LinePointPayload> Points { get; init; }

    /// <summary>
    /// Gets the baseline Y coordinate used to close the area shape.
    /// </summary>
    public required double BaselineY { get; init; }
}
