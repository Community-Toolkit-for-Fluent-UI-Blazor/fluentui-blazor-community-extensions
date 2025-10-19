using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the stop button.
/// </summary>
public partial class StopButton
    : FluentComponentBase
{
    /// <summary>
    /// Represents the icon displaye.
    /// </summary>
    private static readonly Icon Icon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Stop();

    /// <summary>
    /// Gets or sets the event callback that is invoked when the stop button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnStop { get; set; }

    /// <summary>
    /// Gets or sets the label for the stop button.
    /// </summary>
    [Parameter]
    public string? Label { get; set; } = "Stop";

    /// <summary>
    /// Gets or sets a value indicating whether the stop button is disabled.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StopButton"/> class.
    /// </summary>
    public StopButton()
    {
        Id = $"stop-button-{Identifier.NewId()}";
    }

    /// <summary>
    /// Occurs when the stop button is clicked.
    /// </summary>
    /// <returns>Returns a task which invokes the <see cref="OnStop" /> callback.</returns>
    private async Task OnStopAsync()
    {
        if (OnStop.HasDelegate)
        {
            await OnStop.InvokeAsync();
        }
    }
}
