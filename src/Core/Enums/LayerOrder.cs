namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the possible rendering orders for visual layers within a component or control.
/// </summary>
/// <remarks>Use this enumeration to define or query the stacking order of layers when composing complex UI
/// elements. The specific values and their intended usage depend on the context in which the enumeration is
/// applied.</remarks>
public enum LayerOrder
{
    /// <summary>
    /// Layer for background elements, rendered behind all other layers.
    /// </summary>
    Background,

    /// <summary>
    /// Layer for elements that should be rendered behind the main content but in front of the background.
    /// </summary>
    Back,

    /// <summary>
    /// Layer for the main content, rendered in front of the background and back layers, but behind the front and overlay layers.
    /// </summary>
    Content,

    /// <summary>
    /// Layer for elements that should be rendered in front of the main content but behind the overlay layer.
    /// </summary>
    Front,

    /// <summary>
    /// Layer for elements that should be rendered in front of all other layers.
    /// </summary>
    Overlay
}
