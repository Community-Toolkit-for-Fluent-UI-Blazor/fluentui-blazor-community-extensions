using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a set of asynchronous animation actions that can be performed on a motion item, such as fading, moving, and
/// scaling. This class encapsulates common motion behaviors for UI elements.
/// </summary>
/// <remarks>Use this class to trigger standard motion effects on a target item in a consistent and reusable way.
/// All actions are performed asynchronously and can be awaited to coordinate with other UI logic. Instances of this
/// class are typically created for a specific motion item and are not thread-safe.</remarks>
public sealed class MotionActions : IMotionActions
{
    /// <summary>
    /// Represents the motion item associated with this instance.
    /// </summary>
    private readonly MotionNode _node;

    /// <summary>
    /// Provides an instance of the double value interpolator used for performing interpolation operations.
    /// </summary>
    private readonly DoubleInterpolator _interpolator = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MotionActions"/> class with the specified <paramref name="node"/>.
    /// </summary>
    /// <param name="node">The motion node to animate.</param>
    public MotionActions(MotionNode node)
    {
        ArgumentNullException.ThrowIfNull(node, nameof(node));
        _node = node;
    }

    /// <inheritdoc />
    public Task FadeInAsync(TimeSpan duration) => AnimateDoubleAsync("o", _node.State.Opacity, 1.0, new MotionTransition { Duration = duration });

    /// <inheritdoc />
    public Task FadeOutAsync(TimeSpan duration) => AnimateDoubleAsync("o", _node.State.Opacity, 0.0, new MotionTransition { Duration = duration });

    /// <inheritdoc />
    public Task MoveToAsync(double x, double y, TimeSpan duration)
    {
        var motion = new MotionTransition { Duration = duration };

        return RunParallelAsync(
             AnimateDoubleAsync("x", _node.State.X, x, motion),
             AnimateDoubleAsync("y", _node.State.Y, y, motion)
         );
    }

    /// <inheritdoc />
    public Task ScaleToAsync(double x, double y, TimeSpan duration)
    {
        var motion = new MotionTransition { Duration = duration };

        return RunParallelAsync(
             AnimateDoubleAsync("x", _node.State.ScaleX, x, motion),
             AnimateDoubleAsync("y", _node.State.ScaleY, y, motion)
         );
    }

    /// <inheritdoc />
    private static async Task RunParallelAsync(params Task[] tasks)
    {
        var buckets = tasks.Interleaved();
        var last = buckets[^1];

        var t = await last.ConfigureAwait(false);
        await t.ConfigureAwait(false);
    }

    /// <summary>
    /// Animates a double value from a specified start value to an end value over a given duration using a cubic easing
    /// function.
    /// </summary>
    /// <remarks>The animation uses a cubic easing function with an 'Out' mode for smooth transitions. The
    /// returned task completes asynchronously when the animation reaches the end value.</remarks>
    /// <param name="key">A unique identifier for the animation track. Used to distinguish this animation from others.</param>
    /// <param name="start">The initial value of the double to animate from.</param>
    /// <param name="end">The final value of the double to animate to.</param>
    /// <param name="transition">The motion transition parameters that define the duration, delay, and easing function for the animation.</param>
    /// <returns>A task that completes when the animation has finished.</returns>
    public Task AnimateDoubleAsync(string key, double start, double end, MotionTransition transition)
    {
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var track = new MotionTrack<double>(
            key,
            _node.State,
            start,
            end,
            transition.ToCurve(),
            _interpolator,
            transition.Delay ?? TimeSpan.Zero
        );

        void Handler(object? sender, EventArgs e)
        {
            track.Completed -= Handler;
            tcs.TrySetResult();
        }

        track.Completed += Handler;

        _node.Timeline.AddTrack(track);
        _node.Timeline.Start();

        return tcs.Task;
    }

    /// <summary>
    /// Animates a double-precision value from its current state to a specified end value using the provided motion
    /// transition.
    /// </summary>
    /// <param name="key">The key that identifies the value to animate.</param>
    /// <param name="end">The target value to which the animation will progress.</param>
    /// <param name="transition">The motion transition that defines the animation's timing and easing behavior.</param>
    /// <returns>A task that represents the asynchronous animation operation.</returns>
    public Task AnimateDoubleAsync(string key, double end, MotionTransition transition)
    {
        var start = _node.State.GetOrDefault<double>(key, 0.0);

        return AnimateDoubleAsync(key, start, end, transition);
    }
}
