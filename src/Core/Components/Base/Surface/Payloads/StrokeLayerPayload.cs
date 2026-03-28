namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an immutable payload containing the identifier for a stroke layer.
/// </summary>
public sealed class StrokeLayerPayload : ILayerPayload, IEquatable<StrokeLayerPayload>
{
    /// <summary>
    /// Gets the unique identifier for this instance.
    /// </summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the collection of stroke payloads representing the drawn strokes.
    /// </summary>
    public IReadOnlyList<StrokePayload> Strokes { get; init; } = [];

    /// <inheritdoc />
    public bool Equals(StrokeLayerPayload? other)
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
               EqualityComparer<IReadOnlyList<StrokePayload>>.Default.Equals(Strokes, other.Strokes);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not StrokeLayerPayload other)
        {
            return false;
        }

        return this == other;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Strokes);
    }
}
