namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for customizing the visual appearance and layout of a rendered barcode, including
/// background, grid, axes, watermark, quiet zone, label, and symbology settings.
/// </summary>
/// <remarks>Use this class to specify detailed rendering preferences when generating barcodes. Each property
/// allows fine-grained control over a specific aspect of the barcode's presentation, enabling consistent and branded
/// output across different barcode types.</remarks>
public sealed class BarcodeRenderingOptions
{
    /// <summary>
    /// Gets or sets the background options for the surface component.
    /// </summary>
    public SurfaceBackgroundOptions Background { get; set; } = new();

    /// <summary>
    /// Gets or sets the options that define the foreground appearance for the surface component.
    /// </summary>
    /// <remarks>Use this property to customize the foreground styling, such as text or icon color, for the
    /// surface. The specific effects depend on the values provided in the SurfaceForegroundOptions object.</remarks>
    public SurfaceForegroundOptions Foreground { get; set; } = new();

    /// <summary>
    /// Gets or sets the options of the view for the surface component.
    /// </summary>
    public SurfaceViewOptions View { get; set; } = new();

    /// <summary>
    /// Gets or sets the options that define the quiet zone around the barcode.
    /// </summary>
    /// <remarks>The quiet zone is the blank margin surrounding a barcode that helps scanners distinguish the
    /// barcode from its surroundings. Adjusting these options can affect barcode readability and compliance with
    /// barcode standards.</remarks>
    public BarcodeQuietZoneOptions QuietZone { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to configure the label displayed with the barcode.
    /// </summary>
    public BarcodeLabelOptions Label { get; set; } = new();
}

