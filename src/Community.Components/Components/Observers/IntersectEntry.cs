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

    /// <summary>
    /// Gets the bounding rectangle of the element relative to the viewport, if available.
    /// </summary>
    /// <remarks>The bounding rectangle provides the position and dimensions of the element as rendered in the
    /// browser. If the element is not currently rendered or its layout information is unavailable, this property may be
    /// <see langword="null"/>.</remarks>
    public DomRect? BoundingClientRect { get; init; }

    /// <summary>
    /// Gets the rectangle representing the intersection area between the target element and its root, if any.
    /// </summary>
    public DomRect? IntersectionRect { get; init; }

    /// <summary>
    /// Gets the bounding rectangle of the root element at the time of observation, if available.
    /// </summary>
    /// <remarks>If the root element is no longer present or its bounds cannot be determined, this property
    /// returns <see langword="null"/>.</remarks>
    public DomRect? RootBounds { get; init; }
}
