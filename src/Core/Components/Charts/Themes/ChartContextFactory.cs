using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a factory that creates a chart theme context by applying overrides, density, and contrast to a base theme.
/// </summary>
public static class ChartThemeContextFactory
{
    /// <summary>
    /// Creates a new ChartThemeContext by applying optional overrides, density, and contrast settings to the specified
    /// chart theme.
    /// </summary>
    /// <remarks>Use this method to generate a fully configured chart theme context for rendering charts with
    /// customized appearance. The method applies overrides, density, and contrast in sequence to produce the final
    /// theme configuration.</remarks>
    /// <param name="theme">The base chart theme to use as the starting point for the context.</param>
    /// <param name="overrides">Optional theme overrides to apply on top of the base theme. If null, no overrides are applied.</param>
    /// <param name="density">Optional density settings to apply to the theme layout. If null, default density is used.</param>
    /// <param name="contrast">Optional contrast settings to apply to the theme. If null, default contrast is used.</param>
    /// <returns>A ChartThemeContext instance representing the resulting theme after applying overrides, density, and contrast
    /// settings.</returns>
    public static ChartThemeContext Create(
        ChartTheme theme,
        ChartThemeOverride? overrides,
        ChartDensity? density = null,
        ChartContrast? contrast = null)
    {
        var finalPalette = ApplyPaletteOverride(theme.Palette, overrides?.Palette);
        var finalTypography = ApplyTypographyOverride(theme.Typography, overrides?.Typography);
        var finalLayout = ApplyLayoutOverride(theme.Layout, overrides?.Layout);
        var finalStrategies = ApplyStrategyOverride(theme.Strategies, overrides?.Strategies);

        var finalDensity = density ?? new ChartDensity();
        finalLayout = ApplyDensity(finalLayout, finalDensity);

        var finalContrast = contrast ?? new ChartContrast();
        (finalPalette, finalTypography, finalLayout) = ApplyContrast(
            theme,
            finalPalette,
            finalTypography,
            finalLayout,
            finalContrast);

        var computed = ComputeValues(
            finalPalette,
            finalTypography,
            finalLayout);

        return new ChartThemeContext
        {
            Theme = new ChartTheme
            {
                Metadata = theme.Metadata,
                Palette = finalPalette,
                Typography = finalTypography,
                Layout = finalLayout,
                Strategies = finalStrategies
            },
            Overrides = overrides,
            Density = finalDensity,
            Contrast = finalContrast,
            ComputedValues = computed
        };
    }

    /// <summary>
    /// Applies the specified override values to the given chart strategies, returning a new instance with updated
    /// properties where overrides are provided.
    /// </summary>
    /// <param name="strategies">The base set of chart strategies to which overrides will be applied.</param>
    /// <param name="o">An optional object containing override values for the chart strategies. If null, no overrides are applied.</param>
    /// <returns>A new ChartStrategies instance with properties updated according to the provided overrides. If no overrides are
    /// specified, returns the original strategies instance.</returns>
    private static ChartAnimationStrategies ApplyStrategyOverride(
        ChartAnimationStrategies strategies,
        ChartStrategiesOverride? o)
    {
        if (o is null)
        {
            return strategies;
        }

        return strategies with
        {
            AnimationsEnabled = o.AnimationsEnabled ?? strategies.AnimationsEnabled,
            InitialAppear = o.InitialAppear ?? strategies.InitialAppear,
            Update = o.Update ?? strategies.Update,
            Remove = o.Remove ?? strategies.Remove,
            HoverIn = o.HoverIn ?? strategies.HoverIn,
            HoverOut = o.HoverOut ?? strategies.HoverOut,
            SelectIn = o.SelectIn ?? strategies.SelectIn,
            SelectOut = o.SelectOut ?? strategies.SelectOut
        };
    }

    /// <summary>
    /// Applies the specified palette override to the given chart palette and returns the resulting palette.
    /// </summary>
    /// <param name="palette">The base chart palette to which the override will be applied.</param>
    /// <param name="o">An optional palette override that modifies the base palette. If null, no override is applied.</param>
    /// <returns>A ChartPalette instance representing the palette after applying the override. If no override is provided,
    /// returns the original palette.</returns>
    private static ChartPalette ApplyPaletteOverride(
        ChartPalette palette,
        ChartPaletteOverride? o)
    {
        if (o is null)
        {
            return palette;
        }

        return palette with
        {
            Background = o.Background ?? palette.Background,
            Foreground = o.Foreground ?? palette.Foreground,
            Grid = o.Grid ?? palette.Grid,
            Axis = o.Axis ?? palette.Axis,
            Series = o.Series ?? palette.Series,
            StrokeSeries = o.SeriesStroke ?? palette.StrokeSeries
        };
    }

