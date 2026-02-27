using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the configuration settings for the pen tool component, providing options for customizing pen behavior and
/// appearance.
/// </summary>
/// <remarks>Use this class to specify and manage pen-related settings within Fluent UI Blazor extensions. The
/// settings are initialized using a provided configuration object, which must not be null. This class is intended for
/// scenarios where pen customization is required, such as drawing or annotation features.</remarks>
public partial class PanelSettings
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PanelSettings"/> class.
    /// </summary>
    /// <param name="configuration">The configuration settings to be used for initializing the pen tool options. Cannot be null.</param>
    public PanelSettings(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the callback that is invoked when the component is clicked.
    /// </summary>
    /// <remarks>Use this property to handle click events for the component. The callback is triggered when a
    /// user interaction causes a click event. If no callback is assigned, the click event will be ignored.</remarks>
    [Parameter]
    public EventCallback OnClick { get; set; }
}
