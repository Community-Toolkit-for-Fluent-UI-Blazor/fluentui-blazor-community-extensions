namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the visual layer of a grid component, such as background or foreground, used to control rendering order
/// and appearance.
/// </summary>
public enum GridLayer
{
    /// <summary>
    /// The grid is rendered in the background layer, behind other content.
    /// </summary>
    Background,

    /// <summary>
    /// The grid is rendered in the foreground layer, in front of other content.
    /// </summary>
    Foreground
}
