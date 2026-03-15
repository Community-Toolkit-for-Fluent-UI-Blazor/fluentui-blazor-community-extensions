using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a UI component that provides a clear tool button.
/// </summary>
public partial class ClearTool
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClearTool"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this component. Cannot be null.</param>
    public ClearTool(LibraryConfiguration configuration)
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
