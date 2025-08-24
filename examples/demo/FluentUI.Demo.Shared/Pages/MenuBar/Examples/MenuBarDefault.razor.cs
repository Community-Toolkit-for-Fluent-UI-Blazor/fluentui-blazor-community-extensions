using FluentUI.Blazor.Community.Components;
using Microsoft.AspNetCore.Components;
using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons;

namespace FluentUI.Demo.Shared.Pages.MenuBar.Examples;

public partial class MenuBarDefault
{
    private string _statusMessage = string.Empty;
    private string _searchText = string.Empty;

    private readonly List<MenuBarItem> _basicMenuItems = new()
    {
        MenuBarItem.Create("Home", new Icons.Regular.Size20.Home(), EventCallback<MenuBarItem>.Empty),
        MenuBarItem.Create("Products", null, EventCallback<MenuBarItem>.Empty),
        MenuBarItem.Create("Services", null, EventCallback<MenuBarItem>.Empty),
        MenuBarItem.Create("About", null, EventCallback<MenuBarItem>.Empty),
        MenuBarItem.Create("Contact", null, EventCallback<MenuBarItem>.Empty)
    };

    private readonly List<MenuBarItem> _dropdownMenuItems = new()
    {
        new MenuBarItem
        {
            Text = "File",
            Icon = new Icons.Regular.Size20.Document(),
            Children = new List<MenuBarItem>
            {
                MenuBarItem.Create("New", null, EventCallback<MenuBarItem>.Empty),
                MenuBarItem.Create("Open", null, EventCallback<MenuBarItem>.Empty),
                MenuBarItem.Create("Save", null, EventCallback<MenuBarItem>.Empty),
                new MenuBarItem { Text = "---" }, // Separator
                MenuBarItem.Create("Exit", null, EventCallback<MenuBarItem>.Empty)
            }
        },
        new MenuBarItem
        {
            Text = "Edit",
            Icon = null,
            Children = new List<MenuBarItem>
            {
                MenuBarItem.Create("Cut", null, EventCallback<MenuBarItem>.Empty),
                MenuBarItem.Create("Copy", null, EventCallback<MenuBarItem>.Empty),
                MenuBarItem.Create("Paste", null, EventCallback<MenuBarItem>.Empty),
                new MenuBarItem { Text = "---" }, // Separator
                MenuBarItem.Create("Find", null, EventCallback<MenuBarItem>.Empty),
                MenuBarItem.Create("Replace", null, EventCallback<MenuBarItem>.Empty)
            }
        },
        new MenuBarItem
        {
            Text = "View",
            Icon = null,
            Children = new List<MenuBarItem>
            {
                MenuBarItem.Create("Zoom In", null, EventCallback<MenuBarItem>.Empty),
                MenuBarItem.Create("Zoom Out", null, EventCallback<MenuBarItem>.Empty),
                MenuBarItem.Create("Full Screen", null, EventCallback<MenuBarItem>.Empty)
            }
        },
        MenuBarItem.Create("Help", null, EventCallback<MenuBarItem>.Empty)
    };

    private readonly List<MenuBarItem> _appMenuItems = new()
    {
        MenuBarItem.CreateLink("Dashboard", "/", null),
        MenuBarItem.CreateLink("Analytics", "/analytics", null),
        MenuBarItem.CreateLink("Reports", "/reports", null),
        new MenuBarItem
        {
            Text = "Settings",
            Icon = null,
            Children = new List<MenuBarItem>
            {
                MenuBarItem.Create("Preferences", null, EventCallback<MenuBarItem>.Empty),
                MenuBarItem.Create("Account", null, EventCallback<MenuBarItem>.Empty),
                MenuBarItem.Create("Security", null, EventCallback<MenuBarItem>.Empty)
            }
        }
    };

    private List<MenuBarItem> _accentMenuItems = new();
    private List<MenuBarItem> _outlineMenuItems = new();

    protected override void OnInitialized()
    {
        // Create accent menu items with selection
        _accentMenuItems = new List<MenuBarItem>
        {
            new MenuBarItem { Text = "Overview", Icon = null, Selected = true },
            MenuBarItem.Create("Features", null, EventCallback<MenuBarItem>.Empty),
            MenuBarItem.Create("Pricing", null, EventCallback<MenuBarItem>.Empty),
            MenuBarItem.Create("Support", null, EventCallback<MenuBarItem>.Empty)
        };

        // Create outline menu items
        _outlineMenuItems = new List<MenuBarItem>
        {
            MenuBarItem.Create("All", null, EventCallback<MenuBarItem>.Empty),
            new() { Text = "Active", Icon = null, Selected = true },
            MenuBarItem.Create("Completed", null, EventCallback<MenuBarItem>.Empty),
            new() { Text = "Archived", Icon = null, Disabled = true }
        };

        // Add click handlers
        foreach (var item in _basicMenuItems.Concat(_dropdownMenuItems).Concat(_appMenuItems)
                     .Concat(_accentMenuItems).Concat(_outlineMenuItems))
        {
            if (item.OnClick.HasDelegate == false)
            {
                item.OnClick = EventCallback.Factory.Create<MenuBarItem>(this, HandleMenuClick);
            }

            // Add handlers to children too
            if (item.Children != null)
            {
                foreach (var child in item.Children.Where(c => !string.IsNullOrEmpty(c.Text) && c.Text != "---"))
                {
                    if (child.OnClick.HasDelegate == false)
                    {
                        child.OnClick = EventCallback.Factory.Create<MenuBarItem>(this, HandleMenuClick);
                    }
                }
            }
        }
    }

    private void OnMenuItemClick(MenuBarItem item)
    {
        if (!string.IsNullOrEmpty(item.Href))
        {
            _statusMessage = $"Navigating to: {item.Href}";
        }
        else
        {
            HandleMenuClick(item);
        }
    }

    private void HandleMenuClick(MenuBarItem item)
    {
        if (string.IsNullOrEmpty(item.Text) || item.Text == "---")
            return;

        _statusMessage = $"Clicked: {item.Text}" +
                        (!string.IsNullOrEmpty(item.Icon?.Name) ? $" (Icon: {item.Icon})" : "");

        StateHasChanged();
    }
}
