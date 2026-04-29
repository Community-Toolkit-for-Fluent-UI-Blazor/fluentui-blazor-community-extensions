using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using FluentUI.Blazor.Community.Components.Enums;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;
using XYS = FluentUI.Blazor.Community.Components.Charts.Series.XYSerie;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

internal sealed class ChartXYComposer
    : ISurfaceComposer<CO>
{
    private readonly string _chartId;
    private readonly Func<ChartContext> _context;
    private readonly Func<IEnumerable<XYS>> _series;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChartXYComposer"/> class with the specified chart context, series collection, and chart identifier.
    /// </summary>
    /// <param name="context">The chart context for composition.</param>
    /// <param name="series">The collection of XY series to be rendered.</param>
    /// <param name="chartId">The unique identifier of the chart, used for interaction payloads.</param>
    public ChartXYComposer(
        string chartId,
        Func<ChartContext> context,
        Func<IEnumerable<XYS>> series)
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
        var scatterSeries = filtered.Where(s => s.XYType == XYChartType.Scatter).ToList();
        var bubbleSeries = filtered.Where(s => s.XYType == XYChartType.Bubble).ToList();
        var lineSeries = filtered.Where(s => s.XYType == XYChartType.Line).ToList();

        if (scatterSeries.Count > 0)
        {
            BuildScatterGroup(target, scatterSeries, ctx, options);
        }

        if (bubbleSeries.Count > 0)
        {
            BuildBubbleGroup(target, bubbleSeries, ctx, options);
        }

        if (lineSeries.Count > 0)
        {
            BuildLineGroup(target, lineSeries, ctx, options);
        }

        return true;
    }

    /// <inheritdoc/>
    public ValueTask<bool> ComposeAsync(ISurfaceRenderTarget target, CO options)
    {
        return ValueTask.FromResult(Compose(target, options));
    }

    /// <summary>
    /// Represents the logic to build a group of bubble series and add them to the render target.
    /// </summary>
    /// <param name="target">The render target where the bubble series will be added.</param>
    /// <param name="series">The collection of bubble series to be rendered.</param>
    /// <param name="ctx">The chart context for composition.</param>
    /// <param name="options">The chart options for rendering.</param>
    private void BuildBubbleGroup(
        ISurfaceRenderTarget target,
        List<XYS> series,
        ChartContext ctx,
        CO options)
    {
        var payloads = new List<XYBubblePayload>(series.Count);

        for (var i = 0; i < series.Count; i++)
        {
            var serie = series[i];
            var points = BubbleLayoutEngine.Layout(serie, ctx, options);
            var pointPayloads = new List<XYPointPayload>(points.Count);
            BuildPointPayloads(options, i, serie, points, pointPayloads);

            payloads.Add(new XYBubblePayload
            {
                Id = $"xy-bubble-{serie.Name}",
                SerieIndex = i,
                Points = pointPayloads
            });
        }

        target.AddLayer(new XYBubbleLayer(new(payloads)));
    }

    /// <summary>
    /// Represents the logic to build a group of line series and add them to the render target.
    /// </summary>
    /// <param name="target">The render target where the line series will be added.</param>
    /// <param name="series">The collection of line series to be rendered.</param>
    /// <param name="ctx">The chart context for composition.</param>
    /// <param name="options">The chart options for rendering.</param>
    private void BuildLineGroup(
        ISurfaceRenderTarget target,
        List<XYS> series,
        ChartContext ctx,
        CO options)
    {
        var payloads = new List<XYLinePayload>(series.Count);

        for (var i = 0; i < series.Count; i++)
        {
            var serie = series[i];
            var points = XYLineLayoutEngine.Layout(serie, ctx);
            var pointPayloads = new List<XYPointPayload>(points.Count);
            BuildPointPayloads(options, i, serie, points, pointPayloads);

            payloads.Add(new XYLinePayload
            {
                Id = $"xy-line-{serie.Name}",
                SerieIndex = i,
                ChartId = _chartId,
                GroupId = serie.Id,
                Index = i,
                Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, options.LineStyles.Normal),
                Points = pointPayloads
            });
        }

        target.AddLayer(new XYLineLayer(new(payloads)));
    }

    /// <summary>
    /// Builds and adds a scatter chart layer to the render target by processing series data, calculating point layouts,
    /// and creating payloads with styling and animation configurations.
    /// </summary>
    /// <param name="target">The render target where the scatter layer will be added.</param>
    /// <param name="series">The collection of scatter series to render.</param>
    /// <param name="ctx">The chart context used for layout calculations.</param>
    /// <param name="options">The chart options containing marker styles, animation settings, and other configuration.</param>
    private void BuildScatterGroup(
        ISurfaceRenderTarget target,
        List<XYS> series,
        ChartContext ctx,
        CO options)
    {
        var payloads = new List<XYScatterPayload>(series.Count);

        for (var i = 0; i < series.Count; i++)
        {
            var serie = series[i];
            var points = ScatterLayoutEngine.Layout(serie, ctx, options);
            var pointPayloads = new List<XYPointPayload>(points.Count);
            BuildPointPayloads(options, i, serie, points, pointPayloads);

            payloads.Add(new XYScatterPayload
            {
                Id = $"xy-scatter-{serie.Name}",
                Points = pointPayloads,
                SerieIndex = i
            });
        }

        target.AddLayer(new XYScatterLayer(new(payloads)));
    }

    /// <summary>
    /// Builds chart point payloads by combining coordinate data from points with series item properties and resolved
    /// styles.
    /// </summary>
    /// <param name="options">The chart options containing default marker styles and animation settings.</param>
    /// <param name="i">The index of the series being processed.</param>
    /// <param name="serie">The series containing items and animation configuration.</param>
    /// <param name="points">The collection of coordinate points to transform into payloads.</param>
    /// <param name="pointPayloads">The list to populate with the created point payloads.</param>
    private void BuildPointPayloads(
        CO options,
        int i,
        XYS serie,
        IReadOnlyList<Drawing.XYPoint> points,
        List<XYPointPayload> pointPayloads)
    {
        for (var j = 0; j < points.Count; j++)
        {
            var p = points[j];
            var item = serie.Items[p.Index];

            pointPayloads.Add(new XYPointPayload
            {
                X = p.X,
                Y = p.Y,
                Radius = p.Radius,
                Id = item.Id!,
                RawX = p.RawX,
                RawY = p.RawY,
                Index = p.Index,
                SerieIndex = i,
                GroupId = serie.Id,
                ChartId = _chartId,
                InteractionState = item.InteractionState,
                Trigger = item.Trigger,
                Effect = item.Effect,
                Normal = ChartStyleResolver.Resolve(item.Style?.Normal, options.MarkerStyles.Normal),
                Hover = ChartStyleResolver.Resolve(item.Style?.Hover, options.MarkerStyles.Hover),
                Pressed = ChartStyleResolver.Resolve(item.Style?.Pressed, options.MarkerStyles.Pressed),
                Selected = ChartStyleResolver.Resolve(item.Style?.Selected, options.MarkerStyles.Selected),
                AnimationEnabled = serie.AnimationEnabled,
                Animation = ChartAnimationResolver.Resolve(item.Animation, serie.Animation, options.Animation),
                Tooltip = new ChartTooltipPayload(),
                Value = item.Value
            });
        }
    }
}
