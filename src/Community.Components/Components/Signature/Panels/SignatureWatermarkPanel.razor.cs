using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a panel for watermarking a signature with configurable content options.
/// </summary>
/// <remarks>This component allows users to specify the content options for watermarking a signature. The <see
/// cref="Content"/> property can be used to configure the watermark settings.</remarks>
public partial class SignatureWatermarkPanel
{
    /// <summary>
    /// Gets or sets the content options for creating a watermark on a signature.
    /// </summary>
    [Parameter]
    public (SignatureLabels, SignatureWatermarkOptions) Content { get; set; } = default!;
}
