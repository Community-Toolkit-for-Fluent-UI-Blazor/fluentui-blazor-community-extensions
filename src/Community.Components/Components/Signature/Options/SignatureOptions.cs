namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the configuration options for a signature component, including settings for the pen, eraser, grid, and
/// watermark.
/// </summary>
/// <remarks>This class provides a collection of customizable options that control the behavior and appearance of
/// a signature component. Use the properties to configure specific aspects such as the pen style, eraser functionality,
/// grid layout, and watermark display.</remarks>
public class SignatureOptions
{
    /// <summary>
    /// Gets or sets the options for configuring the pen used in the signature component.
    /// </summary>
    public SignaturePenOptions Pen { get; set; } = new();

    /// <summary>
    /// Gets or sets the options for configuring the eraser used in the signature component.
    /// </summary>
    public SignatureEraserOptions Eraser { get; set; } = new();

    /// <summary>
    /// Gets or sets the options for configuring the grid displayed in the signature component.
    /// </summary>
    public SignatureGridOptions Grid { get; set; } = new();

    /// <summary>
    /// Gets or sets the options for configuring the watermark displayed in the signature component.
    /// </summary>
    public SignatureWatermarkOptions Watermark { get; set; } = new();

    /// <summary>
    /// Gets or sets the options for configuring the export settings of the signature component.
    /// </summary>
    public SignatureExportOptions Export { get; set; } = new();
}
