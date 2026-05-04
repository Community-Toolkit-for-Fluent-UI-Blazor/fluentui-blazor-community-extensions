using FluentUI.Blazor.Community.Components.ColorSpace.Vision;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a provider that generates chart themes based on specified requests. 
/// </summary>
internal sealed class ChartThemeProvider(
    ChartPaletteProviders paletteGenerators,
    IChartColorResolver colorResolver) : IChartThemeProvider
{
    /// <summary>
    /// Creates a new chart theme based on the specified theme request parameters.
    /// </summary>
    /// <remarks>The generated theme includes palette, typography, layout, and strategy settings. The palette
    /// is selected and generated based on the specified palette style and vision settings in the request.</remarks>
    /// <param name="request">The theme request containing palette style, series and category counts, vision settings, and other configuration
    /// options used to generate the chart theme.</param>
    /// <returns>A ChartTheme instance configured according to the provided request parameters.</returns>
    public async Task<ChartTheme> CreateThemeAsync(ChartThemeRequest request)
    {
        var paletteProvider = request.CustomPaletteProvider is not null
        ? paletteGenerators.Get(request.CustomPaletteProvider)
        : request.PaletteStyle switch
        {
            ChartPaletteStyle.Neon => paletteGenerators.Get("Neon"),
            ChartPaletteStyle.Pastel => paletteGenerators.Get("Pastel"),
            ChartPaletteStyle.PowerBi => paletteGenerators.Get("PowerBi"),
            ChartPaletteStyle.Protanopia => paletteGenerators.Get("Protanopia"),
            ChartPaletteStyle.Deuteranopia => paletteGenerators.Get("Deuteranopia"),
            ChartPaletteStyle.Tritanopia => paletteGenerators.Get("Tritanopia"),
            ChartPaletteStyle.Achroma => paletteGenerators.Get("Achroma"),
            ChartPaletteStyle.Monochrome => paletteGenerators.Get("Monochrome"),
            ChartPaletteStyle.HighContrast => paletteGenerators.Get("HighContrast"),
            ChartPaletteStyle.Autumn => paletteGenerators.Get("Autumn"),
            ChartPaletteStyle.Summer => paletteGenerators.Get("Summer"),
            ChartPaletteStyle.Spring => paletteGenerators.Get("Spring"),
            ChartPaletteStyle.Winter => paletteGenerators.Get("Winter"),
            ChartPaletteStyle.Radar => paletteGenerators.Get("Radar"),
            _ => paletteGenerators.Get("Default")
        };

        var palette = await paletteProvider.GenerateAsync(new ChartPaletteRequest
        {
            ColorCount = request.ColorCount,
            Vision = request.Vision,
            WorkingSpace = request.WorkingSpace,
            Style = request.PaletteStyle
        });

        var background = await colorResolver.ResolveAsync(
            request.Options.Background,
            request.Options.BackgroundVar,
            request.Options.IsDark ? ChartThemeTokens.DarkBackground : ChartThemeTokens.LightBackground
        );

        var foreground = await colorResolver.ResolveAsync(
            request.Options.Foreground,
            request.Options.ForegroundVar,
            request.Options.IsDark ? ChartThemeTokens.DarkForeground : ChartThemeTokens.LightForeground
        );

        var grid = await colorResolver.ResolveAsync(
            request.Options.Grid,
            request.Options.GridVar,
            request.Options.IsDark ? ChartThemeTokens.DarkGrid : ChartThemeTokens.LightGrid
        );

        var axis = await colorResolver.ResolveAsync(
            request.Options.Axis,
            request.Options.AxisVar,
            request.Options.IsDark ? ChartThemeTokens.DarkAxis : ChartThemeTokens.LightAxis
        );

        background = ColorVisionTransform.Apply(background, request.Vision, request.WorkingSpace);
        foreground = ColorVisionTransform.Apply(foreground, request.Vision, request.WorkingSpace);
        grid = ColorVisionTransform.Apply(grid, request.Vision, request.WorkingSpace);
        axis = ColorVisionTransform.Apply(axis, request.Vision, request.WorkingSpace);

        var textColor = await colorResolver.ResolveAsync(
            request.Options.Text,
            request.Options.TextVar,
            request.Options.IsDark ? ChartThemeTokens.DarkForeground : ChartThemeTokens.LightForeground
        );

        textColor = ColorVisionTransform.Apply(textColor, request.Vision, request.WorkingSpace);

        var typography = new ChartTypography
        {
            Title = ChartTextStyle.DefaultTitle with { Color = textColor },
            Subtitle = ChartTextStyle.DefaultSubtitle with { Color = textColor },
            Axis = ChartTextStyle.DefaultAxis with { Color = textColor },
            Legend = ChartTextStyle.DefaultLegend with { Color = textColor },
            Label = ChartTextStyle.DefaultLabel with { Color = textColor },
            Value = ChartTextStyle.DefaultValue with { Color = textColor },
            SmallLabel = ChartTextStyle.SmallLabel with { Color = textColor },
            Tooltip = ChartTextStyle.Tooltip with { Color = textColor }
        };

        return new ChartTheme
        {
            WorkingSpace = request.WorkingSpace,
            Metadata = new ChartThemeMetadata
            {
                Name = request.Name,
                Author = "Jeremy",
                Version = "1.0",
                Description = "Generated theme",
                Tags = [request.PaletteStyle.ToString()]
            },
            Palette = palette with
            {
                Background = background,
                Foreground = foreground,
                Grid = grid,
                Axis = axis
            },
            Typography = typography,
            Layout = new ChartLayout(),
            Strategies = new ChartAnimationStrategies(),
            PaletteStyle = request.PaletteStyle
        };
    }
}
