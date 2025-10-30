using System.ComponentModel;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents angle units for CSS properties.
/// </summary>
public enum AngleUnit
{
    /// <summary>
    /// Represents degrees, the most common unit for angles.
    /// </summary>
    [Description("deg")]
    Degrees,

    /// <summary>
    /// Represents radians, a mathematical unit for angles.
    /// </summary>
    [Description("rad")]
    Radians,

    /// <summary>
    /// Represents gradians, a less common unit where a right angle is 100 gradians.
    /// </summary>
    [Description("grad")]
    Gradians,

    /// <summary>
    /// Represents turns, where a full circle is 1 turn.
    /// </summary>
    [Description("turn")]
    Turns
}
