namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an entry that contains information about the intersection state of an entity.
/// </summary>
public record IntersectEntry
{
    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the current object intersects with another object.
    /// </summary>
    public bool IsIntersecting { get; init; }

    /// <summary>
    /// Gets the ratio representing the intersection of two entities as a value between 0.0 and 1.0.
    /// </summary>
    public double IntersectionRatio { get; init; }
}
