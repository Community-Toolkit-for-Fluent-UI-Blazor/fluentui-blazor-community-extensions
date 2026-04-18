using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Provides a chart palette optimized for users with protanopia (red-blindness) by generating color series that remain
/// visually distinct for this type of color vision deficiency.
/// </summary>
/// <remarks>This palette provider is intended for use in data visualizations where accessibility for users with
/// protanopia is a priority. The generated palette ensures that chart series colors are perceptible and distinguishable
/// for individuals affected by this form of color blindness. Use this provider when you want to improve the inclusivity
/// of your charts for a broader audience.</remarks>
internal sealed class ProtanopiaPaletteProvider : IChartPaletteProvider
{
    /// <inheritdoc />
    public Task<ChartPalette> GenerateAsync(ChartPaletteRequest request)
    {
        var n = Math.Max(1, request.ColorCount);
        var series = new List<Srgb8>(n);
        var strokeSeries = new List<Srgb8>(n);
        var L = request.Options.IsDark ? 0.75 : 0.70;
        var C = 0.12;
        var hueZones = new[] { 0.75 * Math.PI, 1.25 * Math.PI, 2.0 * Math.PI };

        for (var i = 0; i < n; i++)
        {
            var h = hueZones[i % hueZones.Length] + (i * 0.2);
            var lch = new Oklch(L, C, h);
            var lab = ColorSpaceConverters.FromOklch(lch);
            var xyz = ColorSpaceConverters.FromOklab(lab);

            var srgb = ColorSpaceConverters.ToSrgb8(
                xyz,
                request.WorkingSpace.XyzToRgbMatrix,
                request.WorkingSpace.Profile.Gamma
            );

            var transformed = ColorVisionTransform.Apply(
                srgb,
                ColorVisionType.Protanopia,
                request.WorkingSpace
            );

            series.Add(transformed);
            strokeSeries.Add(ColorSpaceConverters.ModifyColor(transformed, request.Options.IsDark, request.WorkingSpace, request.Vision));
        }

        return Task.FromResult(new ChartPalette
        {
            Series = series,
            StrokeSeries = strokeSeries,
        });
    }
}
