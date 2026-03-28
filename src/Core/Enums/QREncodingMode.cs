namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the encoding mode used for QR code data representation.
/// </summary>
/// <remarks>Use this enumeration to select how input data is encoded within a QR code. The encoding mode affects
/// which characters can be represented and may impact the efficiency and compatibility of the generated QR code. The
/// 'Auto' mode selects the most appropriate encoding based on the input data.</remarks>
public enum QREncodingMode
{
    /// <summary>
    /// Specifies that a value or behavior should be determined automatically based on context or default logic.
    /// </summary>
    Auto,

    /// <summary>
    /// Represents an encoding mode that supports numeric characters (0-9) only.
    /// </summary>
    Numeric,

    /// <summary>
    /// Represents an encoding mode that supports alphanumeric characters, including uppercase letters (A-Z), digits (0-9), and a limited set of special characters (space, $, %, *, +, -, ., /, :).
    /// </summary>
    Alphanumeric,

    /// <summary>
    /// Represents an encoding mode that supports byte data, allowing for a wider range of characters.
    /// </summary>
    Byte,

    /// <summary>
    /// Represents an encoding mode that supports Kanji characters, which are used in Japanese text.
    /// </summary>
    Kanji
}

