using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for generating PDF417 and MicroPDF417 barcodes.
/// </summary>
/// <remarks>This class allows customization of barcode generation, including error correction level, symbol size,
/// compaction mode, and grid selection for MicroPDF417. Use these options to control the encoding and layout of the
/// resulting barcode according to application requirements.</remarks>
public sealed class Pdf417Options : IBarcode1DStackedOptions
{
    /// <summary>
    /// Gets the encoding mode used for generating the PDF417 barcode.
    /// </summary>
    /// <remarks>The encoding mode determines how input data is processed and encoded in the PDF417 symbol.
    /// Different modes may optimize for text, numeric, or binary data. The default is Normal mode.</remarks>
    public PDF417Mode Mode { get; set; } = PDF417Mode.Normal;

    /// <summary>
    /// Gets the error level associated with the current instance.
    /// </summary>
    public PDF417ErrorCorrectionLevel ErrorLevel { get; set; } = PDF417ErrorCorrectionLevel.Level2;

    /// <summary>
    /// Gets the number of columns to display, or null to use the default layout.
    /// </summary>
    public int? Columns { get; set; } = 4;

    /// <summary>
    /// Gets the number of rows to display, or null to use the default value.
    /// </summary>
    public int? Rows { get; set; }

    /// <summary>
    /// Gets a value indicating whether the component is displayed in a compact layout.
    /// </summary>
    public bool Compact { get; set; }

    /// <summary>
    /// Gets a value indicating if the micro PDF417 grid should be automatically determined based on the input data length.
    /// </summary>
    public bool AutoMicroGrid { get; set; }

    /// <summary>
    /// Gets the number of rows and columns to use for displaying content in a micro grid layout, if specified.
    /// </summary>
    /// <remarks>If the value is null, the default layout is used. Use this property to customize the
    /// arrangement of items in a compact grid format.</remarks>
    public (int Rows, int Columns)? MicroGrid { get; set; }

    /// <summary>
    /// Gets or sets the width of the module.
    /// </summary>
    public double ModuleWidth { get; set; } = 1;

    /// <summary>
    /// Gets or sets the height of the module.
    /// </summary>
    public double ModuleHeight { get; set; } = 2;
}
