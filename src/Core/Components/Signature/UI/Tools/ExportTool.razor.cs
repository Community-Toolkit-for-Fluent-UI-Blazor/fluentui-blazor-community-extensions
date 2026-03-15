using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a tool component that provides export functionality within the library.
/// </summary>
/// <remarks>Use this component to enable export-related actions in your application. The component can be
/// configured with a callback to handle clear tool button clicks. Ensure that a valid library configuration is provided
/// when initializing the component.</remarks>
public partial class ExportTool
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExportTool"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this component. Cannot be null.</param>
    public ExportTool(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the callback that is invoked when the clear tool button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }
}
