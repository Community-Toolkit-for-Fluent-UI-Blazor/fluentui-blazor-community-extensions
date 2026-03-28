using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the PDF417 barcode symbology for use with barcode components.
/// </summary>
/// <remarks>Use this class to specify the PDF417 symbology when configuring barcode generation or recognition
/// components. PDF417 is a two-dimensional barcode format capable of encoding large amounts of data and is commonly
/// used in transport, identification, and inventory applications.</remarks>
public sealed class Pdf417Symbology : ComponentBase, ISymbology, IBarcode1DStackedOptions
{
    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxBarcode"/> component in the cascading parameter hierarchy.
    /// </summary>
    [CascadingParameter]
    private FluentCxBarcode? Parent { get; set; }

    /// <inheritdoc />
    public Symbology Symbology => Symbology.Pdf417;

    /// <summary>
    /// Gets or sets the encoding mode used for generating the PDF417 barcode.
    /// </summary>
    [Parameter]
    public PDF417Mode Mode { get; set; } = PDF417Mode.Normal;

    /// <summary>
    /// Gets or sets the error level associated with the current instance.
    /// </summary>
    [Parameter]
    public PDF417ErrorCorrectionLevel ErrorLevel { get; set; } = PDF417ErrorCorrectionLevel.Level2;

    /// <summary>
    /// Gets or sets the number of columns to display, or null to use the default layout.
    /// </summary>
    /// <remarks>By defaut, set to 4.</remarks>
    [Parameter]
    public int? Columns { get; set; } = 4;

    /// <summary>
    /// Gets or sets the number of rows to display, or null to use the default value.
    /// </summary>
    [Parameter]
    public int? Rows { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is displayed in a compact layout.
    /// </summary>
    [Parameter]
    public bool Compact { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the micro PDF417 grid should be automatically determined based on the input data length.
    /// </summary>
    [Parameter]
    public bool AutoMicroGrid { get; set; }

    /// <summary>
    /// Gets or sets the number of rows and columns to use for displaying content in a micro grid layout, if specified.
    /// </summary>
    [Parameter]
    public (int Rows, int Columns)? MicroGrid { get; set; }

    /// <inheritdoc />
    [Parameter]
    public double ModuleWidth { get; set; } = 1.0;

    /// <inheritdoc />
    [Parameter]
    public double ModuleHeight { get; set; } = 2.0;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("The Pdf417Symbology component must be used within a FluentCxBarcode component.");
        }

        Parent.AddSymbology(this);
    }
}
