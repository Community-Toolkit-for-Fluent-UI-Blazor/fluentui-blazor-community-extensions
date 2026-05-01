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
    /// <summary>
    /// Represents the set of chart types that are considered to have category-based legends.
    /// </summary>
    private static readonly HashSet<ChartType> s_PolySeriesLegendTypes =
    [
        ChartType.Bar,
        ChartType.Column,
        ChartType.Line,
        ChartType.Area,
        ChartType.StackedBar,
        ChartType.StackedColumn,
        ChartType.StackedArea,
        ChartType.Stacked100Area,
        ChartType.Stacked100Column,
        ChartType.Stacked100Bar,
        ChartType.Step,
        ChartType.StackedStep,
        ChartType.Stacked100Step,
        ChartType.Scatter,
        ChartType.Bubble,
        ChartType.Radar,
        ChartType.PolarBar,
        ChartType.PolarLine,
        ChartType.PolarScatter,
        ChartType.PolarBubble,
        ChartType.XYLine,
        ChartType.XYArea,
    ];

    /// <summary>
    /// Represents the set of chart types that are considered to have circular legends, such as pie and donut charts.
    /// </summary>
    private static readonly HashSet<ChartType> s_MonoSeriesLegendTypes =
    [
        ChartType.Pie,
        ChartType.Donut,
        ChartType.SemiDonut,
        ChartType.PolarArea,
        ChartType.PolarRose
    ];

    /// <inheritdoc />
    public bool Compose(ISurfaceRenderTarget target, Options.ChartOptions options)
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
            return false;
        }

        target.AddLayer(new ChartLayoutLayer(new(titlePayload, subtitlePayload, legendPayload)));

        return true;
    }

    /// <inheritdoc />
    public ValueTask<bool> ComposeAsync(ISurfaceRenderTarget target, Options.ChartOptions options)
    {
        return ValueTask.FromResult(Compose(target, options));
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

        var items = BuildLegendItems(series, ctx);

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
    /// <param name="ctx">The chart context providing additional information such as histogram bin edges, if applicable.</param>
    /// <returns>A list of legend items representing the labels and color indices for the provided chart series. The list may be
    /// empty if no series are provided.</returns>
    private static List<LegendItem> BuildLegendItems(
        IEnumerable<ChartSerie> series,
        ChartContext ctx)
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

            if (s_PolySeriesLegendTypes.Contains(serie.ChartType))
            {
                items.Add(new LegendItem(serie.Name, colorIndex++));
            }
            else if (s_MonoSeriesLegendTypes.Contains(serie.ChartType))
            {
                foreach (var item in serie.RawItems)
                {
                    items.Add(new LegendItem(item.Name, colorIndex++));
                }
            }
            else if (serie.ChartType == ChartType.Histogram)
            {
                var model = ctx.HistogramModel;

                if (model is null)
                {
                    continue;
                }

                var edges = model.BinEdges;
                var binCount = model.Counts.Length;

                for (var b = 0; b < binCount; b++)
                {
                    var start = edges[b];
                    var end = edges[b + 1];
                    var label = $"{start:G3} – {end:G3}";

                    items.Add(new LegendItem(label, colorIndex++));
                }
            }
            else if (serie.ChartType == ChartType.MultiDonut)
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
        }

        return items;
    }
}
