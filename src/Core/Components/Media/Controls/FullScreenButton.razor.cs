using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the fullscreen button.
/// </summary>
public partial class FullScreenButton
    : FluentComponentBase
{
    /// <summary>
    /// Represents the icon displayed for the fullscreen button.
    /// </summary>
    private static readonly Icon Icon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.FullScreenMaximize();

    /// <summary>
    /// Initializes a new instance of the <see cref="NextButton"/> class.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this component.</param>
    public FullScreenButton(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = $"fullscreen-button-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the next button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnFullscreen { get; set; }

    /// <summary>
    /// Gets or sets the label for the next button.
    /// </summary>
    [Parameter]
    public string? Label { get; set; } = "Fullscreen";

    /// <summary>
    /// Gets or sets a value indicating whether the next button is disabled.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Occurs when the next button is clicked.
    /// </summary>
    /// <returns>Returns a task which invokes the <see cref="OnFullscreen" /> callback.</returns>
    private async Task OnClickAsync()
    {
        if (OnFullscreen.HasDelegate)
        {
            await OnFullscreen.InvokeAsync();
        }
    }
}
