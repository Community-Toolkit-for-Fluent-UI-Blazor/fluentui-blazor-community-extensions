namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a polar scatter point in a chart.
/// </summary>
public sealed record PolarScatterPointPayload : ChartItemPayloadBase
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
    /// Gets the radius of the point.
    /// </summary>
    public required double Radius { get; init; }

    /// <summary>
    /// Gets the value associated with the point.
    /// </summary>
    public required double Value { get; init; }
}

