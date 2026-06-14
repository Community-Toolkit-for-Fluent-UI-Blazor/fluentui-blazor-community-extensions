using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a panel that provides options for exporting data to the console.
/// </summary>
public partial class ConsoleExportOptionsPanel
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the ConsoleExportOptionsPanel class using the specified library configuration.
    /// </summary>
    /// <param name="libraryConfiguration">The configuration settings for the library that determine how console export options are applied.</param>
    public ConsoleExportOptionsPanel(LibraryConfiguration libraryConfiguration)
        : base(libraryConfiguration)
    {
    }

    /// <summary>
    /// Gets or sets the options for exporting to the console.
    /// </summary>
    [Parameter]
    public ConsoleExportOptions Options { get; set; } = new ConsoleExportOptions();

    /// <summary>
    /// Gets or sets the options for exporting console data to a file.
    /// </summary>
    [Parameter]
    public ConsoleExportFileOptions FileOptions { get; set; } = new ConsoleExportFileOptions();
}
