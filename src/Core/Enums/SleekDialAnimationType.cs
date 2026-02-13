namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the available animation types for the Sleek Dial interface.
/// </summary>
/// <remarks>Each animation type defines a distinct visual effect that is applied when the dial is interacted
/// with. Selecting an appropriate animation can enhance user experience by providing visual feedback and improving the
/// interface's responsiveness.</remarks>
public enum SleekDialAnimationType
{
    /// <summary>
    /// Specifies that no animation is applied.
    /// </summary>
    None,

    /// <summary>
    /// Represents the fade effect applied to the visual element.
    /// </summary>
    Fade,

    /// <summary>
    /// Represents the scale factor applied to the visual element.
    /// </summary>
    Scale,

    /// <summary>
    /// Represents the slide effect applied to the visual element, where it moves in a specified direction.
    /// </summary>
    Slide,

    /// <summary>
    /// Represents a transformation that applies a radial sweep effect to a graphical element, rotating it around a
    /// specified center point.
    /// </summary>
    RadialSweep,

    /// <summary>
    /// Represents the orbital path of an object in space, defining its trajectory and key characteristics.
    /// </summary>
    Orbit,

    /// <summary>
    /// Represents a staggered layout for arranging items in a collection, allowing for variable spacing between
    /// elements.
    /// </summary>
    Staggered
}

