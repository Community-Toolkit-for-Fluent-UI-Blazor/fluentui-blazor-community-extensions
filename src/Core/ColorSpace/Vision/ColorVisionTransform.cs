using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Vision;

/// <summary>
/// Represents a color vision transformation that simulates how colors are perceived by individuals with different types of color vision deficiencies.
/// </summary>
public static class ColorVisionTransform
{
    /// <summary>
    /// Applies the appearance of a color as perceived by an individual with a specified type and severity of color
    /// vision deficiency.
    /// </summary>
    /// <remarks>This method applies a color vision deficiency simulation by converting the input color
    /// through a series of color space transformations and projecting it onto the confusion axis associated with the
    /// specified deficiency. The severity parameter allows for partial simulation of anomalous vision. The method is
    /// useful for accessibility testing and visualization of color perception differences.</remarks>
    /// <param name="color">The original color to be transformed, represented in the sRGB color space.</param>
    /// <param name="type">The type and severity of color vision deficiency to simulate.</param>
    /// <param name="space">The RGB working space that defines the color profile and transformation matrices to use for color conversions.</param>
    /// <returns>A new Srgb8 color representing how the input color would appear to someone with the specified color vision
    /// deficiency. If the type is normal or severity is zero, the original color is returned.</returns>
    public static Srgb8 Apply(Srgb8 color, ColorVisionType type, RgbWorkingSpace space)
    {
        var (group, severity) = ColorVisionMapper.Decompose(type);

        if (group == ColorVisionGroup.Normal || severity <= 0)
        {
            return color;
        }

        var isAnomaly = type == ColorVisionType.Protanomaly ||
                type == ColorVisionType.Deuteranomaly ||
                type == ColorVisionType.Tritanomaly ||
                type == ColorVisionType.Achromatomaly;

        var line = group switch
        {
            ColorVisionGroup.Protan => ConfusionLineSet.Protanopia,
            ColorVisionGroup.Deutan => ConfusionLineSet.Deutanopia,
            ColorVisionGroup.Tritan => ConfusionLineSet.Tritanopia,
            ColorVisionGroup.Achroma => ConfusionLineSet.Unknown,
            _ => ConfusionLineSet.Unknown
        };

        if (line == ConfusionLineSet.Unknown)
        {
            var uniformColor = (byte)Math.Round(color.R * 0.212656 + color.G * 0.715158 + color.B * 0.072186, 0);
            var resultColor = new Srgb8(uniformColor, uniformColor, uniformColor);

            if (isAnomaly)
            {
                var v = 1.75;
                var n = v + 1;

                resultColor = new Srgb8(
                    (byte)Math.Round((v * resultColor.R + color.R) / n, 0),
                    (byte)Math.Round((v * resultColor.G + color.G) / n, 0),
                    (byte)Math.Round((v * resultColor.B + color.B) / n, 0)
                );
            }

            return resultColor;
        }

        var c = Xyy.ToXyy(Xyz.ToXYZ(color, space.RgbToXyzMatrix, space.Profile.Gamma, space.Profile.Name == Icc.IccProfileName.Srgb));
        var slope = (c.Y - line.Y) / (c.X - line.X);
        var yi = c.Y - c.X * slope;
        var dx = (line.Intercept - yi) / (slope - line.Slope);
        var dy = (slope * dx) + yi;
        var dY = 0.0;
        var xyz = new Xyz(dx * c.Y2 / dy,
                          c.Y2,
                          (1.0 - (dx + dy)) * c.Y2 / dy);

        var ngx = 0.312713 * c.Y2 / 0.329016;
        var ngz = 0.358271 * c.Y2 / 0.329016;
        var dX = ngx - xyz.X;
        var dZ = ngz - xyz.Z;
        var matrix = space.XyzToRgbMatrix;

        var dR = dX * matrix.M11 +
                 dY * matrix.M21 +
                 dZ * matrix.M31;
        
        var dG = dX * matrix.M12 +
                 dY * matrix.M22 +
                 dZ * matrix.M32;

        var dB = dX * matrix.M13 +
                 dY * matrix.M23 +
                 dZ * matrix.M33;

        var r = xyz.X * matrix.M11 +
                xyz.Y * matrix.M21 +
                xyz.Z * matrix.M31;

        var g = xyz.X * matrix.M12 +
                xyz.Y * matrix.M22 +
                xyz.Z * matrix.M32;

        var b = xyz.X * matrix.M13 +
                xyz.Y * matrix.M23 +
                xyz.Z * matrix.M33;

        var _r = ((r < 0 ? 0 : 1) - r) / dR;
        var _g = ((g < 0 ? 0 : 1) - g) / dG;
        var _b = ((b < 0 ? 0 : 1) - b) / dB;

        _r = (_r > 1 || _r < 0) ? 0 : _r;
        _g = (_g > 1 || _g < 0) ? 0 : _g;
        _b = (_b > 1 || _b < 0) ? 0 : _b;

        var adjust = _r > _g ? _r : _g;

        if (_b > adjust)
        {
            adjust = _b;
        }

        r += adjust * dR;
        g += adjust * dG;
        b += adjust * dB;

        r = 255 * (r <= 0 ? 0 : GetColorComponentValue(r));
        g = 255 * (g <= 0 ? 0 : GetColorComponentValue(g));
        b = 255 * (b <= 0 ? 0 : GetColorComponentValue(b));

        if (isAnomaly)
        {
            var v = 1.75;
            var n = v + 1;

            r = (v * r + color.R) / n;
            g = (v * g + color.G) / n;
            b = (v * b + color.B) / n;
        }

        r = double.IsNaN(r) ? 0 : Math.Round(r, 0);
        g = double.IsNaN(g) ? 0 : Math.Round(g, 0);
        b = double.IsNaN(b) ? 0 : Math.Round(b, 0);

        return new Srgb8((byte)r, (byte)g, (byte)b);

        double GetColorComponentValue(double r)
        {
            return r >= 1 ? 1 : Math.Pow(r, 1 / space.Profile.Gamma);
        }
    }
}
