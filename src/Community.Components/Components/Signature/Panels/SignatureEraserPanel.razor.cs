using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a panel for erasing a signature with configurable content options.
/// </summary>
/// <remarks>This component allows users to specify the content options for erasing a signature. The <see
/// cref="Content"/> property can be used to configure the export settings.</remarks>
public partial class SignatureEraserPanel
{
    /// <summary>
    /// Gets or sets the content options for erasing a signature.
    /// </summary>
    [Parameter]
    public (SignatureLabels, SignatureEraserOptions) Content { get; set; } = default!;
}
