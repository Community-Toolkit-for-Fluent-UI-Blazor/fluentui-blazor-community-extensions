namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a rose segment in a rose chart.
/// </summary>
public sealed record RoseSegmentPayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the x-coordinate of the center.
    /// </summary>
    public required double CenterX { get; init; }

    /// <summary>
    /// Gets the y-coordinate of the center.
    /// </summary>
    public required double CenterY { get; init; }

    /// <summary>
    /// Gets the start angle of the segment.
    /// </summary>
    public required double StartAngle { get; init; }

    /// <summary>
    /// Gets the end angle of the segment.
    /// </summary>
    public required double EndAngle { get; init; }

    /// <summary>
    /// Gets the inner radius of the segment.
    /// </summary>
    public required int InnerRadius { get; init; }

    /// <summary>
    /// Gets the outer radius of the segment.
    /// </summary>
    public required double OuterRadius { get; init; }

    /// <summary>
    /// Gets the value of the segment.
    /// </summary>
    public required double Value { get; init; }

    /// <summary>
    /// Gets the category of the segment.
    /// </summary>
    public required string Category { get; init; }
}
