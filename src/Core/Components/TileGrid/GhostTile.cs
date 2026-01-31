namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a ghost tile used for drag-and-drop operations within a tile grid.
/// </summary>
/// <remarks>A ghost tile temporarily holds the position and size information of a tile grid item while it is
/// being dragged. This type is used to manage the visual and logical state of a tile during drag-and-drop interactions,
/// allowing the grid to reflect the item's movement without immediately committing changes. The ghost tile can be
/// activated to represent a specific item and cleared to reset its state for reuse.</remarks>
internal sealed record GhostTile
{
    /// <summary>
    /// Represents the source tile grid item associated with the ghost tile.
    /// </summary>
    private ITileGridItem? _source;

    /// <summary>
    /// Gets or sets the ghost index used for drag-and-drop operations.
    /// </summary>
    public long? Index { get; private set; }

    /// <summary>
    /// Gets or sets the ghost row span used for drag-and-drop operations.
    /// </summary>
    public int RowSpan { get; private set; }

    /// <summary>
    /// Gets or sets the ghost column span used for drag-and-drop operations.
    /// </summary>
    public int ColumnSpan { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the current instance is active.
    /// </summary>
    /// <remarks>The property returns <see langword="true"/> if the instance has a valid index; otherwise, it
    /// returns <see langword="false"/>. Use this property to determine whether the instance is considered active in
    /// contexts where the index is relevant.</remarks>
    public bool IsActive => Index.HasValue;

    /// <summary>
    /// Activates the specified tile grid item by updating its position and size, and marks it as being dragged.
    /// </summary>
    /// <remarks>Call this method to begin a drag operation for a tile grid item. After activation, the item's
    /// IsDragging property is set to <see langword="true"/>, indicating that it is currently being manipulated within
    /// the grid.</remarks>
    /// <param name="item">The tile grid item to activate. Must not be null. The item's index, row span, and column span are used to update
    /// the current state, and its dragging state is set to active.</param>
    public void Activate(ITileGridItem item)
    {
        Index = item.Index;
        RowSpan = item.RowSpan;
        ColumnSpan = item.ColumnSpan;
        _source = item;
        _source.IsDragging = true;
    }

    /// <summary>
    /// Resets the index and span properties to their default values, clearing the current state of the object.
    /// </summary>
    /// <remarks>After calling this method, the Index property is set to null, and both RowSpan and ColumnSpan
    /// are set to 0. Use this method to reinitialize the object before reuse or to remove any previously set
    /// values.</remarks>
    public void Clear()
    {
        Index = null;
        RowSpan = 0;
        ColumnSpan = 0;
        _source?.IsDragging = false;
        _source = null;
    }
}
