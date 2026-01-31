using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides drag-and-drop management for a tile grid, handling the initiation, preview, and completion of item movement
/// within the grid.
/// </summary>
/// <remarks>This class coordinates the drag-and-drop workflow by tracking the source and target items, updating
/// their visual states, and reordering items upon drop. It ensures that the grid's layout and item states are updated
/// appropriately throughout the drag operation.</remarks>
/// <typeparam name="TItem">The type of items contained in the tile grid.</typeparam>
internal class TileGridDragHandler<TItem>
{
    /// <summary>
    /// Represents the current immutable state of the tile grid, including the arrangement and status of items.
    /// </summary>
    private readonly TileGridState<TItem> _state;

    /// <summary>
    /// Represents the layout engine responsible for arranging tiles in the grid.
    /// </summary>
    private readonly TileGridLayoutEngine<TItem> _layout;

    /// <summary>
    /// Reprensetns the list of items stored in the collection.
    /// </summary>
    private readonly List<TItem> _items;

    /// <summary>
    /// Initializes a new instance of the TileGridDragHandler class to manage drag-and-drop operations within a tile
    /// grid.
    /// </summary>
    /// <param name="state">The current state of the tile grid, containing information about the grid's layout and items. Cannot be null.</param>
    /// <param name="layout">The layout engine that arranges tiles within the grid according to defined rules. Cannot be null.</param>
    /// <param name="items">The collection of items to be managed and made draggable within the tile grid. Cannot be null.</param>
    public TileGridDragHandler(
        TileGridState<TItem> state,
        TileGridLayoutEngine<TItem> layout,
        List<TItem> items)
    {
        _state = state;
        _layout = layout;
        _items = items;
    }

    /// <summary>
    /// Initiates a drag operation using the specified event arguments.
    /// </summary>
    /// <remarks>If the source item implements the ITileGridItem interface, this method activates a ghost tile
    /// to visually represent the dragged item during the operation.</remarks>
    /// <param name="e">The event arguments that provide information about the drag operation, including the source item being dragged.</param>
    public void Start(FluentDragEventArgs<TItem> e)
    {
        _state.DragSource = e.Source.Item;

        if (_state.DragSource is ITileGridItem source)
        {
            _state.GhostTile.Activate(source);
        }
    }

    /// <summary>
    /// Sets the current drag target and updates the preview state of items in the grid during a drag operation.
    /// </summary>
    /// <remarks>Call this method when a drag operation enters a new item in the grid to update which item is
    /// considered the preview target. Only one item can be marked as the preview target at a time.</remarks>
    /// <param name="e">The event arguments that provide information about the drag operation, including the item being targeted.</param>
    public void Enter(FluentDragEventArgs<TItem> e)
    {
        _state.DragTarget = e.Target.Item;

        foreach (var item in _items.OfType<ITileGridItem>())
        {
            item.IsPreviewTarget = false;
        }

        if (_state.DragTarget is ITileGridItem target)
        {
            target.IsPreviewTarget = true;
        }
    }

    /// <summary>
    /// Resets the drag target state, removing any preview indication from the target item.
    /// </summary>
    /// <remarks>This method is typically called when the drag operation is completed or canceled, ensuring
    /// that the target item no longer reflects a preview state. It is important to note that this method does not
    /// perform any additional cleanup beyond resetting the drag target.</remarks>
    public void Leave()
    {
        if (_state.DragTarget is ITileGridItem target)
        {
            target.IsPreviewTarget = false;
        }

        _state.DragTarget = default;
    }

    /// <summary>
    /// Swaps the positions of the drag source and drag target tile grid items within the grid.
    /// </summary>
    /// <remarks>This method performs the swap only if both the drag source and drag target are valid tile
    /// grid items. After swapping their indices, the grid items are re-sorted to maintain the correct order. This
    /// operation is typically used to update the visual arrangement of tiles following a drag-and-drop
    /// action.</remarks>
    public void Drop()
    {
        if (_state.DragSource is not ITileGridItem source ||
            _state.DragTarget is not ITileGridItem target)
        {
            return;
        }

        (target.Index, source.Index) = (source.Index, target.Index);

        var sorted = _items.OfType<ITileGridItem>()
                           .OrderBy(i => i.Index)
                           .Cast<TItem>()
                           .ToList();

        _items.Clear();
        _items.AddRange(sorted);
    }

    /// <summary>
    /// Ends the current drag-and-drop operation and resets the state of all items in the grid.
    /// </summary>
    /// <remarks>This method clears the drag source and target, resets the preview state of all items, and
    /// applies any layout changes required after the drag-and-drop operation completes. Call this method to finalize a
    /// drag-and-drop sequence and restore the grid to its normal state.</remarks>
    public void End()
    {
        foreach (var item in _items.OfType<ITileGridItem>())
        {
            item.IsPreviewTarget = false;
            item.IsDragging = false;
        }

        _state.DragSource = default;
        _state.DragTarget = default;
        _state.GhostTile.Clear();

        _layout.ApplyLayout();
    }
}
