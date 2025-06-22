namespace FluentUI.Blazor.Community.Components.Internal;

internal sealed class DropZoneState<TItem>
{
    public TItem? ActiveItem { get; internal set; }

    public TItem? TargetItem { get; internal set; }

    public int? ActivePlaceholderId { get; internal set; }

    public IList<TItem>? Items { get; internal set; }

    internal void RemoveActiveItem()
    {
        if (ActiveItem is not null)
        {
            Items?.Remove(ActiveItem);
        }
    }

    internal void Reset()
    {
        ActiveItem = default;
        TargetItem = default;
        ActivePlaceholderId = null;
        Items = null;
    }
}
