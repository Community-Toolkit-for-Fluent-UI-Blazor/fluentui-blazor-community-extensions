using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides options for configuring the generation of Code 128 barcodes.
/// </summary>
/// <remarks>Use this class to specify parameters such as the character subset and the dimensions of barcode
/// modules when generating a Code 128 barcode. Adjusting these options allows customization of the barcode's appearance
/// and encoding behavior.</remarks>
internal sealed class Code128Options
{
    /// <summary>
    /// Gets or sets the Code 128 subset used for encoding the barcode data.
    /// </summary>
    /// <remarks>Use this property to specify which Code 128 character subset (A, B, C, or Auto) should be
    /// applied when generating the barcode. The default value is Auto, which automatically selects the most efficient
    /// subset based on the input data.</remarks>
    public Code128Subset Subset { get; set; } = Code128Subset.Auto;

    /// <summary>
    /// Gets or sets the width of an individual module.
    /// </summary>
    public int ModuleWidth { get; set; } = 1;

    /// <summary>
    /// Gets or sets the height of the module.
    /// </summary>
    public int ModuleHeight { get; set; } = 50;

    /// <summary>
    /// Gets or sets a value indicating whether text is displayed alongside the component.
    /// </summary>
    public bool ShowText { get; set; } = true;
}
