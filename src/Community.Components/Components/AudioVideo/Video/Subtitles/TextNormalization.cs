namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the available options for normalizing text casing and formatting.
/// </summary>
/// <remarks>Use this enumeration to select how input text should be transformed, such as converting to lower
/// case, upper case, or sentence case. The selected normalization option determines the formatting applied to the text
/// during processing.</remarks>
public enum TextNormalization
{
    /// <summary>
    /// No normalization is applied; the text remains unchanged.
    /// </summary>
    None,

    /// <summary>
    /// Lowercase normalization converts all characters in the text to lowercase.
    /// </summary>
    LowerCase,

    /// <summary>
    /// Uppercase normalization converts all characters in the text to uppercase.
    /// </summary>
    UpperCase,

    /// <summary>
    /// Capitalizes the first letter of each sentence while converting the rest to lowercase.
    /// </summary>
    Sentence,
}
