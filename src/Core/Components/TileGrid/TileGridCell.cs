namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a cell within a tile grid layout.
/// </summary>
public sealed class TileGridCell
{
    /// <summary>
    /// Gets or sets the number of columns used to arrange items in the layout.
    /// </summary>
    public int ColumnCount { get; set; } = 1;

    /// <summary>
    /// Gets or sets the width of each cell in the grid.
    /// </summary>
    public double CellWidth { get; set; } = 1;

    /// <summary>
    /// Gets or sets the height of each cell in the grid.
    /// </summary>
    public double CellHeight { get; set; } = 1;

    /// <summary>
    /// Gets or sets the collection of heights, in device-independent units (DIP), for each row in the layout.
    /// </summary>
    /// <remarks>Each element in the list corresponds to the height of a row, in the order the rows appear.
    /// The list must contain a value for every row in the layout; otherwise, rendering issues may occur. Negative or
    /// non-finite values may result in undefined behavior.</remarks>
    public List<double> RowHeights { get; set; } = [];
}
