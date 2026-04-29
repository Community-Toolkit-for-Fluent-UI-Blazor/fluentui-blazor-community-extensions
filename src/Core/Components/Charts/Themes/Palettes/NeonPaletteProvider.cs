using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a chart palette provider that generates a neon color scheme for chart elements.
/// </summary>
internal sealed class NeonPaletteProvider : IChartPaletteProvider
{
    private static readonly NeonPlugin _plugin = new();

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

        var baseHex = request.BaseColor?.ToString() ?? "#3B82F6";
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
