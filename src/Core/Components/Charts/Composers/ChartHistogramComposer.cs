using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;
using HS = FluentUI.Blazor.Community.Components.Charts.Series.HistogramSerie;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

internal sealed class ChartHistogramComposer
    : ISurfaceComposer<CO>
{
    private readonly string _chartId;
    private readonly Func<ChartContext> _context;
    private readonly Func<IEnumerable<HS>> _series;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChartHistogramComposer"/> class with the specified chart context, series collection, and chart identifier.
    /// </summary>
    /// <param name="context">The chart context for composition.</param>
    /// <param name="series">The collection of histogram series to be rendered.</param>
    /// <param name="chartId">The unique identifier of the chart, used for interaction payloads.</param>
    public ChartHistogramComposer(
        string chartId,
        Func<ChartContext> context,
        Func<IEnumerable<HS>> series)
    {
        _chartId = chartId;
        _context = context;
        _series = series;
    }

    /// <inheritdoc/>
    public bool Compose(ISurfaceRenderTarget target, CO options)
    {
        var filtered = _series().Where(s => s.IsVisible).ToList();

        if (filtered.Count == 0)
        {
            return false;
        }

        var ctx = _context();
        BuildHistogramGroup(target, filtered, ctx, options);

        return true;
    }

    /// <inheritdoc/>
    public ValueTask<bool> ComposeAsync(ISurfaceRenderTarget target, CO options)
    {
        return ValueTask.FromResult(Compose(target, options));
    }

    /// <summary>
    /// Builds and adds a histogram chart layer to the render target by processing series data, calculating bar layouts,
    /// and generating payloads for rendering.
    /// </summary>
    /// <param name="target">The render target where the histogram layer will be added.</param>
    /// <param name="series">The collection of histogram series to be rendered.</param>
    /// <param name="ctx">The chart context for composition.</param>
    /// <param name="options">The chart options for rendering.</param>
    private void BuildHistogramGroup(
        ISurfaceRenderTarget target,
        List<HS> series,
        ChartContext ctx,
        CO options)
    {
        var payloads = new List<HistogramPayload>(series.Count);
        var seriesCount = series.Count;

        for (var i = 0; i < seriesCount; i++)
        {
            var serie = series[i];
            var bars = HistogramLayoutEngine.Layout(ctx);
            var barPayloads = new List<HistogramBarPayload>(bars.Count);

            for (var b = 0; b < bars.Count; b++)
            {
                var bar = bars[b];

                barPayloads.Add(new HistogramBarPayload
                {
                    Id = $"hist-bar-{serie.Name}-{b}",
                    GroupId = serie.Id,
                    ChartId = _chartId,
                    Index = b,
                    SerieIndex = i,
                    X = bar.X1,
                    Y = bar.Y2,
                    Width = bar.X2 - bar.X1,
                    Height = bar.Y1 - bar.Y2,
                    Count = bar.Count,
                    Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, options.HistogramStyle.Normal),
                    Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, options.HistogramStyle.Hover),
                    Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, options.HistogramStyle.Pressed),
                    AnimationEnabled = serie.AnimationEnabled,
                    Animation = ChartAnimationResolver.Resolve(null, serie.Animation, options.Animation),
                });
            }

            payloads.Add(new HistogramPayload
            {
                SerieIndex = i,
                Bars = barPayloads,
                Id = $"histogram-{serie.Name}"
            });
        }

        target.AddLayer(new HistogramLayer(new(payloads)));
    }
}
