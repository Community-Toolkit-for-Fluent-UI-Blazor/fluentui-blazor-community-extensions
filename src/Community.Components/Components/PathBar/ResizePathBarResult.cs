namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the result of the javascript refresh method.
/// </summary>
public class RefreshPathBarResult
{
    /// <summary>
    /// Gets or sets the overflow items.
    /// </summary>
    public List<string> OverflowItems { get; set; } = [];

    /// <summary>
    /// Gets or sets the visible items.
    /// </summary>
    public List<string> VisibleItems { get; set; } = [];

    /// <summary>
    /// Gets or sets the text of the last visible item.
    /// </summary>
    public string? LastVisibleItemText { get; set; }
}
