namespace FluentUI.Blazor.Community.Components.Charts.Drawing;

/// <summary>
/// Represents a point in a polar coordinate system with both Cartesian (X, Y) and polar (Angle, Radius)
/// representations, along with associated category and value information.
/// </summary>
public sealed class PolarPoint
{
    /// <summary>
    /// Gets the unique identifier.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the x-coordinate of the point in Cartesian coordinates.
    /// </summary>
    public required double X { get; init; }

    /// <summary>
    /// Gets the y-coordinate of the point in Cartesian coordinates.
    /// </summary>
    public required double Y { get; init; }

    /// <summary>
    /// Gets the angle of the point.
    /// </summary>
    public required double Angle { get; init; }

    /// <summary>
    /// Gets the radius of the point.
    /// </summary>
    public required double Radius { get; init; }

    /// <summary>
    /// Gets the index of the category associated with the point.
    /// </summary>
    public required int CategoryIndex { get; init; }

    /// <summary>
    /// Gets the value of the point.
    /// </summary>
    public required double Value { get; init; }
}
