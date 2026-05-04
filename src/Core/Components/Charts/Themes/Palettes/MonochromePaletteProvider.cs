using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a monochrome palette provider that generates a series of colors based on a single base color.
/// </summary>
internal sealed class MonochromePaletteProvider : IChartPaletteProvider
{
    /// <inheritdoc />
    public Task<ChartPalette> GenerateAsync(ChartPaletteRequest request)
    {
        var n = Math.Max(1, request.ColorCount);
        var series = new List<Srgb8>(n);
        var strokeSeries = new List<Srgb8>(n);

        var baseColor = request.BaseColor ?? new Srgb8(0, 120, 212);
        var xyzBase = ColorSpaceConverters.ToXyz(baseColor, request.WorkingSpace);
        var labBase = ColorSpaceConverters.ToOklab(xyzBase);
        var lchBase = ColorSpaceConverters.ToOklch(labBase);

        var H = lchBase.H;
        var C = lchBase.C;
        var startL = request.Options.IsDark ? 0.35 : 0.75;
        var endL = request.Options.IsDark ? 0.85 : 0.35;

        for (var i = 0; i < n; i++)
        {
            var t = (double)i / Math.Max(1, n - 1);
            var L = startL + (endL - startL) * t;

            var lch = new Oklch(L, C, H);
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
