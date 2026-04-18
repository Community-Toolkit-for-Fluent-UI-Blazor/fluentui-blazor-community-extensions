using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Provides a chart palette with pastel colors suitable for data visualization, generating a distinct color for each
/// series based on the requested count and display options.
/// </summary>
/// <remarks>This provider creates a palette of visually distinct pastel colors, adjusting brightness for dark or
/// light themes as specified in the palette request. The generated palette is intended to maximize clarity and
/// accessibility in charts, especially when multiple series are present. Colors are transformed to account for color
/// vision deficiencies and the specified working color space.</remarks>
internal sealed class PastelPaletteProvider : IChartPaletteProvider
{
    /// <inheritdoc />
    public Task<ChartPalette> GenerateAsync(ChartPaletteRequest request)
    {
        var n = Math.Max(1, request.ColorCount);
        var series = new List<Srgb8>(n);
        var strokeSeries = new List<Srgb8>(n);
        var L = request.Options.IsDark ? 0.85 : 0.90;
        var C = 0.07;
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
            StrokeSeries = strokeSeries
        });
    }
}
