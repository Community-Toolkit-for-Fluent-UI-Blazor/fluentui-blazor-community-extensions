namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a single point in a polar bubble chart.
/// </summary>
public sealed record PolarBubblePointPayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the x-coordinate of the bubble point.
    /// </summary>
    public required double X { get; init; }

    /// <summary>
    /// Gets the y-coordinate of the bubble point.
    /// </summary>
    public required double Y { get; init; }

    /// <summary>
    /// Gets the radius of the bubble point.
    /// </summary>
    public required double Radius { get; init; }

    /// <summary>
    /// Gets the value associated with the bubble point.
    /// </summary>
    public required double Value { get; init; }
}
