using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a path bar.
/// </summary>
public partial class FluentCxPathBar
    : FluentComponentBase, IAsyncDisposable
{
    #region Fields

    /// <summary>
    /// Represents the home icon.
    /// </summary>
    /// <remarks>
    /// Used when the <see cref="DeviceInfo"/> is null.
    /// </remarks>
    private static readonly Icon _homeIcon = new Size24.Home();

    /// <summary>
    /// Represents the desktop icon.
    /// </summary>
    /// <remarks>
    /// Used when the <see cref="DeviceInfo"/> is not null and if the device is a desktop.
    /// </remarks>
    private static readonly Icon _desktopIcon = new Size24.Desktop();

    /// <summary>
    /// Represents the tablet icon.
    /// </summary>
    /// <remarks>
    /// Used when the <see cref="DeviceInfo"/> is not null and if the device is a tablet.
    /// </remarks>
    private static readonly Icon _tabletIcon = new Size24.Tablet();

    /// <summary>
    /// Represents the desktop icon.
    /// </summary>
    /// <remarks>
    /// Used when the <see cref="DeviceInfo"/> is not null and if the device is a smartphone.
    /// </remarks>
    private static readonly Icon _phoneIcon = new Size24.Phone();

    /// <summary>
    /// Represents the width of the overflow button.
    /// </summary>
    /// <remarks>
    /// This value is fixed because the button has a fixed width of 32px.
    /// </remarks>
    private const int OverflowButtonWidth = 32;

    /// <summary>
    /// Represents the overflow items.
    /// </summary>
    private readonly List<IPathBarItem> _overflowItems = [];

    /// <summary>
    /// Represents the visible items.
    /// </summary>
    private readonly List<IPathBarItem> _visibleItems = [];

    /// <summary>
    /// Represents all items.
    /// </summary>
    private readonly List<IPathBarItem> _allItems = [];

    /// <summary>
    /// Represents the JavaScript module for the component.
    /// </summary>
    private const string JavascriptModule = "./_content/FluentUI.Blazor.Community.Components/Components/PathBar/FluentCxPathBar.razor.js";

    /// <summary>
    /// Represents the JavaScript module reference.
    /// </summary>
    private IJSObjectReference? _jsModule;

    /// <summary>
    /// Represents the dot net object reference.
    /// </summary>
    private readonly DotNetObjectReference<FluentCxPathBar> _dotNetRef;

    /// <summary>
    /// Represents a value indicating if the path has changed.
    /// </summary>
    private bool _hasPathChanged;

    /// <summary>
    /// Represents the cached values.
    /// </summary>
    private readonly Dictionary<string, double> _cachedSizes = [];

    /// <summary>
    /// Represents the last container width.
    /// </summary>
    private double _lastContainerWidth;

    /// <summary>
    /// Represents the number of elements to measure.
    /// </summary>
    private int _measuredCount;

    /// <summary>
    /// Represents the current container width.
    /// </summary>
    private double _containerWidth;

    /// <summary>
    /// Represents the mutation observer values.
    /// </summary>
    private static readonly string[] _mutationValue = ["class", "style"];

    /// <summary>
    /// Represents a value indicating if the items size should be refreshed. 
    /// </summary>
    private bool _refreshItemsSize;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxPathBar"/> class.
    /// </summary>
    public FluentCxPathBar()
    {
        Id = Identifier.NewId();
        _dotNetRef = DotNetObjectReference.Create(this);
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets the css class which is used internally.
    /// </summary>
    private string? InternalCss => new CssBuilder(Class)
        .AddClass("fluentcx-path-bar")
        .Build();

    /// <summary>
    /// Gets or sets the javaScript runtime.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the root of the path, with all possible paths.
    /// </summary>
    [Parameter]
    public IPathBarItem? Root { get; set; }

    /// <summary>
    /// Gets or sets the path.
    /// </summary>
    [Parameter]
    public string? Path { get; set; }

    /// <summary>
    /// Gets or sets the callback when the path changed.
    /// </summary>
    [Parameter]
    public EventCallback<string?> PathChanged { get; set; }

    /// <summary>
    /// Gets or sets the state for the information of the current used device.
    /// </summary>
    [Inject]
    private DeviceInfoState DeviceInfoState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the percentage threshold to invalidate the path.
    /// </summary>
    /// <remarks>This value is between 0.0 and 1.0</remarks>
    [Parameter]
    public double InvalidationThresholdPercentage { get; set; } = 0.1;

    /// <summary>
    /// Gets or sets the pixel threshold to invalidate the path.
    /// </summary>
    [Parameter]
    public double InvalidationThresholdPixels { get; set; } = 30;

    /// <summary>
    /// Gets or sets the mutation observer configuration.
    /// </summary>
    [Parameter]
    public Dictionary<string, object> MutationObserverConfig { get; set; } = new()
    {
        { "attributes", true },
        { "childList", true },
        { "subtree", false },
        { "attributeFilter", _mutationValue }
    };

    /// <summary>
    /// Gets or sets the event callback which is invoked when the cache is invalidated.
    /// </summary>
    [Parameter]
    public EventCallback OnCacheInvalidated { get; set; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Occurs when the path is selected by <see cref="FluentCxPathBarItem"/>.
    /// </summary>
    /// <returns>Returns a task which changes the path when completed.</returns>
    private async Task OnPathSelectedAsync()
    {
        await UpdatePathAsync(Root);
    }

    /// <summary>
    /// Gets the home icon.
    /// </summary>
    /// <returns>Returns the home icon.</returns>
    private Icon GetHomeIcon()
    {
        if (DeviceInfoState is null || DeviceInfoState.DeviceInfo is null)
        {
            return _homeIcon;
        }

        var deviceInfo = DeviceInfoState.DeviceInfo;

        if (deviceInfo.IsTablet)
        {
            return _tabletIcon;
        }

        return DeviceInfoState.DeviceInfo.IsMobile ? _phoneIcon : _desktopIcon;
    }

    /// <summary>
    /// Updates the path and invokes the <see cref="PathChanged"/> callback.
    /// </summary>
    /// <param name="value"></param>
    internal async Task UpdatePathAsync(IPathBarItem? value)
    {
        Path = PathBarItemBuilder.GetPath(value);

        if (PathChanged.HasDelegate)
        {
            await PathChanged.InvokeAsync(Path);
        }

        InvalidatePath();
        await RefreshPathBarAsync();
    }

    /// <summary>
    /// Invalidates the size of the item with the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Identifier of the item.</param>
    internal void InvalidateItemSize(string id)
    {
        var realId = PathBarItemBuilder.GetIdentifier(id);

        if (!string.IsNullOrEmpty(realId) &&
            _cachedSizes.ContainsKey(realId))
        {
            _cachedSizes[realId] = 0;
        }
    }

    /// <summary>
    /// Clears the cached sizes of the specified <paramref name="values"/>.
    /// </summary>
    /// <param name="values">Values to remove from the cached sizes.</param>
    internal void ClearItems(IEnumerable<string?>? values)
    {
        if (values is null || !values.Any())
        {
            return;
        }

        foreach (var id in values.Where(x => !string.IsNullOrEmpty(x)))
        {
            _cachedSizes.Remove(id!);
        }
    }

    /// <summary>
    /// Invalidate the path and recalculates the visible and overflow items.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void InvalidatePath()
    {
        var segments = string.IsNullOrWhiteSpace(Path) ? [] : Path!.Split(System.IO.Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var lastItemCount = _allItems.Count;

        _allItems.Clear();
        _allItems.AddRange(PathBarItemBuilder.GetAllParts(Root, segments));

        _refreshItemsSize = lastItemCount != _allItems.Count;

        if (_allItems.Any(x => string.IsNullOrEmpty(x.Id)))
        {
            throw new InvalidOperationException("All PathBarItems must have an Id.");
        }

        if (_lastContainerWidth > 0)
        {
            _lastContainerWidth = 0;
        }

        _measuredCount = 0;
    }

    /// <summary>
    /// Invalidates the cache and invokes the <see cref="OnCacheInvalidated"/> callback.
    /// </summary>
    /// <returns>Returns a task which invokes the callback when completed.</returns>
    private async Task OnCacheInvalidatedAsync()
    {
        if (OnCacheInvalidated.HasDelegate)
        {
            await OnCacheInvalidated.InvokeAsync();
        }
    }

    /// <summary>
    /// Refreshes the path bar and recalculates the visible and overflow items.
    /// </summary>
    /// <returns>Returns a task which computes the visible and overflow items when completed.</returns>
    private async Task RefreshPathBarAsync()
    {
        if (_jsModule is null ||
            _allItems.Count == 0)
        {
            _overflowItems.Clear();
            _visibleItems.Clear();
            return;
        }

        if (_containerWidth == 0)
        {
            _containerWidth = await _jsModule.InvokeAsync<double>("getWidth", $"fluentcx-path-bar-container-{Id}");
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
        await UpdateCacheSizesAsync(_measuredCount);

        _measuredCount = _allItems.Count;

        _visibleItems.Clear();
        _overflowItems.Clear();

        var overflow = _cachedSizes.Values.Sum() + OverflowButtonWidth > _containerWidth;
        var used = 0.0;
        var reserve = overflow ? OverflowButtonWidth : 0;

        for (var i = _allItems.Count - 1; i >= 0; i--)
        {
            var itemSize = _cachedSizes[_allItems[i].Id!];

            if (used + itemSize + reserve <= _containerWidth)
            {
                _visibleItems.Insert(0, _allItems[i]);
                used += itemSize;
            }
            else
            {
                _overflowItems.Insert(0, _allItems[i]);
            }
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Updates the cache sizes starting from the specified index.
    /// </summary>
    /// <param name="startIndex">Index of the starting cache.</param>
    /// <returns>Returns <see langword="true"/> when the cache is updated,
    /// <see langword="false" /> otherwise.</returns>
    private async Task<bool> UpdateCacheSizesAsync(int startIndex)
    {
        if (_jsModule is null)
        {
            return false;
        }

        var hasChanges = false;

        for (var i = startIndex; i < _allItems.Count; i++)
        {
            var id = _allItems[i].Id;

            if (!_cachedSizes.TryGetValue(id!, out var d) || d == 0)
            {
                _cachedSizes[id!] = await _jsModule.InvokeAsync<double>("getWidth", id);
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
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptModule);
            await _jsModule.InvokeVoidAsync("initialize", Id, _dotNetRef, MutationObserverConfig);
            await RefreshPathBarAsync();
        }

        // When a new segment is added, we need to measure it, and refresh the bar.
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

    /// <summary>
    /// Occurs when the component is resized.
    /// </summary>
    /// <param name="width">New size of the component.</param>
    /// <returns>Returns a task which refresh the component.</returns>
    [JSInvokable("OnResize")]
    public async Task OnResizeAsync(double width)
    {
        _containerWidth = width;
        await RefreshPathBarAsync();
    }

    /// <summary>
    /// Occurs when a mutation has changed.
    /// </summary>
    /// <returns>Returns a task which invalidate the cache.</returns>
    [JSInvokable("OnMutated")]
    public async Task OnMutatedAsync()
    {
        _measuredCount = 0;
        await OnCacheInvalidatedAsync();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("dispose", Id);
                await _jsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
        }

        _dotNetRef.Dispose();

        GC.SuppressFinalize(this);
    }

    #endregion Methods
}
