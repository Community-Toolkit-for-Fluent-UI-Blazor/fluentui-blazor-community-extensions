using System.Text.Json;

namespace FluentUI.Blazor.Community.Components;

internal class TileGridLayoutEngine<TItem>
{
    private readonly FluentCxTileGrid<TItem> _tileGrid;
    private readonly List<TItem> _initialLayout = [];

    /// <summary>
    /// Stores cached boolean arrays for each row, indexed by row number.
    /// </summary>
    /// <remarks>Each key in the dictionary represents a row index, and the corresponding value is an array of
    /// boolean values associated with that row. This cache can be used to efficiently track or retrieve the state of
    /// multiple conditions for each row.</remarks>
    private readonly Dictionary<int, bool[]> _rowCache = [];

    public TileGridLayoutEngine(
        FluentCxTileGrid<TItem> tileGrid)
    {
        _tileGrid = tileGrid;
        _initialLayout = [.. tileGrid.Items.Select(CloneItem)];
    }

    /// <summary>
    /// Creates a copy of the specified item.
    /// </summary>
    /// <remarks>This method is intended to be overridden in a derived class to provide the cloning
    /// functionality.</remarks>
    /// <param name="item">The item to be cloned. This parameter cannot be null.</param>
    /// <returns>A new instance of the item that is a copy of the original.</returns>
    /// <exception cref="NotImplementedException">Thrown when the method is called, as the implementation is not provided.</exception>
    private static TItem CloneItem(TItem item)
    {
        var json = JsonSerializer.Serialize(item);

        return JsonSerializer.Deserialize<TItem>(json)!;
    }

    /// <summary>
    /// Retrieves a cloned array of Boolean values representing a row with the specified number of columns.
    /// </summary>
    /// <remarks>If the row for the specified number of columns has not been cached, a new array is created
    /// and cached for future use.</remarks>
    /// <param name="cols">The number of columns in the row to retrieve. Must be a positive integer.</param>
    /// <returns>A Boolean array representing the row, with each element initialized to <see langword="false"/>.</returns>
    private bool[] GetRow(int cols)
    {
        if (!_rowCache.TryGetValue(cols, out var row))
        {
            row = new bool[cols];
            _rowCache[cols] = row;
        }

        return (bool[])row.Clone();
    }

    /// <summary>
    /// Arranges tile grid items into a logical layout based on their row and column spans.
    /// </summary>
    /// <remarks>Items are placed in the grid in order of their index. If an item cannot fit in the current
    /// row due to its span, it is placed in the next available row. The method ensures that items do not overlap and
    /// that all items fit within the specified column constraints.</remarks>
    /// <param name="cols">The total number of columns available in the grid. Must be a positive integer.</param>
    /// <returns>A dictionary that maps each tile grid item to its assigned position and span in the layout. Each value is a
    /// tuple containing the row index, column index, row span, and column span for the item.</returns>
    private Dictionary<ITileGridItem, (int row, int col, int rowSpan, int colSpan)> BuildLogicalLayout(int cols)
    {
        var result = new Dictionary<ITileGridItem, (int row, int col, int rowSpan, int colSpan)>();
        var ordered = _tileGrid.Items.OfType<ITileGridItem>().OrderBy(i => i.Index).ToList();
        var rows = new List<bool[]>();

        bool Fits(int row, int col, int rs, int cs)
        {
            for (var r = row; r < row + rs; r++)
            {
                if (r >= rows.Count)
                {
                    return true;
                }

                var line = rows[r];
                for (var c = col; c < col + cs; c++)
                {
                    if (line[c])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        void Occupy(int row, int col, int rs, int cs)
        {
            for (var r = row; r < row + rs; r++)
            {
                while (r >= rows.Count)
                {
                    rows.Add(GetRow(cols));
                }

                var line = rows[r];

                for (var c = col; c < col + cs; c++)
                {
                    line[c] = true;
                }
            }
        }

        foreach (var item in ordered)
        {
            var rs = item.RowSpan;
            var cs = item.ColumnSpan;
            var foundRow = 0;
            var foundCol = 0;
            var placed = false;

            for (var row = 0; !placed; row++)
            {
                for (var col = 0; col <= cols - cs; col++)
                {
                    if (Fits(row, col, rs, cs))
                    {
                        foundRow = row;
                        foundCol = col;
                        placed = true;

                        break;
                    }
                }
            }

            Occupy(foundRow, foundCol, rs, cs);
            result[item] = (foundRow, foundCol, rs, cs);
        }

        return result;
    }

    /// <summary>
    /// Applies the current layout configuration to all tiles in the grid, updating their row and column spans based on
    /// the specified or default column count.
    /// </summary>
    /// <remarks>If the Columns property is not set or is less than or equal to zero, the method uses a
    /// default column count to determine the layout. This method should be called whenever the tile arrangement or
    /// column configuration changes to ensure the visual layout remains consistent.</remarks>
    public void ApplyLayout(bool preview = false)
    {
        var cols = _tileGrid.ColumnCount;
        var layout = BuildLogicalLayout(cols);

        foreach (var kvp in layout)
        {
            var tile = kvp.Key;
            var (row, col, rowSpan, colSpan) = kvp.Value;

            if (!preview)
            {
                tile.RowSpan = rowSpan;
                tile.ColumnSpan = colSpan;
            }

            tile.Row = row;
            tile.Column = col;
        }
    }

    /// <summary>
    /// Determines whether the specified column and row spans are valid for resizing the grid.
    /// </summary>
    /// <remarks>The method checks that the new column span does not exceed the available columns and that
    /// both spans are greater than zero.</remarks>
    /// <param name="newColSpan">The number of columns to span. Must be at least 1 and cannot exceed the total number of columns in the grid.</param>
    /// <param name="newRowSpan">The number of rows to span. Must be at least 1.</param>
    /// <returns>true if the specified spans are valid for the current grid configuration; otherwise, false.</returns>
    public bool IsResizeValid(int newColSpan, int newRowSpan)
    {
        var colCount = _tileGrid.ColumnCount;

        if (colCount <= 0)
        {
            return true;
        }

        if (newColSpan > colCount)
        {
            return false;
        }

        if (newColSpan < 1 || newRowSpan < 1)
        {
            return false;
        }

        return true;
    }

    internal void Reset()
    {
        _tileGrid.Items.Clear();
        _tileGrid.Items.AddRange(_initialLayout.Select(CloneItem));
    }
}
