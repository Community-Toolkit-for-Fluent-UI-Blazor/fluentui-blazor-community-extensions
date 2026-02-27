namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a set of flags indicating which visual elements require updating in a rendering context.
/// </summary>
/// <remarks>This class is used to track the 'dirty' state of various rendering components, such as background,
/// grid, axes, and others. Setting a flag to <see langword="true"/> indicates that the corresponding element needs to
/// be refreshed or redrawn. The <see cref="Any"/> property provides a quick way to determine if any element is marked
/// as dirty. The <see cref="Reset"/> method clears all flags, marking all elements as clean.</remarks>
public sealed class DirtyFlags
{
    /// <summary>
    /// Gets or sets a value indicating whether the background is dirty.
    /// </summary>
    public bool Background { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the grid is dirty.
    /// </summary>
    public bool Grid { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the axes is dirty.
    /// </summary>
    public bool Axes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the watermark is dirty.
    /// </summary>
    public bool Watermark { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the strokelayer is dirty.
    /// </summary>
    public bool StrokeLayer { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dynamicstroke is dirty.
    /// </summary>
    public bool DynamicStroke { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the selection is dirty.
    /// </summary>
    public bool Selection { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the debug is dirty.
    /// </summary>
    public bool Debug { get; set; }

    /// <summary>
    /// Gets a value indicating whether any visual element is dirty.
    /// </summary>
    public bool Any => Hover || Background || Grid || Axes || Watermark || StrokeLayer || DynamicStroke || Selection || Debug;

    /// <summary>
    /// Gets a value indicating whether the mouse pointer is currently hovering over the component.
    /// </summary>
    public bool Hover { get; set; }

    /// <summary>
    /// Resets all dirty flags to false, indicating that all visual elements are clean and do not require updating.
    /// </summary>
    public void Reset()
    {
        Background = Grid = Axes = Watermark =
        StrokeLayer = DynamicStroke = Selection = Debug =
        Hover = false;
    }
}

