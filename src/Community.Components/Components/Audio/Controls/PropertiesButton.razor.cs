using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a button component designed for initiating properties actions.
/// </summary>
public partial class PropertiesButton : FluentComponentBase
{
    /// <summary>
    /// Represents the icon to be displayed on the button.
    /// </summary>
    private static readonly Icon Icon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.ArrowDownload();

    /// <summary>
    /// Gets or sets the event callback that is invoked when the download button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnProperties { get; set; }

    /// <summary>
    /// Gets or sets the label for the download button.
    /// </summary>
    [Parameter]
    public string? Label { get; set; } = "Properties";

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertiesButton"/> class.
    /// </summary>
    public PropertiesButton()
    {
        Id = $"properties-button-{Identifier.NewId()}";
    }

    /// <summary>
    /// Occurs when the download button is clicked.
    /// </summary>
    /// <returns>Returns a task which invokes the <see cref="OnProperties" /> callback.</returns>
    private async Task OnClickAsync()
    {
        if (OnProperties.HasDelegate)
        {
            await OnProperties.InvokeAsync();
        }
    }
}
