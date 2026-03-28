using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the ITF-14 barcode symbology component for use in Fluent UI Blazor applications.
/// </summary>
/// <remarks>ITF-14 is a standardized barcode format used primarily for packaging and shipping. This component
/// enables rendering and configuration of ITF-14 barcodes within Blazor applications, following the conventions of the
/// Fluent UI Blazor library.</remarks>
public sealed class Itf14Symbology
    : ComponentBase, ISymbology
{
    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxBarcode"/> component in the cascading parameter hierarchy.
    /// </summary>
    [CascadingParameter]
    private FluentCxBarcode? Parent { get; set; }

    /// <summary>
    /// Gets or sets the width of an individual module.
    /// </summary>
    [Parameter]
    public int ModuleWidth { get; set; } = 1;

    /// <summary>
    /// Gets or sets the height of the module.
    /// </summary>
    [Parameter]
    public int ModuleHeight { get; set; } = 50;

    /// <summary>
    /// Gets or sets a value indicating whether the text content is displayed.
    /// </summary>
    [Parameter]
    public bool ShowText { get; set; } = true;

    /// <summary>
    /// Gets the symbology used by the component.
    /// </summary>
    public Symbology Symbology => Symbology.Itf14;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("The Upc-A Symbology component must be used within a FluentCxBarcode component.");
        }

        Parent.AddSymbology(this);
    }
}
