using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a centralized registry for managing active FluentMenu instances within the application.
/// </summary>
/// <remarks>This static class allows for the registration, unregistration, and bulk closing of all tracked
/// FluentMenu components. It is intended for internal use to coordinate menu state across the application.</remarks>
internal static class MenuRegistry
{
    /// <summary>
    /// Represents the collection of all registered FluentMenu instances.
    /// </summary>
    /// <remarks>This list is intended to store references to FluentMenu components that are currently
    /// registered or active within the application. Modifying this collection directly may affect menu registration and
    /// behavior.</remarks>
    public static readonly List<FluentMenu> _menus = [];

    /// <summary>
    /// Registers the specified menu instance if it has not already been registered.
    /// </summary>
    /// <param name="menu">The menu instance to register. Cannot be null.</param>
    public static void Register(FluentMenu menu)
    {
        if (!_menus.Contains(menu))
        {
            _menus.Add(menu);
        }
    }

    /// <summary>
    /// Unregisters the specified menu from the collection of tracked menus.
    /// </summary>
    /// <param name="menu">The menu instance to remove from the collection. Cannot be null.</param>
    public static void Unregister(FluentMenu menu)
    {
        if (_menus.Contains(menu))
        {
            _menus.Remove(menu);
        }
    }

    /// <summary>
    /// Asynchronously closes all open menus managed by the current context.
    /// </summary>
    /// <remarks>This method initiates the close operation for each menu in the collection and waits for all
    /// close operations to complete. If no menus are open, the method completes immediately.</remarks>
    /// <returns>A task that represents the asynchronous close operation. The task completes when all menus have been closed.</returns>
    public static async Task CloseAllAsync()
    {
        foreach (var menu in _menus)
        {
            await menu.CloseMenuAsync();
        }
    }
}
