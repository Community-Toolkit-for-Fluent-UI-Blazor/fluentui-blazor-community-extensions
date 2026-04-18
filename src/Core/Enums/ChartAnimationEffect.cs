namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the available animation effects that can be applied to chart elements.
/// </summary>
/// <remarks>Use this enumeration to select the type of visual animation effect when displaying or updating chart
/// data. The choice of effect can enhance the user experience by providing visual feedback during data
/// transitions.</remarks>
public enum ChartAnimationEffect
{
    /// <summary>
    /// No animation effect.
    /// </summary>
    None,

    /// <summary>
    /// Specifies the available fade animation options.
    /// </summary>
    Fade,

    /// <summary>
    /// Specifies the available scale animation options.
    /// </summary>
    Scale,

    /// <summary>
    /// Specifies the available sweep animation options.
    /// </summary>
    Sweep,

    /// <summary>
    /// Specifies the available slide animation options.
    /// </summary>
    Slide,

    /// <summary>
    /// Specifies a bounce animation effect.
    /// </summary>
    Bounce
}
