namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the settings for an animation. 
/// </summary>
public class SleekDialAnimationSettings
{
    /// <summary>
    /// Gets or sets the duration of the animation.
    /// </summary>
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(400);

    /// <summary>
    /// Gets or sets the delay of the animation.
    /// </summary>
    public TimeSpan Delay { get; set; }

    /// <summary>
    /// Gets or sets the animation to play.
    /// </summary>
    public SleekDialAnimation Animation { get; set; } = SleekDialAnimation.Fade;
}
