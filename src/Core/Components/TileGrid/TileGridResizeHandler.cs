using Microsoft.AspNetCore.Components.Web;

namespace FluentUI.Blazor.Community.Components;

internal class TileGridResizeHandler<TItem>
{
    private readonly TileGridState<TItem> _state;
    private readonly TileGridLayoutEngine<TItem> _layout;

    public TileGridResizeHandler(
        TileGridState<TItem> state,
        TileGridLayoutEngine<TItem> layout)
    {
        _state = state;
        _layout = layout;
    }

    public void BeginResize(TItem item, TileGridItemResizeHandle handle, double x, double y)
    {
        _state.ResizeSource = item;
        _state.ResizeHandle = handle;
        _state.ResizeStartX = x;
        _state.ResizeStartY = y;

        if (item is ITileGridItem tile)
        {
            _state.ResizeInitialColumnSpan = tile.ColumnSpan;
            _state.ResizeInitialRowSpan = tile.RowSpan;
        }

        _state.IsResizing = true;
    }

    public void Move(MouseEventArgs e)
    {
        if (_state.CellWidth <= 0 || _state.CellHeight <= 0)
        {
            return;
        }

        var deltaX = e.ClientX - _state.ResizeStartX;
        var deltaY = e.ClientY - _state.ResizeStartY;

        var colDelta = deltaX / _state.CellWidth;
        var rowDelta = deltaY / _state.CellHeight;

        var newColSpan = _state.ResizeInitialColumnSpan;
        var newRowSpan = _state.ResizeInitialRowSpan;

        switch (_state.ResizeHandle)
        {
            case TileGridItemResizeHandle.Right:
                newColSpan = Math.Max(1, _state.ResizeInitialColumnSpan + (int)Math.Round(colDelta));
                break;

            case TileGridItemResizeHandle.Bottom:
                newRowSpan = Math.Max(1, _state.ResizeInitialRowSpan + (int)Math.Round(rowDelta));
                break;

            case TileGridItemResizeHandle.BottomRight:
                newColSpan = Math.Max(1, _state.ResizeInitialColumnSpan + (int)Math.Round(colDelta));
                newRowSpan = Math.Max(1, _state.ResizeInitialRowSpan + (int)Math.Round(rowDelta));
                break;
        }

        if (!_layout.IsResizeValid(newColSpan, newRowSpan))
        {
            return;
        }

        _state.ResizePreviewColumnSpan = newColSpan;
        _state.ResizePreviewRowSpan = newRowSpan;

        if (_state.ResizeSource is ITileGridItem tile)
        {
            tile.ColumnSpan = newColSpan;
            tile.RowSpan = newRowSpan;
        }
    }

    public void End()
    {
        _state.IsResizing = false;
        _state.ResizeSource = default;
        _state.ResizeHandle = TileGridItemResizeHandle.None;
    }
}
