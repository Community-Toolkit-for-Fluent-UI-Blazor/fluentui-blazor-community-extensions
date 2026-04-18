using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

/// <summary>
/// Provides a surface composer for chart layouts using the specified chart context and chart options providers.
/// </summary>
/// <param name="context">A delegate that returns the current chart context. Used to supply chart state and data for layout composition.</param>
/// <param name="legendItemShape">A delegate that returns the current chart legend item shape. Used to determine the shape of legend items for layout calculations.</param>
/// <param name="title">A delegate that returns the current chart title. Used to provide the title text for layout composition.</param>
/// <param name="subtitle">A delegate that returns the current chart subtitle. Used to provide the subtitle text for layout composition.</param>
/// <param name="series">A delegate that returns the current collection of chart series. Used to supply series data for layout composition.</param>
internal sealed class ChartLayoutComposer(
    Func<ChartContext> context,
    Func<string?> title,
    Func<string?> subtitle,
    Func<ChartLegendItemShape> legendItemShape,
    Func<IEnumerable<ChartSerie>> series)
     : ISurfaceComposer<Options.ChartOptions>
{
    /// <inheritdoc />
    public void Compose(ISurfaceRenderTarget target, Options.ChartOptions options)
    {
        ArgumentNullException.ThrowIfNull(target);

        var ctx = context();

        var titlePayload = BuildTitlePayload(ctx, title());
        var subtitlePayload = BuildSubtitlePayload(ctx, subtitle());
        var legendPayload = BuildLegendPayload(ctx, legendItemShape(), series());

        if (titlePayload is null &&
            subtitlePayload is null &&
            legendPayload is null)
        {
            return;
        }

        target.AddLayer(new ChartLayoutLayer(new(titlePayload, subtitlePayload, legendPayload)));
    }

    /// <inheritdoc />
    public ValueTask ComposeAsync(ISurfaceRenderTarget target, Options.ChartOptions options)
    {
        Compose(target, options);

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Builds a <see cref="ChartTitlePayload"/> based on the provided chart context and title text.
    /// </summary>
    /// <param name="ctx"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    private static ChartTitlePayload? BuildTitlePayload(ChartContext ctx, string? text)
    {
        if (string.IsNullOrWhiteSpace(text) ||
            ctx.TitleArea == ChartRect.Empty)
        {
            return null;
        }

        return new ChartTitlePayload
        {
            Text = text,
            Area = ctx.TitleArea
        };
    }

    /// <summary>
    /// Builds a payload object for the chart subtitle if the specified text and subtitle area are valid.
    /// </summary>
    /// <param name="ctx">The chart context containing layout information, including the subtitle area to use for the payload.</param>
    /// <param name="text">The subtitle text to include in the payload. May be null or whitespace.</param>
    /// <returns>A ChartSubtitlePayload instance containing the specified text and subtitle area if both are valid; otherwise,
    /// null.</returns>
    private static ChartSubtitlePayload? BuildSubtitlePayload(ChartContext ctx, string? text)
    {
        if (string.IsNullOrWhiteSpace(text) ||
            ctx.SubtitleArea == ChartRect.Empty)
        {
            return null;
        }

        return new ChartSubtitlePayload
        {
            Text = text,
            Area = ctx.SubtitleArea
        };
    }

    /// <summary>
    /// Creates a new ChartLegendPayload instance based on the specified chart context and legend item shape.
    /// </summary>
    /// <param name="ctx">The chart context containing legend area and item count information. Must not be null.</param>
    /// <param name="shape">The shape to use for the legend item in the payload.</param>
    /// <param name="series">The collection of chart series to extract legend labels from.</param>
    /// <returns>A ChartLegendPayload instance initialized with the provided context and shape, or null if the legend area is
    /// empty or there are no legend items.</returns>
    private static ChartLegendPayload? BuildLegendPayload(
        ChartContext ctx,
        ChartLegendItemShape shape,
        IEnumerable<ChartSerie> series)
    {
        if (ctx.LegendItemCount <= 0 ||
            ctx.LegendArea == ChartRect.Empty)
        {
            return null;
        }

        var items = BuildLegendItems(series);

        return new ChartLegendPayload
        {
            Area = ctx.LegendArea,
            ItemCount = items.Count,
            Shape = shape,
            Items = items
        };
    }

    /// <summary>
    /// Builds a list of legend items based on the provided chart series collection.
    /// </summary>
    /// <remarks>The method generates legend items differently depending on the chart type of each series. For
    /// pie, donut, and multi-donut charts, legend items are created for each data item within the series. For other
    /// chart types, a single legend item is created per series. The order of legend items corresponds to the order in
    /// which they are processed.</remarks>
    /// <param name="series">An enumerable collection of chart series from which to generate legend items. Each series determines how its
    /// legend items are constructed according to its chart type.</param>
    /// <returns>A list of legend items representing the labels and color indices for the provided chart series. The list may be
    /// empty if no series are provided.</returns>
    private static List<LegendItem> BuildLegendItems(IEnumerable<ChartSerie> series)
    {
        var items = new List<LegendItem>();
        var colorIndex = 0;

        foreach (var serie in series)
        {
            if (serie.ChartType == ChartType.Donut &&
               serie is Charts.Series.DonutSerie ds &&
               ds.IsPartOfMultiDonut)
            {
                continue;
            }

            switch (serie.ChartType)
            {
                case ChartType.Bar:
                case ChartType.Column:
                case ChartType.CategoryLine:
                case ChartType.CategoryArea:
                    {
                        items.Add(new LegendItem(serie.Name, colorIndex++));
                    }

                    break;

                case ChartType.Pie:
                case ChartType.Donut:
                case ChartType.SemiDonut:
                    {
                        foreach (var item in serie.RawItems)
                        {
                            items.Add(new LegendItem(item.Name, colorIndex++));
                        }
                    }

                    break;

                case ChartType.MultiDonut:
                    {
                        var multi = (Series.MultiDonutSerie)serie;

                        switch (multi.PaletteMode)
                        {
                            case DonutPaletteMode.ByCategory:
                                {
                                    foreach (var label in multi.Series
                                        .SelectMany(d => d.Items)
                                        .Select(i => i.Name)
                                        .Distinct())
                                    {
                                        items.Add(new LegendItem(label, colorIndex++));
                                    }
                                }

                                break;

                            case DonutPaletteMode.ByDonut:
                                {
                                    foreach (var donut in multi.Series)
                                    {
                                        items.Add(new LegendItem(donut.Name, colorIndex++));
                                    }
                                }

                                break;

                            case DonutPaletteMode.BySlice:
                                {
                                    foreach (var donut in multi.Series)
                                    {
                                        foreach (var item in donut.Items)
                                        {
                                            items.Add(new LegendItem(item.Name, colorIndex++));
                                        }
                                    }
                                }

                                break;
                        }
                    }

                    break;

               /* case ChartType.Radar:
                    {
                        foreach (var axis in serie.Values)
                        {
                            items.Add(new LegendItem(axis.Label, colorIndex++));
                        }
                    }

                    break;*/
            }
        }

        return items;
    }
}
