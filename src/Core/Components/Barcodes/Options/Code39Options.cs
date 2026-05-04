namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for generating Code 39 barcodes, including layout and validation settings.
/// </summary>
internal class Code39Options
{
    /// <summary>
    /// Gets or sets the aspect ratio used for wide layouts.
    /// </summary>
    public double WideRatio { get; set; } = 3.0;

    /// <summary>
    /// Gets or sets a value indicating whether checksum validation is enabled.
    /// </summary>
    public bool EnableChecksum { get; set; }

    /// <summary>
    /// Gets or sets the width of an individual module.
    /// </summary>
    public int ModuleWidth { get; set; } = 1;

    /// <summary>
    /// Gets or sets the height of the module.
    /// </summary>
    public int ModuleHeight { get; set; } = 50;

    /// <summary>
    /// Gets or sets a value indicating whether extended mode is enabled.
    /// </summary>
    public bool IsExtended { get; set; }
}
