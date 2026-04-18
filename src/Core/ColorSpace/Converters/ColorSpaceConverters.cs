using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;
using FluentUI.Blazor.Community.Components.Maths;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Converters;

/// <summary>
/// Provides static methods for converting colors between various color spaces, including linear RGB, HSL (Hue,
/// Saturation, Lightness), HSV (Hue, Saturation, Value), HSB (Hue, Saturation, Brightness), and RYB (Red, Yellow,
/// Blue).
/// </summary>
/// <remarks>This class offers utility functions for color space transformations commonly used in graphics,
/// design, and color manipulation scenarios. All methods are static and thread-safe. The conversions are intended for
/// use with linear RGB values and may not account for gamma correction or perceptual differences between color spaces.
/// Some conversions, such as RYB, use simplified algorithms and may not be suitable for all professional color
/// workflows.</remarks>
public static class ColorSpaceConverters
{
    /// <summary>
    /// Converts a color from the linear RGB color space to the HSL (Hue, Saturation, Lightness) color space.
    /// </summary>
    /// <param name="rgb">The linear RGB color to convert.</param>
    /// <param name="alpha">The alpha (opacity) value of the color.</param>
    /// <returns>An HSL color equivalent to the specified linear RGB color.</returns>
    public static Hsl ToHsl(RgbLinear rgb, double alpha = 1.0)
    {
        var r = rgb.R;
        var g = rgb.G;
        var b = rgb.B;

        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));
        var delta = max - min;

        var h = 0.0;
        var l = (max + min) / 2.0;
        var s = 0.0;

        if (delta > 0)
        {
            s = l < 0.5 ? delta / (max + min) : delta / (2 - max - min);

            if (max == r)
            {
                h = ((g - b) / delta) % 6;
            }
            else if (max == g)
            {
                h = (b - r) / delta + 2;
            }
            else
            {
                h = (r - g) / delta + 4;
            }

            h *= 60;

            if (h < 0)
            {
                h += 360;
            }
        }

        return new Hsl(h, s, l, alpha);
    }

    /// <summary>
    /// Converts a color from HSL (Hue, Saturation, Lightness) representation to its equivalent sRGB color in 8-bit per channel format.
    /// </summary>
    /// <param name="hsl">The HSL color to convert.</param>
    /// <returns>An Srgb8 instance representing the equivalent color in sRGB 8-bit format.</returns>
    public static Srgb8 ToSrgb8(Hsl hsl)
    {
        return FromHsl(hsl).ToSrgb8();
    }

    /// <summary>
    /// Converts a color from HSL (Hue, Saturation, Lightness) representation to its equivalent linear RGB color.
    /// </summary>
    /// <remarks>This method performs the conversion using the standard HSL to RGB algorithm. The resulting
    /// RGB values are in linear space, not gamma-corrected.</remarks>
    /// <param name="hsl">The HSL color to convert. The hue should be in degrees (0–360), and saturation and lightness should be in the
    /// range 0.0 to 1.0.</param>
    /// <returns>An RgbLinear instance representing the equivalent color in linear RGB space.</returns>
    public static RgbLinear FromHsl(Hsl hsl)
    {
        var h = hsl.H / 360.0;
        var s = hsl.S;
        var l = hsl.L;

        if (s == 0)
        {
            return new RgbLinear(l, l, l);
        }

        var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
        var p = 2 * l - q;

        var r = HueToRgb(p, q, h + 1.0 / 3.0);
        var g = HueToRgb(p, q, h);
        var b = HueToRgb(p, q, h - 1.0 / 3.0);

        return new RgbLinear(r, g, b);
    }

    /// <summary>
    /// Calculates the RGB component value for a given hue position as part of the HSL to RGB color conversion process.
    /// </summary>
    /// <remarks>This method is typically used internally when converting HSL color values to their RGB
    /// representation. The parameters p and q are derived from the lightness and saturation values of the HSL color
    /// model.</remarks>
    /// <param name="p">The first intermediate value used in the HSL to RGB conversion. Typically represents a lower bound for the RGB
    /// component.</param>
    /// <param name="q">The second intermediate value used in the HSL to RGB conversion. Typically represents an upper bound for the RGB
    /// component.</param>
    /// <param name="t">The hue position, normalized to the range [0, 1], used to determine the RGB component value.</param>
    /// <returns>The calculated RGB component value as a double in the range [0, 1].</returns>
    private static double HueToRgb(double p, double q, double t)
    {
        if (t < 0)
        {
            t += 1;
        }

        if (t > 1)
        {
            t -= 1;
        }

        if (t < 1.0 / 6.0)
        {
            return p + (q - p) * 6 * t;
        }

        if (t < 1.0 / 2.0)
        {
            return q;
        }

        if (t < 2.0 / 3.0)
        {
            return p + (q - p) * (2.0 / 3.0 - t) * 6;
        }

        return p;
    }

    /// <summary>
    /// Converts a color from the linear RGB color space to the HSV (Hue, Saturation, Value) color space.
    /// </summary>
    /// <param name="rgb">The linear RGB color to convert.</param>
    /// <param name="alpha">The alpha (opacity) value of the color. Default is 1.0.</param>
    /// <returns>An Hsv instance representing the equivalent color in HSV color space.</returns>
    public static Hsv ToHsv(RgbLinear rgb, double alpha = 1.0)
    {
        var r = rgb.R;
        var g = rgb.G;
        var b = rgb.B;

        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));
        var delta = max - min;

        var h = 0.0;
        var s = max == 0 ? 0 : delta / max;
        var v = max;

        if (delta > 0)
        {
            if (max == r)
            {
                h = ((g - b) / delta) % 6;
            }
            else if (max == g)
            {
                h = (b - r) / delta + 2;
            }
            else
            {
                h = (r - g) / delta + 4;
            }

            h *= 60;

            if (h < 0)
            {
                h += 360;
            }
        }

        return new Hsv(h, s, v, alpha);
    }

    /// <summary>
    /// Converts a color from HSV (Hue, Saturation, Value) color space to its equivalent linear RGB representation.
    /// </summary>
    /// <param name="hsv">The HSV color to convert. The hue should be in degrees [0, 360), and saturation and value should be in the range
    /// [0, 1].</param>
    /// <returns>An RgbLinear structure representing the color in linear RGB space that corresponds to the specified HSV value.</returns>
    public static RgbLinear FromHsv(Hsv hsv)
    {
        var h = hsv.H;
        var s = hsv.S;
        var v = hsv.V;

        if (s == 0)
        {
            return new RgbLinear(v, v, v);
        }

        var c = v * s;
        var x = c * (1 - Math.Abs((h / 60 % 2) - 1));
        var m = v - c;

        var r = 0.0;
        var g = 0.0;
        var b = 0.0;

        if (h < 60)
        {
            r = c;
            g = x;
        }

        else if (h < 120)
        {
            r = x;
            g = c;
        }
        else if (h < 180)
        {
            g = c;
            b = x;
        }
        else if (h < 240)
        {
            g = x;
            b = c;
        }
        else if (h < 300)
        {
            r = x;
            b = c;
        }
        else
        {
            r = c;
            b = x;
        }

        return new RgbLinear(r + m, g + m, b + m);
    }

    /// <summary>
    /// Converts an RGB color in linear space to its equivalent HSB (Hue, Saturation, Brightness) representation.
    /// </summary>
    /// <param name="rgb">The RGB color in linear space to convert.</param>
    /// <param name="alpha">The alpha (opacity) component of the color. The value must be between 0.0 and 1.0. The default is 1.0.</param>
    /// <returns>An Hsb structure representing the hue, saturation, brightness, and alpha components of the specified color.</returns>
    public static Hsb ToHsb(RgbLinear rgb, double alpha = 1.0)
    {
        var hsv = ToHsv(rgb, alpha);

        return new Hsb(hsv.H, hsv.S, hsv.V, hsv.A);
    }

    /// <summary>
    /// Creates an instance of the RgbLinear color from the specified HSB (Hue, Saturation, Brightness) color.
    /// </summary>
    /// <param name="hsb">The HSB color to convert. The hue is specified in degrees, saturation and brightness are in the range 0 to 1,
    /// and alpha represents opacity.</param>
    /// <returns>An RgbLinear color that represents the equivalent of the specified HSB color.</returns>
    public static RgbLinear FromHsb(Hsb hsb) => FromHsv(new Hsv(hsb.H, hsb.S, hsb.B, hsb.A));

    /// <summary>
    /// Converts a color from the linear RGB color space to the RYB (Red-Yellow-Blue) color space.
    /// </summary>
    /// <param name="rgb">The color to convert, represented in the linear RGB color space.</param>
    /// <param name="alpha">The alpha (opacity) value to assign to the resulting RYB color. The value must be between 0.0 (fully
    /// transparent) and 1.0 (fully opaque). The default is 1.0.</param>
    /// <returns>A new Ryb instance representing the equivalent color in the RYB color space, with the specified alpha value.</returns>
    public static Ryb ToRyb(RgbLinear rgb, double alpha = 1.0)
    {
        var r = rgb.R;
        var g = rgb.G;
        var b = rgb.B;

        var y = (r + g) / 2;
        var newR = r - y;

        return new Ryb(newR + y, y, b, alpha);
    }

    /// <summary>
    /// Converts a color from the RYB (Red, Yellow, Blue) color model to its equivalent in the linear RGB color space.
    /// </summary>
    /// <remarks>This method performs a simplified conversion from RYB to linear RGB. The conversion may not
    /// be perceptually accurate for all color values.</remarks>
    /// <param name="ryb">The RYB color to convert. Each component should be in the range [0, 1].</param>
    /// <returns>An RgbLinear instance representing the equivalent color in the linear RGB color space.</returns>
    public static RgbLinear FromRyb(Ryb ryb)
    {
        var r = ryb.R;
        var y = ryb.Y;
        var b = ryb.B;
        var g = y * 2 - r;

        return new RgbLinear(r, Math.Clamp(g, 0, 1), b);
    }

    /// <summary>
    /// Converts a color from the sRGB color space (with 8 bits per channel) to the linear RGB color space.
    /// </summary>
    /// <param name="c">The sRGB color to convert. Each component should be in the range [0, 255].</param>
    /// <returns>An <see cref="RgbLinear"/> instance representing the equivalent color in the linear RGB color space.</returns>
    public static RgbLinear ToLinear(Srgb8 c)
    {
        return new RgbLinear(
            SrgbToLinear(c.R / 255.0),
            SrgbToLinear(c.G / 255.0),
            SrgbToLinear(c.B / 255.0)
        );
    }

    /// <summary>
    /// Converts a linear RGB color to its equivalent sRGB representation with the specified alpha channel value.
    /// </summary>
    /// <remarks>The conversion applies the standard sRGB transfer function to each color channel. The alpha
    /// value is not modified by the conversion.</remarks>
    /// <param name="rgb">The linear RGB color to convert to sRGB.</param>
    /// <param name="alpha">The alpha channel value to assign to the resulting sRGB color. The default is 255.</param>
    /// <returns>An Srgb8 structure representing the sRGB equivalent of the specified linear RGB color and alpha value.</returns>
    public static Srgb8 ToSrgb8(
        RgbLinear rgb,
        byte alpha = 255)
    {
        return new Srgb8(
            LinearToSrgb(rgb.R),
            LinearToSrgb(rgb.G),
            LinearToSrgb(rgb.B),
            alpha
        );
    }

    /// <summary>
    /// Converts a color component value from the sRGB color space to its linear representation.
    /// </summary>
    /// <remarks>This conversion is commonly used in color processing to ensure accurate color computations,
    /// as many color operations require linear color values rather than gamma-encoded sRGB values.</remarks>
    /// <param name="c">The sRGB color component value to convert. Must be in the range 0.0 to 1.0.</param>
    /// <returns>The linearized value of the color component, in the range 0.0 to 1.0.</returns>
    public static double SrgbToLinear(double c) => c <= 0.04045 ? c / 12.92 : Math.Pow((c + 0.055) / 1.055, 2.4);

    /// <summary>
    /// Calculates the inverse of the lower incomplete gamma function for the specified probability and shape parameter.
    /// </summary>
    /// <remarks>This method is commonly used in statistical computations involving the gamma distribution,
    /// such as determining quantiles. The input parameters must be within their valid ranges to avoid undefined
    /// results.</remarks>
    /// <param name="gamma">The probability value for which to compute the inverse. Must be between 0.0 and 1.0, inclusive.</param>
    /// <param name="v">The shape parameter of the gamma distribution. Must be greater than 0.0.</param>
    /// <returns>The value x such that the lower incomplete gamma function evaluated at x and the specified shape parameter
    /// equals the given probability.</returns>
    public static double InverseGamma(double gamma, double v)
    {
        if (Math.Abs(gamma - 2.4) < 0.001)
        {
            return SrgbToLinear(v);
        }

        return Math.Pow(v, gamma);
    }

    /// <summary>
    /// Converts a linear color channel value to its corresponding sRGB 8-bit value using the standard sRGB transfer
    /// function.
    /// </summary>
    /// <remarks>This method applies the sRGB gamma correction curve to convert a linear color component to
    /// its sRGB equivalent. Values outside the [0, 1] range are clamped before conversion.</remarks>
    /// <param name="c">The linear color channel value to convert. Must be in the range [0, 1].</param>
    /// <param name="gamma">The gamma value to use for the conversion. The default is 2.4, which corresponds to the standard sRGB gamma curve.</param>
    /// <returns>An 8-bit sRGB value representing the gamma-corrected color channel. The value is clamped to the range 0 to 255.</returns>

    public static byte LinearToSrgb(double c, double gamma = 2.4)
    {
        var v = c <= 0.0031308
            ? 12.92 * c
            : 1.055 * Math.Pow(c, 1 / gamma) - 0.055;

        return (byte)Math.Clamp(v * 255.0, 0, 255);
    }

    /// <summary>
    /// Converts an RGB color in linear space to its corresponding XYZ color using the specified transformation matrix.
    /// </summary>
    /// <param name="rgb">The RGB color in linear space to convert.</param>
    /// <param name="rgbToXyz">The 3x3 matrix used to transform the RGB color to the XYZ color space.</param>
    /// <returns>An Xyz structure representing the color in the XYZ color space.</returns>
    public static Xyz ToXyz(RgbLinear rgb, Matrix3x3 rgbToXyz)
    {
        var v = rgbToXyz.Transform(new(rgb.R, rgb.G, rgb.B));

        return new Xyz(v.X, v.Y, v.Z);
    }

    /// <summary>
    /// Converts an XYZ color value to its linear RGB representation using the specified transformation matrix.
    /// </summary>
    /// <param name="xyz">The XYZ color value to convert.</param>
    /// <param name="xyzToRgb">The 3x3 matrix used to transform the XYZ color to linear RGB space.</param>
    /// <returns>A linear RGB color value corresponding to the transformed XYZ input.</returns>
    public static RgbLinear ToLinear(Xyz xyz, Matrix3x3 xyzToRgb)
    {
        var v = xyzToRgb.Transform(new(xyz.X, xyz.Y, xyz.Z));

        return new RgbLinear(v.X, v.Y, v.Z);
    }

    /// <summary>
    /// Converts a color from the CIE 1931 XYZ color space to the CIE 1931 xyY color space.
    /// </summary>
    /// <remarks>If the sum of the X, Y, and Z components is zero, the resulting x and y chromaticity
    /// coordinates are set to zero, and the Y component is preserved as luminance.</remarks>
    /// <param name="xyz">The color value in the CIE 1931 XYZ color space to convert.</param>
    /// <returns>A new XyY structure representing the equivalent color in the CIE 1931 xyY color space.</returns>
    public static Xyy ToXyY(Xyz xyz)
    {
        var sum = xyz.X + xyz.Y + xyz.Z;

        if (sum == 0)
        {
            return new Xyy(0, 0, xyz.Y);
        }

        return new Xyy(
            xyz.X / sum,
            xyz.Y / sum,
            xyz.Y
        );
    }

    /// <summary>
    /// Converts a color value from the CIE 1931 xyY color space to the CIE 1931 XYZ color space.
    /// </summary>
    /// <remarks>Use this method to translate color representations between the xyY and XYZ color spaces,
    /// which are commonly used in color science and color management workflows.</remarks>
    /// <param name="xyY">The color value in the xyY color space to convert.</param>
    /// <returns>A new Xyz instance representing the equivalent color in the XYZ color space. If the Y component of <paramref
    /// name="xyY"/> is zero, returns an Xyz value with all components set to zero.</returns>
    public static Xyz ToXyz(Xyy xyY)
    {
        if (xyY.Y == 0)
        {
            return new Xyz(0, 0, 0);
        }

        var X = xyY.X * xyY.Y2 / xyY.Y;
        var Z = (1 - xyY.X - xyY.Y) * xyY.Y2 / xyY.Y;

        return new Xyz(X, xyY.Y2, Z);
    }

    /// <summary>
    /// Converts an sRGB color to its corresponding CIE XYZ color representation using the specified transformation
    /// matrix and gamma value.
    /// </summary>
    /// <remarks>If the gamma value differs from 2.4, custom gamma correction is applied before the RGB to XYZ
    /// transformation. This allows for conversion from non-standard RGB profiles.</remarks>
    /// <param name="color">The sRGB color to convert.</param>
    /// <param name="rgbToXyz">The 3x3 matrix used to transform linear RGB values to CIE XYZ coordinates.</param>
    /// <param name="gamma">The gamma value to apply for gamma correction. Use 2.4 for standard sRGB; other values apply custom gamma
    /// correction.</param>
    /// <returns>A new Xyz instance representing the color in the CIE XYZ color space.</returns>
    public static Xyz ToXyz(Srgb8 color, Matrix3x3 rgbToXyz, double gamma)
    {
        var linear = ToLinear(color);

        if (gamma != 2.4)
        {
            linear = new RgbLinear(
                Math.Pow(linear.R, gamma),
                Math.Pow(linear.G, gamma),
                Math.Pow(linear.B, gamma)
            );
        }

        return ToXyz(linear, rgbToXyz);
    }

    /// <summary>
    /// Converts an sRGB color in 8-bit per channel format to its equivalent CIE XYZ color representation using the specified
    ///  working space's RGB to XYZ transformation matrix and gamma correction. 
    /// </summary>
    /// <param name="srgb">The sRGB color to convert.</param>
    /// <param name="workingSpace">The RGB working space to use for the conversion.</param>
    /// <returns>A new Xyz instance representing the color in the CIE XYZ color space.</returns>
    public static Xyz ToXyz(Srgb8 srgb, RgbWorkingSpace workingSpace)
    {
        var r = srgb.R / 255.0;
        var g = srgb.G / 255.0;
        var b = srgb.B / 255.0;
        var gamma = workingSpace.Profile.Gamma;
        var m = workingSpace.RgbToXyzMatrix;

        r = InverseGamma(gamma, r);
        g = InverseGamma(gamma, g);
        b = InverseGamma(gamma, b);

        var X = m.M11 * r + m.M12 * g + m.M13 * b;
        var Y = m.M21 * r + m.M22 * g + m.M23 * b;
        var Z = m.M31 * r + m.M32 * g + m.M33 * b;

        return new Xyz(X, Y, Z);
    }

    /// <summary>
    /// Converts a color from the CIE XYZ color space to an 8-bit sRGB color using the specified transformation matrix
    /// and gamma correction.
    /// </summary>
    /// <remarks>If the specified gamma differs from the standard sRGB gamma of 2.4, an inverse gamma
    /// correction is applied before converting to sRGB. This allows for accurate conversion from non-sRGB
    /// profiles.</remarks>
    /// <param name="xyz">The color value in the CIE XYZ color space to convert.</param>
    /// <param name="xyzToRgb">The 3x3 matrix used to transform XYZ values to linear RGB values.</param>
    /// <param name="gamma">The gamma value to use for inverse gamma correction. Use 2.4 for standard sRGB conversion.</param>
    /// <returns>An Srgb8 structure representing the color in 8-bit sRGB format.</returns>
    public static Srgb8 ToSrgb8(Xyz xyz, Matrix3x3 xyzToRgb, double gamma)
    {
        var linear = ToLinear(xyz, xyzToRgb);

        if (gamma != 2.4)
        {
            linear = new RgbLinear(
                Math.Pow(linear.R, 1.0 / gamma),
                Math.Pow(linear.G, 1.0 / gamma),
                Math.Pow(linear.B, 1.0 / gamma)
            );
        }

        return ToSrgb8(linear);
    }

    /// <summary>
    /// Converts a value from the <see cref="Xyz"/> color space to the <see cref="Xyy"/> color space.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>Returns the converted value in the <see cref="Xyy"/> color space.</returns>
    public static Xyy FromXyz(Xyz value)
    {
        var n = value.X + value.Y + value.Z;

        if (n == 0)
        {
            return new Xyy(0, 0, value.Y);
        }

        return new Xyy(value.X / n,
                       value.Y / n,
                       value.Y);
    }

    /// <summary>
    /// Converts a color from the CIE 1931 XYZ color space to the OKLab color space using the standard transformation.
    /// </summary>
    /// <param name="xyz">The color value in the CIE 1931 XYZ color space to convert. The X, Y, and Z components should be in the range [0, 1].</param>
    /// <returns>An Oklab structure representing the color in the OKLab color space.</returns>
    public static Oklab ToOklab(Xyz xyz)
    {
        var l = 0.8189330101 * xyz.X + 0.3618667424 * xyz.Y - 0.1288597137 * xyz.Z;
        var m = 0.0329845436 * xyz.X + 0.9293118715 * xyz.Y + 0.0361456387 * xyz.Z;
        var s = 0.0482003018 * xyz.X + 0.2643662691 * xyz.Y + 0.6338517070 * xyz.Z;
        var l_ = Math.Cbrt(l);
        var m_ = Math.Cbrt(m);
        var s_ = Math.Cbrt(s);

        return new Oklab(
            0.2104542553 * l_ + 0.7936177850 * m_ - 0.0040720468 * s_,
            1.9779984951 * l_ - 2.4285922050 * m_ + 0.4505937099 * s_,
            0.0259040371 * l_ + 0.7827717662 * m_ - 0.8086757660 * s_
        );
    }

    /// <summary>
    /// Converts a color from the Oklab color space to the CIE XYZ color space.
    /// </summary>
    /// <remarks>Use this method to transform perceptually uniform Oklab color values into the
    /// device-independent CIE XYZ color space, which is commonly used as an intermediate for further color
    /// conversions.</remarks>
    /// <param name="lab">The Oklab color to convert. The L, A, and B components represent the lightness and chromaticity values in the
    /// Oklab color space.</param>
    /// <returns>An Xyz structure representing the equivalent color in the CIE XYZ color space.</returns>
    public static Xyz FromOklab(Oklab lab)
    {
        var l_ = lab.L + 0.3963377774 * lab.A + 0.2158037573 * lab.B;
        var m_ = lab.L - 0.1055613458 * lab.A - 0.0638541728 * lab.B;
        var s_ = lab.L - 0.0894841775 * lab.A - 1.2914855480 * lab.B;
        var l = l_ * l_ * l_;
        var m = m_ * m_ * m_;
        var s = s_ * s_ * s_;

        return new Xyz(
            1.2270138511 * l - 0.5577999807 * m + 0.2812561490 * s,
           -0.0405801784 * l + 1.1122568696 * m - 0.0716766787 * s,
           -0.0763812845 * l - 0.4214819784 * m + 1.5861632204 * s
        );
    }

    /// <summary>
    /// Converts a color from the Oklab color space to the Oklch color space.
    /// </summary>
    /// <remarks>The Oklch color space represents colors using lightness, chroma, and hue, which can be more
    /// intuitive for certain color manipulations compared to the Oklab color space.</remarks>
    /// <param name="lab">The Oklab color to convert.</param>
    /// <returns>An Oklch color representing the equivalent of the specified Oklab color.</returns>
    public static Oklch ToOklch(Oklab lab)
    {
        var c = Math.Sqrt(lab.A * lab.A + lab.B * lab.B);
        var h = Math.Atan2(lab.B, lab.A);

        if (h < 0)
        {
            h += 2 * Math.PI;
        }

        return new Oklch(lab.L, c, h);
    }

    /// <summary>
    /// Converts an Oklch color value to its equivalent Oklab representation.
    /// </summary>
    /// <param name="lch">The Oklch color value to convert.</param>
    /// <returns>An Oklab color value that represents the same color as the specified Oklch value.</returns>
    public static Oklab FromOklch(Oklch lch)
    {
        var a = lch.C * Math.Cos(lch.H);
        var b = lch.C * Math.Sin(lch.H);

        return new Oklab(lch.L, a, b);
    }

    /// <summary>
    /// Converts a color from the CIE XYZ color space to an sRGB color with 8-bit channels, applying a specified
    /// transformation matrix and gamma correction.
    /// </summary>
    /// <remarks>The resulting sRGB values are clamped to the range 0–255 for each channel. The alpha channel
    /// is set to 255 (fully opaque). Ensure that the transformation matrix and gamma value are appropriate for the
    /// intended color space conversion.</remarks>
    /// <param name="xyz">The color in the CIE XYZ color space to convert.</param>
    /// <param name="xyzToRgbMatrix">A 3x3 matrix used to transform XYZ values to linear RGB values. The matrix should be ordered such that each row
    /// corresponds to the R, G, and B channels, respectively.</param>
    /// <param name="gamma">The gamma value to use for the sRGB gamma correction. Must be a positive number.</param>
    /// <returns>An Srgb8 structure representing the converted sRGB color with 8-bit red, green, and blue channels.</returns>
    public static Srgb8 FromXyzToSrgb8(
        Xyz xyz,
        Matrix3x3 xyzToRgbMatrix,
        double gamma)
    {
        var rLin =
            xyzToRgbMatrix.M11 * xyz.X +
            xyzToRgbMatrix.M12 * xyz.Y +
            xyzToRgbMatrix.M13 * xyz.Z;

        var gLin =
            xyzToRgbMatrix.M21 * xyz.X +
            xyzToRgbMatrix.M22 * xyz.Y +
            xyzToRgbMatrix.M23 * xyz.Z;

        var bLin =
            xyzToRgbMatrix.M31 * xyz.X +
            xyzToRgbMatrix.M32 * xyz.Y +
            xyzToRgbMatrix.M33 * xyz.Z;

        var r = LinearToSrgb(rLin, gamma);
        var g = LinearToSrgb(gLin, gamma);
        var b = LinearToSrgb(bLin, gamma);

        return new Srgb8(
            Math.Clamp(r, byte.MinValue, byte.MaxValue),
            Math.Clamp(g, byte.MinValue, byte.MaxValue),
            Math.Clamp(b, byte.MinValue, byte.MaxValue),
            byte.MaxValue
        );
    }

    /// <summary>
    /// Modifies the specified color based on the provided parameters.
    /// </summary>
    /// <param name="fill">The original color to modify.</param>
    /// <param name="isDark">Indicates whether the color should be adjusted for a dark theme.</param>
    /// <param name="ws">The working space to use for color conversions.</param>
    /// <param name="vision">The type of color vision to apply.</param>
    /// <returns>The modified color as an Srgb8 value.</returns>
    internal static Srgb8 ModifyColor(
        Srgb8 fill,
        bool isDark,
        RgbWorkingSpace ws,
        ColorVisionType vision)
    {
        var xyz = ToXyz(fill, ws);
        var lab = ToOklab(xyz);
        var lch = ToOklch(lab);

        // Adjust
        var newL = isDark ? lch.L + 0.10 : lch.L - 0.10;
        var newC = Math.Max(0, lch.C - 0.02);
        var strokeLch = new Oklch(newL, newC, lch.H);

        var lab2 = FromOklch(strokeLch);
        var xyz2 = FromOklab(lab2);

        var srgb = ToSrgb8(
            xyz2,
            ws.XyzToRgbMatrix,
            ws.Profile.Gamma
        );

        return ColorVisionTransform.Apply(srgb, vision, ws);
    }
}
