namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the hide mode for a SleekDial component.
/// </summary>
public enum SleekDialHideMode
{
    /// <summary>
    /// SleekDial is always visible.
    /// </summary>
    None,

    /// <summary>
    /// SleekDial is hidden when there are no items to display.
    /// </summary>
    WhenEmpty,

    /// <summary>
    /// SleekDial is hidden when there are no visible items to display.
    /// </summary>
    WhenNoVisible,

    /// <summary>
    /// SleekDial is hidden when there are no items or no visible items to display.
    /// </summary>
    WhenEmptyOrNoVisible
}
