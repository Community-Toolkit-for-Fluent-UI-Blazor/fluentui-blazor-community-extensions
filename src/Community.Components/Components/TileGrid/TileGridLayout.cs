
namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the layout of the <see cref="FluentCxTileGrid{TItem}"/>
/// </summary>
public sealed class TileGridLayout
    : GridLayoutBase
{
    /// <summary>
    /// Updates the span of the item.
    /// </summary>
    /// <param name="key">Key of the item.</param>
    /// <param name="columnSpan">Column span of the item.</param>
    /// <param name="rowSpan">Row span of the item.</param>
    internal void UpdateSpan(string key, int columnSpan, int rowSpan)
    {
        var item = Get<TileGridLayoutItem>(key);

        if (item is not null)
        {
            item.ColumnSpan = columnSpan;
            item.RowSpan = rowSpan;
        }
    }
}
