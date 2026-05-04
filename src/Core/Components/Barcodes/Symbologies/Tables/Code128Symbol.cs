namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a symbol in the Code 128 barcode standard, including its encoded value and bar pattern.
/// </summary>
/// <remarks>This struct encapsulates the data for a single Code 128 symbol, which consists of a numeric value and
/// the corresponding bar pattern as an array of integers. It is intended for internal use in barcode generation or
/// decoding processes.</remarks>
internal readonly struct Code128Symbol
{
    /// <summary>
    /// Gets the current value represented by the property.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Gets the sequence of integers that defines the pattern for this instance.
    /// </summary>
    public int[] Pattern { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Code128Symbol"/> struct with the specified value and pattern string.
    /// </summary>
    /// <param name="value">Value of the symbol, typically corresponding to a character or control code in the Code 128 specification.</param>
    /// <param name="pattern">The string representation of the bar pattern,
    ///  where each character is a digit representing the width of bars and spaces in the symbol.
    ///  The pattern is converted to an array of integers for internal use.</param>
    public Code128Symbol(int value, string pattern)
    {
        Value = value;
        Pattern = [.. pattern.Select(c => c - '0')];
    }
}
