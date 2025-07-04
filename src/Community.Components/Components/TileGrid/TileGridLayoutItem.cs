namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an item for the <see cref="TileGridLayout"/>.
/// </summary>
public class TileGridLayoutItem
    : GridLayoutBaseItem
{
    /// <summary>
    /// Gets or sets the column span of the item.
    /// </summary>
    public int ColumnSpan { get; set; }

    /// <summary>
    /// Gets or sets the row span of the item.
    /// </summary>
    public int RowSpan { get; set; }
}
