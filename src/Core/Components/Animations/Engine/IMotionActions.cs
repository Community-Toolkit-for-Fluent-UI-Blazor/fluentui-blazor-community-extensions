namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a set of asynchronous actions for animating motion effects such as fading, moving, and scaling elements over
/// a specified duration.
/// </summary>
/// <remarks>Implementations of this interface provide motion-based animations that can be applied to UI elements.
/// Each method initiates a specific animation effect and completes when the animation finishes. The interface is
/// intended for use in scenarios where visual transitions or effects are required, such as enhancing user experience in
/// interactive applications.</remarks>
public interface IMotionActions
{
    /// <summary>
    /// Asynchronously performs a fade-in animation over the specified duration.
    /// </summary>
    /// <param name="duration">The length of time over which the fade-in animation occurs.</param>
    /// <returns>A task that represents the asynchronous fade-in operation.</returns>
    Task FadeInAsync(TimeSpan duration);

    /// <summary>
    /// Asynchronously fades out the element over the specified duration.
    /// </summary>
    /// <param name="duration">The length of time over which the fade-out animation occurs.</param>
    /// <returns>A task that represents the asynchronous fade-out operation.</returns>
    Task FadeOutAsync(TimeSpan duration);

    /// <summary>
    /// Asynchronously moves the object to the specified coordinates over the given duration.
    /// </summary>
    /// <param name="x">The target X-coordinate to move to.</param>
    /// <param name="y">The target Y-coordinate to move to.</param>
    /// <param name="duration">The duration over which the movement occurs.</param>
    /// <returns>A task that represents the asynchronous move operation.</returns>
    Task MoveToAsync(double x, double y, TimeSpan duration);

    /// <summary>
    /// Animates the scaling of an element to the specified X and Y scale factors over the given duration.
    /// </summary>
    /// <param name="x">The target scale factor along the X axis.</param>
    /// <param name="y">The target scale factor along the Y axis.</param>
    /// <param name="duration">The duration of the scaling animation.</param>
    /// <returns>A task that represents the asynchronous scaling operation.</returns>
    Task ScaleToAsync(double x, double y, TimeSpan duration);

    /// <summary>
    /// Animates a double value from a specified start value to an end value over a given duration.
    /// </summary>
    /// <remarks>If an animation with the same key is already running, it may be replaced or interrupted
    /// depending on the implementation. The method is typically used to create smooth transitions for UI elements or
    /// component properties.</remarks>
    /// <param name="key">A unique identifier for the animation. Used to distinguish between multiple concurrent animations.</param>
    /// <param name="start">The initial value of the double to begin the animation from.</param>
    /// <param name="end">The final value of the double to animate to.</param>
    /// <param name="transition">The transition parameters that define the duration, delay, easing function, and mode for the animation.</param>
    /// <returns>A task that represents the asynchronous animation operation. The task completes when the animation finishes.</returns>
    Task AnimateDoubleAsync(string key, double start, double end, MotionTransition transition);

    /// <summary>
    /// Animates a double value identified by the specified key to the given end value using the provided motion
    /// transition.
    /// </summary>
    /// <param name="key">The unique identifier for the value to animate. This key is used to track and update the animated value.</param>
    /// <param name="end">The target value to which the animation will progress.</param>
    /// <param name="transition">The motion transition that defines the animation's timing and easing behavior.</param>
    /// <returns>A task that represents the asynchronous animation operation. The task completes when the animation finishes.</returns>
    Task AnimateDoubleAsync(string key, double end, MotionTransition transition);
}

