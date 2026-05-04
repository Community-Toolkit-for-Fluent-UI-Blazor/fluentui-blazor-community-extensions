using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to encode strings into Codabar barcode payloads using specified encoding options.
/// </summary>
/// <remarks>Codabar is a linear barcode symbology commonly used in libraries, blood banks, and parcel services.
/// This encoder validates input for required start and stop characters and ensures all characters are valid for the
/// Codabar standard. The encoder produces a payload suitable for rendering as a 1D barcode.</remarks>
internal sealed class CodabarEncoder : IBarcodeEncoder<Barcode1DPayload, CodabarOptions>
{
    /// <summary>
    /// Gets the singleton instance of the CodabarEncoder class.
    /// </summary>
    /// <remarks>Use this property to access a shared, thread-safe instance of CodabarEncoder without creating
    /// a new object.</remarks>
    public static CodabarEncoder Instance { get; } = new();

    /// <inheritdoc />
    public Barcode1DPayload Encode(string data, CodabarOptions options)
    {
        List<Barcode1DBar> bars = [];
        List<BarcodeText> texts = [];

        if (string.IsNullOrWhiteSpace(data))
        {
            throw new ArgumentException("Codabar value cannot be empty.");
        }

        data = data.Trim().ToUpperInvariant();

        if (data.Length < 2)
        {
            throw new ArgumentException("Codabar requires start and stop characters.");
        }

        if (!CodabarTable.StartStop.Contains(data[0]) ||
            !CodabarTable.StartStop.Contains(data[^1]))
        {
            throw new ArgumentException("Codabar must start and end with A, B, C, or D.");
        }

        foreach (var c in data)
        {
            if (!CodabarTable.Patterns.ContainsKey(c))
            {
                throw new ArgumentException($"Invalid Codabar character: {c}");
            }
        }

        var x = 0.0;
        var height = options.ModuleHeight;

        foreach (var c in data)
        {
            var pattern = CodabarTable.Patterns[c];

            for (var i = 0; i < pattern.Length; i++)
            {
                var isBar = (i % 2 == 0);
                var width = pattern[i] == 'W'
                    ? options.ModuleWidth * options.WideRatio
                    : options.ModuleWidth;

                if (isBar)
                {
                    bars.Add(new Barcode1DBar
                    {
                        X = x,
                        Y = 0,
                        Width = width,
                        Height = height
                    });
                }

                x += width;
            }

            x += options.ModuleWidth;
        }

        if (options.ShowText)
        {
            texts.Add(new BarcodeText
            {
                Text = data,
                X = x / 2.0,
                Y = 0,
                Anchor = SvgTextAnchor.Middle
            });
        }

        return new Barcode1DPayload()
        {
            Bars = bars,
            Value = data,
            Texts = texts
        };
    }
}
