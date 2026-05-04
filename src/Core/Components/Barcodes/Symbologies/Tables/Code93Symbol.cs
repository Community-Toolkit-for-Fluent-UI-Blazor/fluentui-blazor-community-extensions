namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a symbol in the Code 93 barcode symbology.
/// </summary>
/// <remarks>This type encapsulates the pattern information for a single Code 93 symbol. It is intended for
/// internal use within barcode encoding or decoding operations.</remarks>
internal sealed class Code93Symbol
{
    /// <summary>
    /// Gets the regular expression pattern used for matching input values.
    /// </summary>
    /// <remarks>The pattern must be a valid regular expression. This property is typically used to define
    /// validation or parsing rules for input data.</remarks>
    public required string Pattern { get; init; }
}
