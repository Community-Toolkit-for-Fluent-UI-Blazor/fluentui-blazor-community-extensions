using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a path bar.
/// </summary>
public partial class FluentCxPathBar
    : FluentComponentBase
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
    /// Gets or sets the maximum number items visible.
    /// </summary>
    [Parameter]
    public int? MaxVisibleItems { get; set; }

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
    private IEnumerable<IPathBarItem> Find(string[] segments)
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
}
