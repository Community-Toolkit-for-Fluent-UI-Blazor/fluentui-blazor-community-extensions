namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload data for a single point in a bubble chart.
/// </summary>
public sealed record BubblePointPayload : LinePointPayload
{
    /// <summary>
    /// Gets the radius of the bubble point.
    /// </summary>
    public required double Radius { get; init; }

    /// <summary>
    /// Gets the value of the bubble point.
    /// </summary>
    public required double BubbleValue { get; init; }
}
