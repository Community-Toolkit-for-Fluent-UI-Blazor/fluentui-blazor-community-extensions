namespace FluentUI.Blazor.Community.Components.Charts.Drawing;

/// <summary>
/// Represents a point in a two-dimensional chart with X and Y coordinates.
/// </summary>
/// <remarks>Use this struct to specify or compare positions within a chart or graph. ChartPoint is mutable and
/// supports value equality comparison.</remarks>
public struct ChartPoint : IEquatable<ChartPoint>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChartPoint"/> struct with the specified X and Y coordinates.
    /// </summary>
    /// <param name="x">The X coordinate of the chart point.</param>
    /// <param name="y">The Y coordinate of the chart point.</param>
    public ChartPoint(double x, double y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Gets the chart point at the origin (0, 0).
    /// </summary>
    public static ChartPoint Zero { get; } = new ChartPoint(0, 0);

    /// <summary>
    /// Gets the X-coordinate value.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Gets the Y-coordinate value.
    /// </summary>
    public double Y { get; set; }

    /// <inheritdoc />
    public bool Equals(ChartPoint other) => X == other.X && Y == other.Y;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ChartPoint other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y);

    /// <summary>
    /// Determines whether two ChartPoint instances are equal.
    /// </summary>
    /// <param name="left">The first ChartPoint to compare.</param>
    /// <param name="right">The second ChartPoint to compare.</param>
    /// <returns>true if the specified ChartPoint instances are equal; otherwise, false.</returns>
    public static bool operator ==(ChartPoint left, ChartPoint right) => left.Equals(right);

    /// <summary>
    /// Determines whether two ChartPoint instances are not equal.
    /// </summary>
    /// <param name="left">The first ChartPoint to compare.</param>
    /// <param name="right">The second ChartPoint to compare.</param>
    /// <returns>true if the specified ChartPoint instances are not equal; otherwise, false.</returns>
    public static bool operator !=(ChartPoint left, ChartPoint right) => !(left == right);
}
