namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload data for a single point in a line chart, including its coordinates, category index, and
/// value.
/// </summary>
/// <remarks>This type is typically used to convey information about a specific data point within a line chart
/// layer, such as when handling events or rendering custom visuals. Instances are considered equal if all properties
/// match.</remarks>
public record LinePointPayload : ChartItemPayloadBase, IEquatable<LinePointPayload>
{
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

    /// <inheritdoc />
    public virtual bool Equals(LinePointPayload? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id == other.Id &&
               X == other.X &&
               Y == other.Y &&
               CategoryIndex == other.CategoryIndex &&
               Value == other.Value;
    }

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Id, X, Y, CategoryIndex, Value);
}

