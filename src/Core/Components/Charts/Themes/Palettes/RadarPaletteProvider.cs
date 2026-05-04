using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Provides a palette for radar charts.
/// </summary>
internal sealed class RadarPaletteProvider : IChartPaletteProvider
{
    /// <summary>
    /// Represents the lightness value for the generated colors in the OKLCH color space.
    /// </summary>
    private const double L = 0.75;

    /// <summary>
    /// Represents the saturation value for the generated colors in the OKLCH color space.
    /// </summary>
    private const double C = 0.15;

    /// <inheritdoc />
    public Task<ChartPalette> GenerateAsync(ChartPaletteRequest request)
    {
        var n = Math.Max(1, request.ColorCount);
        var series = new List<Srgb8>(n);
        var strokeSeries = new List<Srgb8>(n);

        for (var i = 0; i < n; i++)
        {
            var h = (i / (double)n) * 360.0;

            var oklch = new Oklch(L, C, h);
            var oklab = ColorSpaceConverters.FromOklch(oklch);
            var xyz = ColorSpaceConverters.FromOklab(oklab);

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

            var stroke = ColorSpaceConverters.ModifyColor(
                transformed,
                request.Options.IsDark,
                request.WorkingSpace,
                request.Vision
            );

            strokeSeries.Add(stroke);
        }

        return Task.FromResult(new ChartPalette
        {
            Series = series,
            StrokeSeries = strokeSeries
        });
    }
}

