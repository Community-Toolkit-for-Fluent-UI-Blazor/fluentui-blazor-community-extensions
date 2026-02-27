using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the configuration options for the eraser tool component.
/// </summary>
/// <remarks>Use this class to specify settings and behaviors for the pen tool within the Fluent UI Blazor
/// extension. The options are initialized based on the provided library configuration, which must not be
/// null.</remarks>
public partial class EraserToolOptions
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EraserToolOptions" /> class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to be used for initializing the pen tool options. Cannot be null.</param>
    public EraserToolOptions(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

   
}
