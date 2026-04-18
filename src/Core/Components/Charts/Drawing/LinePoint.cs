namespace FluentUI.Blazor.Community.Components.Charts.Drawing;

/// <summary>
/// Represents a data point with X and Y coordinates, a unique identifier, a category index, and an associated value.
/// </summary>
internal sealed class LinePoint
{
    /// <summary>
    /// Gets the unique identifier for this instance.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the value of the X coordinate.
    /// </summary>
    public required double X { get; init; }

    /// <summary>
    /// Gets the Y-coordinate value.
    /// </summary>
    public required double Y { get; init; }

    /// <summary>
    /// Gets the zero-based index of the category associated with this instance.
    /// </summary>
    public required int CategoryIndex { get; init; }

    /// <summary>
    /// Gets the value represented by this instance.
    /// </summary>
    public required double Value { get; init; }
}
