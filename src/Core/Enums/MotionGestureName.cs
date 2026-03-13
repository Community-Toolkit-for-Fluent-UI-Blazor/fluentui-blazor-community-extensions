namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the types of motion gestures that can be recognized or handled by a component.
/// </summary>
/// <remarks>Use this enumeration to identify user interactions such as hovering, pressing, tapping, or dragging
/// within UI components. The values correspond to common touch or pointer gestures in interactive
/// applications.</remarks>
public enum MotionGestureName
{
    /// <summary>
    /// Represents the hover state for a <see cref="MotionItem"/>.
    /// </summary>
    Hover,

    /// <summary>
    /// Represents the press state for a <see cref="MotionItem"/>.
    /// </summary>
    Press,

    /// <summary>
    /// Represents the tap state for a <see cref="MotionItem"/>.
    /// </summary>
    Tap,

    /// <summary>
    /// Represents the drag state for a <see cref="MotionItem"/>.
    /// </summary>
    Drag
}
