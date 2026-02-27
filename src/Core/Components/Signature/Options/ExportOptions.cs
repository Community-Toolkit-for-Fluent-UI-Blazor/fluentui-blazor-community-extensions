namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for exporting signature images, including settings for background color, padding,
/// grid and watermark inclusion, cropping, DPI, and JPEG quality.
/// </summary>
/// <remarks>Use this class to customize the appearance and quality of exported signature images. The options
/// allow control over visual elements and output parameters for formats such as PNG and JPEG. Adjust properties as
/// needed before initiating an export operation.</remarks>
public sealed class ExportOptions
{
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
    }
}
