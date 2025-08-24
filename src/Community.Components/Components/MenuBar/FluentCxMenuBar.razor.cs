using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// A horizontal menu bar component that integrates with Fluent UI styling and theming.
/// </summary>
/// <remarks>
/// <para>
/// The FluentCxMenuBar provides a horizontal navigation menu with features including:
/// </para>
/// <list type="bullet">
/// <item>Support for flat menu items and dropdown menus</item>
/// <item>Integration with Fluent UI buttons and styling</item>
/// <item>Navigation links and click handlers</item>
/// <item>Icons and text labels</item>
/// <item>Disabled state support</item>
/// <item>Custom content areas</item>
/// <item>Keyboard navigation support</item>
/// </list>
/// <para>
/// The component follows Fluent UI design principles and integrates seamlessly with
/// other Fluent UI components.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// &lt;FluentCxMenuBar Items="menuItems" OnItemClick="HandleItemClick" /&gt;
/// </code>
/// </example>
public partial class FluentCxMenuBar : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the collection of menu items to display.
    /// </summary>
    [Parameter] public IEnumerable<MenuBarItem>? Items { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when a menu item is clicked.
    /// </summary>
    [Parameter] public EventCallback<MenuBarItem> OnItemClick { get; set; }

    /// <summary>
    /// Gets or sets custom content to display in the menu bar (e.g., search box, user profile).
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the appearance style for menu items.
    /// </summary>
    [Parameter] public Appearance ItemAppearance { get; set; } = Appearance.Stealth;

    /// <summary>
    /// Gets or sets the appearance style for selected menu items.
    /// </summary>
    [Parameter] public Appearance SelectedItemAppearance { get; set; } = Appearance.Filled;

    /// <summary>
    /// Gets or sets whether the menu bar should have a visible border.
    /// </summary>
    [Parameter] public bool ShowBorder { get; set; } = true;

    /// <summary>
    /// Gets or sets additional CSS classes to apply to menu items.
    /// </summary>
    [Parameter] public string? ItemCssClass { get; set; }

    private async Task OnMenuItemClick(MenuBarItem item)
    {
        if (item.Disabled)
            return;

        // Invoke item-specific click handler first
        if (item.OnClick.HasDelegate)
        {
            await item.OnClick.InvokeAsync(item);
        }

        // Then invoke the general click handler
        if (OnItemClick.HasDelegate)
        {
            await OnItemClick.InvokeAsync(item);
        }
    }

    private Appearance GetButtonAppearance(MenuBarItem item)
    {
        return item.Selected ? SelectedItemAppearance : ItemAppearance;
    }

    private string GetItemCssClass(MenuBarItem item)
    {
        var classes = new List<string> { "fluentcx-menubar-item" };

        if (!string.IsNullOrEmpty(ItemCssClass))
            classes.Add(ItemCssClass);

        if (!string.IsNullOrEmpty(item.CssClass))
            classes.Add(item.CssClass);

        if (item.Selected)
            classes.Add("selected");

        if (item.Disabled)
            classes.Add("disabled");

        if (item.HasChildren)
            classes.Add("has-children");

        return string.Join(" ", classes);
    }

}