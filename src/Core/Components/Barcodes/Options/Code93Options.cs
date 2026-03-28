namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for generating Code 93 barcodes.
/// </summary>
/// <remarks>Use this class to specify parameters such as module dimensions and whether to enable extended
/// encoding when generating a Code 93 barcode.</remarks>
internal class Code93Options
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
    /// Gets or sets a value indicating whether extended mode is enabled.
    /// </summary>
    public bool IsExtended { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the text is displayed.
    /// </summary>
    public bool ShowText { get; set; }
}
