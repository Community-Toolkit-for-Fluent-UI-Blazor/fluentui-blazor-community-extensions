using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an item in a horizontal menu bar.
/// </summary>
public class MenuBarItem
{
    /// <summary>
    /// Gets or sets the unique identifier for this menu item.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the text displayed for this menu item.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the icon to display next to the text.
    /// Use Fluent UI icon objects (e.g., new Icons.Regular.Size20.Add()).
    /// </summary>
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets whether this menu item is disabled.
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets whether this menu item is currently selected/active.
    /// </summary>
    public bool Selected { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when this menu item is clicked.
    /// </summary>
    public EventCallback<MenuBarItem> OnClick { get; set; }

    /// <summary>
    /// Gets or sets the child menu items for creating dropdown menus.
    /// </summary>
    public List<MenuBarItem>? Children { get; set; }

    /// <summary>
    /// Gets or sets the URL to navigate to when this item is clicked.
    /// Takes precedence over OnClick if both are specified.
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the target for navigation (e.g., "_blank", "_self").
    /// Only used when Href is specified.
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// Gets or sets additional CSS classes to apply to this menu item.
    /// </summary>
    public string? CssClass { get; set; }

    /// <summary>
    /// Gets or sets custom data associated with this menu item.
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// Gets whether this menu item has child items.
    /// </summary>
    public bool HasChildren => Children?.Count > 0;

    /// <summary>
    /// Creates a simple menu item with text and click handler.
    /// </summary>
    /// <param name="text">The text to display</param>
    /// <param name="onClick">The click handler</param>
    /// <returns>A new MenuBarItem</returns>
    public static MenuBarItem Create(string text, EventCallback<MenuBarItem> onClick)
    {
        return new MenuBarItem
        {
            Text = text,
            OnClick = onClick
        };
    }

    /// <summary>
    /// Creates a menu item with text, icon, and click handler.
    /// </summary>
    /// <param name="text">The text to display</param>
    /// <param name="icon">The Fluent UI icon object</param>
    /// <param name="onClick">The click handler</param>
    /// <returns>A new MenuBarItem</returns>
    public static MenuBarItem Create(string text, Icon? icon, EventCallback<MenuBarItem> onClick)
    {
        return new MenuBarItem
        {
            Text = text,
            Icon = icon,
            OnClick = onClick
        };
    }

    /// <summary>
    /// Creates a navigation menu item.
    /// </summary>
    /// <param name="text">The text to display</param>
    /// <param name="href">The URL to navigate to</param>
    /// <param name="icon">Optional icon object</param>
    /// <returns>A new MenuBarItem</returns>
    public static MenuBarItem CreateLink(string text, string href, Icon? icon = null)
    {
        return new MenuBarItem
        {
            Text = text,
            Href = href,
            Icon = icon
        };
    }
}
