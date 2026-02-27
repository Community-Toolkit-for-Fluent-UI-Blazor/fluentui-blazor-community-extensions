namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a payload used for debugging purposes.
/// </summary>
public sealed class DebugPayload : IEquatable<DebugPayload>
{
    /// <summary>
    /// Gets the dots per inch (DPI) value associated with the debug payload.
    /// </summary>
    public double Dpi { get; init; }

    /// <summary>
    /// Gets the total number of strokes that have been processed or rendered in the context of the debug payload. 
    /// </summary>
    public int StrokeCount { get; init; }

    /// <summary>
    /// Gets the total number of points that have been processed or rendered in the context of the debug payload.
    /// </summary>
    public int PointCount { get; init; }

    /// <summary>
    /// Gets the background payload associated with this instance.
    /// </summary>
    public BackgroundPayload? Background { get; init; }

    /// <summary>
    /// Gets the debug text payload associated with the signature.
    /// </summary>
    public DebugTextPayload? Text { get; init; }

    /// <inheritdoc />
    public bool Equals(DebugPayload? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Dpi.Equals(other.Dpi) &&
               StrokeCount == other.StrokeCount &&
               PointCount == other.PointCount;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is DebugPayload other)
        {
            return Equals(other);
        }

        return false;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Dpi, StrokeCount, PointCount);
    }
}
