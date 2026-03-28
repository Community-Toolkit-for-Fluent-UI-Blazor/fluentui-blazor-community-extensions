namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the error correction level for a QR code symbol.
/// </summary>
/// <remarks>The error correction level determines the amount of data that can be restored if the QR code is
/// damaged or partially obscured. Higher levels provide greater error correction capability at the cost of reduced data
/// capacity. The available levels are L (Low), M (Medium), Q (Quartile), and H (High), corresponding to approximately
/// 7%, 15%, 25%, and 30% error recovery, respectively.</remarks>
public enum QRErrorCorrectionLevel
{
    /// <summary>
    /// Represents the lowest level of error correction, allowing for approximately 7% of the data to be restored if the QR code is damaged or obscured.
    /// This level provides the highest data capacity but the least error correction capability.
    /// </summary>
    Low,

    /// <summary>
    /// Represents a medium level of error correction, allowing for approximately 15% of the data to be restored if the QR code is damaged or obscured.
    /// </summary>
    Medium,

    /// <summary>
    /// Represents a quartile level of error correction, allowing for approximately 25% of the data to be restored if the QR code is damaged or obscured.
    /// </summary>
    Quartile,

    /// <summary>
    /// Represents the highest level of error correction, allowing for approximately 30% of the data to be restored if the QR code is damaged or obscured.
    /// </summary>
    High
}

