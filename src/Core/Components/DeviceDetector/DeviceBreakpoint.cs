using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the set of responsive device breakpoints used to adapt UI layouts based on screen size.
/// </summary>
/// <remarks>Use this enumeration to determine or specify layout behavior for different device widths, such as
/// extra small (Xs), small (Sm), medium (Md), large (Lg), extra large (Xl), and extra extra large (Xxl) screens. These
/// breakpoints are typically used in responsive design scenarios to adjust component visibility, sizing, or arrangement
/// according to the current viewport.</remarks>
[JsonConverter(typeof(DeviceBreakpointConverter))]
public enum DeviceBreakpoint
{
    /// <summary>
    /// Represents an unknown or unspecified breakpoint.
    /// </summary>
    Unknown,

    /// <summary>
    /// Represents the extra small breakpoint.
    /// </summary>
    Xs,

    /// <summary>
    /// Represents the small breakpoint.
    /// </summary>
    Sm,

    /// <summary>
    /// Represents the medium breakpoint.
    /// </summary>
    Md,

    /// <summary>
    /// Represents the large breakpoint.
    /// </summary>
    Lg,

    /// <summary>
    /// Represents the extra large breakpoint.
    /// </summary>
    Xl,

    /// <summary>
    /// Represents the extra extra large breakpoint.
    /// </summary>
    Xxl
}
