namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for an animation track that can be updated over time and queried for completion status.
/// </summary>
/// <remarks>Implementations of this interface represent individual animation tracks that progress as time
/// advances. The interface allows consumers to update the track with the current timeline time and check whether the
/// animation has finished. This is useful for coordinating multiple animations or integrating with animation
/// systems.</remarks>
public interface IMotionTrack
{
    /// <summary>
    /// Gets a value indicating whether the operation has completed.
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// Gets the animation curve used to define the interpolation behavior for the animation sequence.
    /// </summary>
    MotionCurve Curve { get; }

    /// <summary>
    /// Gets the duration of the delay interval.
    /// </summary>
    TimeSpan Delay { get; }

    /// <summary>
    /// Updates the state of the component based on the specified timeline time.
    /// </summary>
    /// <param name="timelineTime">The current time on the timeline used to update the component's state. Represents an absolute time value and
    /// must be non-negative.</param>
    void Update(TimeSpan timelineTime);
}
