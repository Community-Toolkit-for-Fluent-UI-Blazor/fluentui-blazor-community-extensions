using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the Code 128 barcode symbology component for use within a FluentCxBarcode container.
/// </summary>
/// <remarks>Use this component to configure and render Code 128 barcodes as part of a FluentCxBarcode. The
/// component allows selection of the Code 128 subset and customization of module dimensions. It must be nested within a
/// FluentCxBarcode component to function correctly.</remarks>
public sealed class UpcASymbology : ComponentBase, ISymbology
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
    /// Gets the symbology used by the component.
    /// </summary>
    public Symbology Symbology => Symbology.UpcA;

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
