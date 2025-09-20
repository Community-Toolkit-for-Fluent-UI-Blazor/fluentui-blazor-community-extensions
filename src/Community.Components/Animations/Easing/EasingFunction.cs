namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the various easing functions available for animations.
/// </summary>
public enum EasingFunction
{
    /// <summary>
    /// Represents a linear easing function, which results in a constant speed animation.
    /// </summary>
    Linear,

    /// <summary>
    /// Represents a "back" easing function, which creates an animation that overshoots its target and then settles back.
    /// </summary>
    Back,

    /// <summary>
    /// Represents a "bounce" easing function, which creates an animation that simulates a bouncing effect.
    /// </summary>
    Bounce,

    /// <summary>
    /// Represents a "circular" easing function, which creates an animation that follows a circular path.
    /// </summary>
    Circular,

    /// <summary>
    /// Represents a "cubic" easing function, which creates an animation that accelerates and decelerates in a cubic manner.
    /// </summary>
    Cubic,

    /// <summary>
    /// Represents an "elastic" easing function, which creates an animation that simulates an elastic effect.
    /// </summary>
    Elastic,

    /// <summary>
    /// Represents an "exponential" easing function, which creates an animation that accelerates and decelerates exponentially.
    /// </summary>
    Exponential,

    /// <summary>
    /// Represents a "quadratic" easing function, which creates an animation that accelerates and decelerates in a quadratic manner.
    /// </summary>
    Quadratic,

    /// <summary>
    /// Represents a "quartic" easing function, which creates an animation that accelerates and decelerates in a quartic manner.
    /// </summary>
    Quartic,

    /// <summary>
    /// Represents a "quintic" easing function, which creates an animation that accelerates and decelerates in a quintic manner.
    /// </summary>
    Quintic,
}
