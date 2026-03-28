using System.Globalization;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to encode data into the GS1-128 barcode symbology using specified encoding options.
/// </summary>
/// <remarks>This encoder implements the GS1-128 standard, supporting application identifiers and variable-length
/// fields as required by GS1 specifications. It selects the optimal Code 128 subset for each data segment to maximize
/// encoding efficiency. The encoder is intended for use with barcode generation components that require GS1-128
/// compliant output.</remarks>
internal sealed class Gs1_128Encoder : IBarcodeEncoder<Barcode1DPayload, Gs1_128Options>
{
    private static readonly CultureInfo s_invariantCulture = CultureInfo.InvariantCulture;
    private const int StartA = 103;
    private const int StartB = 104;
    private const int StartC = 105;
    private const int CodeA = 101;
    private const int CodeB = 100;
    private const int CodeC = 99;
    private const int Fnc1 = 102;
    private const int Stop = 106;

    /// <summary>
    /// Gets the singleton instance of the Gs1_128Encoder class.
    /// </summary>
    /// <remarks>Use this property to access a shared, thread-safe instance of the encoder without creating a
    /// new object. This is useful when a single encoder instance is sufficient for application-wide use.</remarks>
    public static Gs1_128Encoder Instance { get; } = new();

    /// <inheritdoc />
    public Barcode1DPayload Encode(string data, Gs1_128Options options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(data);

        var elements = Gs1Parser.Parse(data, options.ValidationMode);

        // Build the logical sequence GS1.
        var tokens = BuildTokens(elements);

        // Select the optimal subset for each segment.
        var segments = AnalyzeSegments(tokens);

        // Generate the sequence of symbols (start, data, code switches, FNC1)
        var symbols = BuildSymbols(segments);

        // Checksum + Stop
        AddChecksumAndStop(symbols);

        // Payload
        return BuildPayload(BuildReadableText(elements), symbols, options);
    }

    /// <summary>
    /// Builds a list of tokens from the parsed GS1 elements, inserting FNC1 where necessary for variable-length fields.
    /// </summary>
    /// <param name="elements">The list of parsed GS1 elements, each containing an AI and its corresponding value.</param>
    /// <returns>Returns a list of tokens representing the logical sequence of data to be encoded,
    ///  including FNC1 separators for variable-length fields.</returns>
    private static List<string> BuildTokens(List<Gs1Element> elements)
    {
        var list = new List<string> { "FNC1" };

        foreach (var el in elements)
        {
            if (!string.IsNullOrEmpty(el.Ai))
            {
                list.Add(el.Ai);
            }

            list.Add(el.Value);

            if (el.IsVariableLength && el != elements[^1])
            {
                list.Add("FNC1");
            }
        }

        return list;
    }

    /// <summary>
    /// Analyzes a list of string tokens and determines the appropriate Code 128 subset for each token.
    /// </summary>
    /// <remarks>The method assigns Code128Subset.C to tokens that consist only of digits and have an even
    /// length, Code128Subset.B to tokens containing printable ASCII characters, and Code128Subset.A to all other
    /// tokens. The special token "FNC1" is always assigned to Code128Subset.B.</remarks>
    /// <param name="tokens">The list of string tokens to analyze. Each token is evaluated to determine which Code 128 subset it belongs to.</param>
    /// <returns>A list of tuples, each containing a token and its corresponding Code128Subset value.</returns>
    private static List<(string token, Code128Subset subset)> AnalyzeSegments(List<string> tokens)
    {
        var result = new List<(string, Code128Subset)>();

        foreach (var t in tokens)
        {
            if (t == "FNC1")
            {
                result.Add((t, Code128Subset.B));
                continue;
            }

            // AI = always subset B
            if (Gs1AiTable.TryGet(t, out _))
            {
                result.Add((t, Code128Subset.B));
                continue;
            }

            // Numeric value with even length → subset C
            if (t.All(char.IsDigit) && t.Length % 2 == 0)
            {
                result.Add((t, Code128Subset.C));
                continue;
            }

            // ASCII value printable → subset B
            if (t.All(c => c >= 32 && c <= 127))
            {
                result.Add((t, Code128Subset.B));
                continue;
            }

            // Otherwise → subset A
            result.Add((t, Code128Subset.A));
        }

        return result;
    }

