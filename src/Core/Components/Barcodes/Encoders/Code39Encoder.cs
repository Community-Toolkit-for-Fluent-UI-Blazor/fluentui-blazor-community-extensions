using System.Text;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to encode data into the Code 39 barcode symbology using configurable options.
/// </summary>
/// <remarks>This encoder supports standard Code 39 encoding, including optional checksum calculation as specified
/// in the provided options. The input data must consist only of valid Code 39 characters and cannot include the
/// start/stop character ('*'). Use this class to generate barcode payloads suitable for rendering or further
/// processing.</remarks>
internal sealed class Code39Encoder : IBarcodeEncoder<Barcode1DPayload, Code39Options>
{
    /// <summary>
    /// Represents the set of characters used in the encoding alphabet for barcode operations.
    /// </summary>
    /// <remarks>The alphabet includes digits, uppercase letters, and specific symbols commonly used in
    /// barcode standards such as Code 39. This constant can be used to validate or encode barcode data.</remarks>
    private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";

    /// <summary>
    /// Initializes a new instance of the Code39Encoder class.
    /// </summary>
    /// <remarks>This constructor is private to prevent direct instantiation of the Code39Encoder class.
    /// Instances should be created using the provided factory methods or properties.</remarks>
    private Code39Encoder()
    { }

    /// <summary>
    /// Gets the singleton instance of the Code39Encoder class.
    /// </summary>
    /// <remarks>Use this property to access a shared, thread-safe instance of the Code39Encoder without
    /// creating a new object.</remarks>
    public static Code39Encoder Instance { get; } = new();

    /// <inheritdoc />
    public Barcode1DPayload Encode(string data, Code39Options options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(data);
        string encoded;
        string readable;

        if (options.IsExtended)
        {
            // Code 39 étendu : ASCII 0–127 sauf '*'
            var sb = new StringBuilder();

            foreach (var c in data)
            {
                if (c == '*')
                {
                    throw new ArgumentException("Character '*' is reserved in Code39.");
                }

                if (!Code39ExtendedTable.TryGet(c, out var seq))
                {
                    throw new ArgumentException($"Character '{c}' not encodable in Code39 extended.");
                }

                sb.Append(seq);
            }

            encoded = sb.ToString();
            readable = data;
        }
        else
        {
            foreach (var c in data)
            {
                if (!Code39Table.Patterns.ContainsKey(c) || c == '*')
                {
                    throw new ArgumentException($"Character '{c}' not allowed in Code39.");
                }
            }

            encoded = data;
            readable = data;
        }

        // Optional checksum
        if (options.EnableChecksum)
        {
            var checksumChar = ComputeChecksum(data);
            encoded += checksumChar;
            readable += checksumChar;
        }

        // Add start/stop
        var full = "*" + encoded + "*";

        return BuildPayload(full, readable, options);
    }

    /// <summary>
    /// Calculates the checksum character for the specified data string using the Code 39 barcode alphabet.
    /// </summary>
    /// <remarks>The checksum is calculated by summing the index values of each character in the input string
    /// and returning the character at the position of the sum modulo 43 in the Code 39 alphabet. The method does not
    /// validate that all characters in the input are valid; invalid characters will result in a checksum based on their
    /// index of -1.</remarks>
    /// <param name="data">The input string for which the checksum character is to be computed. Each character must be present in the Code
    /// 39 alphabet: "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%".</param>
    /// <returns>The checksum character corresponding to the input data, as defined by the Code 39 barcode specification.</returns>
    private static char ComputeChecksum(string data)
    {
        var sum = 0;

        foreach (var c in data)
        {
            sum += Alphabet.IndexOf(c);
        }

        return Alphabet[sum % 43];
    }

    /// <summary>
    /// Constructs a payload representing the bars and value for a Code39 1D barcode using the specified input string
    /// and rendering options.
    /// </summary>
    /// <remarks>The method generates the barcode bars based on the Code39 symbology and applies the specified
    /// rendering options. The input string must only contain valid Code39 characters; otherwise, an exception may
    /// occur.</remarks>
    /// <param name="full">The string to encode in the barcode. Each character must be supported by the Code39 symbology.</param>
    /// <param name="readable">The human-readable text to display alongside the barcode</param>
    /// <param name="options">The rendering options that define module width, height, and wide ratio for the barcode generation.</param>
    /// <returns>A Barcode1DPayload containing the encoded value and a collection of bars representing the barcode structure.</returns>
    private static Barcode1DPayload BuildPayload(
        string full,
        string readable,
        Code39Options options)
    {
        var bars = new List<Barcode1DBar>();
        var texts = new List<BarcodeText>();
        var x = 0.0;

        foreach (var c in full)
        {
            var pattern = Code39Table.Patterns[c];

            for (var i = 0; i < pattern.Length; i++)
            {
                var isWide = pattern[i] == 'w';
                var width = isWide ? options.ModuleWidth * options.WideRatio : options.ModuleWidth;

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

            // Inter-character gap (narrow space)
            x += options.ModuleWidth;
        }

        texts.Add(new BarcodeText
        {
            X = x / 2.0,               
            Y = options.ModuleHeight,
            Text = readable,
            Anchor = SvgTextAnchor.Middle
        });

        return new Barcode1DPayload
        {
            Value = full,
            Bars = bars,
            Texts = texts
        };
    }
}
