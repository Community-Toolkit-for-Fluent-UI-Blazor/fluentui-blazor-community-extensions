namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the data required to describe a single column in a layered visualization, including its position, size,
/// category, value, and layer information.
/// </summary>
/// <remarks>This type is typically used to transfer column-related data between components or layers in a
/// visualization system. It implements value-based equality to support comparison and hashing scenarios, such as use in
/// collections.</remarks>
public sealed record ColumnPayload : ChartItemPayloadBase, ILayerPayload, IEquatable<ColumnPayload>
{
    /// <summary>
    /// Gets the X coordinate of the column.
    /// </summary>
    public required double X { get; init; }

    /// <summary>
    /// Gets the Y coordinate of the column.
    /// </summary>
    public required double Y { get; init; }

    /// <summary>
    /// Gets the width of the column.
    /// </summary>
    public required double Width { get; init; }

    /// <summary>
    /// Gets the height of the column.
    /// </summary>
    public required double Height { get; init; }

    /// <summary>
    /// Gets the index of the category.
    /// </summary>
    public required int CategoryIndex { get; init; }

    /// <summary>
    /// Gets the value of the column.
    /// </summary>
    public required double Value { get; init; }

    /// <inheritdoc />
    public bool Equals(ColumnPayload? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return string.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase) &&
               X == other.X &&
               Y == other.Y &&
               Width == other.Width &&
               Height == other.Height &&
               CategoryIndex == other.CategoryIndex &&
               Value == other.Value;
    }

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Id, X, Y, Width, Height, CategoryIndex, Value);
}

