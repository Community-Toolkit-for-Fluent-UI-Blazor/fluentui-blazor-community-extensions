using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.TrailMenu;

/// <summary>
/// Provides factory methods for creating instances of TrailMenuBuilder with specified configuration options.
/// </summary>
/// <remarks>This static class simplifies the creation of TrailMenuBuilder objects by encapsulating the
/// initialization logic. Use the Create method to construct a new TrailMenuBuilder with a required label and optional
/// icon and identifier.</remarks>
public static class TrailMenuFactory
{
    /// <summary>
    /// Creates a new instance of the TrailMenuBuilder class with the specified label, and optionally an icon and
    /// identifier.
    /// </summary>
    /// <param name="label">The text to display for the menu item. This parameter cannot be null.</param>
    /// <param name="icon">An optional icon to associate with the menu item. If not specified, no icon is displayed.</param>
    /// <param name="id">An optional unique identifier for the menu item. If not specified, a default identifier is used.</param>
    /// <returns>A TrailMenuBuilder instance configured with the provided label, icon, and identifier.</returns>
    public static TrailMenuBuilder Create(
        string id,
        string label,
        Icon? icon = null)
    {
        return new(new InternalTrailMenuItem
        {
            Label = label,
            Icon = icon,
            Id = id
        });
    }
}

/// <summary>
/// Provides a fluent interface for constructing and modifying a hierarchical trail menu structure by adding, removing,
/// and manipulating menu items.
/// </summary>
/// <remarks>Use this builder to incrementally define a menu's structure by chaining method calls that add items,
/// navigate the hierarchy, or perform bulk and conditional operations. The builder maintains an internal cursor to the
/// current menu item, allowing for intuitive navigation and modification of nested menus. Once the desired structure is
/// defined, call Build to produce an immutable menu representation suitable for display or further processing. This
/// class is not thread-safe.</remarks>
public sealed class TrailMenuBuilder
{
    /// <summary>
    /// Represents the root item of the internal trail menu structure.
    /// </summary>
    /// <remarks>This field maintains a reference to the top-level menu item and is initialized during menu
    /// construction. It should not be modified directly.</remarks>
    private readonly InternalTrailMenuItem _root;

    /// <summary>
    /// Gets or sets the current internal trail menu item being processed.
    /// </summary>
    private InternalTrailMenuItem _current;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrailMenuBuilder"/> class using the specified root menu item as the
    /// starting point for menu construction.
    /// </summary>
    /// <remarks>This constructor sets the current menu item to the specified root, enabling subsequent
    /// modifications to the menu structure from this starting point.</remarks>
    /// <param name="root">The root menu item that serves as the base for building the trail menu. This parameter cannot be null.</param>
    internal TrailMenuBuilder(InternalTrailMenuItem root)
    {
        _root = root;
        _current = root;
    }

    /// <summary>
    /// Adds a new menu item with the specified identifier, label, and optional icon to the current menu item. The new item
    /// </summary>
    /// <param name="id">Identifier of the item.</param>
    /// <param name="label">Label of the item.</param>
    /// <param name="icon">Icon of the item.</param>
    /// <returns>Returns an instance of <see cref="TrailMenuBuilder"/> with the added item.</returns>
    public TrailMenuBuilder Add(string id, string label, Icon? icon = null)
    {
        var child = new InternalTrailMenuItem
        {
            Label = label,
            Icon = icon,
            Id = id
        };

        var list = _current.Items.ToList();
        list.Add(child);
        _current.Items = list;

        _current = child;

        return this;
    }

    /// <summary>
    /// Sets the current menu context to the root menu and returns the builder instance for further configuration.
    /// </summary>
    /// <returns>The current instance of the TrailMenuBuilder, enabling method chaining for additional menu configuration.</returns>
    public TrailMenuBuilder Root()
    {
        _current = _root;

        return this;
    }

    /// <summary>
    /// Adds a new root menu item to the trail menu builder and sets it as the current context for subsequent menu item
    /// additions.
    /// </summary>
    /// <remarks>This method explicitly sets the context to the root before adding the specified item as a
    /// child. Use this method to start building a new root-level menu structure.</remarks>
    /// <param name="id">The unique identifier for the root menu item. Cannot be null or empty.</param>
    /// <param name="label">The display label for the root menu item. Cannot be null or empty.</param>
    /// <param name="icon">An optional icon to associate with the root menu item. If null, no icon is displayed.</param>
    /// <returns>The current instance of the TrailMenuBuilder, enabling method chaining.</returns>
    public TrailMenuBuilder AddRoot(string id, string label, Icon? icon = null)
    {
        _current = _root;

        return Add(id, label, icon);
    }

