using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a button component designed for initiating chapter actions.
/// </summary>
/// <remarks>The <see cref="ChapterButton"/> component provides a customizable button with an optional label and
/// icon. It supports an event callback that is triggered when the button is clicked, allowing developers to handle
/// chapter-related logic in their applications.</remarks>
public partial class ChapterButton : FluentComponentBase
{
    /// <summary>
    /// Represents the icon to be displayed on the button.
    /// </summary>
    private static readonly Icon Icon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.BookNumber();

    /// <summary>
    /// Indicates whether the chapter should be displayed.
    /// </summary>
    private bool _showChapters;

    /// <summary>
    /// Gets or sets the event callback that is invoked when the download button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnChapters { get; set; }

    /// <summary>
    /// Gets or sets the label for the download button.
    /// </summary>
    [Parameter]
    public string? Label { get; set; } = "Chapters";

    /// <summary>
    /// Gets or sets a value indicating whether the download button is disabled.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChapterButton"/> class.
    /// </summary>
    public ChapterButton()
    {
        Id = $"chapter-button-{Identifier.NewId()}";
    }

    /// <summary>
    /// Occurs when the download button is clicked.
    /// </summary>
    /// <returns>Returns a task which invokes the <see cref="OnChapters" /> callback.</returns>
    private async Task OnClickAsync()
    {
        _showChapters = !_showChapters;

        if (OnChapters.HasDelegate)
        {
            await OnChapters.InvokeAsync(_showChapters);
        }
    }
}
