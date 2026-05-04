namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a polar area chart segment.
/// </summary>
public sealed record PolarAreaSegmentPayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets or sets the starting angle of the segment in degrees.
    /// </summary>
    public required double StartAngle { get; init; }

    /// <summary>
    /// Gets or sets the ending angle of the segment in degrees.
    /// </summary>
    public required double EndAngle { get; init; }

    /// <summary>
    /// Gets or sets the radius of the segment.
    /// </summary>
    public required double Radius { get; init; }

    /// <summary>
    /// Gets or sets the x-coordinate of the center.
    /// </summary>
    public required double CenterX { get; init; }

    /// <summary>
    /// Gets or sets the y-coordinate of the center.
    /// </summary>
    public required double CenterY { get; init; }
}
