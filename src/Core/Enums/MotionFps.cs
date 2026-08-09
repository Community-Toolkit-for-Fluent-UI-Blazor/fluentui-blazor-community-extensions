namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the available frame rate options for motion or video processing.
/// </summary>
/// <remarks>Use this enumeration to select a predefined or custom frames-per-second (FPS) value when configuring
/// motion-related components. The 'Auto' value allows the system to determine the optimal frame rate automatically,
/// while 'Custom' enables specifying a user-defined FPS value.</remarks>
public enum MotionFps
{
    /// <summary>
    /// Specifies a frame rate that is automatically determined by the system.
    /// </summary>
    Auto,

    /// <summary>
    /// Specifies a frame rate of 30 frames per second.
    /// </summary>
    Fps30,

    /// <summary>
    /// Specifies a frame rate of 50 frames per second.
    /// </summary>
    Fps50,

    /// <summary>
    /// Specifies a frame rate of 60 frames per second.
    /// </summary>
    Fps60,

    /// <summary>
    /// Specifies a frame rate of 75 frames per second.
    /// </summary>
    Fps75,

    /// <summary>
    /// The user can specify a custom frame rate value.
    /// </summary>
    Custom
}
