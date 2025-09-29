namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the export options for the signature component.
/// </summary>
public sealed class SignatureExportOptions
{
    /// <summary>
    /// Gets or sets the image format for exporting the signature. Default is PNG.
    /// </summary>
    public SignatureImageFormat Format { get; set; } = SignatureImageFormat.Png;

    /// <summary>
    /// Gets or sets the quality of the exported image, ranging from 0 to 100.
    /// </summary>
    public int Quality { get; set; } = 100;

    /// <summary>
    /// Resets all properties to their default values.
    /// </summary>
    public void Reset()
    {
        Format = SignatureImageFormat.Png;
        Quality = 100;
    }
}
