namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for the layout and animation of a login interface transition.
/// </summary>
/// <remarks>Use this record to specify visual transition parameters, such as opacity range, animation duration,
/// and slide direction, when customizing the appearance of a login screen. All properties are immutable and should be
/// set during initialization.</remarks>
public record LoginLayoutOptions
{
    /// <summary>
    /// Gets the initial opacity value to apply at the start of an animation.
    /// </summary>
    public double StartOpacity { get; init; }

    /// <summary>
    /// Gets the target opacity value at the end of the animation.
    /// </summary>
    public double EndOpacity { get; init; } = 1.0;

    /// <summary>
    /// Gets the duration of the animation transition.
    /// </summary>
    public TimeSpan AnimationDuration { get; init; } = TimeSpan.FromMilliseconds(500);

    /// <summary>
    /// Gets the direction in which the slide animation is performed.
    /// </summary>
    public SlideDirection Direction { get; init; } = SlideDirection.Left;

    /// <summary>
    /// Gets the easing function used to control the animation's acceleration and deceleration.
    /// </summary>
    public EasingFunction EasingFunction { get; init; } = EasingFunction.Linear;

    /// <summary>
    /// Gets the easing mode that determines how the interpolation is applied at the start and end of the animation.
    /// </summary>
    public EasingMode EasingMode { get; init; } = EasingMode.InOut;
}
