namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the settings for a <see cref="FluentCxTileGrid{TItem}"/>.
/// </summary>
public interface ITileGridSettings
{
    /// <summary>
    /// Gets the width of the column.
    /// </summary>
    string ColumnWidth { get; }

    /// <summary>
    /// Gets the minimum width of the column.
    /// </summary>
    string? MinimumColumnWidth { get; }

    /// <summary>
    /// Gets the number of columns.
    /// </summary>
    int? Columns { get; }

    /// <summary>
    /// Gets the height of the row.
    /// </summary>
    string RowHeight { get; }

    /// <summary>
    /// Gets the width of the tile grid.
    /// </summary>
    string? Width { get; }

    /// <summary>
    /// Gets the height of the tile grid.
    /// </summary>
    string? Height { get; }
}
