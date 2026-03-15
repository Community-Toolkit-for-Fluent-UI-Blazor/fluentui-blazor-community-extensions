using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a dialog panel for configuring export settings within a Fluent UI Blazor application.
/// </summary>
/// <remarks>This component extends the FluentDialogInstance to provide a user interface for managing
/// export-related options. It is intended to be used as part of a dialog workflow where users can confirm or cancel
/// export actions.</remarks>
public partial class ExportSettingsPanel
    : FluentDialogInstance
{
    /// <summary>
    /// Gets or sets the options that configure the export format for the component.
    /// </summary>
    /// <remarks>Use this property to specify settings such as file type, encoding, or other format-related
    /// options when exporting data. The available options are defined by the ExportFormatOptions class.</remarks>
    [Parameter]
    public SurfaceExportOptions Options { get; set; } = new();

    /// <summary>
    /// Gets or sets the format used to export the surface.
    /// </summary>
    [Parameter]
    public SurfaceExportFormat SurfaceExportFormat { get; set; } = SurfaceExportFormat.All;

    /// <inheritdoc />
    protected override async Task OnActionClickedAsync(bool primary)
    {
        if (primary)
        {
            await DialogInstance.CloseAsync(Options);
        }
        else
        {
            await DialogInstance.CancelAsync();
        }
    }
}
