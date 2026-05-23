namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the visual styles available for highlighting a target during drag-and-drop operations.
/// </summary>
/// <remarks>Use this enumeration to select the type of visual feedback presented to users when an item is dragged
/// over a drop target. Each style provides a distinct visual cue to enhance the drag-and-drop experience. The choice of
/// style can affect user perception and accessibility.</remarks>
public enum TileDragOverHighlightStyle
{
    /// <summary>
    /// Represents a glow effect that can be applied to the target element.
    /// </summary>
    Glow,

    /// <summary>
    /// Represents a side bar highlight effect on the target element.
    /// </summary>
    SideBar,

    /// <summary>
    /// Represents a focus ring highlight effect on the target element.
    /// </summary>
    FocusRing,

    /// <summary>
    /// Represents an animated glow effect that can be applied to the target element.
    /// </summary>
    AnimatedGlow
}