    /// <summary>
    /// Navigates to the parent menu item in the trail menu hierarchy.
    /// </summary>
    /// <remarks>If the current menu item has a parent, this method updates the current context to that
    /// parent. This is useful for traversing up the menu structure when building or modifying a trail menu.</remarks>
    /// <returns>The current instance of the TrailMenuBuilder, enabling method chaining.</returns>
    public TrailMenuBuilder Up()
    {
        if (_current.Parent is InternalTrailMenuItem parent)
        {
            _current = parent;
        }

        return this;
    }

    /// <summary>
    /// Builds and returns the root menu item for the trail menu.
    /// </summary>
    /// <returns>An instance of <see cref="ITrailMenuItem"/> representing the root menu item.</returns>
    public ITrailMenuItem Build() => _root;

    /// <summary>
    /// Adds a collection of menu items to the trail menu builder.
    /// </summary>
    /// <remarks>This method iterates over the provided collection and adds each item to the menu, moving up
    /// the menu structure after each addition.</remarks>
    /// <param name="items">The collection of items to add, where each item is a tuple containing the label, an optional icon, and an
    /// optional identifier.</param>
    /// <returns>The current instance of the TrailMenuBuilder, allowing for method chaining.</returns>
    public TrailMenuBuilder AddRange(IEnumerable<(string id, string label, Icon? icon)> items)
    {
        foreach (var (id, label, icon) in items)
        {
            Add(id, label, icon);
            Up();
        }

        return this;
    }

    /// <summary>
    /// Adds a sibling to the current item.
    /// </summary>
    public TrailMenuBuilder AddSibling(string id, string label, Icon? icon = null)
    {
        if (_current.Parent is not InternalTrailMenuItem parent)
        {
            throw new InvalidOperationException("Cannot add a sibling to the root item.");
        }

        _current = parent;

        return Add(id, label, icon);
    }

    /// <summary>
    /// Adds multiple siblings to the current item.
    /// </summary>
    public TrailMenuBuilder AddSiblings(IEnumerable<(string id, string label, Icon? icon)> items)
    {
        foreach (var (id, label, icon) in items)
        {
            AddSibling(id, label, icon);
        }

        return this;
    }

    /// <summary>
    /// Adds a menu item with the specified label and optional icon to the trail if the specified condition is <see
    /// langword="true"/>.
    /// </summary>
    /// <remarks>Use this method to conditionally add menu items based on runtime logic, such as user
    /// permissions or application state.</remarks>
    /// <param name="condition">A value indicating whether the menu item should be added. If <see langword="true"/>, the item is added;
    /// otherwise, no action is taken.</param>
    /// <param name="label">The text label to display for the menu item.</param>
    /// <param name="icon">An optional icon to display alongside the menu item. If <see langword="null"/>, no icon is shown.</param>
    /// <param name="id">An optional unique identifier for the menu item. If <see langword="null"/>, a default identifier is used.</param>
    /// <returns>The current <see cref="TrailMenuBuilder"/> instance, enabling method chaining.</returns>
    public TrailMenuBuilder AddIf(string id, string label, Icon? icon = null, bool condition = true)
    {
        if (condition)
        {
            Add(id, label, icon);
        }

        return this;
    }

    /// <summary>
    /// Adds a sequence of path segments to the trail menu, creating a hierarchical structure based on the specified
    /// path.
    /// </summary>
    /// <param name="path">The path to add, represented as a string. The path is split into segments using the system directory separator
    /// character. Empty segments are ignored.</param>
    /// <param name="icon">An optional icon to associate with each path segment. If not specified, no icon is assigned.</param>
    /// <returns>The current instance of the TrailMenuBuilder, enabling method chaining.</returns>
    public TrailMenuBuilder AddPath(string path, Icon? icon = null)
    {
        var segments = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

        foreach (var segment in segments)
        {
            Add(segment, segment, icon);
        }

        return this;
    }

    /// <summary>
    /// Finds a trail menu item that matches the specified unique identifier.
    /// </summary>
    /// <remarks>This method searches through the root items of the trail menu and removes any prefix from the
    /// identifier before searching.</remarks>
    /// <param name="id">The unique identifier of the trail menu item to locate. This parameter cannot be null or empty.</param>
    /// <returns>The trail menu item that matches the specified identifier, or null if no item is found.</returns>
    public ITrailMenuItem? Find(string id)
        => TrailMenuUtils.Find(_root.Items, id, removePrefix: true);

