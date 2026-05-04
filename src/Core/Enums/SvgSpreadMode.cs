namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the method by which a gradient is extended beyond its normal range in SVG rendering.
/// </summary>
/// <remarks>Use this enumeration to control how gradient fills are applied when the gradient vector does not
/// fully cover the target area. The spread mode determines how the colors are repeated or extended outside the
/// gradient's defined range.</remarks>
public enum SvgSpreadMode
{
    /// <summary>
    /// Specifies that the gradient should not be extended beyond its normal range.
    /// </summary>
    Pad,

    /// <summary>
    /// Specifies that the gradient should be reflected outside the normal range.
    /// </summary>
    Reflect,

    /// <summary>
    /// Specifies that the gradient should be repeated in a tiled pattern.
    /// </summary>
    Repeat
}
