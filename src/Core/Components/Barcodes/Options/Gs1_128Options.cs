using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for generating GS1-128 barcodes.
/// </summary>
/// <remarks>Use this class to specify encoding parameters such as the Code 128 subset, module dimensions, and
/// whether to display human-readable text. These options control the appearance and encoding behavior of the generated
/// GS1-128 barcode.</remarks>
internal sealed class Gs1_128Options
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
    /// Gets or sets a value indicating whether text is displayed alongside the component.
    /// </summary>
    public bool ShowText { get; set; } = true;

    /// <summary>
    /// Gets or sets the mode used for GS1 validation.
    /// </summary>
    /// <remarks>Use this property to specify how GS1 data should be validated. The selected mode determines
    /// the validation rules applied to the input data.</remarks>
    public Gs1ValidationMode ValidationMode { get; set; }
}
