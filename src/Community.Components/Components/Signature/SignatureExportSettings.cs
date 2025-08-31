using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents settings for exporting a signature.
/// </summary>
public sealed class SignatureExportSettings
    : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the format in which the signature will be exported.
    /// </summary>
    [Parameter]
    public SignatureExportFormat Format { get; set; } = SignatureExportFormat.Webp;

    /// <summary>
    /// Gets or sets the quality of the exported image. This is relevant for lossy formats like JPEG and WebP.
    /// </summary>
    [Parameter]
    public int Quality { get; set; } = 90;
}
