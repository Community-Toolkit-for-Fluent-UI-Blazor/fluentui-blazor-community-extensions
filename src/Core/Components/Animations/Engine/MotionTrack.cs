namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a single animation track that interpolates between two values over time using a specified curve and
/// interpolator.
/// </summary>
/// <remarks>An animation track defines the progression of a value from a start to an end state, controlled by an
/// animation curve and an interpolator. The track supports an optional delay before the animation begins. The current
/// value and completion state are updated as the timeline advances. This class is sealed and cannot be
/// inherited.</remarks>
/// <typeparam name="T">The type of value being animated. Must be compatible with the provided interpolator.</typeparam>
public sealed class MotionTrack<T> : IMotionTrack
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MotionTrack{T}"/> class with the specified parameters.
    /// </summary>
    /// <param name="property">Sets the name of the property or aspect being animated, used for identification purposes.</param>
    /// <param name="state">Sets the motion state associated with this track, indicating the context or phase of the animation.</param>
    /// <param name="startValue">Sets the initial value assigned to the component or operation.</param>
    /// <param name="endValue">Sets the final value of the range or interval represented by this instance.</param>
    /// <param name="curve">Sets the animation curve used to define the interpolation behavior for the animation sequence.</param>
    /// <param name="interpolator">Sets the interpolator used to compute intermediate values between two points of type T.</param>
    /// <param name="delay">Sets the duration of the delay applied by the operation. If not provided, defaults to zero.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public MotionTrack(
        string property,
        MotionState state,
        T startValue,
        T endValue,
        MotionCurve curve,
        IMotionInterpolator<T> interpolator,
        TimeSpan? delay = null)
    {
        Key = property ?? throw new ArgumentNullException(nameof(property));
        State = state ?? throw new ArgumentNullException(nameof(state));
        StartValue = startValue;
        EndValue = endValue;
        Curve = curve ?? throw new ArgumentNullException(nameof(curve));
        Interpolator = interpolator ?? throw new ArgumentNullException(nameof(interpolator));
        Delay = delay ?? TimeSpan.Zero;

        CurrentValue = startValue;
    }

    /// <summary>
    /// Occurs when the associated operation has completed.
    /// </summary>
    /// <remarks>Subscribe to this event to be notified when the operation finishes. The event handler
    /// receives an <see cref="EventArgs"/> instance containing event data.</remarks>
    public event EventHandler? Completed;

    /// <summary>
    /// Gets the property to animate.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the current motion state of the component.
    /// </summary>
    public MotionState State { get; }

    /// <summary>
    /// Gets the initial value assigned to the component or operation.
    /// </summary>
    public T StartValue { get; }

    /// <summary>
    /// Gets the final value of the range or interval represented by this instance.
    /// </summary>
    public T EndValue { get; }

    /// <summary>
    /// Gets the animation curve used to define the interpolation behavior for the animation sequence.
    /// </summary>
    public MotionCurve Curve { get; }

    /// <summary>
    /// Gets the interpolator used to compute intermediate values between two points of type T.
    /// </summary>
    public IMotionInterpolator<T> Interpolator { get; }

    /// <summary>
    /// Gets the duration of the delay applied by the operation.
    /// </summary>
    public TimeSpan Delay { get; }

    /// <summary>
    /// Gets the current value held by the instance.
    /// </summary>
    public T CurrentValue { get; private set; }

    /// <inheritdoc />
    public bool IsCompleted { get; private set; }

    /// <inheritdoc />
    public void Update(TimeSpan timelineTime)
    {
        if (IsCompleted)
        {
            return;
        }

        var localTime = timelineTime - Delay;

        if (localTime <= TimeSpan.Zero)
        {
            CurrentValue = StartValue;
            State.Set(Key, CurrentValue);
            return;
        }

        var progress = Curve.Evaluate(localTime);
        CurrentValue = Interpolator.Lerp(StartValue, EndValue, progress);

        State.Set(Key, CurrentValue);

        if (localTime >= Curve.Duration)
        {
            IsCompleted = true;
            Completed?.Invoke(this, EventArgs.Empty);
        }
    }
}
