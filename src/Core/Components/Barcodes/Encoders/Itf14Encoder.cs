using System.Globalization;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides ITF-14 barcode encoding functionality for digit-only input data.
/// </summary>
/// <remarks>This encoder generates ITF-14 barcodes, which are commonly used for packaging and logistics. The
/// input data must consist of 13 or 14 numeric digits. If 13 digits are provided, the encoder automatically calculates
/// and appends the checksum digit as the 14th digit. If 14 digits are provided, the last digit is assumed to be the
/// checksum. Any non-numeric input or incorrect length will result in an exception.</remarks>
internal sealed class Itf14Encoder : IBarcodeEncoder<Barcode1DPayload, Itf14Options>
{
    /// <summary>
    /// Gets the singleton instance of the ITF-14 barcode encoder.
    /// </summary>
    /// <remarks>Use this property to access a shared, thread-safe instance of the encoder without creating a
    /// new object.</remarks>
    public static Itf14Encoder Instance { get; } = new Itf14Encoder();

    /// <inheritdoc />
    public Barcode1DPayload Encode(string data, Itf14Options options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(data);

        if (!data.All(char.IsDigit))
        {
            throw new ArgumentException("ITF-14 accepts digits only.");
        }

        if (data.Length == 13)
        {
            var checksum = ItfTools.ComputeChecksum(data);
            data += checksum.ToString(CultureInfo.InvariantCulture);
        }
        else if (data.Length != 14)
        {
            throw new ArgumentException("ITF-14 requires 13 or 14 digits.");
        }

        return BuildPayload(data, options);
    }

    /// <summary>
    /// Builds a payload representing a 1D ITF-14 barcode from the specified data and options.
    /// </summary>
    /// <remarks>The input data must be exactly 14 numeric digits. If the <see cref="Itf14Options.ShowText"/>
    /// property is set to <see langword="true"/>, the human-readable text is included in the payload.</remarks>
    /// <param name="data">A 14-digit string containing the numeric data to encode in the barcode. Each character must be a digit
    /// ('0'-'9').</param>
    /// <param name="options">The options that specify barcode rendering parameters, such as module width, height, and whether to display
    /// human-readable text.</param>
    /// <returns>A <see cref="Barcode1DPayload"/> object containing the encoded bars and optional text for the ITF-14 barcode.</returns>
    private static Barcode1DPayload BuildPayload(string data, Itf14Options options)
    {
        var bars = new List<Barcode1DBar>();
        var texts = new List<BarcodeText>();
        var x = 0.0;

        GS1Pattern.Add(bars, ref x, options.ModuleWidth, options.ModuleHeight, ItfTable.Start);

        for (var i = 0; i < 14; i += 2)
        {
            var d1 = data[i] - '0';
            var d2 = data[i + 1] - '0';

            var pattern = ItfTools.BuildInterleavedPair(d1, d2);
            GS1Pattern.Add(bars, ref x, options.ModuleWidth, options.ModuleHeight, pattern);
        }

        GS1Pattern.Add(bars, ref x, options.ModuleWidth, options.ModuleHeight, ItfTable.Stop);

        if (options.ShowText)
        {
            texts.Add(new BarcodeText
            {
                Text = data,
                X = x / 2,
                Y = options.ModuleHeight,
                Anchor = SvgTextAnchor.Middle
            });
        }

        return new Barcode1DPayload
        {
            Value = data,
            Bars = bars,
            Texts = texts
        };
    }
}

