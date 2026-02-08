using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a panel that displays console details, allowing for customization of the console message shown to the
/// user.
/// </summary>
/// <remarks>This class is initialized with a library configuration that dictates how console details are
/// presented. The Message property can be set to customize the displayed console message.</remarks>
public partial class ConsoleDetailsPanel
    : FluentComponentBase
{
    /// <summary>
    /// Provides options for configuring JSON serialization, including formatting settings.
    /// </summary>
    /// <remarks>This instance of <see cref="JsonSerializerOptions"/> is configured to write JSON in an
    /// indented format, making it more readable. It is intended for use in scenarios where human-readable output is
    /// preferred.</remarks>
    private static readonly JsonSerializerOptions s_json = new() { WriteIndented = true };

    /// <summary>
    /// Initializes a new instance of the ConsoleDetailsPanel class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the library that determine how console details are displayed.</param>
    public ConsoleDetailsPanel(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the console message to be displayed in the panel.
    /// </summary>
    /// <remarks>This property allows customization of the message shown in the console details panel. The
    /// default value is a new instance of the ConsoleMessage class.</remarks>
    [Parameter]
    public ConsoleMessage Message { get; set; } = new ConsoleMessage();
}
