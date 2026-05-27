using System.Diagnostics.CodeAnalysis;
using FluentUI.Blazor.Community.Components.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component which detects the device where the app is running.
/// </summary>
public partial class FluentCxDeviceDetector : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = FluentCxConstants.JAVASCRIPT_ROOT + "DeviceDetector/FluentCxDeviceDetector.razor.js";
    private readonly DotNetObjectReference<FluentCxDeviceDetector> _dotNetHelper;
    private IJSObjectReference? _jsInstance;

    /// <summary>
    /// Initialize a new instance of the <see cref="FluentCxDeviceDetector"/> class.
    /// </summary>
    public FluentCxDeviceDetector(LibraryConfiguration configuration) : base(configuration)
    {
        _dotNetHelper = DotNetObjectReference.Create(this);
    }

    /// <summary>
    /// Gets or sets the ChildContent render fragment.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <summary>
    /// Gets the state which contains the information of the device.
    /// </summary>
    [Inject]
    public required DeviceInfoState State { get; set; }

    /// <summary>
    /// Gets or sets the callback to use when the device information is updated.
    /// </summary>
    [Parameter]
    public EventCallback<DeviceInfo> DeviceInfoUpdated { get; set; }

    /// <summary>
    /// Gets the information about the device.
    /// </summary>
    public DeviceInfo? DeviceInfo => State.DeviceInfo;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            var module = await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);
            _jsInstance = await JSModule.ObjectReference.InvokeAsync<IJSObjectReference>("FluentUI.Blazor.Community.DeviceDetector.Initialize", _dotNetHelper);
            State.DeviceInfo = await module.InvokeAsync<DeviceInfo>("FluentUI.Blazor.Community.DeviceDetector.GetDeviceInfo");

            if (DeviceInfoUpdated.HasDelegate)
            {
                await DeviceInfoUpdated.InvokeAsync(State.DeviceInfo);
            }
        }
    }

    /// <summary>
    /// Occurs when the orientation of the device changed.
    /// </summary>
    /// <param name="orientation">Value indicating the orientation of the device.</param>
    [JSInvokable]
    public async Task OrientationChanged(string orientation)
    {
        if (State.DeviceInfo is not null)
        {
            State.DeviceInfo.Orientation = Enum.Parse<DeviceOrientation>(orientation);

            if (DeviceInfoUpdated.HasDelegate)
            {
                await DeviceInfoUpdated.InvokeAsync(State.DeviceInfo);
            }
        }
    }

    /// <summary>
    /// Occurs when the orientation of the device changed.
    /// </summary>
    /// <param name="value">Value indicating the breakpoint of the device.</param>
    [JSInvokable]
    public async Task BreakpointChanged(string value)
    {
        if (State.DeviceInfo is not null &&
            Enum.TryParse(value, true, out DeviceBreakpoint db))
        {
            State.DeviceInfo.Breakpoint = db;

            if (DeviceInfoUpdated.HasDelegate)
            {
                await DeviceInfoUpdated.InvokeAsync(State.DeviceInfo);
            }
        }
    }

    /// <summary>
    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync" />
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    protected override async ValueTask DisposeAsync(IJSObjectReference jsModule)
    {
        if (_jsInstance is not null)
        {
            await _jsInstance.InvokeVoidAsync("FluentUI.Blazor.Community.DeviceDetector.Dispose");
            await _jsInstance.DisposeAsync().ConfigureAwait(false);
        }

        _dotNetHelper?.Dispose();

        await base.DisposeAsync(jsModule);
    }
}
