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
    string? RowHeight { get; }

    /// <summary>
    /// Gets or sets the vertical spacing between rows, typically specified as a CSS length value.
    /// </summary>
    /// <remarks>Set this property to control the gap between rows in a grid or layout. Common values include
    /// pixel (e.g., "10px"), em (e.g., "1em"), or percentage units. If the value is <c>null</c> or an empty string, no
    /// additional row gap will be applied.</remarks>
    string? RowGap { get; set; }

    /// <summary>
    /// Gets or sets the gap size between columns, typically specified as a CSS length value.
    /// </summary>
    /// <remarks>Set this property to control the spacing between columns in a multi-column layout. Common
    /// values include pixel (e.g., "16px"), em (e.g., "1em"), or other valid CSS units. If the value is null or empty,
    /// the default column gap will be used.</remarks>
    string? ColumnGap { get; set; }

    /// <summary>
    /// Gets the width of the tile grid.
    /// </summary>
    string? Width { get; }

    /// <summary>
    /// Gets the height of the tile grid.
    /// </summary>
    string? Height { get; }
}
