using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the options for displaying the menu in the Signature component.
/// </summary>
public sealed class MenuOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the menu is visible.
    /// </summary>
    [Parameter]
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the export button is shown.
    /// </summary>
    [Parameter]
    public bool ShowExport { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the undo button is shown.
    /// </summary>
    [Parameter]
    public bool ShowUndo { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the redo button is shown.
    /// </summary>
    [Parameter]
    public bool ShowRedo { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the clear button is shown.
    /// </summary>
    [Parameter]
    public bool ShowClear { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the eraser button is shown.
    /// </summary>
    [Parameter]
    public bool ShowEraser { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the custom options button is shown.
    /// </summary>
    [Parameter]
    public bool ShowCustomOptions { get; set; } = true;
}
