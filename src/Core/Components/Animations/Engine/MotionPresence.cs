namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the configuration for enter and exit motion variants and transitions used to animate the presence of a
/// component.
/// </summary>
/// <remarks>Use this class to specify how a component should animate when it appears or disappears, by defining
/// enter and exit motion variants and an optional transition. This is typically used in UI scenarios where smooth
/// presence animations are desired.</remarks>
public sealed class MotionPresence
{
    /// <summary>
    /// Gets or sets the motion variant to use when the element enters the view.
    /// </summary>
    /// <remarks>Set this property to specify a custom animation or transition effect that is applied when the
    /// element appears. If not set, the default enter animation is used.</remarks>
    public MotionVariant? Enter { get; set; }

    /// <summary>
    /// Gets or sets the motion variant to use when the component exits or is removed from view.
    /// </summary>
    public MotionVariant? Exit { get; set; }

    /// <summary>
    /// Gets or sets the transition to use for motion animations.
    /// </summary>
    public MotionTransition? Transition { get; set; }
}
