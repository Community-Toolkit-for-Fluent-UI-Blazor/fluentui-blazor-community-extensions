namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Represents the threshold levels for determining visibility intersection in chart rendering.
/// </summary>
public enum VisibilityIntersectionTheshold
{
    /// <summary>
    /// Specifies a 10% threshold for visibility intersection.
    /// </summary>
    Smooth,

    /// <summary>
    /// Specifies a 25% threshold for visibility intersection.
    /// </summary>
    Balanced,

    /// <summary>
    /// Specifies a 50% threshold for visibility intersection.
    /// </summary>
    Strict
}
