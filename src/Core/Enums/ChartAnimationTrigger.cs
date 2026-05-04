namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the available triggers for chart animations.
/// </summary>
/// <remarks>Use this enumeration to control when animations are applied to chart elements, such as on appearance,
/// update, selection, or interaction events. The selected trigger determines the user interaction or state change that
/// initiates the animation.</remarks>
public enum ChartAnimationTrigger
{
    /// <summary>
    /// No animation.
    /// </summary>
    None,

    /// <summary>
    /// Animation triggered when the item first appears.
    /// </summary>
    InitialAppear,

    /// <summary>
    /// Animation triggered when the item updates from its previous state to a new state.
    /// </summary>
    UpdateFromPrevious,

    /// <summary>
    /// Animation triggered when the item is removed.
    /// </summary>
    Remove,

    /// <summary>
    /// Animation triggered when the item enters the hover state.
    /// </summary>
    HoverIn,

    /// <summary>
    /// Animation triggered when the item leaves the hover state.
    /// </summary>
    HoverOut,

    /// <summary>
    /// Animation triggered when the item enters the pressed state.
    /// </summary>
    PressIn,

    /// <summary>
    /// Animation triggered when the item leaves the pressed state.
    /// </summary>
    PressOut,

    /// <summary>
    /// Animation triggered when the item becomes selected.
    /// </summary>
    SelectIn,

    /// <summary>
    /// Animation triggered when the item is deselected.
    /// </summary>
    SelectOut,

    /// <summary>
    /// Animation triggered when the item becomes enabled again.
    /// </summary>
    Enable,

    /// <summary>
    /// Animation triggered when the item becomes disabled.
    /// </summary>
    Disable
}
