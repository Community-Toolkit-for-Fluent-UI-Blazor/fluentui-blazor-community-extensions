namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the available animation effects for popups in the Fluent UI framework.
/// </summary>
/// <remarks>This enumeration provides a set of predefined animation options, including directional slides, fade,
/// scale, and combined effects. Developers can also select the Custom value to apply a user-defined CSS class for
/// custom animations.</remarks>
public enum PopupAnimation
{
    /// <summary>
    /// No animation effect will be applied when the popup is shown or hidden.
    /// </summary>
    None,

    /// <summary>
    /// Slides the element, hiding it from view with a smooth transition effect.
    /// </summary>
    Slide,

    /// <summary>
    /// Slides and scales the element simultaneously, creating a combined animation effect that changes both position and size.
    /// </summary>
    SlideScale,

    /// <summary>
    /// Fades the element in or out by gradually changing its opacity.
    /// </summary>
    Fade,
    /// <summary>
    /// Scales the element up or down by smoothly increasing or decreasing its size,
    /// </summary>
    Scale,

    /// <summary>
    /// Fades and scales the element simultaneously, creating a combined animation effect that changes both opacity and size.
    /// </summary>
    FadeScale,

    /// <summary>
    /// Represents a custom animation effect defined by the user through a CSS class.
    /// </summary>
    Custom
}

