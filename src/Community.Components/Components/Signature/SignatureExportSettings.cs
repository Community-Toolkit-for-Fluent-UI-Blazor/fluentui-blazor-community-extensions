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

    /// <summary>
    /// Updates the internal values of the export settings based on the specified signature state.
    /// </summary>
    /// <param name="state">The <see cref="SignatureState"/> containing the updated quality and export format values.</param>
    internal void UpdateInternalValues(SignatureState state)
    {
        Quality = state.Quality;
        Format = state.ExportFormat;
    }
}
