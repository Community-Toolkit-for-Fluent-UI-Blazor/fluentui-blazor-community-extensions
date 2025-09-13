using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a panel for configuring the signature pen settings.
/// </summary>
/// <remarks>This component allows users to customize the appearance and behavior of the signature pen by setting
/// various options through the <see cref="Content"/> property.</remarks>
public partial class SignaturePenPanel
{
    /// <summary>
    /// Gets or sets the options for configuring the signature pen.
    /// </summary>
    [Parameter]
    public (SignatureLabels, SignaturePenOptions) Content { get; set; } = default!;
}
