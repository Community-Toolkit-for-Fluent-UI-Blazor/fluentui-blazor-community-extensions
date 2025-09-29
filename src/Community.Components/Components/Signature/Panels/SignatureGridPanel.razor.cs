using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a panel for creating a grid on a signature with configurable content options.
/// </summary>
/// <remarks>This component allows users to specify the content options for creating a grid on a signature. The <see
/// cref="Content"/> property can be used to configure the grid settings.</remarks>
public partial class SignatureGridPanel
{
    /// <summary>
    /// Gets or sets the content options for exporting a signature.
    /// </summary>
    [Parameter]
    public (SignatureLabels, SignatureGridOptions) Content { get; set; } = default!;
}
