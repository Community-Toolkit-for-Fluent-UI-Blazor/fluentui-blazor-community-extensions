namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the payload for a single point in a signature stroke.
/// </summary>
public sealed class StrokePointPayload : IEquatable<StrokePointPayload>
{
    /// <summary>
    /// Gets or sets the X coordinate of the point.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Gets or sets the Y-coordinate value.
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Gets or sets the pressure applied at this point, which can affect the stroke width and opacity.
    /// </summary>
    public double P { get; set; }

    /// <summary>
    /// Gets or sets the width value.
    /// </summary>
    public double W { get; set; }

    /// <inheritdoc />
    public bool Equals(StrokePointPayload? other)
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
            P == other.P &&
            W == other.W;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        Equals(obj as StrokePointPayload);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(
            X,
            Y,
            P,
            W
        );
    }
}

