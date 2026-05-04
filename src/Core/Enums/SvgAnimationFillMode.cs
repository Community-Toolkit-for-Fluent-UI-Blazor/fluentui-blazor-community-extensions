namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Represents the fill mode for SVG animations,
///  determining how the animation's effects are applied after it completes.
/// </summary>
public enum SvgAnimationFillMode
{
    /// <summary>
    /// The animation's effects are not applied after it completes; the element returns to its original state.
    /// </summary>
    Remove,

    /// <summary>
    /// The animation's effects are applied after it completes, and the element retains the final state of the animation.
    /// </summary>
    Freeze,
}
