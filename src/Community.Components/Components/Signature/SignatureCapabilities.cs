using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the capabilities of the <see cref="FluentCxSignature"/> component.
/// </summary>
public sealed class SignatureCapabilities
    : FluentComponentBase
{
    /// <summary>
    /// Gets or sets a value indicating whether the user can undo the last stroke.
    /// </summary>
    [Parameter]
    public bool CanUndo { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the user can redo the last undone stroke.
    /// </summary>
    [Parameter]
    public bool CanRedo { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the user can clear the entire signature.
    /// </summary>
    [Parameter]
    public bool CanClear { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the user can export the signature as an image.
    /// </summary>
    [Parameter]
    public bool CanExport { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the user can erase parts of the signature.
    /// </summary>
    [Parameter]
    public bool CanErase { get; set; } = true;
}
