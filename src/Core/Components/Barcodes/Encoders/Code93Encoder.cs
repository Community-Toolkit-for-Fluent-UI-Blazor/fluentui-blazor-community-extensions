using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to encode input data into the Code 93 barcode symbology using specified encoding options.
/// </summary>
/// <remarks>This encoder supports both standard and extended Code 93 character sets, allowing for flexible
/// encoding of alphanumeric and special characters. It computes the required checksums and generates a payload suitable
/// for rendering as a 1D barcode. The encoder is intended for internal use within barcode generation workflows and
/// adheres to the Code 93 specification for symbol encoding and checksum calculation.</remarks>
internal sealed class Code93Encoder : IBarcodeEncoder<Barcode1DPayload, Code93Options>
{
    /// <summary>
    /// Gets the singleton instance of the Code93Encoder class.
    /// </summary>
    /// <remarks>Use this property to access a shared, thread-safe instance of the encoder without creating a
    /// new object.</remarks>
    public static Code93Encoder Instance { get; } = new Code93Encoder();

    /// <inheritdoc />
    public Barcode1DPayload Encode(string data, Code93Options options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(data);

        var expanded = options.IsExtended
            ? ExpandExtended(data)
            : ExpandStandard(data);

        var values = new List<int>
        {
            // Start (*)
            47
        };

        foreach (var token in expanded)
        {
            if (!Code93Table.TryGetValue(token, out var v))
            {
                throw new ArgumentException($"Token '{token}' not encodable in Code93.");
            }

            values.Add(v);
        }

        // Checksum C
        var cChecksum = ComputeChecksum([.. values.Skip(1)], 20);
        values.Add(cChecksum);

        // Checksum K
        var kChecksum = ComputeChecksum([.. values.Skip(1)], 15);
        values.Add(kChecksum);

        // Stop (*)
        values.Add(47);

        return BuildPayload(data, values, options);
    }

    /// <summary>
    /// Converts the specified string into a list of individual characters, validating that each character is allowed in
    /// the Code93 standard.
    /// </summary>
    /// <param name="data">The string to be expanded and validated. Each character must be valid according to the Code93 standard.</param>
    /// <returns>A list of strings, each representing a single character from the input that is valid in the Code93 standard.</returns>
    /// <exception cref="ArgumentException">Thrown if the input string contains a character that is not allowed in the Code93 standard.</exception>
    private static List<string> ExpandStandard(string data)
    {
        var list = new List<string>();

        foreach (var c in data)
        {
            var s = c.ToString();

            if (!Code93Table.TryGetValue(s, out _))
            {
                throw new ArgumentException($"Character '{c}' not allowed in Code93 standard.");
            }

            list.Add(s);
        }

        return list;
    }

    /// <summary>
    /// Converts a string into a list of Code 93 extended encoding tokens, mapping each character to its corresponding
    /// Code 93 sequence.
    /// </summary>
    /// <remarks>Each character in the input is mapped to one or more Code 93 extended tokens. Extended
    /// sequences are split into their prefix and character components as required by the encoding
    /// specification.</remarks>
    /// <param name="data">The input string to encode using the Code 93 extended character set.</param>
    /// <returns>A list of strings representing the Code 93 extended encoding tokens for each character in the input string.</returns>
    /// <exception cref="ArgumentException">Thrown if the input string contains a character that cannot be encoded in the Code 93 extended character set.</exception>
    private static List<string> ExpandExtended(string data)
    {
        var list = new List<string>();

        foreach (var c in data)
        {
            if (!Code93ExtendedTable.TryGet(c, out var seq) || string.IsNullOrEmpty(seq))
            {
                throw new ArgumentException($"Character '{c}' not encodable in Code93 extended.");
            }

            // seq can be "A" or "(%)A" etc.
            // We split into tokens of 1 or 3 chars
            if (seq.Length == 1)
            {
                list.Add(seq);
            }
            else
            {
                // Extended sequences : "(%)A" → "(%)" + "A"
                list.Add(seq[..3]);
                list.Add(seq[3..]);
            }
        }

        return list;
    }

    /// <summary>
    /// Calculates a weighted checksum for a sequence of integer values using a repeating weight pattern.
    /// </summary>
    /// <remarks>The weighting starts at 1 for the last element in the sequence and increments by 1 for each
    /// preceding element, resetting to 1 after reaching the specified maximum weight. This method is commonly used in
    /// barcode or validation algorithms that require a modular checksum.</remarks>
    /// <param name="values">The sequence of integer values to include in the checksum calculation. The order of values affects the result.</param>
    /// <param name="maxWeight">The maximum weight to apply before the weighting pattern repeats. Must be greater than zero.</param>
    /// <returns>The computed checksum as an integer in the range 0 to 46, inclusive.</returns>
    private static int ComputeChecksum(IReadOnlyList<int> values, int maxWeight)
    {
        var weight = 1;
        var sum = 0;

        for (var i = values.Count - 1; i >= 0; i--)
        {
            sum += values[i] * weight;
            weight++;

            if (weight > maxWeight)
            {
                weight = 1;
            }
        }

        return sum % 47;
    }

    /// <summary>
    /// Builds a payload representing a 1D barcode using the specified readable text, symbol sequence, and rendering
    /// options.
    /// </summary>
    /// <remarks>If the ShowText option is enabled, the readable text is included in the payload and
    /// positioned below the barcode. The method uses the provided symbol indices to generate the bar patterns according
    /// to the Code 93 specification.</remarks>
    /// <param name="readable">The human-readable text to associate with the barcode. This text may be displayed below the barcode if the
    /// options specify.</param>
    /// <param name="symbols">A list of symbol indices representing the encoded barcode data. Each index corresponds to a pattern in the Code
    /// 93 symbol table.</param>
    /// <param name="options">The rendering options that control barcode appearance, such as module width, height, and whether to display the
    /// readable text.</param>
    /// <returns>A Barcode1DPayload object containing the barcode's bar and text elements, ready for rendering.</returns>
    private static Barcode1DPayload BuildPayload(
        string readable,
        List<int> symbols,
        Code93Options options)
    {
        var bars = new List<Barcode1DBar>();
        var texts = new List<BarcodeText>();
        var x = 0.0;

        foreach (var symbol in symbols)
        {
            var pattern = Code93Table.Symbols[symbol].Pattern;

            for (var i = 0; i < pattern.Length; i++)
            {
                var width = (pattern[i] - '0') * options.ModuleWidth;

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
                X = x / 2.0,
                Y = options.ModuleHeight,
                Text = readable,
                Anchor = SvgTextAnchor.Middle
            });
        }

        return new Barcode1DPayload
        {
            Value = readable,
            Bars = bars,
            Texts = texts
        };
    }
}
