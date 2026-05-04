using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a chart palette provider that generates color palettes optimized for users with deuteranopia.
/// </summary>
internal sealed class DeuteranopiaPaletteProvider : IChartPaletteProvider
{
    /// <inheritdoc />
    public Task<ChartPalette> GenerateAsync(ChartPaletteRequest request)
    {
        var n = Math.Max(1, request.ColorCount);
        var series = new List<Srgb8>(n);
        var strokeSeries = new List<Srgb8>(n);
        var L = request.Options.IsDark ? 0.75 : 0.70;
        var C = 0.12;
        var hueZones = new[] { 1.0 * Math.PI, 1.5 * Math.PI, 0.5 * Math.PI };

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
                ColorVisionType.Deuteranopia,
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
