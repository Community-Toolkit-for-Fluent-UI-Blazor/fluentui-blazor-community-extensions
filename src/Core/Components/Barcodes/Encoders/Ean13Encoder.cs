using System.Globalization;
using FluentUI.Blazor.Community.Components.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides an encoder for generating EAN-13 barcode payloads from numeric input data and encoding options.
/// </summary>
/// <remarks>This encoder implements the EAN-13 barcode standard, which encodes 12 or 13-digit numeric data into a
/// barcode format suitable for retail and inventory applications. The encoder validates input data for correct length
/// and digit-only content, and automatically computes the checksum digit if only 12 digits are provided. Use this
/// encoder to convert EAN-13 data into a payload that can be rendered as a barcode using the specified
/// options.</remarks>
internal sealed class Ean13Encoder : IBarcodeEncoder<Barcode1DPayload, Ean13Options>
{
    /// <summary>
    /// Represents the invariant culture, which is culture-insensitive and associated with the English language but not
    /// with any country or region.
    /// </summary>
    /// <remarks>The invariant culture is used in operations that require culture-independent results, such as
    /// formatting and parsing operations that must yield consistent results regardless of the system's culture
    /// settings.</remarks>
    private static readonly CultureInfo s_invariantCulture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Gets the singleton instance of the Ean13Encoder class.
    /// </summary>
    /// <remarks>Use this property to access a shared, thread-safe instance of the Ean13Encoder without
    /// creating a new object.</remarks>
    public static Ean13Encoder Instance { get; } = new Ean13Encoder();

    /// <inheritdoc />
    public Barcode1DPayload Encode(string data, Ean13Options options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(data);

        if (data.Length != 12)
        {
            throw new ArgumentException("EAN-13 requires 12.");
        }

        if (!data.All(char.IsDigit))
        {
            throw new ArgumentException("EAN-13 accepts digits only.");
        }

        if (data.Length == 12)
        {
            var checksum = ComputeChecksum(data);
            data += checksum.ToString(s_invariantCulture);
        }

        return BuildPayload(data, options);
    }

    /// <summary>
    /// Calculates the checksum digit for a 12-digit numeric string using the standard UPC-A algorithm.
    /// </summary>
    /// <remarks>The input string must be exactly 12 digits long. This method does not validate the input
    /// format; passing a string with non-digit characters or an incorrect length may result in incorrect results or
    /// exceptions.</remarks>
    /// <param name="data12">A string containing exactly 12 numeric characters for which to compute the checksum digit. Each character must
    /// be a digit ('0'-'9').</param>
    /// <returns>The computed checksum digit as an integer in the range 0 to 9.</returns>
    private static int ComputeChecksum(string data12)
    {
        var sum = 0;

        for (var i = 0; i < 12; i++)
        {
            var digit = data12[i] - '0';
            sum += (i % 2 == 0) ? digit * 3 : digit;
        }

        return (10 - (sum % 10)) % 10;
    }

    /// <summary>
    /// Builds the barcode payload for the given 13-digit EAN-13 data string and encoding options.
    /// </summary>
    /// <param name="data">The 13-digit numeric string representing the EAN-13 data to encode.</param>
    /// <param name="options">The encoding options that specify parameters such as module width and heights for the barcode bars and text zones.</param>
    /// <returns></returns>
    private static Barcode1DPayload BuildPayload(string data, Ean13Options options)
    {
        var bars = new List<Barcode1DBar>();
        var texts = new List<BarcodeText>();
        var x = 0.0;
        var w = options.ModuleWidth;

        // Start guard
        GS1Pattern.Add(bars, ref x, w, options.GuardModuleHeight, "101");
        var leftGuardEnd = x;

        var first = data[0] - '0';
        var parity = Ean13Table.Parity[first];

        // Left 6 digits
        for (var i = 1; i <= 6; i++)
        {
            var digit = data[i] - '0';
            var p = parity[i - 1];

            var pattern = p == 'L'
                ? EanTable.L[digit]
                : EanTable.G[digit];

            GS1Pattern.Add(bars, ref x, w, options.ModuleHeight, pattern);
        }

        var centerGuardStart = x;
        GS1Pattern.Add(bars, ref x, w, options.GuardModuleHeight, "01010");
        var centerGuardEnd = x;

        // Right 6 digits
        for (var i = 7; i <= 12; i++)
        {
            var digit = data[i] - '0';
            GS1Pattern.Add(bars, ref x, w, options.ModuleHeight, EanTable.R[digit]);
        }

        var rightGuardStart = x;
        GS1Pattern.Add(bars, ref x, w, options.GuardModuleHeight, "101");

        var y = options.ModuleHeight;

        // Zones texte
        var zone1Start = leftGuardEnd;
        var zone1End = centerGuardStart;
        var zone2Start = centerGuardEnd;
        var zone2End = rightGuardStart;

        var zone1Width = zone1End - zone1Start;
        var slot1Width = zone1Width / 6.0;

        // Outer left digit
        Gs1Zone.AddOuterLeftDigit(texts, data[0], zone1Start, slot1Width, y);

        // 6 digits left
        Gs1Zone.AddDigitZone(texts, data, 1, 6, zone1Start, zone1End, y);

        // 6 digits right
        Gs1Zone.AddDigitZone(texts, data, 7, 6, zone2Start, zone2End, y);

        return new Barcode1DPayload
        {
            Value = data,
            Bars = bars,
            Texts = texts
        };
    }
}
