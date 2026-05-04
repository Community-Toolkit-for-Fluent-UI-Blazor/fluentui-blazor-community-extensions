using System.Globalization;
using FluentUI.Blazor.Community.Components.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides an encoder for generating EAN-8 barcode payloads from numeric input data and encoding options.
/// </summary>
/// <remarks>The Ean8Encoder validates input data to ensure it consists of 7 or 8 numeric digits, automatically
/// calculating and appending the checksum digit if only 7 digits are provided. It produces a Barcode1DPayload that
/// represents the encoded EAN-8 barcode, including bar and text zone information as specified by the provided
/// Ean8Options. This encoder is intended for use with the IBarcodeEncoder interface for EAN-8 barcode generation
/// scenarios.</remarks>
internal sealed class Ean8Encoder : IBarcodeEncoder<Barcode1DPayload, Ean8Options>
{
    /// <summary>
    /// Represents the culture-independent (invariant) culture, which is associated with the English language but not
    /// with any country or region.
    /// </summary>
    /// <remarks>The invariant culture is used in operations that require culture-independent results, such as
    /// formatting and parsing operations that must behave consistently regardless of the user's locale.</remarks>
    private static readonly CultureInfo s_invariantCulture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Gets the singleton instance of the Ean8Encoder class.
    /// </summary>
    /// <remarks>Use this property to access a shared, thread-safe instance of the Ean8Encoder. This approach
    /// avoids unnecessary allocations and ensures consistent behavior across the application.</remarks>
    public static Ean8Encoder Instance { get; } = new Ean8Encoder();

    /// <inheritdoc />
    public Barcode1DPayload Encode(string data, Ean8Options options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(data);

        if (data.Length != 7 && data.Length != 8)
        {
            throw new ArgumentException("EAN-8 requires 7 or 8 digits.");
        }

        if (!data.All(char.IsDigit))
        {
            throw new ArgumentException("EAN-8 accepts digits only.");
        }

        if (data.Length == 7)
        {
            var checksum = ComputeChecksum(data);
            data += checksum.ToString(s_invariantCulture);
        }

        return BuildPayload(data, options);
    }

    /// <summary>
    /// Calculates the checksum digit for a 7-digit EAN-8 code using the standard algorithm, which involves summing the digits
    ///  with alternating weights of 3 and 1, then computing the checksum as the value that must be added to this sum to reach the next multiple of 10.
    /// </summary>
    /// <param name="data7">The 7-digit string for which to compute the checksum.</param>
    /// <returns>Returns the computed checksum digit as an integer between 0 and 9.</returns>
    private static int ComputeChecksum(string data7)
    {
        var sum = 0;

        for (var i = 0; i < 7; i++)
        {
            var digit = data7[i] - '0';
            sum += (i % 2 == 0) ? digit * 3 : digit;
        }

        return (10 - (sum % 10)) % 10;
    }

    /// <summary>
    /// Generates a barcode payload for an EAN-8 barcode using the specified data and rendering options.
    /// </summary>
    /// <remarks>The method constructs the barcode by generating bar patterns and text zones according to the
    /// EAN-8 specification. The input data must be validated for length and numeric content before calling this
    /// method.</remarks>
    /// <param name="data">The string containing the 8-digit EAN-8 barcode data to encode. Must be exactly 8 numeric characters.</param>
    /// <param name="options">The options that define rendering parameters such as module width and bar heights for the barcode.</param>
    /// <returns>A Barcode1DPayload object containing the encoded bars and text zones representing the EAN-8 barcode.</returns>
    private static Barcode1DPayload BuildPayload(string data, Ean8Options options)
    {
        var bars = new List<Barcode1DBar>();
        var texts = new List<BarcodeText>();
        var x = 0.0;
        var w = options.ModuleWidth;

        // Start guard
        GS1Pattern.Add(bars, ref x, w, options.GuardModuleHeight, "101");
        var leftGuardEnd = x;

        // 4 digits left
        for (var i = 0; i < 4; i++)
        {
            var digit = data[i] - '0';
            GS1Pattern.Add(bars, ref x, w, options.ModuleHeight, EanTable.L[digit]);
        }

        var centerGuardStart = x;
        GS1Pattern.Add(bars, ref x, w, options.GuardModuleHeight, "01010");
        var centerGuardEnd = x;

        // 4 digits right
        for (var i = 4; i < 8; i++)
        {
            var digit = data[i] - '0';
            GS1Pattern.Add(bars, ref x, w, options.ModuleHeight, EanTable.R[digit]);
        }

        var rightGuardStart = x;
        GS1Pattern.Add(bars, ref x, w, options.GuardModuleHeight, "101");

        var y = options.ModuleHeight;
        var zone1Start = leftGuardEnd;
        var zone1End = centerGuardStart;
        var zone2Start = centerGuardEnd;
        var zone2End = rightGuardStart;

        // 4 digits left
        Gs1Zone.AddDigitZone(texts, data, 0, 4, zone1Start, zone1End, y);

        // 4 digits right
        Gs1Zone.AddDigitZone(texts, data, 4, 4, zone2Start, zone2End, y);

        return new Barcode1DPayload
        {
            Value = data,
            Bars = bars,
            Texts = texts
        };
    }
}
