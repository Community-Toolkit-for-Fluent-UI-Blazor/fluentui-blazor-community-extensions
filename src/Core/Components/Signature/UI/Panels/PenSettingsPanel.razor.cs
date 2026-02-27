using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a panel for configuring pen settings within the Fluent UI Blazor extension library.
/// </summary>
/// <remarks>Use this component to provide users with options for customizing pen-related parameters in
/// applications that leverage the Fluent UI Blazor library. The panel is initialized with a library configuration,
/// ensuring consistency with the application's overall Fluent UI setup.</remarks>
public partial class PenSettingsPanel
    : FluentComponentBase
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe PenSettingsPanel avec la configuration de bibliothèque spécifiée.
    /// </summary>
    /// <param name="configuration">La configuration de bibliothèque utilisée pour initialiser le panneau des paramètres du stylo. Ne peut pas être
    /// null.</param>
    public PenSettingsPanel(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the options used to configure the pen engine for signature input.
    /// </summary>
    /// <remarks>Use this property to customize the behavior and appearance of the signature pen, such as
    /// stroke width, color, and smoothing settings. Changing these options affects how the signature is rendered and
    /// captured.</remarks>
    [Parameter]
    public SignaturePenEngineOptions PenEngineOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the rendering options used for the pen in the signature component.
    /// </summary>
    /// <remarks>Use this property to customize the appearance and behavior of the pen when rendering
    /// signatures. Changing these options affects how strokes are displayed within the component.</remarks>
    [Parameter]
    public SignatureRenderingOptions PenRenderingOptions { get; set; } = new();
}
