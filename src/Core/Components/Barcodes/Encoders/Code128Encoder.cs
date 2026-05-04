using System.Globalization;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to encode input data into the Code 128 barcode symbology using specified encoding options.
/// </summary>
/// <remarks>This encoder supports automatic or manual selection of Code 128 subsets (A, B, or C) based on the
/// input data and options provided. It validates input and generates the appropriate symbol sequence, including start,
/// checksum, and stop codes, to produce a barcode payload suitable for rendering. The encoder throws exceptions for
/// invalid input or unsupported characters according to the selected subset.</remarks>
internal sealed class Code128Encoder : IBarcodeEncoder<Barcode1DPayload, Code128Options>
{
    /// <summary>
    /// Provides a static reference to the invariant culture, which is culture-insensitive and associated with the
    /// English language but not with any country or region.
    /// </summary>
    /// <remarks>Use this field when formatting or parsing operations require culture-independent results,
    /// such as for consistent data serialization or protocol formatting.</remarks>
    private static readonly CultureInfo s_culture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Represents the start code value for Code 128 subset A.
    /// </summary>
    private const int StartA = 103;

    /// <summary>
    /// Represents the starting value for the B range.
    /// </summary>
    private const int StartB = 104;

    /// <summary>
    /// Represents the start code value for Code 128C barcodes.
    /// </summary>
    /// <remarks>This constant is typically used when encoding data using the Code 128C symbology, which is
    /// optimized for numeric data. The value corresponds to the start character required by the barcode
    /// specification.</remarks>
    private const int StartC = 105;

    /// <summary>
    /// Represents the constant value used to indicate a stop condition.
    /// </summary>
    private const int Stop = 106;

    /// <summary>
    /// Initializes a new instance of the Code128Encoder class.
    /// </summary>
    /// <remarks>This constructor is private to prevent direct instantiation of the Code128Encoder class.
    /// Instances should be created using the provided factory methods or properties.</remarks>
    private Code128Encoder()
    { }

    /// <summary>
    /// Gets the singleton instance of the Code128Encoder class.
    /// </summary>
    public static Code128Encoder Instance { get; } = new();

