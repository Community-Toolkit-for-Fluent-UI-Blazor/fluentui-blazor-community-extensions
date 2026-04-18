using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a chart palette provider that generates color palettes inspired by the Power BI visual style.
/// </summary>
internal sealed class PowerBiPaletteProvider : IChartPaletteProvider
{
    /// <summary>
    /// Represents the default set of Power BI color values in the sRGB color space.
    /// </summary>
    /// <remarks>These colors can be used to provide a consistent visual style that matches Power BI's
    /// standard palette. The array is read-only and contains a fixed sequence of colors.</remarks>
    private static readonly Srgb8[] PowerBiColors =
    [
        new(0x01, 0xB8, 0xAA),
        new(0x37, 0x46, 0x49),
        new(0xFD, 0x62, 0x5E),
        new(0xF2, 0xC8, 0x0F),
        new(0x5F, 0x6B, 0x6D),
        new(0x8A, 0xD4, 0xEB),
        new(0xFE, 0x96, 0x66),
        new(0xA6, 0x69, 0x99)
    ];

    /// <inheritdoc />
    public Task<ChartPalette> GenerateAsync(ChartPaletteRequest request)
    {
        var n = Math.Max(1, request.ColorCount);
        var series = new List<Srgb8>(n);
        var strokeSeries = new List<Srgb8>(n);

        for (var i = 0; i < n; i++)
        {
            var baseColor = PowerBiColors[i % PowerBiColors.Length];

            var transformed = ColorVisionTransform.Apply(
                baseColor,
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
