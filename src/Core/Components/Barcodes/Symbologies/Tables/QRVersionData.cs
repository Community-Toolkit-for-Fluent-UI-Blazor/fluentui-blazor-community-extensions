using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents version-specific metadata for a QR code, including version number, size, total codewords, and error
/// correction information.
/// </summary>
/// <remarks>Use this type to access QR code version details required for encoding or decoding operations. The
/// properties provide essential parameters for QR code structure and error correction, enabling correct generation and
/// interpretation of QR symbols. This class is intended for internal use within QR code processing logic.</remarks>
internal sealed class QRVersionData
{
    /// <summary>
    /// Gets the version number associated with the current instance.
    /// </summary>
    public int Version { get; init; }

    /// <summary>
    /// Gets the size value associated with the current instance.
    /// </summary>
    /// <remarks>The size is calculated with this formula : 21 + 4 * (version - 1)</remarks>
    public int Size { get; init; }

    /// <summary>
    /// Gets the total number of codewords represented by the current instance.
    /// </summary>
    public int TotalCodewords { get; init; }

    /// <summary>
    /// Gets the mapping of QR error correction levels to their corresponding error correction information.
    /// </summary>
    /// <remarks>Use this property to retrieve error correction parameters associated with each supported QR
    /// error correction level. The dictionary provides quick access to error correction details required for QR code
    /// generation or analysis.</remarks>
    public Dictionary<QRErrorCorrectionLevel, QREccInfo[]> Ecc { get; init; } = [];
}

