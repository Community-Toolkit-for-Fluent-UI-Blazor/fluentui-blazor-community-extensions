using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a chart palette provider that generates an autumn-themed color palette for chart components.
/// </summary>
internal sealed class AutumnPaletteProvider : IChartPaletteProvider
{
    /// <summary>
    /// Represents the base colors for the palette.
    /// </summary>
    private static readonly Srgb8[] BaseColors =
    [
        Srgb8.Parse("#E8A700"),
        Srgb8.Parse("#F47B00"),
        Srgb8.Parse("#C0392B"),
        Srgb8.Parse("#6B8E23")
    ];

    /// <summary>
    /// Generates a color palette for charts by creating variants of base colors in OKLCH color space and applying color
    /// vision transformations.
    /// </summary>
    /// <remarks>The method generates color variants by adjusting lightness (±4%), chroma (±3%), and hue
    /// (±3.5°) in OKLCH color space before converting back to sRGB and applying color vision transformations for
    /// accessibility.</remarks>
    /// <param name="request">The palette generation configuration specifying the number of colors, working color space, and color vision
    /// simulation mode.</param>
    /// <returns>A task that represents the asynchronous operation, containing the generated chart palette with the requested
    /// number of color series.</returns>
    public Task<ChartPalette> GenerateAsync(ChartPaletteRequest request)
    {
        var n = Math.Max(1, request.ColorCount);
        var variants = (int)Math.Ceiling(n / 4.0);
        var series = new List<Srgb8>(n);
        var strokeSeries = new List<Srgb8>(n);

        for (var i = 0; i < BaseColors.Length; i++)
        {
            var baseColor = BaseColors[i];
            var xyzColor = ColorSpaceConverters.ToXyz(baseColor, request.WorkingSpace);
            var lab = ColorSpaceConverters.ToOklab(xyzColor);
            var lch = ColorSpaceConverters.ToOklch(lab);

            for (var v = 0; v < variants; v++)
            {
                if (series.Count == n)
                {
                    break;
                }

                var factor = (v - (variants - 1) / 2.0) * 0.04;
                var cFactor = (v - (variants - 1) / 2.0) * 0.03;
                var hFactor = (v - (variants - 1) / 2.0) * 0.035;

                var variant = new Oklch(
                    lch.L + factor,
                    Math.Max(0, lch.C + cFactor),
                    lch.H + hFactor
                );

                var lab2 = ColorSpaceConverters.FromOklch(variant);
                var xyz = ColorSpaceConverters.FromOklab(lab2);

                var srgb = ColorSpaceConverters.ToSrgb8(
                    xyz,
                    request.WorkingSpace.XyzToRgbMatrix,
                    request.WorkingSpace.Profile.Gamma
                );

                var transformed = ColorVisionTransform.Apply(
                    srgb,
                    request.Vision,
                    request.WorkingSpace
                );

                series.Add(transformed);
                strokeSeries.Add(ColorSpaceConverters.ModifyColor(transformed, request.Options.IsDark, request.WorkingSpace, request.Vision));
            }
        }

        return Task.FromResult(new ChartPalette
        {
            Series = series,
            StrokeSeries = strokeSeries
        });
    }
}
