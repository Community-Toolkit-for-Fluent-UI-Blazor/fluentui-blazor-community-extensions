using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the description of a menu item.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public record FileManagerEntryMenuItemDescription<TItem> where TItem : class, new()
{
    /// <summary>
    /// Gets or sets the function to execute when the item is clicked.
    /// </summary>
    public Func<FileManagerEntry<TItem>, Task>? OnClick { get; set; }

    /// <summary>
    /// Gets or sets the icon of the menu.
    /// </summary>
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the label of the menu.
    /// </summary>
    public string? Label { get; set; }
}
