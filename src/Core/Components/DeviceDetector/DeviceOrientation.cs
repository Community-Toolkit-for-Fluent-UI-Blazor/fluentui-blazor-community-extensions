namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the orientation of the device.
/// </summary>
public enum DeviceOrientation
{
    /// <summary>
    /// The orientation is portrait.
    /// </summary>
    Portrait,

    /// <summary>
    /// The orientation is portrait but image is flipped.
    /// </summary>
    PortraitReversed,

    /// <summary>
    /// The device is oriented horizontally.
    /// </summary>
    Landscape,

    /// <summary>
    /// The device is oriented horizontally, but the image is flipped.
    /// </summary>
    LandscapeReversed,
}
