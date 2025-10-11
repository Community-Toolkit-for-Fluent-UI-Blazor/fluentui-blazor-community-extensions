using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a button to control the volume of an audio player.
/// </summary>
public partial class VolumeButton
    : FluentComponentBase
{
    /// <summary>
    /// Represents an icon for a speaker with zero volume.
    /// </summary>
    /// <remarks>This icon can be used to visually indicate a muted or silent state in the user
    /// interface.</remarks>
    private static readonly Icon VolumeZero = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Speaker0();

    /// <summary>
    /// Represents an icon for a speaker with one sound wave, typically used to indicate low volume.
    /// </summary>
    private static readonly Icon VolumeOne = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Speaker1();

    /// <summary>
    /// Represents an icon for a speaker with two sound waves, typically used to indicate medium volume.
    /// </summary>
    private static readonly Icon VolumeTwo = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Speaker2();

    /// <summary>
    /// Represents whether the volume popover is open.
    /// </summary>
    private bool _isVolumePopoverOpen;

    /// <summary>
    /// Represents the volume level, where 1.0 is the default maximum value.
    /// </summary>
    private double _volume = 1.0;

    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeButton"/> class.
    /// </summary>
    public VolumeButton()
    {
        Id = $"volume-button-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets the appropriate volume icon based on the current volume level.
    /// </summary>
    private Icon VolumeIcon => _volume switch
    {
        > 0.5 => VolumeTwo,
        > 0.0 => VolumeOne,
        _ => VolumeZero
    };

    /// <summary>
    /// Gets or sets the callback that is invoked when the volume changes.
    /// </summary>
    [Parameter]
    public EventCallback<double> OnVolumeChanged { get; set; }

    /// <summary>
    /// Gets or sets the label for the volume button.
    /// </summary>
    [Parameter]
    public string Label { get; set; } = "Volume";

    /// <summary>
    /// Gets the ARIA label for the volume button, combining the volume label and the current volume percentage.
    /// </summary>
    private string AriaLabel => $"{Label} ({_volume * 100:0}%)";

    /// <summary>
    /// Handles the volume change event asynchronously.
    /// </summary>
    /// <remarks>This method updates the internal volume state and invokes the <see cref="OnVolumeChanged"/>
    /// event if it has subscribers.</remarks>
    /// <param name="value">The new volume level. Must be a value between 0.0 and 1.0, where 0.0 represents mute and 1.0 represents the
    /// maximum volume.</param>
    /// <returns></returns>
    private async Task OnVolumeChangedAsync(double value)
    {
        _volume = value;

        if (OnVolumeChanged.HasDelegate)
        {
            await OnVolumeChanged.InvokeAsync(value);
        }
    }
}
