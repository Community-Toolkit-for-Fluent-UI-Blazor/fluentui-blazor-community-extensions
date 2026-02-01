namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the visibility options for the open button in a cookie consent component.
/// </summary>
public enum OpenCookieVisibility
{
    /// <summary>
    /// The open button is always visible.
    /// </summary>
    Always,

    /// <summary>
    /// The open button is never visible.
    /// </summary>
    Never,

    /// <summary>
    /// The open button is visible only when the first cookie consent banner is hidden.
    /// </summary>
    WhenFirstHidden
}
