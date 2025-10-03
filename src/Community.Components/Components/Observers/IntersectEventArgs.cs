namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event data for an intersection event, providing details about the observed element's intersection
/// state.
/// </summary>
/// <remarks>This class is typically used in scenarios where intersection events are monitored, such as detecting
/// when an element enters or leaves the viewport or a specified root element. It provides information about the
/// observed element's identifiers, intersection state, and the ratio of the intersection area.</remarks>
public record IntersectEventArgs
{
    /// <summary>
    /// Gets the unique identifier for the group.
    /// </summary>
    public string GroupId { get; init; } = string.Empty;

    /// <summary>
    /// Gets the unique identifier for the element.
    /// </summary>
    public string ElementId { get; init; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the observed element is currently intersecting with the root element or viewport.
    /// </summary>
    public bool IsIntersecting { get; init; }

    /// <summary>
    /// Gets the ratio of the intersection area to the bounding box area of the observed element.
    /// </summary>
    public double IntersectionRatio { get; init; }
}
