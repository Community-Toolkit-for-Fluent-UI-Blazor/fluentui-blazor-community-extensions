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
    private DeviceInfoState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback to use when the device information is updated.
    /// </summary>
    [Parameter]
    public EventCallback<DeviceInfo> DeviceInfoUpdated { get; set; }

    /// <summary>
    /// Gets the information about the device.
    /// </summary>
    public DeviceInfo? DeviceInfo => State?.DeviceInfo;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            var module = await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);
            State.DeviceInfo = await module.InvokeAsync<DeviceInfo>("FluentUI.Blazor.Community.DeviceDetector.GetDeviceInfo");

            //await _module.InvokeVoidAsync("getDeviceOrientation", _deviceDetectorReference);

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
    public async Task ChangeOrientation(string orientation)
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
}
