using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component which detects the device where the app is running.
/// </summary>
public partial class FluentCxDeviceDetector
    : FluentComponentBase
{
    /// <summary>
    /// Represents the reference of the device detector.
    /// </summary>
    private readonly DotNetObjectReference<FluentCxDeviceDetector> _deviceDetectorReference;

    /// <summary>
    /// Represents the javascript object.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents the name of the javascript.
    /// </summary>
    private const string JavascriptFileName = "./_content/FluentUI.Blazor.Community.Components/Components/DeviceDetector/FluentCxDeviceDetector.razor.js";

    /// <summary>
    /// Initialize a new instance of the <see cref="FluentCxDeviceDetector"/> class.
    /// </summary>
    public FluentCxDeviceDetector() : base()
    {
        _deviceDetectorReference = DotNetObjectReference.Create(this);
    }

    /// <summary>
    /// Gets or sets the ChildContent render fragment.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <summary>
    /// Gets or sets the Javascript Runtime.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

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
        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptFileName);
            State.DeviceInfo = await _module.InvokeAsync<DeviceInfo>("getDeviceInfo");
            await _module.InvokeVoidAsync("getDeviceOrientation", _deviceDetectorReference);

            if (DeviceInfoUpdated.HasDelegate)
            {
                await DeviceInfoUpdated.InvokeAsync(State.DeviceInfo);
            }
        }

        await base.OnAfterRenderAsync(firstRender);
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

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.DisposeAsync();
                _module = null;
            }
        }
        catch (JSDisconnectedException)
        {

        }
    }
}
