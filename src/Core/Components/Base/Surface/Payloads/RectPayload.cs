namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a rectangular region defined by its top-left corner (X, Y) and its dimensions (Width, Height).
/// </summary>
public sealed class RectPayload : ILayerPayload, IEquatable<RectPayload>
{
    /// <summary>
    /// Gets or sets the X-coordinate of the top-left corner of the rectangle.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Gets or sets the Y-coordinate of the top-left corner of the rectangle.
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Gets or sets the width of the rectangle.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the rectangle.
    /// </summary>
    public double Height { get; set; }

    /// <inheritdoc />
    public bool Equals(RectPayload? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return
            X == other.X &&
            Y == other.Y &&
            Width == other.Width &&
            Height == other.Height;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        Equals(obj as RectPayload);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(
            X,
            Y,
            Width,
            Height
        );
    }
}
