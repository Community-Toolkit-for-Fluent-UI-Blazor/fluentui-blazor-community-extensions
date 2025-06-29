namespace FluentUI.Blazor.Community.Components.Internal;

/// <summary>
/// Represents the state of a drop zone.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
internal sealed class DropZoneState<TItem>
{
    /// <summary>
    /// Gets the active item.
    /// </summary>
    public TItem? ActiveItem { get; internal set; }

    /// <summary>
    /// Gets the target item.
    /// </summary>
    public TItem? TargetItem { get; internal set; }

    /// <summary>
    /// Gets the identifier of the active placeholder.
    /// </summary>
    public int? ActivePlaceholderId { get; internal set; }

    /// <summary>
    /// Gets the list of items inside the container.
    /// </summary>
    public IList<TItem>? Items { get; internal set; }

    /// <summary>
    /// Remove the active item from the list of item.
    /// </summary>
    internal void RemoveActiveItem()
    {
        if (ActiveItem is not null)
        {
            Items?.Remove(ActiveItem);
        }
    }

    /// <summary>
    /// Reset the drop zone.
    /// </summary>
    internal void Reset()
    {
        ActiveItem = default;
        TargetItem = default;
        ActivePlaceholderId = null;
        Items = null;
    }
}
