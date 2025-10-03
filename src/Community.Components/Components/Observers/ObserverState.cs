using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

internal sealed class ObserverState
{
    private readonly Dictionary<string, GroupObserverItem> _groups = [];
    private readonly Dictionary<string, ObserverItem> _items = [];
    private FluentCxObserverProvider? _provider;
    private readonly Queue<GroupObserverItem> _addPendingGroups = new();
    private readonly Queue<ObserverItem> _addPendingItems = new();
    private readonly Queue<string> _removePendingGroups = new();
    private readonly Queue<string> _removePendingItems = new();

    /// <summary>
    /// Adds a group to the collection and notifies the provider if available.
    /// </summary>
    /// <remarks>If the provider is not available, the group is added to a pending queue for later processing.
    /// Otherwise, the provider is immediately notified of the added group.</remarks>
    /// <param name="group">The group to add. The group's <see cref="GroupObserverItem.GroupId"/> must not be null, empty, or whitespace.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="group"/> has a <see cref="GroupObserverItem.GroupId"/> that is null, empty, or
    /// consists only of whitespace.</exception>
    public async ValueTask AddGroupAsync(GroupObserverItem group)
    {
        ArgumentNullException.ThrowIfNull(group);

        if (string.IsNullOrWhiteSpace(group.GroupId))
        {
            throw new ArgumentException("GroupId cannot be null or empty.", nameof(group));
        }

        _groups[group.GroupId] = group;

        if (_provider is null)
        {
            _addPendingGroups.Enqueue(group);
        }
        else
        {
            await _provider.NotifyAsync(group);
        }
    }

    /// <summary>
    /// Removes the specified group from the collection and updates its notification status.
    /// </summary>
    /// <remarks>If the provider is not available, the group is added to a queue of pending removals.
    /// Otherwise, the group is unnotified immediately.</remarks>
    /// <param name="groupId">The unique identifier of the group to remove. Cannot be null, empty, or consist only of whitespace.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="groupId"/> is null, empty, or consists only of whitespace.</exception>
    public async ValueTask RemoveGroupAsync(string groupId)
    {
        if (string.IsNullOrWhiteSpace(groupId))
        {
            throw new ArgumentException("GroupId cannot be null or empty.", nameof(groupId));
        }

        _groups.Remove(groupId);

        if (_provider is null)
        {
            _removePendingGroups.Enqueue(groupId);
        }
        else
        {
            await _provider.UnnotifyGroupAsync(groupId);
        }
    }

    /// <summary>
    /// Adds an item to the collection and notifies the provider if available.
    /// </summary>
    /// <remarks>If the provider is not available, the item is added to a pending queue to be processed later.
    /// Otherwise, the provider is notified immediately after the item is added.</remarks>
    /// <param name="item">The <see cref="ObserverItem"/> to add. The item's <see cref="FluentComponentBase.Id"/> must not be null or empty.</param>
    public async ValueTask AddItemAsync(ObserverItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentException.ThrowIfNullOrEmpty(item.Id, nameof(ObserverItem.Id));

        _items[item.Id] = item;

        if (_provider is null)
        {
            _addPendingItems.Enqueue(item);
        }
        else
        {
             await _provider.NotifyAsync(item);
        }
    }

    /// <summary>
    /// Removes the specified item from the collection and updates its notification state.
    /// </summary>
    /// <remarks>If a notification provider is available, the item's notification state is updated by calling
    /// the provider.  Otherwise, the item's ID is added to a queue for pending removal.</remarks>
    /// <param name="item">The item to remove. The item's <see cref="FluentComponentBase.Id"/> must not be null or empty.</param>
    public async ValueTask RemoveItemAsync(ObserverItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentException.ThrowIfNullOrEmpty(item.Id, nameof(ObserverItem.Id));

        _items.Remove(item.Id);

        if (_provider is null)
        {
            _removePendingItems.Enqueue(item.Id);
        }
        else
        {
            await _provider.UnnotifyItemAsync(item.Id);
        }
    }

    /// <summary>
    /// Sets the provider responsible for handling notifications and processes any pending notification actions.
    /// </summary>
    /// <remarks>When a new provider is set, any pending groups or items queued for notification are processed
    /// and sent to the provider. If the provider is set to <see langword="null"/>, all pending notifications are
    /// discarded.</remarks>
    /// <param name="value">The <see cref="FluentCxObserverProvider"/> instance to set as the current provider.  If <paramref name="value"/>
    /// is <see langword="null"/>, all pending notifications are cleared.</param>
    internal async ValueTask SetProviderAsync(FluentCxObserverProvider? value)
    {
        if (value is null && _provider is not null)
        {
            if (_removePendingGroups.Count > 0)
            {
                while (_removePendingGroups.TryDequeue(out var groupId))
                {
                    await _provider.UnnotifyGroupAsync(groupId);
                }
            }

            if (_removePendingItems.Count > 0)
            {
                while (_removePendingItems.TryDequeue(out var itemId))
                {
                    await _provider.UnnotifyItemAsync(itemId);
                }
            }
        }

        _provider = value;

        if (_provider is null)
        {
            return;
        }

        if (_addPendingGroups.Count > 0)
        {
            while (_addPendingGroups.TryDequeue(out var group))
            {
                await _provider.NotifyAsync(group);
            }
        }

        if (_addPendingItems.Count > 0)
        {
            while (_addPendingItems.TryDequeue(out var item))
            {
                await _provider.NotifyAsync(item);
            }
        }
    }

    /// <summary>
    /// Retrieves the list of items associated with the specified group identifier.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group whose items are to be retrieved. Cannot be null or empty.</param>
    /// <returns>A list of <see cref="ObserverItem"/> objects associated with the specified group.  Returns an empty list if the
    /// group does not exist.</returns>
    internal List<ObserverItem> GetItems(string groupId)
    {
        ArgumentException.ThrowIfNullOrEmpty(groupId, nameof(groupId));

        if (_groups.TryGetValue(groupId, out var group))
        {
            return group.Items;
        }

        return [];
    }

    /// <summary>
    /// Retrieves an item by its identifier, searching within a specified group or globally if the group is not found.
    /// </summary>
    /// <param name="groupId">The identifier of the group to search within. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="itemId">The identifier of the item to retrieve. Cannot be <see langword="null"/> or empty.</param>
    /// <returns>The <see cref="ObserverItem"/> that matches the specified <paramref name="itemId"/>.  If the group is found, the
    /// search is limited to that group; otherwise, a global search is performed.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if no item with the specified <paramref name="itemId"/> is found in the specified group or globally.</exception>
    internal ObserverItem GetItem(string groupId, string itemId)
    {
        ArgumentException.ThrowIfNullOrEmpty(itemId, nameof(itemId));
        ArgumentException.ThrowIfNullOrEmpty(groupId, nameof(groupId));

        if (_groups.TryGetValue(groupId, out var group))
        {
            var item = group.Items.Find(i => i.Id == itemId);

            if (item is not null)
            {
                return item;
            }
        }
        else if (_items.TryGetValue(itemId, out var item))
        {
            return item;
        }

        throw new KeyNotFoundException($"Item with ID '{itemId}' not found.");
    }
}
