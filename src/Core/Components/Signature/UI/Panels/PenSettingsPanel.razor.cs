using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a panel for configuring pen settings within the Fluent UI Blazor extension library.
/// </summary>
/// <remarks>Use this component to provide users with options for customizing pen-related parameters in
/// applications that leverage the Fluent UI Blazor library. The panel is initialized with a library configuration,
/// ensuring consistency with the application's overall Fluent UI setup.</remarks>
public partial class PenSettingsPanel
    : FluentDialogInstance
{
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

    /// <inheritdoc />
    protected override async Task OnActionClickedAsync(bool primary)
    {
        if (primary)
        {
            await DialogInstance.CloseAsync(new object[]
            {
                PenEngineOptions,
                PenRenderingOptions
            });
        }
        else
        {
            await DialogInstance.CancelAsync();
        }
    }
}