    /// <summary>
    /// Applies the specified typography overrides to a base ChartTypography instance and returns the resulting
    /// typography settings.
    /// </summary>
    /// <param name="typography">The base ChartTypography instance to which overrides will be applied.</param>
    /// <param name="o">An optional ChartTypographyOverride containing properties to override in the base typography. If null, no
    /// overrides are applied.</param>
    /// <returns>A new ChartTypography instance with properties overridden by the specified ChartTypographyOverride values where
    /// provided; otherwise, the original typography if no overrides are specified.</returns>
    private static ChartTypography ApplyTypographyOverride(
        ChartTypography typography,
        ChartTypographyOverride? o)
    {
        if (o is null)
        {
            return typography;
        }

        return typography with
        {
            Title = o.Title ?? typography.Title,
            Subtitle = o.Subtitle ?? typography.Subtitle,
            Axis = o.Axis ?? typography.Axis,
            Legend = o.Legend ?? typography.Legend,
            Label = o.Label ?? typography.Label,
            Value = o.Value ?? typography.Value,
            SmallLabel = o.SmallLabel ?? typography.SmallLabel,
            Tooltip = o.Tooltip ?? typography.Tooltip
        };
    }

    /// <summary>
    /// Applies the specified layout override to a chart layout, returning a new layout with updated values where
    /// overrides are provided.
    /// </summary>
    /// <remarks>Only properties with non-null values in the override are applied; all other properties retain
    /// their values from the base layout.</remarks>
    /// <param name="layout">The base chart layout to which overrides will be applied.</param>
    /// <param name="o">An optional layout override containing values to replace in the base layout. If null, no overrides are applied.</param>
    /// <returns>A new ChartLayout instance with properties updated according to the specified override. If the override is null,
    /// returns the original layout.</returns>
    private static ChartLayout ApplyLayoutOverride(
        ChartLayout layout,
        ChartLayoutOverride? o)
    {
        if (o is null)
        {
            return layout;
        }

        return layout with
        {
            PaddingTop = o.PaddingTop ?? layout.PaddingTop,
            PaddingBottom = o.PaddingBottom ?? layout.PaddingBottom,
            PaddingLeft = o.PaddingLeft ?? layout.PaddingLeft,
            PaddingRight = o.PaddingRight ?? layout.PaddingRight,
            TitleSpacing = o.TitleSpacing ?? layout.TitleSpacing,
            SubtitleSpacing = o.SubtitleSpacing ?? layout.SubtitleSpacing,
            LegendSpacing = o.LegendSpacing ?? layout.LegendSpacing,
            AxisSpacing = o.AxisSpacing ?? layout.AxisSpacing,
            GridThickness = o.GridThickness ?? layout.GridThickness,
            AxisThickness = o.AxisThickness ?? layout.AxisThickness,
            TickLength = o.TickLength ?? layout.TickLength,
            BarCornerRadius = o.BarCornerRadius ?? layout.BarCornerRadius,
            MinBubbleRadius = o.MinBubbleRadius ?? layout.MinBubbleRadius,
            MaxBubbleRadius = o.MaxBubbleRadius ?? layout.MaxBubbleRadius,
            LineSmoothing = o.LineSmoothing ?? layout.LineSmoothing,
            SeriesGap = o.SeriesGap ?? layout.SeriesGap,
            CategoryGap = o.CategoryGap ?? layout.CategoryGap
        };
    }

