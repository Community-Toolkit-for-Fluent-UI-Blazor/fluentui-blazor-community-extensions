namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the state of an animation timeline during its lifecycle.
/// </summary>
/// <remarks>This enumeration is used to indicate whether an animation timeline is stopped, running, paused, or
/// completed. The state can be used to control or query the progress of an animation in UI scenarios.</remarks>
public enum MotionTimelineState
{
    /// <summary>
    /// Indicates that the animation timeline is not currently active.
    /// </summary>
    Stopped,

    /// <summary>
    /// Indicates that the animation timeline is currently active and progressing over time.
    /// </summary>
    Running,

    /// <summary>
    /// Indicates that the animation timeline is temporarily halted but can be resumed from the same point.
    /// </summary>
    Paused,

    /// <summary>
    /// Indicates that the animation timeline has finished its course and reached its end state.
    ///  Once completed, the timeline is no longer active and cannot be resumed without restarting.
    /// </summary>
    Completed
}
