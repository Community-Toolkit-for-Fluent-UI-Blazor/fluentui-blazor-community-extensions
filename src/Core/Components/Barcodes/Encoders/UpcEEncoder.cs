using System.Globalization;
using FluentUI.Blazor.Community.Components.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides an encoder for generating UPC-E barcodes from numeric input data and encoding options.
/// </summary>
/// <remarks>UPC-E is a compressed version of the standard UPC-A barcode, used primarily for smaller packages
/// where space is limited. This encoder validates input data for correct length and digit-only content, and supports
/// number systems 0 and 1 as required by the UPC-E specification. The encoder expands UPC-E data to UPC-A format
/// internally to compute the checksum and generate the barcode pattern. Use this encoder when you need to produce UPC-E
/// barcodes for retail or inventory applications.</remarks>
internal sealed class UpcEEncoder : IBarcodeEncoder<Barcode1DPayload, UpcEOptions>
{
    /// <summary>
    /// Represents a static reference to the invariant culture, which is culture-insensitive and associated with the
    /// English language but not with any country or region.
    /// </summary>
    /// <remarks>The invariant culture is used in operations that require culture-independent results, such as
    /// formatting and parsing operations that must behave consistently regardless of the user's locale.</remarks>
    private static readonly CultureInfo s_invariantCulture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Initializes a new instance of the UpcEEncoder class.
    /// </summary>
    /// <remarks>This constructor is private to prevent direct instantiation of the UpcEEncoder class.
    /// Instances can only be created internally within the class or by designated factory methods.</remarks>
    private UpcEEncoder() { }

    /// <summary>
    /// Gets the singleton instance of the UpcEEncoder class.
    /// </summary>
    /// <remarks>Use this property to access a shared, thread-safe instance of the UpcEEncoder without
    /// creating a new object. This is useful when a single encoder instance is sufficient for application
    /// needs.</remarks>
    public static UpcEEncoder Instance { get; } = new UpcEEncoder();

    /// <inheritdoc />
    public Barcode1DPayload Encode(string data, UpcEOptions options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(data);

        if (data.Length != 6)
        {
            throw new ArgumentException("UPC-E requires 6 digits.");
        }

        if (!data.All(char.IsDigit))
        {
            throw new ArgumentException("UPC-E accepts digits only.");
        }

        // System number always 0 or 1 for UPC-E
        var system = options.NumberSystem;

        if (system != 0 && system != 1)
        {
            throw new ArgumentException("UPC-E number system must be 0 or 1.");
        }

        // Expand to UPC-A
        var upcA = ExpandToUpcA(system, data);

        // Compute checksum
        var checksum = UpcAEncoder.ComputeChecksum(upcA);
        upcA += checksum.ToString(s_invariantCulture);

        // Encode UPC-E pattern
        return BuildPayload(system, data, checksum, options);
    }

    /// <summary>
    /// Expands a UPC-E barcode value to its equivalent UPC-A barcode string representation.
    /// </summary>
    /// <remarks>The method reconstructs the full UPC-A code from the compressed UPC-E format according to
    /// standard UPC-E expansion rules. The input UPC-E string must be properly formatted; otherwise, the result may be
    /// invalid.</remarks>
    /// <param name="system">The number system digit to use as the leading digit in the UPC-A code. Typically 0 or 1.</param>
    /// <param name="upce">The 6-digit UPC-E barcode string to expand. Must be a valid UPC-E code.</param>
    /// <returns>A string containing the expanded 12-digit UPC-A barcode corresponding to the specified UPC-E value and number
    /// system.</returns>
    private static string ExpandToUpcA(int system, string upce)
    {
        var d = upce;

        return d[5] switch
        {
            '0' or '1' or '2' =>
                $"{system}{d[0]}{d[1]}{d[5]}0000{d[2]}{d[3]}{d[4]}",

            '3' =>
                $"{system}{d[0]}{d[1]}{d[2]}00000{d[3]}{d[4]}",

            '4' =>
                $"{system}{d[0]}{d[1]}{d[2]}{d[3]}00000{d[4]}",

            _ =>
                $"{system}{d[0]}{d[1]}{d[2]}{d[3]}{d[4]}0000{d[5]}"
        };
    }

    /// <summary>
    /// Builds the payload for a 1D barcode using the specified system, data, checksum, and rendering options.
    /// </summary>
    /// <remarks>The method constructs the bar and text elements according to the UPC-E encoding rules,
    /// including guard patterns and digit zones. The resulting payload can be used to render a standards-compliant
    /// barcode image.</remarks>
    /// <param name="system">The system digit that determines the encoding scheme for the barcode. Typically represents the number system or
    /// barcode type.</param>
    /// <param name="data">The string of digits to encode in the barcode. Must be exactly 6 numeric characters.</param>
    /// <param name="checksum">The checksum digit to append to the barcode, used for error detection.</param>
    /// <param name="options">The rendering options that specify module width, heights, and other visual parameters for the barcode.</param>
    /// <returns>A Barcode1DPayload object containing the encoded value, bar patterns, and associated text elements for rendering
    /// the barcode.</returns>
    private static Barcode1DPayload BuildPayload(int system, string data, int checksum, UpcEOptions options)
    {
        var bars = new List<Barcode1DBar>();
        var texts = new List<BarcodeText>();
        var x = 0.0;
        var width = options.ModuleWidth;

        // Left guard
        GS1Pattern.Add(bars, ref x, width, options.GuardModuleHeight, "101");
        var leftGuardEnd = x;

        // Parity pattern depends on system + checksum
        var parity = UpcETable.Parity[system][checksum];

        for (var i = 0; i < 6; i++)
        {
            var digit = data[i] - '0';
            var pattern = parity[i] == 'L'
                ? EanTable.L[digit]
                : EanTable.G[digit];

            GS1Pattern.Add(bars, ref x, width, options.ModuleHeight, pattern);
        }

        // Right guard
        var rightGuardStart = x;
        GS1Pattern.Add(bars, ref x, width, options.GuardModuleHeight, "010101");
        var rightGuardEnd = x;

        var y = options.ModuleHeight;
        var zoneStart = leftGuardEnd;
        var zoneEnd = rightGuardStart;
        var zoneWidth = zoneEnd - zoneStart;
        var slotWidth = zoneWidth / 6.0;

        Gs1Zone.AddOuterLeftDigit(texts, (char)('0' + system), zoneStart, slotWidth, y);
        Gs1Zone.AddDigitZone(texts, data, 0, 6, zoneStart, zoneEnd, y);
        Gs1Zone.AddOuterRightDigit(texts, (char)('0' + checksum), rightGuardEnd, slotWidth, y);

        return new Barcode1DPayload
        {
            Value = $"{system}{data}{checksum}",
            Bars = bars,
            Texts = texts
        };
    }
}