    /// <summary>
    /// Adjusts the chart layout's padding and gap values according to the specified density mode.
    /// </summary>
    /// <remarks>Use this method to apply compact or spacious spacing to chart layouts, ensuring consistent
    /// visual density across different chart presentations.</remarks>
    /// <param name="layout">The original chart layout to be modified based on the density setting.</param>
    /// <param name="density">The density configuration that determines how the layout's spacing is adjusted.</param>
    /// <returns>A new ChartLayout instance with padding and gap values scaled according to the specified density mode. Returns
    /// the original layout if the density mode is unrecognized.</returns>
    private static ChartLayout ApplyDensity(
        ChartLayout layout,
        ChartDensity density)
    {
        return density.Mode switch
        {
            ChartDensityMode.Compact => layout with
            {
                PaddingTop = layout.PaddingTop * 0.75,
                PaddingBottom = layout.PaddingBottom * 0.75,
                PaddingLeft = layout.PaddingLeft * 0.75,
                PaddingRight = layout.PaddingRight * 0.75,
                CategoryGap = layout.CategoryGap * 0.75,
                SeriesGap = layout.SeriesGap * 0.75,
            },

            ChartDensityMode.Spacious => layout with
            {
                PaddingTop = layout.PaddingTop * 1.25,
                PaddingBottom = layout.PaddingBottom * 1.25,
                PaddingLeft = layout.PaddingLeft * 1.25,
                PaddingRight = layout.PaddingRight * 1.25,
                CategoryGap = layout.CategoryGap * 1.25,
                SeriesGap = layout.SeriesGap * 1.25
            },

            _ => layout
        };
    }

    /// <summary>
    /// Adjusts the chart palette, typography, and layout to match the specified contrast mode.
    /// </summary>
    /// <remarks>Use this method to ensure chart elements are visually optimized for different contrast
    /// requirements, such as high or low contrast modes. The returned values can be used to render charts that are more
    /// accessible or visually distinct based on user preferences.</remarks>
    /// <param name="theme">The original chart theme providing the working space for color adjustments.</param>
    /// <param name="palette">The base color palette to be adjusted for contrast.</param>
    /// <param name="typo">The base typography settings to be adjusted for contrast.</param>
    /// <param name="layout">The base layout settings to be adjusted for contrast.</param>
    /// <param name="contrast">The contrast mode and related settings to apply.</param>
    /// <returns>A tuple containing the adjusted chart palette, typography, and layout according to the specified contrast mode.</returns>
    private static (ChartPalette, ChartTypography, ChartLayout) ApplyContrast(
        ChartTheme theme,
        ChartPalette palette,
        ChartTypography typo,
        ChartLayout layout,
        ChartContrast contrast)
    {
        var workingSpace = theme.WorkingSpace;

        return contrast.Mode switch
        {
            ChartContrastMode.High => (
                palette with
                {
                    Grid = palette.Grid.WithLuminanceBoost(1.15, workingSpace),
                    Axis = palette.Axis.WithLuminanceBoost(1.15, workingSpace)
                },
                typo with
                {
                    Axis = typo.Axis with { FontWeight = TextWeight.Semibold }
                },
                layout with
                {
                    GridThickness = layout.GridThickness * 1.5,
                    AxisThickness = layout.AxisThickness * 1.5
                }
            ),

            ChartContrastMode.Low => (
                palette with
                {
                    Grid = palette.Grid.WithLuminanceBoost(0.85, workingSpace),
                    Axis = palette.Axis.WithLuminanceBoost(0.85, workingSpace)
                },
                typo,
                layout with
                {
                    GridThickness = layout.GridThickness * 0.75,
                    AxisThickness = layout.AxisThickness * 0.75
                }
            ),

            _ => (palette, typo, layout)
        };
    }

    /// <summary>
    /// Calculates the final computed values for chart rendering based on the specified palette, typography, and layout
    /// settings.
    /// </summary>
    /// <param name="palette">The color palette to use for chart elements such as grid and axis lines.</param>
    /// <param name="typography">The typography settings that define text appearance, including axis label color.</param>
    /// <param name="layout">The layout configuration specifying dimensions and visual properties such as bar radius, bubble sizes, and line
    /// smoothing.</param>
    /// <returns>A ChartComputedValues object containing the resolved visual properties for rendering the chart.</returns>
    private static ChartComputedValues ComputeValues(
        ChartPalette palette,
        ChartTypography typography,
        ChartLayout layout)
    {
        return new ChartComputedValues
        {
            FinalBarRadius = layout.BarCornerRadius,
            FinalBubbleMin = layout.MinBubbleRadius,
            FinalBubbleMax = layout.MaxBubbleRadius,
            FinalLineSmoothing = layout.LineSmoothing,
            FinalGridThickness = layout.GridThickness,
            FinalAxisThickness = layout.AxisThickness,
            FinalTextColor = typography.Axis.Color,
            FinalGridColor = palette.Grid,
            FinalAxisColor = palette.Axis,
            FinalStrokeThickness = layout.AxisThickness * 1.25
        };
    }
}
