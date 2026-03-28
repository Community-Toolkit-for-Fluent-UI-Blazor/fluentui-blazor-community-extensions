namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines options for configuring 1D stacked barcode generation.
/// </summary>
/// <remarks>Implement this interface to specify settings that control the appearance or behavior of 1D stacked
/// barcodes. The specific options available depend on the implementation.</remarks>
public interface IBarcode1DStackedOptions
{
    /// <summary>
    /// Gets the width of an individual module in the barcode, measured in device-independent units.
    /// </summary>
    double ModuleWidth { get; }

    /// <summary>
    /// Gets the height of the module, in device-independent units (DIU).
    /// </summary>
    double ModuleHeight { get; }
}
