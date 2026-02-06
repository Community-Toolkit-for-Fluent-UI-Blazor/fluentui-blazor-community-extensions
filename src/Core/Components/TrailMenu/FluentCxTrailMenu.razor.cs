using FluentUI.Blazor.Community.Components.TrailMenu;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a menu component that adheres to Fluent UI design principles, providing a customizable interface for user
/// interactions within applications that use the Fluent UI framework.
/// </summary>
/// <remarks>Use this class to create menus that are consistent with Fluent UI standards. The menu's behavior and
/// available options are determined by the provided library configuration. Ensure that the configuration is valid to
/// prevent runtime errors.</remarks>
public partial class FluentCxTrailMenu
    : FluentComponentBase
{
    /// <summary>
    /// Contains the collection of menu items that are placed in the overflow area when there is insufficient space in
    /// the main menu.
    /// </summary>
    private readonly List<ITrailMenuItem> _overflowItems = [];

    /// <summary>
    /// Gets the list of menu items that are currently visible in the trail menu.
    /// </summary>
    private readonly List<ITrailMenuItem> _visibleItems = [];

    /// <summary>
    /// Contains all trail menu items available in the application.
    /// </summary>
    private readonly List<ITrailMenuItem> _allItems = [];

    /// <summary>
    /// Stores the cache instance used for trail menu data within the class.
    /// </summary>
    private readonly TrailMenuCache _cache = new();

    /// <summary>
    /// Provides access to the handler that manages mutations related to trail menu operations.
    /// </summary>
    private readonly TrailMenuMutationHandler _mutations = new();

    /// <summary>
    /// Holds the interop instance used for TrailMenu JavaScript interactions.
    /// </summary>
    private TrailMenuJs? _interop;

    /// <summary>
    /// Indicates whether the path has changed since the last render, which may require invalidating the current path and refreshing the menu layout.
    /// </summary>
    private bool _hasPathChanged;

    /// <summary>
    /// Indicates whether the sizes of the menu items need to be refreshed, which can occur when the number of items changes or when the container size changes significantly,
    ///  necessitating a recalculation of which items are visible and which are in the overflow menu.
    /// </summary>
    private bool _refreshItemsSize;

    /// <summary>
    /// Indicates the width of the container during the last measurement, used to determine if a significant
    ///  change in width has occurred that would require invalidating the cache and refreshing the menu layout.
    /// </summary>
    private double _lastContainerWidth;

    /// <summary>
    /// Indicates the current width of the container, which is used to calculate how many menu items
    ///  can be displayed in the visible area and how many need to be moved to the overflow menu.
    /// </summary>
    private double _containerWidth;

    /// <summary>
    /// Indicates the number of menu items that have been measured for their size.
    /// </summary>
    private int _measuredCount;

    /// <summary>
    /// Represents the list of mutation types that the MutationObserver should listen for when monitoring
    ///  changes to the menu's DOM elements. This configuration is used to determine when to trigger updates
    ///  to the menu layout based on changes in attributes or child elements.
    /// </summary>
    private static readonly string[] _mutationValue = ["class", "style"];

    /// <summary>
    /// Initializes a new instance of the FluentCxTrailMenu class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the library that determine the behavior and available options for the menu.
    /// Cannot be null.</param>
    public FluentCxTrailMenu(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets the css for the trail menu component.
    /// </summary>
    private string? InternalCss => DefaultClassBuilder
        .AddClass("fluentcx-trail-menu")
        .Build();

    /// <summary>
    /// Gets or sets the state information of the device.
    /// </summary>
    [Inject]
    private DeviceInfoState DeviceInfoState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the root menu item for the trail menu.
    /// </summary>
    /// <remarks>This property specifies the top-level item in the trail menu structure. If set to <see
    /// langword="null"/>, the menu will not display any items.</remarks>
    [Parameter]
    public ITrailMenuItem? Root { get; set; }

    /// <summary>
    /// Gets or sets the path representing the current selection in the trail menu. The path is a string that encodes the hierarchy of selected items, typically using a delimiter to separate different levels of the menu.
    ///  Changes to this property will trigger an update to the menu layout to reflect the new selection.
    /// </summary>
    [Parameter]
    public string? Path { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the path changes, providing the new path as a string.
    /// </summary>
    /// <remarks>Use this event to respond to dynamic updates of the path within the component. The callback
    /// receives the updated path value, allowing the application to react to navigation or state changes as
    /// needed.</remarks>
    [Parameter]
    public EventCallback<string?> PathChanged { get; set; }

    /// <summary>
    /// Gets or sets the percentage threshold for invalidating the cache based on container width changes.
    ///  If the container width changes by more than this percentage,
    ///  the cache will be invalidated and the menu layout will be refreshed.
    /// </summary>
    [Parameter]
    public double InvalidationThresholdPercentage { get; set; } = 0.1;

    /// <summary>
    /// Gets or sets the minimum distance, in pixels, that must be exceeded before the component's visual state is
    /// updated.
    /// </summary>
    /// <remarks>Adjusting this value can help optimize rendering performance by reducing unnecessary visual
    /// updates. Set a higher value to decrease update frequency, which may improve performance in scenarios with
    /// frequent minor changes.</remarks>
    [Parameter]
    public double InvalidationThresholdPixels { get; set; } = 30;

    /// <summary>
    /// Gets or sets the configuration options for the MutationObserver, which determine the types of DOM mutations to
    /// observe.
    /// </summary>
    /// <remarks>The configuration is specified as a dictionary where keys represent mutation types, such as
    /// "attributes", "childList", and "subtree". By default, attribute changes and child node additions or removals are
    /// observed, while subtree observation is disabled. The "attributeFilter" key can be used to specify which
    /// attributes to observe when "attributes" is set to <see langword="true"/>.</remarks>
    [Parameter]
    public Dictionary<string, object> MutationObserverConfig { get; set; } = new()
    {
        { "attributes", true },
        { "childList", true },
        { "subtree", false },
        { "attributeFilter", _mutationValue }
    };

    /// <summary>
    /// Gets or sets the callback that is invoked when the cache is invalidated.
    /// </summary>
    /// <remarks>Use this event to notify dependent components or services that cached data has changed,
    /// allowing them to refresh or update their state as needed.</remarks>
    [Parameter]
    public EventCallback OnCacheInvalidated { get; set; }

    /// <summary>
    /// Handles the asynchronous resizing of the container by updating its width and performing any necessary mutation
    /// operations.
    /// </summary>
    /// <remarks>This method updates the internal container width and triggers any associated resize logic. It
    /// should be called whenever the container's size changes to ensure proper layout and state management.</remarks>
    /// <param name="width">The new width of the container, specified in pixels. Must be a positive value.</param>
    /// <returns>A task that represents the asynchronous operation of resizing the container.</returns>
    internal async Task OnResizeAsync(double width)
    {
        _containerWidth = width;
        await _mutations.HandleResizeAsync(RefreshPathBarAsync);
    }

    /// <summary>
    /// Resets the measured count and asynchronously invalidates the cache to reflect recent mutations.
    /// </summary>
    /// <remarks>Call this method after performing operations that may affect cached data to ensure the
    /// internal state is updated and the cache is properly invalidated. This method should be used when consistency
    /// between the measured count and cached data is required.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task OnMutatedAsync()
    {
        _measuredCount = 0;
        await OnCacheInvalidatedAsync();
    }

    /// <summary>
    /// Returns the appropriate home icon based on the current device type.
    /// </summary>
    /// <remarks>This method checks the current device information state to determine which icon to return. If
    /// device information is not available, a default home icon is returned.</remarks>
    /// <returns>An <see cref="Icon"/> representing the home icon. Returns the mobile home icon if the device is mobile;
    /// otherwise, returns the desktop home icon. If device information is unavailable, returns a default home icon.</returns>
    private Icon GetHomeIcon()
    {
        if (DeviceInfoState is null || DeviceInfoState.DeviceInfo is null)
        {
            return TrailMenuIcons.HomeIcon;
        }

        var deviceInfo = DeviceInfoState.DeviceInfo;

        return deviceInfo.IsMobile ? TrailMenuIcons.PhoneIcon : TrailMenuIcons.DesktopIcon;
    }

    /// <summary>
    /// Updates the current path using the specified trail menu item and triggers any associated path change events
    /// asynchronously.
    /// </summary>
    /// <remarks>If the <see langword="PathChanged"/> delegate has subscribers, it is invoked with the updated
    /// path. The path bar is refreshed after the path is updated.</remarks>
    /// <param name="value">The trail menu item used to determine the new path. Can be null to indicate no selection.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task UpdateTrailAsync(ITrailMenuItem? value)
    {
        Path = TrailMenuUtils.GetPath(value);

        if (PathChanged.HasDelegate)
        {
            await PathChanged.InvokeAsync(Path);
        }

        InvalidatePath();
        await RefreshPathBarAsync();
    }

    /// <summary>
    /// Invalidates the cached size for the item identified by the specified identifier.
    /// </summary>
    /// <remarks>If the provided identifier is valid, the method removes the associated size from the cache.
    /// Subsequent requests for the item's size will result in recalculation.</remarks>
    /// <param name="id">The identifier of the item whose cached size should be invalidated. This value must not be null or empty.</param>
    internal void InvalidateItemSize(string id)
    {
        var realId = TrailMenuUtils.GetIdentifier(id);

        if (!string.IsNullOrEmpty(realId))
        {
            _cache.Invalidate(realId);
        }
    }

    /// <summary>
    /// Removes the specified items from the cache if they are present.
    /// </summary>
    /// <remarks>This method does not throw exceptions if <paramref name="values"/> is <see langword="null"/>;
    /// it simply returns without modifying the cache.</remarks>
    /// <param name="values">An enumerable collection of string values representing the items to remove from the cache. If <paramref
    /// name="values"/> is <see langword="null"/>, no items are removed.</param>
    internal void ClearItems(IEnumerable<string?>? values)
    {
        if (values is null)
        {
            return;
        }

        _cache.Clear(values);
    }

    /// <summary>
    /// Invalidates the current path by clearing existing items and repopulating them based on the parsed segments of
    /// the path.
    /// </summary>
    /// <remarks>This method updates the internal collection of items based on the specified path and checks
    /// for the validity of item identifiers. It also resets the measured count and container width if
    /// applicable.</remarks>
    /// <exception cref="InvalidOperationException">Thrown if any TrailMenu items do not have an Id.</exception>
    private void InvalidatePath()
    {
        var segments = TrailMenuUtils.ParseSegments(Path);
        var lastCount = _allItems.Count;

        _allItems.Clear();
        _allItems.AddRange(TrailMenuUtils.GetAllParts(Root, segments));

        _refreshItemsSize = lastCount != _allItems.Count;

        if (_allItems.Any(x => string.IsNullOrEmpty(x.Id)))
        {
            throw new InvalidOperationException("All TrailMenu Items must have an Id.");
        }

        if (_lastContainerWidth > 0)
        {
            _lastContainerWidth = 0;
        }

        _measuredCount = 0;
    }

    /// <summary>
    /// Invokes the OnCacheInvalidated event asynchronously to notify subscribers when the cache is invalidated.
    /// </summary>
    /// <remarks>This method checks whether the OnCacheInvalidated event has any subscribers before invoking
    /// it. Call this method to trigger cache invalidation notifications for event handlers.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnCacheInvalidatedAsync()
    {
        if (OnCacheInvalidated.HasDelegate)
        {
            await OnCacheInvalidated.InvokeAsync();
        }
    }

    /// <summary>
    /// Asynchronously refreshes the path bar layout to update visible and overflow items based on the current container
    /// width and cached item sizes.
    /// </summary>
    /// <remarks>This method recalculates the arrangement of path bar items whenever the container width
    /// changes or the item cache is invalidated. If the interop object is not initialized or there are no items to
    /// display, the visible and overflow item collections are cleared. The method ensures the UI reflects the latest
    /// layout by invoking a state change.</remarks>
    /// <returns>A task that represents the asynchronous refresh operation.</returns>
    private async Task RefreshPathBarAsync()
    {
        if (_interop is null || _allItems.Count == 0)
        {
            _overflowItems.Clear();
            _visibleItems.Clear();
            await InvokeAsync(StateHasChanged);
            return;
        }

        if (_containerWidth == 0)
        {
            _containerWidth = await _interop.GetWidthAsync($"fluentcx-trail-menu-container-{Id}");
        }

        if (_lastContainerWidth > 0)
        {
            var delta = Math.Abs(_lastContainerWidth - _containerWidth);

            if (delta > InvalidationThresholdPixels ||
                delta / _lastContainerWidth > InvalidationThresholdPercentage)
            {
                _measuredCount = 0;
                await OnCacheInvalidatedAsync();
            }
        }

        _lastContainerWidth = _containerWidth;

        if (await UpdateCacheSizesAsync(_measuredCount))
        {
            _measuredCount = _allItems.Count;
        }

        TrailMenuLayoutEngine.Compute(_allItems, _cache, _containerWidth, _visibleItems, _overflowItems);

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Asynchronously updates cached width values for items, starting from the specified index.
    /// </summary>
    /// <remarks>This method requires that the interop object is not null. It iterates through all items
    /// beginning at the specified index, retrieves their widths asynchronously if not already cached, and updates the
    /// cache accordingly.</remarks>
    /// <param name="startIndex">The zero-based index of the first item to update in the cache.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if any cache
    /// entries were updated; otherwise, <see langword="false"/>.</returns>
    private async Task<bool> UpdateCacheSizesAsync(int startIndex)
    {
        if (_interop is null)
        {
            return false;
        }

        var hasChanges = false;

        for (var i = startIndex; i < _allItems.Count; i++)
        {
            var id = _allItems[i].Id;

            if (string.IsNullOrEmpty(id))
            {
                continue;
            }

            if (!_cache.TryGet(id, out var size) || size == 0)
            {
                var width = await _interop.GetWidthAsync($"trail-menu-button-{id}");
                _cache.Set(id, width);
                hasChanges = true;
            }
        }

        return hasChanges;
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _interop ??= new TrailMenuJs(JSModule, this);
            await _interop.InitializeAsync(Id, MutationObserverConfig);
            await RefreshPathBarAsync();
            return;
        }

        if (_refreshItemsSize)
        {
            if (await UpdateCacheSizesAsync(0))
            {
                await RefreshPathBarAsync();
            }

            _refreshItemsSize = false;
        }
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_hasPathChanged)
        {
            InvalidatePath();
            await RefreshPathBarAsync();
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasPathChanged = parameters.HasValueChanged(nameof(Path), Path);

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        try
        {
            if (_interop is not null)
            {
                await _interop.DisposeAsync(Id);
            }
        }
        catch (JSDisconnectedException)
        {
        }

        GC.SuppressFinalize(this);
    }
}
