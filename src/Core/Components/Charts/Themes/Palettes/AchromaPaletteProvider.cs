using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a chart palette provider that generates an achromatic color palette for chart components.
/// </summary>
internal sealed class AchromaPaletteProvider : IChartPaletteProvider
{
    /// <summary>
    /// Asynchronously generates a chart palette with a series of achromatic colors based on the specified request
    /// parameters.
    /// </summary>
    /// <remarks>The generated palette uses perceptual luminance variation to create visually distinct
    /// achromatic colors. The number of colors in the palette corresponds to the SeriesCount specified in the
    /// request.</remarks>
    /// <param name="request">The palette generation request containing the number of series and working color space information. Cannot be
    /// null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a ChartPalette with the generated
    /// achromatic color series.</returns>
    public Task<ChartPalette> GenerateAsync(ChartPaletteRequest request)
    {
        var n = Math.Max(1, request.ColorCount);
        var series = new List<Srgb8>(n);
        var step = 0.5 / n;

        for (var i = 0; i < n; i++)
        {
            var L = 0.30 + i * step;
            var C = 0.0;
            var h = 0.0;

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
                ColorVisionType.Achromatopsia,
                request.WorkingSpace
            );

            series.Add(transformed);
        }

        return Task.FromResult(new ChartPalette
        {
            Series = series
        });
    }
}

