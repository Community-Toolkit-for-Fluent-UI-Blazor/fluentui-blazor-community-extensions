namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the preferred direction for displaying a popup relative to its target element.
/// </summary>
/// <remarks>Use this enumeration to indicate the desired placement of a popup. The actual direction may be
/// adjusted based on available space or layout constraints.</remarks>
public enum PreferredPopupDirection
{
    /// <summary>
    /// Represents the default value for the enumeration.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Specifies that the content is aligned to the left.
    /// </summary>
    Left = 1,

    /// <summary>
    /// Specifies that the content is aligned to the right.
    /// </summary>
    Right = 2,

    /// <summary>
    /// Specifies that the direction is upward.
    /// </summary>
    Up = 3,

    /// <summary>
    /// Represents the downward direction in the enumeration.
    /// </summary>
    Down = 4
}
