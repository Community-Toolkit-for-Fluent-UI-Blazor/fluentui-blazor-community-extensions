using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts;

/// <summary>
/// Resolves the animation settings for a chart item.
/// </summary>
internal static class ChartAnimationResolver
{
    /// <summary>
    /// Resolves the effective chart item animation by selecting the most specific non-null configuration from the
    /// provided parameters.
    /// </summary>
    /// <remarks>The method prioritizes the parameters in the following order: item, serie, then global. If
    /// all parameters are null except for global, the global options are used. The returned object is always
    /// non-null.</remarks>
    /// <param name="item">The chart item animation to use if specified; otherwise, the method will attempt to resolve from the series or
    /// global options.</param>
    /// <param name="serie">The series-level animation options to use if the item animation is not specified; otherwise, the method will
    /// fall back to the global options.</param>
    /// <param name="global">The global animation options to use if neither the item nor the series options are specified. Cannot be null.</param>
    /// <returns>A ChartItemAnimation instance representing the resolved animation configuration, based on the most specific
    /// non-null parameter.</returns>
    public static ChartItemAnimation Resolve(
        ChartItemAnimation? item,
        ChartAnimationOptions? serie,
        ChartAnimationOptions global)
    {
        if (item is not null)
        {
            return item;
        }

        if (serie is not null)
        {
            return new ChartItemAnimation
            {
                Duration = serie.Duration,
                Delay = serie.Delay,
                Easing = serie.Easing,
                CustomBezier = serie.CustomBezier,
                Steps = serie.Steps,
            };
        }

        return new ChartItemAnimation
        {
            Duration = global.Duration,
            Delay = global.Delay,
            Easing = global.Easing,
            CustomBezier = global.CustomBezier,
            Steps = global.Steps
        };
    }
}
