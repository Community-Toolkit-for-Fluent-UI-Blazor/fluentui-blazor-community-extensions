namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the easing functions that control the rate of change for smooth transitions in SleekDial animations.
/// </summary>
public enum SleekDialEasing
{
    /// <summary>
    /// Represents a linear mathematical function.
    /// </summary>
    Linear,
    /// <summary>
    /// Gets or sets a value indicating whether the easing function accelerates at the beginning and decelerates towards
    /// the end of the animation.
    /// </summary>
    EaseIn,

    /// <summary>
    /// Represents an easing function that starts quickly and decelerates towards the end.
    /// </summary>
    /// <remarks>This easing function is commonly used in animations to create a smooth transition effect. It
    /// is particularly useful for scenarios where a natural deceleration is desired, such as in UI
    /// animations.</remarks>
    EaseOut,

    /// <summary>
    /// Represents an easing function that accelerates and decelerates smoothly.
    /// </summary>
    EaseInOut,

    /// <summary>
    /// Represents the season characterized by warmer temperatures and increased plant growth.
    /// </summary>
    Spring
}
