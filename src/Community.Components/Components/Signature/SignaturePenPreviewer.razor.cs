using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a previewer for the signature pen settings.
/// </summary>
public partial class SignaturePenPreviewer
    : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the options for configuring the signature pen.
    /// </summary>
    [Parameter]
    public SignaturePenOptions? Options { get; set; }
}
