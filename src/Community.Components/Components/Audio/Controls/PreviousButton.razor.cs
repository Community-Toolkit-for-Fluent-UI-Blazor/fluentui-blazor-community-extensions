using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the previous button.
/// </summary>
/// <remarks>The <see cref="PreviousButton"/> component provides functionality to toggle between shuffle modes for
/// a collection. It displays an appropriate icon based on the current shuffle state and invokes a callback when the
/// shuffle state changes.</remarks>
public partial class PreviousButton
    : FluentComponentBase
{
    /// <summary>
    /// Represents the icon displayed when shuffling is disabled.
    /// </summary>
    private static readonly Icon PreviousIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Previous();

    /// <summary>
    /// Gets or sets the event callback that is invoked when the stop button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnPrevious { get; set; }

    /// <summary>
    /// Gets or sets the label for the previous button.
    /// </summary>
    [Parameter]
    public string? PreviousLabel { get; set; } = "Previous";

    /// <summary>
    /// Gets or sets a value indicating whether the previous button is disabled.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PreviousButton"/> class.
    /// </summary>
    public PreviousButton()
    {
        Id = $"previous-button-{Identifier.NewId()}";
    }

    /// <summary>
    /// Occurs when the previous button is clicked.
    /// </summary>
    /// <returns>Returns a task which invokes the <see cref="OnPrevious" /> callback.</returns>
    private async Task OnPreviousAsync()
    {
        if (OnPrevious.HasDelegate)
        {
            await OnPrevious.InvokeAsync();
        }
    }
}
