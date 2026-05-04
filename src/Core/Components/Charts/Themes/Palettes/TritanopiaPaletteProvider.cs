using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Provides a chart palette optimized for users with tritanopia (blue-yellow color blindness).
/// </summary>
/// <remarks>This palette provider generates color series that are visually distinct for individuals with
/// tritanopia, ensuring improved accessibility in data visualizations. The palette adapts to light or dark themes based
/// on the provided options. Use this provider when creating charts that need to be accessible to users with blue-yellow
/// color vision deficiency.</remarks>
internal sealed class TritanopiaPaletteProvider : IChartPaletteProvider
{
    /// <inheritdoc />
    public Task<ChartPalette> GenerateAsync(ChartPaletteRequest request)
    {
        var n = Math.Max(1, request.ColorCount);
        var series = new List<Srgb8>(n);
        var strokeSeries = new List<Srgb8>(n);
        var L = request.Options.IsDark ? 0.75 : 0.70;
        var C = 0.15;
        var hueZones = new[] { 0.1 * Math.PI, 1.4 * Math.PI, 1.7 * Math.PI };

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
                ColorVisionType.Tritanopia,
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
