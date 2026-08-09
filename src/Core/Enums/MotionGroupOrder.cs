namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the order in which motion effects are applied relative to a component's child elements.
/// </summary>
/// <remarks>Use this enumeration to control whether a motion effect occurs before or after the animations of
/// child elements. This can be useful when coordinating complex animation sequences in UI components.</remarks>
public enum MotionGroupOrder
{
    /// <summary>
    /// Specifies that the associated operation or content should occur before any child elements are processed.
    /// </summary>
    BeforeChildren,

    /// <summary>
    /// Specifies that the associated operation or content should occur after all child elements have been processed.
    /// </summary>
    AfterChildren
}
