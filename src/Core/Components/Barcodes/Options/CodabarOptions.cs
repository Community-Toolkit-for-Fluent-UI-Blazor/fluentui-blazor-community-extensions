namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for generating Codabar barcodes.
/// </summary>
/// <remarks>Use this class to specify parameters that control the appearance of Codabar barcodes, such as the
/// width of the narrowest bar and the ratio between wide and narrow bars. These options affect the visual output and
/// readability of the generated barcode.</remarks>
public sealed class CodabarOptions
{
    /// <summary>
    /// Gets or sets the width of an individual module in the barcode, measured in device-independent units.
    /// </summary>
    /// <remarks>A module is the smallest unit of width in a barcode symbol. Adjust this property to control
    /// the overall scaling of the barcode. The default value is 1.0.</remarks>
    public double ModuleWidth { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the aspect ratio used for wide layouts.
    /// </summary>
    public double WideRatio { get; set; } = 3.0;

    /// <summary>
    /// Gets or sets the height of the module, in device-independent units (pixels).
    /// </summary>
    public double ModuleHeight { get; set; } = 50.0;

    /// <summary>
    /// Gets or sets a value indicating whether text is displayed alongside the component.
    /// </summary>
    public bool ShowText { get; set; } = true;
}
