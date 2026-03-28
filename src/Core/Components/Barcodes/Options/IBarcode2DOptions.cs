namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines options for configuring a two-dimensional barcode.
/// </summary>
public interface IBarcode2DOptions
{
    /// <summary>
    /// Gets the size of the module as a double-precision floating-point value.
    /// </summary>
    public double ModuleSize { get; }
}