    /// <summary>
    /// Builds a list of Code 128 symbol values from the specified sequence of token and subset segments.
    /// </summary>
    /// <remarks>The method automatically inserts appropriate start and code switch symbols based on the
    /// subset transitions in the input segments.</remarks>
    /// <param name="segments">A list of tuples, each containing a token string and its associated Code128 subset, representing the segments to
    /// encode.</param>
    /// <returns>A list of integers representing the Code 128 symbol values corresponding to the provided segments.</returns>
    /// <exception cref="ArgumentException">Thrown when a character in a token cannot be encoded in the specified Code128 subset.</exception>
    private static List<int> BuildSymbols(List<(string token, Code128Subset subset)> segments)
    {
        var symbols = new List<int>();
        var firstReal = segments
            .SkipWhile(s => s.token == "FNC1" || Gs1AiTable.TryGet(s.token, out _))
            .First()
            .subset;

        symbols.Add(firstReal switch
        {
            Code128Subset.A => StartA,
            Code128Subset.B => StartB,
            Code128Subset.C => StartC,
            _ => StartB
        });

        var current = firstReal;

        foreach (var (token, subset) in segments)
        {
            if (token == "FNC1")
            {
                symbols.Add(Fnc1);
                continue;
            }

            if (subset != current)
            {
                symbols.Add(subset switch
                {
                    Code128Subset.A => CodeA,
                    Code128Subset.B => CodeB,
                    Code128Subset.C => CodeC,
                    _ => CodeB
                });

                current = subset;
            }

            if (current == Code128Subset.C)
            {
                for (var i = 0; i < token.Length; i += 2)
                {
                    symbols.Add(int.Parse(token.AsSpan(i, 2), s_invariantCulture));
                }
            }
            else if (current == Code128Subset.B)
            {
                foreach (var c in token)
                {
                    symbols.Add(c - 32);
                }
            }
            else // A
            {
                foreach (var c in token)
                {
                    symbols.Add(c);
                }
            }
        }

        return symbols;
    }

    /// <summary>
    /// Calculates the checksum for the provided symbol sequence, appends it to the list, and adds a stop code at the
    /// end.
    /// </summary>
    /// <remarks>This method modifies the input list by adding the computed checksum and a stop code as the
    /// final elements. The checksum is calculated according to the Code 128 barcode specification.</remarks>
    /// <param name="symbols">The list of symbol values to which the checksum and stop code will be appended. Must contain at least one
    /// element.</param>
    private static void AddChecksumAndStop(List<int> symbols)
    {
        var checksum = symbols[0];

        for (var i = 1; i < symbols.Count; i++)
        {
            checksum += symbols[i] * i;
        }

        checksum %= 103;
        symbols.Add(checksum);
        symbols.Add(Stop);
    }

    /// <summary>
    /// Builds a payload representing the bars and optional text for a 1D barcode using the specified value, symbol
    /// sequence, and rendering options.
    /// </summary>
    /// <remarks>If the ShowText option is enabled, the encoded value is included as a centered text element
    /// below the barcode.</remarks>
    /// <param name="value">The string value to encode in the barcode and display as text if enabled.</param>
    /// <param name="symbols">A list of symbol indices representing the barcode pattern to render.</param>
    /// <param name="options">The rendering options that control module size, height, and whether to display the encoded value as text.</param>
    /// <returns>A Barcode1DPayload containing the bar and text elements required to render the barcode.</returns>
    private static Barcode1DPayload BuildPayload(
        string value,
        List<int> symbols,
        Gs1_128Options options)
    {
        var bars = new List<Barcode1DBar>();
        var texts = new List<BarcodeText>();
        var x = 0.0;

        foreach (var symbol in symbols)
        {
            var pattern = Code128Table.Symbols[symbol].Pattern;

            for (var i = 0; i < pattern.Length; i++)
            {
                var width = pattern[i] * options.ModuleWidth;

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

        if (options.ShowText)
        {
            texts.Add(new BarcodeText
            {
                Text = value,
                X = x / 2.0,
                Y = options.ModuleHeight,
                Anchor = SvgTextAnchor.Middle
            });
        }

        return new Barcode1DPayload
        {
            Value = value,
            Bars = bars,
            Texts = texts
        };
    }

    /// <summary>
    /// Builds a human-readable string representation of a collection of GS1 elements, formatting each element with its
    /// Application Identifier (AI) and value.
    /// </summary>
    /// <remarks>Elements with a null or empty Application Identifier are included without parentheses. The
    /// resulting string does not have a trailing space.</remarks>
    /// <param name="elements">The list of GS1 elements to include in the formatted string. Each element should contain an Application
    /// Identifier and a value.</param>
    /// <returns>A string that concatenates the formatted GS1 elements, with each AI enclosed in parentheses followed by its
    /// value, separated by spaces.</returns>
    private static string BuildReadableText(List<Gs1Element> elements)
    {
        var sb = new System.Text.StringBuilder();

        foreach (var el in elements)
        {
            if (!string.IsNullOrEmpty(el.Ai))
            {
                sb.Append('(');
                sb.Append(el.Ai);
                sb.Append(')');
            }

            sb.Append(el.Value);
            sb.Append(' ');
        }

        return sb.ToString().TrimEnd();
    }
}
