using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an item for the <see cref="FluentCxPathBar"/> component.
/// </summary>
public sealed class PathBarItem
    : IPathBarItem
{
    private string? _id;

    /// <summary>
    /// Represents the children of the current instance.
    /// </summary>
    private IEnumerable<IPathBarItem> _items = [];

    /// <inheritdoc />
    public string? Label { get; set; }

    /// <inheritdoc />
    public string? Id
    {
        get => _id;
        set => _id ??= PathBarItemBuilder.GetIdentifier(value);
    }

    /// <inheritdoc />
    public Icon? Icon { get; set; }

    /// <inheritdoc />
    public IEnumerable<IPathBarItem> Items
    {
        get => _items;
        set
        {
            _items = value;

            foreach (var item in _items.OfType<PathBarItem>())
            {
                item.Parent = this;
            }
        }
    }

    /// <inheritdoc />
    public IPathBarItem Parent { get; private set; } = default!;
}
