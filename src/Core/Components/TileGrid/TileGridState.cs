namespace FluentUI.Blazor.Community.Components;

internal class TileGridState<TItem>
{
    /// <summary>
    /// Gets or sets the source item being dragged in the drag-and-drop operation.
    /// </summary>
    /// <remarks>This field holds a reference to the item that is currently being dragged. It can be null if
    /// no item is being dragged.</remarks>
    public TItem? DragSource { get; set; }

    /// <summary>
    /// Gets or sets the current drag target item. This value is null if no item is being dragged.
    /// </summary>
    public TItem? DragTarget { get; set; }

    public TItem? ResizeSource { get; set; }
    public int ResizeInitialRowSpan { get; set; }
    public int ResizeInitialColumnSpan { get; set; }
    public double ResizeStartX { get; set; }
    public double ResizeStartY { get; set; }

    public double CellWidth { get; set; }
    public double CellHeight { get; set; }
    public int ColumnCount { get; set; }

    public bool IsResizing { get; set; }
    public int ResizePreviewRowSpan { get; set; }
    public int ResizePreviewColumnSpan { get; set; }

    public TileGridItemResizeHandle ResizeHandle { get; set; } = TileGridItemResizeHandle.None;

    public List<double> RowHeights { get; set; } = [];

    public List<TItem>? InitialLayout { get; set; }

    public GhostTile GhostTile { get; } = new();

    internal void Reset()
    {
        GhostTile.Clear();
        DragSource = default;
        DragTarget = default;
    }
}
