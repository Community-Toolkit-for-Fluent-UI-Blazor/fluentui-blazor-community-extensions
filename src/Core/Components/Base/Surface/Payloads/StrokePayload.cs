namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a stroke composed of a sequence of points.
/// </summary>
public sealed class StrokePayload : IEquatable<StrokePayload>
{
    /// <summary>
    /// Gets or sets the unique identifier for the instance.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the collection of points that define the stroke.
    /// </summary>
    public List<StrokePointPayload> Points { get; set; } = [];

    /// <summary>
    /// Gets the payload containing information about the pen input associated with this instance.
    /// </summary>
    public PenPayload Pen { get; init; } = default!;

    /// <summary>
    /// Gets or sets the blend mode used when rendering stroke.
    /// </summary>
    public string BlendMode { get; set; } = "source-over";

    /// <inheritdoc />
    public bool Equals(StrokePayload? other)
    {
        if (other is null)
        {
            return false;
        }

        return
            EqualityComparer<List<StrokePointPayload>>.Default.Equals(Points, other.Points) &&
            Id == other.Id &&
            BlendMode == other.BlendMode;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        Equals(obj as StrokePayload);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(
            Points
        );
    }
}

