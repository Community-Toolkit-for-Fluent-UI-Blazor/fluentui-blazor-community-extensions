namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the easing functions available for color transitions in the ColorPalette component.
/// </summary>
public enum ColorPaletteEasing
{
    /// <summary>
    /// Represents a linear easing function, where the transition progresses at a constant rate.
    /// </summary>
    Linear,

    /// <summary>
    /// Represents an ease-in easing function, where the transition starts slowly and accelerates towards the end.
    /// </summary>
    ExponentialIn,

    /// <summary>
    /// Represents an ease-out easing function, where the transition starts quickly and decelerates towards the end.
    /// </summary>
    ExponentialOut,

    /// <summary>
    /// Represents an ease-in-out easing function, where the transition starts slowly, accelerates in the middle, and decelerates towards the end.
    /// </summary>
    Sine
}
