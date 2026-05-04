using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the Codabar barcode symbology component for use within a FluentCxBarcode container. Provides parameters
/// to configure the appearance and behavior of Codabar barcodes in Blazor applications.
/// </summary>
/// <remarks>CodabarSymbology must be used as a child of a FluentCxBarcode component. It exposes parameters to
/// control module width, height, wide ratio, and text display for Codabar barcodes. This component adheres to the
/// Fluent UI Blazor extension guidelines and is intended for scenarios where Codabar encoding is required.</remarks>
public sealed class CodabarSymbology : ComponentBase, ISymbology
{
    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxBarcode"/> component in the cascading parameter hierarchy.
    /// </summary>
    [CascadingParameter]
    private FluentCxBarcode? Parent { get; set; }

    /// <summary>
    /// Gets or sets the width of an individual module in the barcode, measured in device-independent units.
    /// </summary>
    /// <remarks>A module is the smallest unit of width in a barcode symbol. Adjust this property to control
    /// the overall scaling of the barcode. The default value is 1.0.</remarks>
    [Parameter]
    public double ModuleWidth { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the aspect ratio used for wide layouts.
    /// </summary>
    [Parameter]
    public double WideRatio { get; set; } = 3.0;

    /// <summary>
    /// Gets or sets the height of the module, in device-independent units (pixels).
    /// </summary>
    [Parameter]
    public double ModuleHeight { get; set; } = 50.0;

    /// <summary>
    /// Gets or sets a value indicating whether text is displayed alongside the component.
    /// </summary>
    [Parameter]
    public bool ShowText { get; set; } = true;

    /// <summary>
    /// Gets the symbology used by the component.
    /// </summary>
    public Symbology Symbology => Symbology.Codabar;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("The Code128Symbology component must be used within a FluentCxBarcode component.");
        }

        Parent.AddSymbology(this);
    }
}
