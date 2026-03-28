using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for generating QR codes, including version, error correction level, and encoding
/// mode.
/// </summary>
/// <remarks>Use this class to specify parameters that control the structure and reliability of the generated QR
/// code. The options allow customization of the QR code's size, data capacity, and resilience to errors, enabling
/// adaptation to various use cases and data requirements.</remarks>
public sealed class QRCodeOptions
    : IBarcode2DOptions
{
    /// <inheritdoc />
    public double ModuleSize { get; set; } = 1;

    /// <summary>
    /// Gets the QR code version used for encoding data.
    /// </summary>
    /// <remarks>The version determines the size and data capacity of the generated QR code. If set to Auto,
    /// the version is selected automatically based on the input data length and error correction level.</remarks>
    public QRVersion Version { get; set; } = QRVersion.Auto;

    /// <summary>
    /// Gets the error correction level used for encoding the QR code.
    /// </summary>
    /// <remarks>The error correction level determines the QR code's ability to recover data if parts of the
    /// code are damaged or obscured. Higher levels provide greater resilience to errors but reduce the amount of data
    /// that can be stored.</remarks>
    public QRErrorCorrectionLevel ErrorCorrection { get; set; }

    /// <summary>
    /// Gets the encoding mode used for generating the QR code.
    /// </summary>
    public QREncodingMode EncodingMode { get; set; }
}
