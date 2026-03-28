namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the available subsets for encoding data in the Code 128 barcode symbology.
/// </summary>
/// <remarks>Use this enumeration to select the appropriate Code 128 subset when generating barcodes. Subset A
/// supports uppercase letters, control characters, and special symbols; Subset B supports upper and lowercase letters
/// and special symbols; Subset C is optimized for numeric data. The Auto option allows automatic selection of the most
/// suitable subset based on the input data.</remarks>
public enum Code128Subset
{
    /// <summary>
    /// Specifies that the value should be determined automatically based on context or default behavior.
    /// </summary>
    Auto,

    /// <summary>
    /// Represents the A type.
    /// </summary>
    A,

    /// <summary>
    /// Represents the B type.
    /// </summary>
    B,

    /// <summary>
    /// Represents the C type.
    /// </summary>
    C
}
