using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the Code 39 barcode symbology component for use in Fluent UI Blazor applications.
/// </summary>
/// <remarks>Code 39 is a widely used barcode standard that encodes alphanumeric characters. This component
/// enables rendering and interaction with Code 39 barcodes within Blazor projects, adhering to Fluent UI design
/// principles.</remarks>
public sealed class Code93Symbology : ComponentBase, ISymbology
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
    /// Gets or sets a value indicating whether the text is displayed alongside the component.
    /// </summary>
    [Parameter]
    public bool ShowText { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether extended mode is enabled, allowing for the encoding of the full ASCII character set using Code 39's extended encoding scheme.
    /// </summary>
    [Parameter]
    public bool IsExtended { get; set; } = true;

    /// <summary>
    /// Gets the symbology used by the component.
    /// </summary>
    public Symbology Symbology => Symbology.Code93;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("The Code93Symbology component must be used within a FluentCxBarcode component.");
        }

        Parent.AddSymbology(this);
    }
}