    /// <summary>
    /// Removes the menu item with the specified identifier from the trail menu.
    /// </summary>
    /// <remarks>If the specified identifier does not correspond to an existing menu item, no action is taken.
    /// Removing a non-existent item has no effect.</remarks>
    /// <param name="id">The unique identifier of the menu item to remove. Cannot be null or empty.</param>
    /// <returns>The current instance of the <see cref="TrailMenuBuilder"/>, enabling method chaining.</returns>
    public TrailMenuBuilder Remove(string id)
    {
        TrailMenuUtils.Remove(_root, [id]);

        return this;
    }

    /// <summary>
    /// Renames the menu item identified by the specified ID with a new label.
    /// </summary>
    /// <remarks>This method updates the label of the specified menu item in the trail menu structure. If the
    /// specified ID does not exist, no changes are made.</remarks>
    /// <param name="id">The unique identifier of the menu item to rename. This value must not be null or empty.</param>
    /// <param name="newLabel">The new label to assign to the menu item. This value must not be null or empty.</param>
    /// <returns>The current instance of the TrailMenuBuilder, enabling method chaining.</returns>
    public TrailMenuBuilder Rename(string id, string newLabel)
    {
        TrailMenuUtils.UpdateLabel(_root, id, newLabel);

        return this;
    }

    /// <summary>
    /// Executes the specified action on each child menu item of the current trail menu.
    /// </summary>
    /// <remarks>Use this method to apply bulk operations or transformations to all child menu items in the
    /// current context. The order in which actions are applied corresponds to the order of the child items.</remarks>
    /// <param name="action">The action to perform on each child menu item. The delegate receives an <see cref="ITrailMenuItem"/>
    /// representing the child item.</param>
    /// <returns>The current <see cref="TrailMenuBuilder"/> instance, enabling method chaining.</returns>
    public TrailMenuBuilder ForEachChild(Action<ITrailMenuItem> action)
    {
        foreach (var child in _current.Items)
        {
            action(child);
        }

        return this;
    }

    /// <summary>
    /// Selects an item in the trail menu by its unique identifier and updates the current selection.
    /// </summary>
    /// <remarks>Use this method to programmatically select a specific item in the trail menu. If the
    /// identifier does not correspond to any item, the method throws an exception and the current selection remains
    /// unchanged.</remarks>
    /// <param name="id">The unique identifier of the item to select. This value cannot be null or empty.</param>
    /// <returns>The current instance of the TrailMenuBuilder, enabling method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if an item with the specified identifier is not found in the trail menu.</exception>
    public TrailMenuBuilder Select(string id)
    {
        var found = TrailMenuUtils.Find(_root.Items, id, removePrefix: true);

        if (found is InternalTrailMenuItem internalItem)
        {
            _current = internalItem;
        }
        else
        {
            throw new InvalidOperationException($"Item '{id}' not found.");
        }

        return this;
    }

    /// <summary>
    /// Adds a new child menu item to the specified parent menu item in the trail menu structure.
    /// </summary>
    /// <remarks>This method first selects the parent menu item identified by the specified parentId before
    /// adding the new child item. Use this method to build hierarchical menu structures by nesting child items under
    /// parent items.</remarks>
    /// <param name="parentId">The unique identifier of the parent menu item to which the new child item will be added. Cannot be null or
    /// empty.</param>
    /// <param name="id">The unique identifier for the new child menu item. Must be unique within the menu structure and cannot be null
    /// or empty.</param>
    /// <param name="label">The display label for the new child menu item. Cannot be null or empty.</param>
    /// <param name="icon">An optional icon to associate with the new child menu item. If not specified, the child item will not display an
    /// icon.</param>
    /// <returns>The current instance of the TrailMenuBuilder, enabling method chaining.</returns>
    public TrailMenuBuilder AddChildOf(string parentId, string id, string label, Icon? icon = null)
    {
        Select(parentId);

        return Add(id, label, icon);
    }

    /// <summary>
    /// Navigates up the trail menu hierarchy to the specified parent item identified by its ID.
    /// </summary>
    /// <remarks>This method traverses the parent items in the trail menu until it finds the specified parent
    /// ID or reaches the top of the hierarchy.</remarks>
    /// <param name="parentId">The ID of the parent menu item to navigate to. This value must not be null or empty.</param>
    /// <returns>The current instance of the TrailMenuBuilder, allowing for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the specified parent ID is not found in the menu hierarchy.</exception>
    public TrailMenuBuilder UpToParent(string parentId)
    {
        while (_current.Parent is InternalTrailMenuItem parent)
        {
            if (TrailMenuUtils.IsSame(parent.Id, parentId))
            {
                _current = parent;
                return this;
            }

            _current = parent;
        }

        throw new InvalidOperationException($"Parent '{parentId}' not found in hierarchy.");
    }
}

