using System.Runtime.CompilerServices;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the state of the device information.
/// </summary>
public sealed class DeviceInfoState
{
    /// <summary>
    /// Represents the information of the device.
    /// </summary>
    private DeviceInfo? _deviceInfo;

    /// <summary>
    /// Gets or sets the information of the device.
    /// </summary>
    public DeviceInfo? DeviceInfo
    {
        get => _deviceInfo;
        internal set
        {
            _deviceInfo = value;
            ForceUpdate();
        }
    }

    /// <summary>
    /// Occurs when the device info is updated.
    /// </summary>
    public event EventHandler<DeviceInfo?>? Updated;

    /// <summary>
    /// Force the update of the state.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]  
    internal void ForceUpdate()
    {
        Updated?.Invoke(this, DeviceInfo);
    }
}
