using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Vision;

/// <summary>
/// Represents a utility class for transforming images based on different types of color vision deficiencies (color blindness). The class provides a method to apply the appropriate color transformation to an image's pixel data, allowing for simulation of how the image would appear to individuals with various types of color vision deficiencies. The transformations are based on the specified ColorVisionType and the working color space (RgbWorkingSpace).
/// </summary>
public static class ColorImageTransform
{
    /// <summary>
    /// Transforms an image's pixel data to simulate the appearance of a specified color vision deficiency using the
    /// given RGB working space.
    /// </summary>
    /// <remarks>This method processes each pixel in parallel and uses caching to optimize repeated color
    /// transformations. The returned array has the same length and format as the input.</remarks>
    /// <param name="pixels">The byte array containing the image's pixel data in RGBA format (4 bytes per pixel).</param>
    /// <param name="type">The type of color vision deficiency to simulate.</param>
    /// <param name="space">The RGB working space to use for color transformation.</param>
    /// <returns>A new byte array containing the transformed pixel data in RGBA format. If no transformation is needed, returns
    /// the original array.</returns>
    /// <exception cref="ArgumentException">Thrown if the length of the pixel buffer is not a multiple of 4, indicating the data is not in RGBA format.</exception>
    public static byte[] TransformImage(
        byte[] pixels,
        ColorVisionType type,
        RgbWorkingSpace space)
    {
        if (pixels.Length % 4 != 0)
        {
            throw new ArgumentException("Pixel buffer must be RGBA (4 bytes per pixel).");
        }

        var (group, severity) = ColorVisionMapper.Decompose(type);

        if (group == ColorVisionGroup.Normal || severity == 0)
        {
            return pixels;
        }

        var pixelCount = pixels.Length / 4;
        var lut = BuildLut(pixels, type, space);
        var output = new byte[pixels.Length];

        Parallel.For(0, pixelCount, i =>
        {
            var index = i * 4;
            var packed = (pixels[index] << 24) |
                         (pixels[index + 1] << 16) |
                         (pixels[index + 2] << 8) |
                         pixels[index + 3];

            var t = lut[packed];

            output[index] = (byte)(t >> 24);
            output[index + 1] = (byte)(t >> 16);
            output[index + 2] = (byte)(t >> 8);
            output[index + 3] = (byte)(t);
        });

        return output;
    }

    /// <summary>
    /// Transforms an array of ARGB pixel values to simulate the appearance of an image as perceived by individuals with
    /// a specified type of color vision deficiency.
    /// </summary>
    /// <remarks>This method applies a color vision deficiency simulation to each pixel in the input array
    /// using parallel processing for performance. If the specified color vision type is normal or has zero severity,
    /// the original pixel array is returned without modification. The transformation is cached per unique pixel value
    /// to improve efficiency when the same color appears multiple times.</remarks>
    /// <param name="pixels">An array of 32-bit ARGB pixel values representing the source image to transform.</param>
    /// <param name="type">The type and severity of color vision deficiency to simulate.</param>
    /// <param name="space">The RGB working space to use for color transformation calculations.</param>
    /// <returns>An array of 32-bit ARGB pixel values representing the transformed image. If no transformation is required,
    /// returns the original array.</returns>
    public static int[] TransformImage(
        int[] pixels,
        ColorVisionType type,
        RgbWorkingSpace space)
    {
        var (group, severity) = ColorVisionMapper.Decompose(type);

        if (group == ColorVisionGroup.Normal || severity == 0)
        {
            return pixels;
        }

        var lut = BuildLut(pixels.AsSpan(), type, space);
        var result = new int[pixels.Length];

        Parallel.For(0, pixels.Length, i =>
        {
            result[i] = lut[pixels[i]];
        });

        return result;
    }

    /// <summary>
    /// Builds a lookup table that maps unique packed pixel values to their transformed equivalents based on the
    /// specified color vision type and RGB working space.
    /// </summary>
    /// <remarks>The lookup table enables efficient mapping of pixel values for color vision simulation,
    /// reducing redundant computations when processing images with repeated colors.</remarks>
    /// <param name="pixels">A read-only span of packed 32-bit integer pixel values to process. Each value represents a pixel in ARGB format.</param>
    /// <param name="type">The type of color vision deficiency to simulate when transforming pixel values.</param>
    /// <param name="space">The RGB working space to use for color transformation.</param>
    /// <returns>A dictionary mapping each unique input pixel value to its transformed packed value according to the specified
    /// color vision type and working space.</returns>
    private static Dictionary<int, int> BuildLut(
        ReadOnlySpan<int> pixels,
        ColorVisionType type,
        RgbWorkingSpace space)
    {
        var unique = new HashSet<int>(pixels.Length);

        foreach (var p in pixels)
        {
            unique.Add(p);
        }

        var lut = new Dictionary<int, int>(unique.Count);

        foreach (var packed in unique)
        {
            var a = (byte)(packed >> 24);
            var r = (byte)(packed >> 16);
            var g = (byte)(packed >> 8);
            var b = (byte)(packed);

            var srgb = new Srgb8(r, g, b, a);
            var t = ColorVisionTransform.Apply(srgb, type, space);

            lut[packed] = (t.A << 24) | (t.R << 16) | (t.G << 8) | t.B;
        }

        return lut;
    }

    /// <summary>
    /// Builds a lookup table (LUT) that maps unique packed pixel values to their transformed equivalents based on the
    /// specified color vision type and RGB working space.
    /// </summary>
    /// <remarks>The lookup table enables efficient color transformation by avoiding redundant computations
    /// for repeated colors in the input pixel data.</remarks>
    /// <param name="pixels">An array of bytes representing pixel data in RGBA format. The length must be a multiple of 4.</param>
    /// <param name="type">The type of color vision transformation to apply to each unique color.</param>
    /// <param name="space">The RGB working space to use for color transformation.</param>
    /// <returns>A dictionary mapping each unique packed pixel value to its transformed packed value according to the specified
    /// color vision type and working space.</returns>
    private static Dictionary<int, int> BuildLut(
        byte[] pixels,
        ColorVisionType type,
        RgbWorkingSpace space)
    {
        var pixelCount = pixels.Length / 4;
        var unique = new HashSet<int>(pixelCount);

        for (var i = 0; i < pixelCount; i++)
        {
            var index = i * 4;
            var packed =
                (pixels[index] << 24) |
                (pixels[index + 1] << 16) |
                (pixels[index + 2] << 8) |
                pixels[index + 3];

            unique.Add(packed);
        }

        var lut = new Dictionary<int, int>(unique.Count);

        foreach (var packed in unique)
        {
            var r = (byte)(packed >> 24);
            var g = (byte)(packed >> 16);
            var b = (byte)(packed >> 8);
            var a = (byte)(packed);
            var srgb = new Srgb8(r, g, b, a);
            var t = ColorVisionTransform.Apply(srgb, type, space);

            var transformed =
                (t.R << 24) |
                (t.G << 16) |
                (t.B << 8) |
                t.A;

            lut[packed] = transformed;
        }

        return lut;
    }
}
