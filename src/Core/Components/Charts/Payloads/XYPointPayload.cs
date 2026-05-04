namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for an XY point in a chart.
/// </summary>
public sealed record XYPointPayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the x-coordinate of the point.
    /// </summary>
    public required double X { get; init; }

    /// <summary>
    /// Gets the y-coordinate of the point.
    /// </summary>
    public required double Y { get; init; }

    /// <summary>
    /// Gets the value of the point.
    /// </summary>
    public required double Value { get; init; }

    /// <summary>
    /// Gets the radius of the point.
    /// </summary>
    public required double Radius { get; init; }

    /// <summary>
    /// Gets the original x-coordinate of the point before any transformations or mappings.
    /// </summary>
    public required double RawX { get; init; }

    /// <summary>
    /// Gets the original y-coordinate of the point before any transformations or mappings.
    /// </summary>
    public required double RawY { get; init; }
}
