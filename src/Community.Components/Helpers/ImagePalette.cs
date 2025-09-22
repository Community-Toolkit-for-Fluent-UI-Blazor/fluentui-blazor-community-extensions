using SkiaSharp;

namespace FluentUI.Blazor.Community.Helpers;

/// <summary>
/// Retrieves the dominant colors from an image using SkiaSharp.
/// </summary>
public static class ImagePalette
{
    /// <summary>
    /// Extracts the most frequent colors from an image stream.
    /// </summary>
    /// <param name="imageStream">Stream of the image.</param>
    /// <param name="count">Number of color to take.</param>
    /// <returns>Returns the list of the most frequent colors extracted from the image.</returns>
    public static List<string> ExtractColorsFromImage(Stream? imageStream, int count)
    {
        ArgumentNullException.ThrowIfNull(imageStream);
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);

        if (imageStream.Length == 0)
        {
            return [];
        }

        using var bmp = SKBitmap.Decode(imageStream);
        var frequency = new Dictionary<int, int>();

        for (var y = 0; y < bmp.Height; y += Math.Max(1, bmp.Height / 256))
        {
            for (var x = 0; x < bmp.Width; x += Math.Max(1, bmp.Width / 256))
            {
                var pixel = bmp.GetPixel(x, y);

                var dr = (pixel.Red / 16d) * 16d;
                var dg = (pixel.Green / 16d) * 16d;
                var db = (pixel.Blue / 16d) * 16d;

                var r = (int)dr;
                var g = (int)dg;
                var b = (int)db;
                var key = (r << 16) | (g << 8) | b;
                frequency.TryGetValue(key, out var f);
                frequency[key] = f + 1;
            }
        }

        var top = frequency.OrderByDescending(kv => kv.Value)
                      .Take(count)
                      .Select(kv => $"#{((kv.Key >> 16) & 0xFF):X2}{((kv.Key >> 8) & 0xFF):X2}{(kv.Key & 0xFF):X2}")
                      .ToList();

        return top;
    }
}

