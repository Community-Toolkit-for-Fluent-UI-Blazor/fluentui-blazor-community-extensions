namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a settings for a grid.
/// </summary>
public interface IGridSettings
{
    /// <summary>
    /// Gets the display for the drop zone.
    /// </summary>
    DropZoneDisplay Display { get; }

    /// <summary>
    /// Gets the width of the grid.
    /// </summary>
    string? Width { get; }

    /// <summary>
    /// Gets the height of the grid.
    /// </summary>
    string? Height { get; }
}
