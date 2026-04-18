using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a chart palette provider that generates a neon color scheme for chart elements.
/// </summary>
internal sealed class NeonPaletteProvider : IChartPaletteProvider
{
    /// <inheritdoc />
    public Task<ChartPalette> GenerateAsync(ChartPaletteRequest request)
    {
        var n = Math.Max(1, request.ColorCount);
        var series = new List<Srgb8>(n);
        var strokeSeries = new List<Srgb8>(n);
        var L = request.Options.IsDark ? 0.80 : 0.70;
        var C = 0.30;
        var step = 2 * Math.PI / n;

        for (var i = 0; i < n; i++)
        {
            var h = i * step;
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
                request.Vision,
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
