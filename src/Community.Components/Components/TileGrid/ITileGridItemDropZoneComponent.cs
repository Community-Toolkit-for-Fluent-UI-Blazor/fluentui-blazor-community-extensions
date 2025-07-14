namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the interface for a <see cref="Internal.FluentCxDropZone{TItem}"/> which contains a
///  <see cref="FluentCxTileGrid{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public interface ITileGridItemDropZoneComponent<TItem>
    : IDropZoneComponent<TItem>
{
    /// <summary>
    /// Gets the column span of the <see cref="FluentCxTileGridItem{TItem}"/>.
    /// </summary>
    int ColumnSpan { get; }

    /// <summary>
    /// Gets the row span of the <see cref="FluentCxTileGridItem{TItem}"/>.
    /// </summary>
    int RowSpan { get; }

    /// <summary>
    /// Sets the span of the item.
    /// </summary>
    /// <param name="columnSpan">Column span of the item.</param>
    /// <param name="rowSpan">Row span of the item.</param>
    void SetSpan(int columnSpan, int rowSpan);
}
