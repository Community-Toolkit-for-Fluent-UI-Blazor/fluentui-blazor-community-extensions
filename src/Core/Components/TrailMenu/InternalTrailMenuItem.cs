using FluentUI.Blazor.Community.Components.TrailMenu;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a menu item within a hierarchical trail structure, supporting child items, labeling, and icon
/// association.
/// </summary>
/// <remarks>This class is intended for internal use to build and manage menu trails. Each instance can contain
/// child menu items and maintains a reference to its parent, enabling hierarchical navigation. The identifier is
/// automatically generated if not explicitly set, ensuring uniqueness within the trail.</remarks>
internal sealed class InternalTrailMenuItem
    : ITrailMenuItem
{
    /// <summary>
    /// Represents the unique identifier for the menu item.
    /// </summary>
    private string? _id;

    /// <summary>
    /// Represents the children of the current instance.
    /// </summary>
    private IEnumerable<ITrailMenuItem> _items = [];

    /// <inheritdoc />
    public string? Label { get; set; }

    /// <inheritdoc />
    public string? Id
    {
        get => _id;
        set => _id ??= TrailMenuUtils.GetIdentifier(value);
    }

    /// <inheritdoc />
    public Icon? Icon { get; set; }

    /// <inheritdoc />
    public IEnumerable<ITrailMenuItem> Items
    {
        get => _items;
        set
        {
            _items = value;

            foreach (var item in _items.OfType<InternalTrailMenuItem>())
            {
                item.Parent = this;
            }
        }
    }

    /// <inheritdoc />
    public ITrailMenuItem Parent { get; private set; } = default!;
}
