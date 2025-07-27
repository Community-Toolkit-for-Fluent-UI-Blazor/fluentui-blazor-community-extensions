using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the settings for the radial mode.
/// </summary>
public class SleekDialRadialSettings
{
    /// <summary>
    /// Gets or sets the start angle of the radial arc.
    /// </summary>
    public int StartAngle { get; set; } = -1;

    /// <summary>
    /// Gets or sets the end angle of the radial arc.
    /// </summary>
    public int EndAngle { get; set; } = -1;

    /// <summary>
    /// Gets or sets the offset of the items on the arc.
    /// </summary>
    public string Offset { get; set; } = "100px";

    /// <summary>
    /// Gets or sets the direction of the items.
    /// </summary>
    public SleekDialRadialDirection Direction { get; set; } = SleekDialRadialDirection.Default;
}
