using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a button component designed for initiating download actions.
/// </summary>
/// <remarks>The <see cref="DownloadButton"/> component provides a customizable button with an optional label and
/// icon. It supports an event callback that is triggered when the button is clicked, allowing developers to handle
/// download-related logic in their applications.</remarks>
public partial class DownloadButton : FluentComponentBase
{
    /// <summary>
    /// Represents the icon to be displayed on the button.
    /// </summary>
    private static readonly Icon DownloadIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.ArrowDownload();

    /// <summary>
    /// Gets or sets the event callback that is invoked when the download button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnDownload { get; set; }

    /// <summary>
    /// Gets or sets the label for the download button.
    /// </summary>
    [Parameter]
    public string? DownloadLabel { get; set; } = "Download";

    /// <summary>
    /// Initializes a new instance of the <see cref="DownloadButton"/> class.
    /// </summary>
    public DownloadButton()
    {
        Id = $"download-button-{Identifier.NewId()}";
    }

    /// <summary>
    /// Occurs when the download button is clicked.
    /// </summary>
    /// <returns>Returns a task which invokes the <see cref="OnDownload" /> callback.</returns>
    private async Task OnDownloadAsync()
    {
        if (OnDownload.HasDelegate)
        {
            await OnDownload.InvokeAsync();
        }
    }
}
