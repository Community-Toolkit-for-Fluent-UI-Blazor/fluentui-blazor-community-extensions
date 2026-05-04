using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the symbology settings for rendering a QR code within a Fluent UI Blazor barcode component.
/// </summary>
/// <remarks>This class is used to configure QR code-specific options when generating barcodes with the
/// FluentCxBarcode component. It is intended to be used as part of the component hierarchy and is not typically
/// instantiated directly by application code.</remarks>
public sealed class QRCodeSymbology : ComponentBase, ISymbology
{
    private bool _hasErrorCorrectionChanged;

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxBarcode"/> component in the cascading parameter hierarchy.
    /// </summary>
    [CascadingParameter]
    private FluentCxBarcode? Parent { get; set; }

    /// <summary>
    /// Gets or sets the size of a module.
    /// </summary>
    [Parameter]
    public int ModuleSize { get; set; } = 1;

    /// <summary>
    /// Gets the QR code version used for encoding data.
    /// </summary>
    /// <remarks>The version determines the size and data capacity of the generated QR code. If set to Auto,
    /// the version is selected automatically based on the input data length and error correction level.</remarks>
    [Parameter]
    public QRVersion Version { get; set; } = QRVersion.Auto;

    /// <summary>
    /// Gets the error correction level used for encoding the QR code.
    /// </summary>
    /// <remarks>The error correction level determines the QR code's ability to recover data if parts of the
    /// code are damaged or obscured. Higher levels provide greater resilience to errors but reduce the amount of data
    /// that can be stored.</remarks>
    [Parameter]
    public QRErrorCorrectionLevel ErrorCorrection { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the QR code error correction level changes.
    /// </summary>
    /// <remarks>Use this event to respond to user actions or programmatic changes that modify the error
    /// correction level of the QR code. The callback receives the new error correction level as its argument.</remarks>
    [Parameter]
    public EventCallback<QRErrorCorrectionLevel> ErrorCorrectionChanged { get; set; }

    /// <summary>
    /// Gets the encoding mode used for generating the QR code.
    /// </summary>
    [Parameter]
    public QREncodingMode EncodingMode { get; set; }

    /// <summary>
    /// Gets the symbology used by the component.
    /// </summary>
    public Symbology Symbology => Symbology.QRCode;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("The QRCode Symbology component must be used within a FluentCxBarcode component.");
        }

        Parent.AddSymbology(this);
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasErrorCorrectionChanged = parameters.TryGetValue<QRErrorCorrectionLevel>(nameof(ErrorCorrection), out var newErrorCorrection) && newErrorCorrection != ErrorCorrection;

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_hasErrorCorrectionChanged)
        {
            await ErrorCorrectionChanged.InvokeAsync(ErrorCorrection);
            await Parent!.RefreshAsync();
        }
    }
}
