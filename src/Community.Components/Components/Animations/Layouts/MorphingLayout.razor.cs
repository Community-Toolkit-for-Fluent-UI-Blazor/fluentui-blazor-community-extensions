using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout strategy that enables smooth transitions between multiple animated layouts.
/// </summary>
/// <remarks>The <see cref="MorphingLayout"/> class is used to manage and animate transitions between different
/// layouts of elements. It supports defining multiple layouts and morphing between them using specified animation
/// parameters such as duration, easing function, and easing mode. This class is typically used in scenarios where
/// dynamic and visually appealing layout transitions are required, such as in UI animations or data visualization.  The
/// layout transitions are controlled by the parent animation context, and the class provides methods to apply layouts,
/// set dimensions, and manage the sequence of layouts. It also ensures proper disposal of resources when no longer
/// needed.</remarks>
public sealed partial class MorphingLayout
    : FluentComponentBase, ILayoutStrategy
{
    /// <summary>
    /// Represents the collection of layouts managed by this instance.
    /// </summary>
    private readonly List<AnimatedLayoutBase> _layouts = [];

    /// <summary>
    /// Represents the layout being transitioned from.
    /// </summary>
    private AnimatedLayoutBase? _fromLayout;

    /// <summary>
    /// Represents the layout being transitioned to.
    /// </summary>
    private AnimatedLayoutBase? _toLayout;

    /// <summary>
    /// Represents the index of the current layout in the sequence.
    /// </summary>
    private int _currentLayoutIndex;

    /// <summary>
    /// Gets or sets the child content to be rendered inside this component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxAnimation"/> component, if any.
    /// </summary>
    [CascadingParameter]
    private FluentCxAnimation? Parent { get; set; }

    /// <summary>
    /// Gets or sets the duration of the layout transition animation.
    /// </summary>
    [Parameter]
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(500);

    /// <summary>
    /// Gets the start time of the current layout.
    /// </summary>
    public DateTime StartTime { get; private set; }

    /// <summary>
    /// Gets or sets the width of the container where the layout is applied.
    /// </summary>
    [Parameter]
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the container where the layout is applied.
    /// </summary>
    [Parameter]
    public double Height { get; set; }

    /// <summary>
    /// Gets or sets the easing function used for the layout transition animation.
    /// </summary>
    [Parameter]
    public EasingFunction EasingFunction { get; set; }

    /// <summary>
    /// Gets or sets the easing mode used for the layout transition animation.
    /// </summary>
    [Parameter]
    public EasingMode EasingMode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether layout changes should be applied immediately without animation.
    /// </summary>
    /// <remarks>
    /// MorphingLayout does not support immediate changes. Attempting to set this property to true will result in a NotSupportedException.
    /// </remarks>
    [Parameter]
    public bool Immediate { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (Parent is not null)
        {
            await Parent.SetLayoutAsync(this);
        }
    }

    /// <inheritdoc />
    public void ApplyLayout(List<AnimatedElement> elements)
    {
        if (_layouts.Count < 2)
        {
            return;
        }

        _fromLayout = _layouts[_currentLayoutIndex];
        _toLayout = _layouts[_currentLayoutIndex + 1];

        var fromElements = elements.Select(x => x.Clone()).ToList();
        var toElements = elements.Select(x => x.Clone()).ToList();

        _fromLayout.ApplyLayout(fromElements);
        _toLayout.ApplyLayout(toElements);

        for (var i = 0; i < elements.Count; i++)
        {
            ApplyMorph(elements[i], fromElements[i], toElements[i]);
        }
    }

    /// <summary>
    /// Advances to the next layout in the sequence, looping back to the start if necessary.
    /// </summary>
    internal bool NextLayout()
    {
        if (_layouts.Count < 2)
        {
            return false;
        }

        _currentLayoutIndex++;

        if (_currentLayoutIndex >= _layouts.Count - 1)
        {
            _currentLayoutIndex = 0;

            return Parent?.Loop ?? false;
        }
        else
        {
            _fromLayout = _layouts[_currentLayoutIndex];
            _toLayout = _layouts[_currentLayoutIndex + 1];

            return true;
        }
    }

    /// <summary>
    /// Applies a morphing transformation to the specified <see cref="AnimatedElement"/> by interpolating its state
    /// properties between two other <see cref="AnimatedElement"/> instances.
    /// </summary>
    /// <remarks>This method updates the state properties of <paramref name="animatedElement"/> (such as
    /// position, scale, rotation, color, and opacity) by interpolating between the corresponding properties of
    /// <paramref name="fromElement"/> and <paramref name="toElement"/>. Default values are used for any null state
    /// properties in the source or target elements.</remarks>
    /// <param name="animatedElement">The <see cref="AnimatedElement"/> to which the morphing transformation will be applied.</param>
    /// <param name="fromElement">The source <see cref="AnimatedElement"/> representing the starting state of the morph.</param>
    /// <param name="toElement">The target <see cref="AnimatedElement"/> representing the ending state of the morph.</param>
    private void ApplyMorph(AnimatedElement animatedElement,
                            AnimatedElement fromElement,
                            AnimatedElement toElement)
    {
        animatedElement.OffsetXState = CreateState(toElement.OffsetXState?.EndValue ?? 0, fromElement.OffsetXState?.EndValue ?? 0);
        animatedElement.OffsetYState = CreateState(toElement.OffsetYState?.EndValue ?? 0, fromElement.OffsetYState?.EndValue ?? 0);
        animatedElement.ScaleXState = CreateState(toElement.ScaleXState?.EndValue ?? 1, fromElement.ScaleXState?.EndValue ?? 1);
        animatedElement.ScaleYState = CreateState(toElement.ScaleYState?.EndValue ?? 1, fromElement.ScaleYState?.EndValue ?? 1);
        animatedElement.RotationState = CreateState(toElement.RotationState?.EndValue ?? 0, fromElement.RotationState?.EndValue ?? 0);
        animatedElement.ColorState = CreateState(toElement.ColorState?.EndValue ?? "#000000", fromElement.ColorState?.EndValue ?? "#000000");
        animatedElement.BackgroundColorState = CreateState(toElement.BackgroundColorState?.EndValue ?? "#FFFFFF", fromElement.BackgroundColorState?.EndValue ?? "#FFFFFF");
        animatedElement.OpacityState = CreateState(toElement.OpacityState?.EndValue ?? 1, fromElement.OpacityState?.EndValue ?? 1);
        animatedElement.ValueState = CreateState(toElement.ValueState?.EndValue ?? 0, fromElement.ValueState?.EndValue ?? 0);
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        if (parameters.HasValueChanged(nameof(Immediate), Immediate) || Immediate)
        {
            throw new NotSupportedException("MorphingLayout does not support Immediate changes.");
        }

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    public void ApplyStartTime(DateTime now)
    {
        StartTime = now;
        _fromLayout?.ApplyStartTime(now);
        _toLayout?.ApplyStartTime(now);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        var items = _layouts.ToArray();

        foreach (var item in items)
        {
            item.Dispose();
        }

        _layouts.Clear();
        Parent?.RemoveLayout(this);
    }

    /// <inheritdoc />
    public void SetDimensions(double width, double height)
    {
        foreach (var item in _layouts)
        {
            item.SetDimensions(width, height);
        }
    }

    /// <summary>
    /// Removes the specified layout from the collection of animated layouts.
    /// </summary>
    /// <remarks>If the specified layout is not found in the collection, no action is taken.</remarks>
    /// <param name="value">The layout to remove from the collection. Must not be <see langword="null"/>.</param>
    internal void Remove(AnimatedLayoutBase value)
    {
        _layouts.Remove(value);
    }

    /// <summary>
    /// Adds the specified layout to the collection if it is not already present.
    /// </summary>
    /// <param name="value">The layout to add to the collection. Cannot be <see langword="null"/>.</param>
    internal void Add(AnimatedLayoutBase value)
    {
        if (_layouts.Contains(value))
        {
            return;
        }

        _layouts.Add(value);
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
        T startValue = default!)
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

    /// <summary>
    /// Sets whether the layout change should be applied immediately.
    /// </summary>
    /// <param name="immediate">A value indicating whether the layout change should be applied immediately.  Must be <see langword="false"/> as
    /// immediate changes are not supported.</param>
    /// <exception cref="NotSupportedException">Thrown if <paramref name="immediate"/> is <see langword="true"/> because immediate changes are not supported.</exception>
    internal static void SetImmediate(bool immediate)
    {
        if (immediate)
        {
            throw new NotSupportedException("MorphingLayout does not support Immediate changes.");
        }
    }
}
