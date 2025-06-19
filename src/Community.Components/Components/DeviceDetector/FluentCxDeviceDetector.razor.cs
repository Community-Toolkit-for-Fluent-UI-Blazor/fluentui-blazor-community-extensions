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
    private readonly DotNetObjectReference<FluentCxDeviceDetector> _deviceDetectorReference;

    /// <summary>
    /// Represents the javascript object.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents the name of the javascript.
    /// </summary>
    private const string JavascriptFileName = "./_content/FluentUI.Blazor.Community.Components/Components/DeviceDetector/FluentCxDeviceDetector.razor.js";

    public FluentCxDeviceDetector()
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
    public required IJSRuntime JSRuntime { get; set; }

    /// <summary>
    /// Gets the information of the running device.
    /// </summary>
    public DeviceInfo? DeviceInfo { get; private set; }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptFileName);
            DeviceInfo = await _module.InvokeAsync<DeviceInfo>("getDeviceInfo");
            await _module.InvokeVoidAsync("getDeviceOrientation", _deviceDetectorReference);
        }
    }

    [JSInvokable]
    public void ChangeOrientation(bool isPortrait)
    {
        if (DeviceInfo is not null)
        {
            DeviceInfo.Orientation = isPortrait ? DeviceOrientation.Portrait : DeviceOrientation.Landscape;
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
