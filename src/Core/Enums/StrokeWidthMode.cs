namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the available modes for determining stroke width in rendering operations.
/// </summary>
/// <remarks>Use this enumeration to select how stroke width is calculated or applied. The mode chosen can affect
/// the appearance and scaling of rendered strokes, depending on the rendering context.</remarks>
public enum StrokeWidthMode
{
    /// <summary>
    /// Specifies a default mode.
    /// </summary>
    Default,

    /// <summary>
    /// Specifies a linear progression or interpolation mode.
    /// </summary>
    Linear,

    /// <summary>
    /// Specifies a power-based progression mode, where stroke width may be determined by raising a value to a specified exponent.
    /// </summary>
    Power
}
