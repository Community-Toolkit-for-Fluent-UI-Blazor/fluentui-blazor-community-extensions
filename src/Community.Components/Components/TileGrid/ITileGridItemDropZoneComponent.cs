namespace FluentUI.Blazor.Community.Components;

public interface ITileGridItemDropZoneComponent<TItem>
    : IDropZoneComponent<TItem>
{
    int ColumnSpan { get; }

    int RowSpan { get; }
}
