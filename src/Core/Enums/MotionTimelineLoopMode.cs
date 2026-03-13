namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the available looping modes for an operation or animation.
/// </summary>
/// <remarks>Use this enumeration to control how an operation or animation repeats. The modes include no looping,
/// repeating, ping-pong (alternating direction), and infinite looping. The specific effect depends on the context in
/// which the mode is applied.</remarks>
public enum MotionTimelineLoopMode
{
    /// <summary>
    /// Specifies that the animation should not loop and will execute only once.
    /// </summary>
    None,

    /// <summary>
    /// Specifies that the animation should repeat from the beginning after it reaches the end, creating a continuous loop.
    /// </summary>
    /// <remarks>In this mode, you can specifie the number of repeat.</remarks>
    Repeat,

    /// <summary>
    /// Specifies that the animation should alternate between playing forward and backward, creating a "ping-pong" effect.
    ///  After reaching the end, it will reverse direction and play back to the start, then repeat this cycle.
    /// </summary>
    PingPong,

    /// <summary>
    /// Specifies that the animation should loop indefinitely without stopping.
    /// </summary>
    Infinite
}
