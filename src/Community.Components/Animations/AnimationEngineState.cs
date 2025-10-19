namespace FluentUI.Blazor.Community.Animations;

/// <summary>
/// Represents the various states of the animation engine.
/// </summary>
public enum AnimationEngineState
{
    /// <summary>
    /// Indicates that the animation engine has not yet started.
    /// </summary>
    NotStarted,

    /// <summary>
    /// Indicates that the animation engine is paused.
    /// </summary>
    Paused,

    /// <summary>
    /// Indicates that the animation engine is running.
    /// </summary>
    Running,

    /// <summary>
    /// Indicates that the animation engine has been stopped.
    /// </summary>
    Stopped,

    /// <summary>
    /// Indicates that the animation engine has completed all animations.
    /// </summary>
    Completed,

}
