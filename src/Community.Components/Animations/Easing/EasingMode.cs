namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the easing mode for animations, indicating how the animation progresses over time.
/// </summary>
public enum EasingMode
{
    /// <summary>
    /// Represents an easing mode where the animation starts slowly and accelerates towards the end.
    /// </summary>
    In,

    /// <summary>
    /// Represents an easing mode where the animation starts quickly and decelerates towards the end.
    /// </summary>
    Out,

    /// <summary>
    /// Represents an easing mode where the animation starts slowly, accelerates in the middle, and then decelerates towards the end.
    /// </summary>
    InOut
}
