namespace FluentUI.Blazor.Community.Components.Charts.Drawing;

/// <summary>
/// Represents a segment of a polar area chart, defined by its start and end angles, radius, and center coordinates.
/// </summary>
public sealed record PolarAreaSegment
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

    /// <summary>
    /// Gets or sets the index of the segment in the series.
    /// </summary>
    public required int Index { get; init; }
}

