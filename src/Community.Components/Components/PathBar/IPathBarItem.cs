using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an interface for the <see cref="FluentCxPathBarItem"/>
/// </summary>
public interface IPathBarItem
{
    /// <summary>
    /// Gets the label of the item.
    /// </summary>
    string? Label { get; set; }

    /// <summary>
    /// Gets the parent of the item.
    /// </summary>
    IPathBarItem Parent { get; }

    /// <summary>
    /// Gets the id of the item.
    /// </summary>
    string? Id { get; }

    /// <summary>
    /// Gets the <see cref="Microsoft.FluentUI.AspNetCore.Components.Icon"/> of the item.
    /// </summary>
    Icon? Icon { get; }

    /// <summary>
    /// Gets the menu items of the item.
    /// </summary>
    IEnumerable<IPathBarItem> Items { get; set; }
}
