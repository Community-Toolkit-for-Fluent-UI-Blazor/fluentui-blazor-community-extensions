namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for generating UPC-E barcodes, including module dimensions and number system
/// settings.
/// </summary>
internal sealed class UpcEOptions
{
    /// <summary>
    /// Gets or sets the width of a single module in the barcode, measured in device-independent units.
    /// </summary>
    /// <remarks>The module width determines the thickness of the narrowest bar or space in the barcode.
    /// Adjust this value to control the overall size and readability of the generated barcode.</remarks>
    public double ModuleWidth { get; set; } = 2.0;

    /// <summary>
    /// Gets or sets the height of the module, in device-independent units (pixels).
    /// </summary>
    public double ModuleHeight { get; set; } = 50.0;

    /// <summary>
    /// Gets or sets the base of the number system used for numeric operations.
    /// </summary>
    public int NumberSystem { get; set; }

    /// <summary>
    /// Gets the height of the guard module, in device-independent units.
    /// </summary>
    public double GuardModuleHeight => ModuleHeight + ModuleHeight * 0.1;
}

