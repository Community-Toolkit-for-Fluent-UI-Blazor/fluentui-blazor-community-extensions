namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the relative priority of a layer when rendering multiple overlapping layers.
/// </summary>
/// <remarks>Use this enumeration to control the stacking order of layers in a user interface. Higher priority
/// values indicate that a layer should appear above those with lower priority. This is commonly used in scenarios where
/// popups, dialogs, or overlays must be displayed above other content.</remarks>
public enum LayerPriority
{
    /// <summary>
    /// Lowest priority layer, rendered behind all other layers.
    /// </summary>
    Lowest,

    /// <summary>
    /// Low priority layer, rendered above the lowest priority but below normal priority layers.
    /// </summary>
    Low,

    /// <summary>
    /// Normal priority layer, rendered above low priority layers and below high priority layers.
    /// </summary>
    Normal,

    /// <summary>
    /// High priority layer, rendered above normal priority layers and below the highest priority layer.
    /// </summary>
    High,

    /// <summary>
    /// Highest priority layer, rendered above all other layers.
    /// </summary>
    Highest
}
