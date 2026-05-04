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
    private static readonly PastelPlugin _plugin = new();

    /// <inheritdoc />
    public Task<ChartPalette> GenerateAsync(ChartPaletteRequest request)
    {
        if (request.ColorCount == 0)
        {
            return Task.FromResult(new ChartPalette());
        }

        var n = Math.Max(1, request.ColorCount);
        var series = new List<Srgb8>(n);
        var strokeSeries = new List<Srgb8>(n);

        var baseHex = request.BaseColor?.ToString() ?? "#FFB3C6";
        var hexColors = _plugin.Generate(baseHex, n, new GenerationOptions
        {
            Easing = ColorPaletteEasing.Linear
        });

        foreach (var hex in hexColors)
        {
            var srgb = Srgb8.Parse(hex);

            var transformed = ColorVisionTransform.Apply(
                srgb,
                request.Vision,
                request.WorkingSpace
            );

            series.Add(transformed);
            strokeSeries.Add(
                ColorSpaceConverters.ModifyColor(
                    transformed,
                    request.Options.IsDark,
                    request.WorkingSpace,
                    request.Vision
                )
            );
        }

        return Task.FromResult(new ChartPalette
        {
            Series = series,
            StrokeSeries = strokeSeries
        });
    }
}
