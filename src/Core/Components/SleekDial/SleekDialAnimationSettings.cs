namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the settings for an animation. 
/// </summary>
public class SleekDialAnimationSettings
{
    /// <summary>
    /// Gets or sets the animation type used for visual transitions in the sleek dial control.
    /// </summary>
    public SleekDialAnimationType Animation { get; set; } = SleekDialAnimationType.Fade;

    /// <summary>
    /// Gets or sets the easing function applied to the dial animation.
    /// </summary>
    public SleekDialEasing Easing { get; set; } = SleekDialEasing.EaseOut;

    /// <summary>
    /// Gets or sets the duration of the animation.
    /// </summary>
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(400);

    /// <summary>
    /// Gets or sets the delay of the animation.
    /// </summary>
    public TimeSpan Delay { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the stagger effect is enabled.
    /// </summary>
    public bool Stagger { get; set; } = true;
}
