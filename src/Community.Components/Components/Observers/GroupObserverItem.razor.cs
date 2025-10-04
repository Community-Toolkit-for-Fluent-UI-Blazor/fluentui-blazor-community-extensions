using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an item within a group observer, typically used to monitor or manage changes in a grouped collection of
/// components.
/// </summary>
/// <remarks>This class is intended to be used as part of a group observation mechanism, where it serves as an
/// individual item within the group. It inherits from <see cref="FluentComponentBase"/>, which provides base
/// functionality for fluent component design.</remarks>
public partial class GroupObserverItem
    : FluentComponentBase, IAsyncDisposable
{
    private readonly List<ObserverItem> _items = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupObserverItem"/> class.
    /// </summary>
    /// <remarks>The constructor generates a new unique identifier for the instance using <see
    /// cref="Identifier.NewId"/>.</remarks>
    public GroupObserverItem()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the unique identifier for the group to which this item belongs.
    /// </summary>
    /// <remarks>This property is used to associate the item with a specific group. It is essential for grouping
    /// functionality and should be unique within the context of the application.</remarks>
    [Parameter, EditorRequired]
    public string GroupId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the child content to be rendered within the group observer item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets the collection of observer items contained within this group.
    /// </summary>
    internal List<ObserverItem> Items => _items;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {

        }
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds an observer item to the collection.
    /// </summary>
    /// <remarks>This method adds the specified observer item to the internal collection. Ensure that the item
    /// is not <c>null</c>  before calling this method to avoid unexpected behavior.</remarks>
    /// <param name="item">The <see cref="ObserverItem"/> to add to the collection. Cannot be <c>null</c>.</param>
    internal void AddObserverItem([NotNull]ObserverItem item) => _items.Add(item);

    /// <summary>
    /// Removes the specified observer item from the collection.
    /// </summary>
    /// <remarks>If the specified item does not exist in the collection, no action is taken.</remarks>
    /// <param name="item">The <see cref="ObserverItem"/> to remove from the collection. Cannot be <see langword="null"/>.</param>
    internal void RemoveItem([NotNull]ObserverItem item) => _items.Remove(item);
}
