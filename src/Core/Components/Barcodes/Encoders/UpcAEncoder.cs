using System.Globalization;
using FluentUI.Blazor.Community.Components.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to encode UPC-A barcodes from numeric data using specified rendering options.
/// </summary>
/// <remarks>This encoder validates input data for UPC-A compliance, computes the required checksum digit, and
/// generates the bar and space patterns representing the barcode. It implements the IBarcodeEncoder interface for UPC-A
/// barcodes and is intended for internal use within the barcode generation library.</remarks>
internal sealed class UpcAEncoder : IBarcodeEncoder<Barcode1DPayload, UpcAOptions>
{
    /// <summary>
    /// Represents a static reference to the invariant culture, which is culture-insensitive and associated with the
    /// English language but not with any country or region.
    /// </summary>
    /// <remarks>The invariant culture is used in operations that require culture-independent results, such as
    /// formatting and parsing operations that must behave consistently regardless of the user's locale.</remarks>
    private static readonly CultureInfo s_invariantCulture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Initializes a new instance of the UpcAEncoder class. This constructor is private to prevent direct instantiation
    /// from outside the class.
    /// </summary>
    /// <remarks>This constructor is typically used to enforce controlled creation of UpcAEncoder instances,
    /// such as through a factory method or static property.</remarks>
    private UpcAEncoder() { }

    /// <summary>
    /// Gets the singleton instance of the UpcAEncoder class.
    /// </summary>
    /// <remarks>Use this property to access a shared, thread-safe instance of the UpcAEncoder without
    /// creating a new object.</remarks>
    public static UpcAEncoder Instance { get; } = new UpcAEncoder();

    /// <inheritdoc />
    public Barcode1DPayload Encode(string data, UpcAOptions options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(data);

        if (data.Length != 11)
        {
            throw new ArgumentException("UPC-A requires 11.");
        }

        if (!data.All(char.IsDigit))
        {
            throw new ArgumentException("UPC-A accepts digits only.");
        }

        // Compute checksum if needed
        if (data.Length == 11)
        {
            var checksum = ComputeChecksum(data);
            data += checksum.ToString(s_invariantCulture);
        }

        return BuildPayload(data, options);
    }

    /// <summary>
    /// Calculates the UPC-A checksum digit for the specified 11-digit numeric string.
    /// </summary>
    /// <remarks>The checksum is calculated according to the UPC-A standard, which requires the input to be 11
    /// digits. The result can be appended as the twelfth digit to form a valid UPC-A code.</remarks>
    /// <param name="data11">A string containing exactly 11 numeric digits representing the UPC-A data for which to compute the checksum.</param>
    /// <returns>The computed checksum digit as an integer in the range 0 through 9.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="data11"/> does not contain exactly 11 characters or contains non-digit characters.</exception>
    internal static int ComputeChecksum(string data11)
    {
        if (data11.Length != 11)
        {
            throw new ArgumentException("UPC-A checksum requires 11 digits.");
        }

        if (!data11.All(char.IsDigit))
        {
            throw new ArgumentException("UPC-A accepts digits only.");
        }

        var sumOdd = 0;
        var sumEven = 0;

        for (var i = 0; i < 11; i++)
        {
            var digit = data11[i] - '0';

            if ((i % 2) == 0)
            {
                sumOdd += digit;
            }
            else
            {
                sumEven += digit;
            }
        }

        var total = (sumOdd * 3) + sumEven;

        return (10 - (total % 10)) % 10;
    }

    /// <summary>
    /// Builds a payload representing the bar and space patterns for a UPC-A barcode based on the provided data and
    /// rendering options.
    /// </summary>
    /// <remarks>The method generates the full UPC-A barcode structure, including guard patterns and digit
    /// encodings, using the specified rendering options. The input data must be a valid UPC-A code; no validation is
    /// performed within this method.</remarks>
    /// <param name="data">A 12-digit string containing the numeric data to encode in the UPC-A barcode. Each character must be a digit
    /// from 0 to 9.</param>
    /// <param name="options">The rendering options that specify module width, module height, and other barcode appearance settings.</param>
    /// <returns>A Barcode1DPayload object containing the original data and a list of bar definitions representing the UPC-A
    /// barcode pattern.</returns>
    private static Barcode1DPayload BuildPayload(string data, UpcAOptions options)
    {
        var bars = new List<Barcode1DBar>();
        var texts = new List<BarcodeText>();
        var x = 0.0;
        var width = options.ModuleWidth;

        // Left guard
        GS1Pattern.Add(bars, ref x, width, options.GuardModuleHeight, "101");
        var leftGuardEnd = x;

        // Left digits (L patterns)
        for (var i = 0; i < 6; i++)
        {
            GS1Pattern.Add(bars, ref x, width, options.ModuleHeight, EanTable.L[data[i] - '0']);
        }

        // Center guard
        var centerGuardStart = x;
        GS1Pattern.Add(bars, ref x, width, options.GuardModuleHeight, "01010");
        var centerGuardEnd = x;

        // Right digits (R patterns)
        for (var i = 6; i < 12; i++)
        {
            GS1Pattern.Add(bars, ref x, width, options.ModuleHeight, EanTable.R[data[i] - '0']);
        }

        // Right guard
        var rightGuardStart = x;
        GS1Pattern.Add(bars, ref x, width, options.GuardModuleHeight, "101");

        var y = options.ModuleHeight;
        var zone1Start = leftGuardEnd;
        var zone1End = centerGuardStart;
        var zone1Width = zone1End - zone1Start;
        var slot1Width = zone1Width / 5.0;

        var zone2Start = centerGuardEnd;
        var zone2End = rightGuardStart;
        var zone2Width = zone2End - zone2Start;
        var slot2Width = zone2Width / 5.0;

        // N1 (System number) - aligned to the left.
        Gs1Zone.AddOuterLeftDigit(texts, data[0], zone1Start, slot1Width, y);

        // N2–N6 (digits 1–5)
        Gs1Zone.AddDigitZone(texts, data, 1, 5, zone1Start, zone1End, y);

        // N7–N11 (digits 6–10)
        Gs1Zone.AddDigitZone(texts, data, 6, 5, zone2Start, zone2End, y);

        // C (checksum) — aligned to the right.
        Gs1Zone.AddOuterRightDigit(texts, data[11], zone2End, slot2Width, y);

        return new Barcode1DPayload
        {
            Value = data,
            Bars = bars,
            Texts = texts
        };
    }
}