    /// <summary>
    /// Encodes the specified data into a Code 128 barcode payload using the provided encoding options.
    /// </summary>
    /// <remarks>If the subset in options is set to Auto, the method automatically detects the appropriate
    /// Code 128 subset for the input data.</remarks>
    /// <param name="data">The data to encode as a Code 128 barcode. Cannot be null or empty. For subset C, the data must contain an even
    /// number of digits.</param>
    /// <param name="options">The options that specify the Code 128 encoding subset and additional encoding parameters.</param>
    /// <returns>A Barcode1DPayload object representing the encoded Code 128 barcode data.</returns>
    /// <exception cref="ArgumentException">Thrown if data is null, empty, or if subset C is selected and data does not contain an even number of digits.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the specified subset in options is not a valid Code 128 subset.</exception>
    public Barcode1DPayload Encode(
        string data,
        Code128Options options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));
        var subset = options.Subset == Code128Subset.Auto ? DetectSubset(data) : options.Subset;

        var symbols = new List<int>
        {
            subset switch
            {
                Code128Subset.A => StartA,
                Code128Subset.B => StartB,
                Code128Subset.C => StartC,
                _ => throw new InvalidOperationException("Invalid subset.")
            }
        };

        // Encode data
        if (subset == Code128Subset.C)
        {
            if (data.Length % 2 != 0)
            {
                throw new ArgumentException("Code128C requires an even number of digits.");
            }

            for (var i = 0; i < data.Length; i += 2)
            {
                var pair = data.Substring(i, 2);
                symbols.Add(int.Parse(pair, s_culture));
            }
        }
        else
        {
            foreach (var c in data)
            {
                symbols.Add(CharToValue(c, subset));
            }
        }

        // Checksum
        var checksum = symbols[0];

        for (var i = 1; i < symbols.Count; i++)
        {
            checksum += symbols[i] * i;
        }

        // Always 103.
        checksum %= StartA;
        symbols.Add(checksum);

        // Stop
        symbols.Add(Stop);

        // Convert to bars
        return BuildPayload(data, symbols, options);
    }

    /// <summary>
    /// Determines the most appropriate Code 128 subset (A, B, or C) for encoding the specified data string.
    /// </summary>
    /// <remarks>Subset C is selected if the data consists only of digits and has an even length. Subset B is
    /// chosen if all characters are within the standard ASCII printable range (32–127). Otherwise, subset A is
    /// used.</remarks>
    /// <param name="data">The input string to analyze for optimal Code 128 subset selection. Cannot be null.</param>
    /// <returns>A value from the Code128Subset enumeration indicating the recommended subset for encoding the input data.</returns>
    private static Code128Subset DetectSubset(string data)
    {
        // If all digits and even length → choose C
        if (data.All(char.IsDigit) && data.Length % 2 == 0)
        {
            return Code128Subset.C;
        }

        // If all chars < 128 → choose B
        if (data.All(c => c >= 32 && c <= 127))
        {
            return Code128Subset.B;
        }

        // Otherwise → A
        return Code128Subset.A;
    }

    /// <summary>
    /// Converts the specified character to its corresponding Code 128 value for the given subset.
    /// </summary>
    /// <remarks>Use this method to obtain the Code 128 value for a character when encoding barcodes using
    /// subset A or B. Subset C is intended for numeric pairs and cannot encode individual characters.</remarks>
    /// <param name="c">The character to convert to a Code 128 value.</param>
    /// <param name="subset">The Code 128 subset to use for conversion. Must be either Code128Subset.A or Code128Subset.B.</param>
    /// <returns>The Code 128 value corresponding to the specified character in the given subset.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the specified subset is Code128Subset.C, which does not support encoding single characters.</exception>
    private static int CharToValue(char c, Code128Subset subset)
    {
        return subset switch
        {
            Code128Subset.A => CharToValueA(c),
            Code128Subset.B => CharToValueB(c),
            _ => throw new InvalidOperationException("Subset C does not encode single characters.")
        };
    }

    /// <summary>
    /// Converts the specified character to its corresponding Code 128A value.
    /// </summary>
    /// <param name="c">The character to convert. Must be in the range U+0000 to U+005F (ASCII 0–95).</param>
    /// <returns>The Code 128A value corresponding to the specified character.</returns>
    /// <exception cref="ArgumentException">Thrown when the specified character is not encodable in Code 128A (outside the range 0–95).</exception>
    private static int CharToValueA(char c)
    {
        if (c >= 0 && c <= 95)
        {
            return c;
        }

        throw new ArgumentException($"Character '{c}' not encodable in Code128A.");
    }

    /// <summary>
    /// Converts a character to its corresponding Code 128B value.
    /// </summary>
    /// <param name="c">The character to convert. Must be in the ASCII range 32 to 127.</param>
    /// <returns>The Code 128B value corresponding to the specified character.</returns>
    /// <exception cref="ArgumentException">Thrown if the specified character is not in the ASCII range 32 to 127.</exception>
    private static int CharToValueB(char c)
    {
        if (c >= 32 && c <= 127)
        {
            return c - 32;
        }

        throw new ArgumentException($"Character '{c}' not encodable in Code128B.");
    }

    /// <summary>
    /// Builds a barcode payload for a 1D barcode using the specified symbol sequence and rendering options.
    /// </summary>
    /// <remarks>The generated payload can be used to render a Code 128 barcode with the specified visual
    /// parameters. The method assumes that all symbol values are valid indices in the Code 128 symbol table.</remarks>
    /// <param name="symbols">A list of integer values representing the symbol sequence to encode in the barcode. Each value corresponds to a
    /// symbol in the Code 128 table.</param>
    /// <param name="options">The rendering options to use when generating the barcode, including module width and height.</param>
    /// <param name="value">The original input value that was encoded into the barcode.</param>
    /// <returns>A Barcode1DPayload object containing the bar definitions for the encoded barcode.</returns>
    private static Barcode1DPayload BuildPayload(
        string value,
        List<int> symbols,
        Code128Options options)
    {
        var bars = new List<Barcode1DBar>();
        var x = 0.0;

        foreach (var symbol in symbols)
        {
            var pattern = Code128Table.Symbols[symbol].Pattern;

            for (var i = 0; i < pattern.Length; i++)
            {
                var width = pattern[i] * options.ModuleWidth;

                // A bar is drawn for even indices (0, 2, 4, etc.), while odd indices represent spaces.
                if (i % 2 == 0)
                {
                    bars.Add(new Barcode1DBar
                    {
                        X = x,
                        Width = width,
                        Height = options.ModuleHeight
                    });
                }

                x += width;
            }
        }

        return new Barcode1DPayload
        {
            Value = value,
            Bars = bars,
            Texts = options.ShowText
                ? new List<BarcodeText>
                {
                    new()
                    {
                        Text = value,
                        X = x / 2.0,
                        Y = options.ModuleHeight,
                        Anchor = SvgTextAnchor.Middle
                    }
                }
                : []
        };
    }
}

