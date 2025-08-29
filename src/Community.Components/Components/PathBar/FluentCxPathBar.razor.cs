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
    : FluentComponentBase, IDisposable
{
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
    /// Represents a flag indicating whether the path has changed.
    /// </summary>
    private bool _pathChanged = true;

    /// <summary>
    /// Represents the overflow items.
    /// </summary>
    private readonly List<IPathBarItem> _overflowItems = [];

    /// <summary>
    /// Represents the visible items.
    /// </summary>
    private readonly List<IPathBarItem> _visibleItems = [];

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
    /// Initializes a new instance of the <see cref="FluentCxPathBar"/> class.
    /// </summary>
    public FluentCxPathBar()
    {
        Id = Identifier.NewId();
        _dotNetRef = DotNetObjectReference.Create(this);
    }

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
    /// Gets or sets the maximum length of the path before it will be truncated.
    /// </summary>
    [Parameter]
    public int MaxLengthBeforeTruncate { get; set; } = 10;

    /// <summary>
    /// Occurs when the <paramref name="path"/> has changed.
    /// </summary>
    /// <param name="path">Current path.</param>
    /// <returns>Returns a task which raise the <see cref="PathChanged"/> callback when completed.</returns>
    private async Task OnPathChangedAsync(string? path)
    {
        if (PathChanged.HasDelegate)
        {
            await PathChanged.InvokeAsync(path);
        }
    }

    /// <summary>
    /// Occurs when the path is selected by <see cref="FluentCxPathBarItem"/>.
    /// </summary>
    /// <param name="item">Represents the selected item.</param>
    /// <returns>Returns a task which changes the path when completed.</returns>
    private async Task OnPathSelectedAsync(IPathBarItem item)
    {
        _pathChanged = true;
        Path = PathBarItem.GetPath(item);
        await OnPathChangedAsync(Path);
        StateHasChanged();
    }

    /// <summary>
    /// Occurs when the path is selected by <see cref="FluentCxPathBarItemMenu"/>.
    /// </summary>
    /// <param name="path">Selected path.</param>
    internal void SetPath(string? path)
    {
        _pathChanged = true;
        Path = path;
        InvokeAsync(() => OnPathChangedAsync(path));
        StateHasChanged();
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
    /// Find all path bar items which are represented by the <paramref name="segments"/>.
    /// </summary>
    /// <param name="segments">Array of string which represents the full path.</param>
    /// <returns>Returns a list of the <see cref="IPathBarItem"/>.</returns>
    private List<IPathBarItem> Find(string[] segments)
    {
        if (segments.Length == 0)
        {
            return [];
        }

        var items = Root?.Items;

        if (items is null || !items.Any())
        {
            return [];
        }

        List<IPathBarItem> result = [];

        foreach (var segment in segments[1..])
        {
            var item = Find(items, segment);

            if (item is not null)
            {
                result.Add(item);
                items = item.Items;
            }
        }

        return result;

        static IPathBarItem? Find(IEnumerable<IPathBarItem> items, string segment)
        {
            foreach (var item in items)
            {
                if (string.Equals(item.Label, segment, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private Task OnHandleOverflowKeyDownAsync(FluentKeyCodeEventArgs e)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private Task OnHandleMenuContainerKeyDownAsync(FluentKeyCodeEventArgs e)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Updates the <paramref name="toUpdate"/> list with the <paramref name="updated"/> list.
    /// </summary>
    /// <param name="toUpdate">List to update.</param>
    /// <param name="updated">Update items.</param>
    private static void Update(List<IPathBarItem> toUpdate, IEnumerable<IPathBarItem> updated)
    {
        toUpdate.Clear();
        toUpdate.AddRange(updated);
    }

    private static IEnumerable<IPathBarItem> GetAllItems(
        IEnumerable<IPathBarItem>? items,
        IEnumerable<string> ids)
    {
        if (items is null || !items.Any() || ids is null || !ids.Any())
        {
            yield break;
        }

        foreach (var id in ids)
        {
            var item = GetItem(items, id);

            if (item is not null)
            {
                yield return item;
            }
        }
    }

    private static IPathBarItem? GetItem(IEnumerable<IPathBarItem> items, string id)
    {
        foreach (var item in items)
        {
            if (string.Equals(item.Id, id, StringComparison.OrdinalIgnoreCase))
            {
                return item;
            }

            var node = GetItem(item.Items, id);

            if (node is not null)
            {
                return node;
            }
        }

        return null;
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptModule);
            await _jsModule.InvokeVoidAsync("initialize", Id, _dotNetRef, MaxLengthBeforeTruncate);
        }

        if (_pathChanged &&
            _jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("refreshPathBar", Id);
        }
    }

    [JSInvokable("updateOverflowAndVisible")]
    public void UpdateOverflowAndVisible(RefreshPathBarResult? result)
    {
        if (result is not null)
        {
            Update(_overflowItems, GetAllItems(Root?.Items, result.OverflowItems));
            Update(_visibleItems, GetAllItems(Root?.Items, result.VisibleItems));
            _pathChanged = false;
            StateHasChanged();
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _dotNetRef.Dispose();

        GC.SuppressFinalize(this);
    }
}
