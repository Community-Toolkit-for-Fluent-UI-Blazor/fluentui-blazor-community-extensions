namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an entry containing dimensions for resizing operations.
/// </summary>
/// <remarks>This record is typically used to store and transfer information about the dimensions of an object,
/// including its unique identifier and its width and height values.</remarks>
public record ResizeEntry
{
    /// <summary>
    /// Gets the unique identifier for the entry.
    /// </summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the width of the entry.
    /// </summary>
    public double Width { get; init; }

    /// <summary>
    /// Gets the height of the entry.
    /// </summary>
    public double Height { get; init; }

    /// <summary>
    /// Gets the DOM rectangle representing position of the entry.
    /// </summary>
    public DomRect Rect { get; init; } = new();
}
