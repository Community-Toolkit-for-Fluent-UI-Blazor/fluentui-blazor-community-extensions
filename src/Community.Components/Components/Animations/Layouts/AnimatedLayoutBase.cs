using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the base class for animated layout strategies used in conjunction with <see cref="FluentCxAnimation"/> and <see cref="AnimationGroup"/> components.
/// </summary>
public abstract class AnimatedLayoutBase
    : FluentComponentBase, ILayoutStrategy
{
    /// <summary>
    /// Represents whether the <see cref="Immediate"/> property has changed.
    /// </summary>
    private bool _hasImmediateChanged;

    /// <summary>
    /// Represents the last set duration to detect changes.
    /// </summary>
    private TimeSpan _lastDuration;

    /// <inheritdoc />
    [Parameter]
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(500);

    /// <summary>
    /// Gets or sets the parent <see cref="AnimationGroup"/> component, if any.
    /// </summary>
    [CascadingParameter]
    private AnimationGroup? Group { get; set; }

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxAnimation"/> component, if any.
    /// </summary>
    [CascadingParameter]
    private FluentCxAnimation? Animation { get; set; }

    /// <summary>
    /// Gets or sets the parent <see cref="MorphingLayout"/> component, if any.
    /// </summary>
    [CascadingParameter]
    private MorphingLayout? MorphingLayout { get; set; }

    /// <summary>
    /// Gets the start time of the current layout application.
    /// </summary>
    protected internal DateTime StartTime { get; private set; }

    /// <inheritdoc />
    [Parameter]
    public EasingFunction EasingFunction { get; set; }

    /// <inheritdoc />
    [Parameter]
    public EasingMode EasingMode { get; set; }

    /// <summary>
    /// Gets the width of the container where the layout is applied.
    /// </summary>
    protected internal double Width { get; private set; }

    /// <summary>
    /// Gets the height of the container where the layout is applied.
    /// </summary>
    protected internal double Height { get; private set; }

    /// <inheritdoc />
    [Parameter]
    public bool Immediate { get; set; }

    /// <summary>
    /// Gets or sets a callback that is invoked when the <see cref="Immediate"/> property changes.
    /// </summary>
    public EventCallback<bool> ImmediateChanged { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (MorphingLayout is not null)
        {
            MorphingLayout.Add(this);
            Duration = MorphingLayout.Duration;
            EasingFunction = MorphingLayout.EasingFunction;
            EasingMode = MorphingLayout.EasingMode;
            Width = MorphingLayout.Width;
            Height = MorphingLayout.Height;
        }
        else if (Group is not null)
        {
            Group.SetLayout(this);
        }
        else
        {
            Animation?.SetLayout(this);
        }
    }

    /// <inheritdoc />
    public void ApplyLayout(List<AnimatedElement> elements)
    {
        var count = elements.Count;

        Parallel.For(0, count, i =>
        {
            Update(i, count, elements[i]);
        });
    }

    /// <summary>
    /// Updates the specified animated element's properties based on its index and the total count of elements.
    /// </summary>
    /// <param name="index">Index of the element in the layout.</param>
    /// <param name="count">Total number of elements in the layout</param>
    /// <param name="animatedElement">Element to update.</param>
    protected abstract void Update(int index, int count, AnimatedElement animatedElement);

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasImmediateChanged = parameters.HasValueChanged(nameof(Immediate), Immediate);

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        if (_hasImmediateChanged)
        {
            _hasImmediateChanged = false;
            await ImmediateChanged.InvokeAsync(Immediate);

            if (Immediate)
            {
                _lastDuration = Duration;
                Duration = TimeSpan.Zero;
            }
            else
            {
                if (_lastDuration == TimeSpan.Zero)
                {
                    _lastDuration = TimeSpan.FromMilliseconds(500);
                }

                Duration = _lastDuration;
            }

            if (MorphingLayout is not null)
            {
                MorphingLayout.SetImmediate(Immediate);
            }
            else if (Animation is not null)
            {
                await Animation.SetImmediateAsync(Immediate);
            }
        }

        await base.OnParametersSetAsync();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Group?.RemoveLayout();
        Animation?.RemoveLayout();
        MorphingLayout?.Remove(this);

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public void ApplyStartTime(DateTime now)
    {
        StartTime = now;
    }

    /// <inheritdoc />
    public void SetDimensions(double width, double height)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Creates a new animation state with the specified start and end values.
    /// </summary>
    /// <typeparam name="T">The type of the animation values. Must be a value type.</typeparam>
    /// <param name="startValue">The initial value of the animation.</param>
    /// <param name="endValue">The final value of the animation.</param>
    /// <returns>An <see cref="AnimationState{T}"/> object representing the animation state,  initialized with the specified
    /// start and end values, as well as the current  duration, start time, easing function, and easing mode.</returns>
    public AnimationState<T> CreateState<T>(
        T endValue,
        T startValue = default)
        where T : struct
    {
        return new AnimationState<T>
        {
            StartValue = startValue,
            EndValue = endValue,
            Duration = Duration,
            StartTime = StartTime,
            EasingFunction = EasingFunction,
            EasingMode = EasingMode
        };
    }
}
