using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the PiP button.
/// </summary>
public partial class PiPButton
    : FluentComponentBase
{
    /// <summary>
    /// Represents the icon displayed when shuffling is disabled.
    /// </summary>
    private static readonly Icon Icon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.PictureIn();

    /// <summary>
    /// Gets or sets the event callback that is invoked when the theater button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnPiP { get; set; }

    /// <summary>
    /// Gets or sets the label for the next button.
    /// </summary>
    [Parameter]
    public string? Label { get; set; } = "Picture In Picture";

    /// <summary>
    /// Gets or sets a value indicating whether the theater button is disabled.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TheaterButton"/> class.
    /// </summary>
    public PiPButton()
    {
        Id = $"pip-button-{Identifier.NewId()}";
    }

    /// <summary>
    /// Occurs when the next button is clicked.
    /// </summary>
    /// <returns>Returns a task which invokes the <see cref="OnPiP" /> callback.</returns>
    private async Task OnClickAsync()
    {
        if (OnPiP.HasDelegate)
        {
            await OnPiP.InvokeAsync();
        }
    }
}
