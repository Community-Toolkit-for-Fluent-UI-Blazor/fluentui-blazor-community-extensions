using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the Code 128 barcode symbology component for use within a FluentCxBarcode container.
/// </summary>
/// <remarks>Use this component to configure and render Code 128 barcodes as part of a FluentCxBarcode. The
/// component allows selection of the Code 128 subset and customization of module dimensions. It must be nested within a
/// FluentCxBarcode component to function correctly.</remarks>
public sealed class GS1_128Symbology : ComponentBase, ISymbology
{
    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxBarcode"/> component in the cascading parameter hierarchy.
    /// </summary>
    [CascadingParameter]
    private FluentCxBarcode? Parent { get; set; }

    /// <summary>
    /// Gets or sets the validation mode to use when processing GS1 data.
    /// </summary>
    /// <remarks>The validation mode determines the strictness of GS1 data validation. Use a stricter mode to
    /// enforce compliance with GS1 standards, or a more relaxed mode to allow for greater flexibility. The default is
    /// Strict.</remarks>
    [Parameter]
    public Gs1ValidationMode ValidationMode { get; set; } = Gs1ValidationMode.Strict;

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
    /// Gets or sets a value indicating whether the text is displayed alongside the component.
    /// </summary>
    [Parameter]
    public bool ShowText { get; set; } = true;

    /// <summary>
    /// Gets the symbology used by the component.
    /// </summary>
    public Symbology Symbology => Symbology.GS1_128;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("The GS1-128 Symbology component must be used within a FluentCxBarcode component.");
        }

        Parent.AddSymbology(this);
    }
}
