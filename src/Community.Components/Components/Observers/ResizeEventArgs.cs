namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event data for a resize operation, including the dimensions of the resized element and its
/// identifiers.
/// </summary>
/// <remarks>This class provides information about a resize event, such as the group and element identifiers, as
/// well as the new width and height of the resized element.</remarks>
public record ResizeEventArgs
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
    /// Gets the width of the element.
    /// </summary>
    public double Width { get; init; }

    /// <summary>
    /// Gets the height of the element.
    /// </summary>
    public double Height { get; init; }

    /// <summary>
    /// Gets the bounding rectangle of the element.
    /// </summary>
    public DomRect Rect { get; init; } = new();
}
