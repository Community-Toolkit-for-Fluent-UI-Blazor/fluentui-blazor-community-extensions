namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for exporting signature images, including settings for background color, padding,
/// grid and watermark inclusion, cropping, DPI, and JPEG quality.
/// </summary>
/// <remarks>Use this class to customize the appearance and quality of exported signature images. The options
/// allow control over visual elements and output parameters for formats such as PNG and JPEG. Adjust properties as
/// needed before initiating an export operation.</remarks>
public sealed class SurfaceExportOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the data should be exported in JSON format.
    /// </summary>
    public bool ExportToJson { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the export operation should generate output in SVG format.
    /// </summary>
    public bool ExportSvg { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether data should be exported in binary format.
    /// </summary>
    public bool ExportBinary { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether images are exported in the AVIF format.
    /// </summary>
    /// <remarks>Set this property to <see langword="true"/> to enable exporting images in the AVIF format,
    /// which may provide improved compression and quality compared to other formats. AVIF support may depend on the
    /// target platform or browser capabilities.</remarks>
    public bool ExportAvif { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the export operation includes BMP format output.
    /// </summary>
    public bool ExportBmp { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to export images in the HEIF format.
    /// </summary>
    /// <remarks>Set this property to <see langword="true"/> to enable exporting images using the High
    /// Efficiency Image File Format (HEIF), which may provide better compression and quality compared to other formats.
    /// Not all platforms or viewers may support HEIF files.</remarks>
    public bool ExportHeif { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the export operation should generate a JPEG file.
    /// </summary>
    public bool ExportJpeg { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the content should be exported as a PNG image.
    /// </summary>
    public bool ExportPng { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the export should include TIFF format output.
    /// </summary>
    public bool ExportTiff { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether images are exported in the WebP format.
    /// </summary>
    public bool ExportWebp { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include the background in the exported image.
    /// </summary>
    public bool IncludeBackground { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include the grid in the exported image.
    /// </summary>
    public bool IncludeGrid { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include the axes in the exported image.
    /// </summary>
    public bool IncludeAxes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include the watermark in the exported image.
    /// </summary>
    public bool IncludeWatermark { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include the view in the exported image.
    /// </summary>
    public bool IncludeView { get; set; }

    /// <summary>
    /// Gets or sets the quality of the exported document.
    /// </summary>
    public int Quality { get; set; } = 95;

    /// <summary>
    /// Resets all export options to their default values.
    /// </summary>
    /// <remarks>Use this method to restore the export settings to their initial state. This is useful when
    /// you want to discard any customizations and start with the default configuration.</remarks>
    public void Reset()
    {
        IncludeBackground = false;
        IncludeAxes = false;
        IncludeWatermark = false;
        IncludeGrid = false;
        IncludeView = false;
        Quality = 95;
        ExportAvif = false;
        ExportBmp = false;
        ExportHeif = false;
        ExportBinary = false;
        ExportJpeg = false;
        ExportPng = false;
        ExportWebp = false;
    }
}
