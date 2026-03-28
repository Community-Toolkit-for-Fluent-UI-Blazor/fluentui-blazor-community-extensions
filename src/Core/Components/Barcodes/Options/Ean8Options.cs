namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for EAN-13 barcode generation or processing.
/// </summary>
internal sealed class Ean8Options
{
    /// <summary>
    /// Gets or sets the width of an individual module.
    /// </summary>
    public int ModuleWidth { get; set; } = 1;

    /// <summary>
    /// Gets or sets the height of the module.
    /// </summary>
    public int ModuleHeight { get; set; } = 50;

    /// <summary>
    /// Gets the height of the guard module, in device-independent units.
    /// </summary>
    public double GuardModuleHeight => ModuleHeight + ModuleHeight * 0.1;
}
