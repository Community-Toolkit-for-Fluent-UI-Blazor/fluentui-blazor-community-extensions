using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using FluentUI.Blazor.Community.Components.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Enums;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

internal sealed class ChartPolarComposer : ISurfaceComposer<CO>
{
    private readonly string _chartId;
    private readonly Func<ChartContext> _context;
    private readonly Func<IEnumerable<PolarSerie>> _series;

    public ChartPolarComposer(
        string chartId,
        Func<ChartContext> context,
        Func<IEnumerable<PolarSerie>> series)
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
        var radarSeries = filtered.Where(s => s.PolarType == PolarChartType.Radar).ToList();
        var areaSeries = filtered.Where(s => s.PolarType == PolarChartType.Area).ToList();
        var barSeries = filtered.Where(s => s.PolarType == PolarChartType.Bar).ToList();
        var lineSeries = filtered.Where(s => s.PolarType == PolarChartType.Line).ToList();
        var roseSeries = filtered.Where(s => s.PolarType == PolarChartType.Rose).ToList();
        var polarScatterSeries = filtered.Where(s => s.PolarType == PolarChartType.Scatter).ToList();
        var polarBubbleSeries = filtered.Where(s => s.PolarType == PolarChartType.Bubble).ToList();

        if (radarSeries.Count > 0)
        {
            BuildRadarGroup(target, radarSeries, ctx, options);
        }

        if (areaSeries.Count > 0)
        {
            BuildPolarAreaGroup(target, areaSeries, ctx, options);
        }

        if (barSeries.Count > 0)
        {
            BuildPolarBarGroup(target, barSeries, ctx, options);
        }

        if (lineSeries.Count > 0)
        {
            BuildPolarLineGroup(target, lineSeries, ctx, options);
        }

        if (roseSeries.Count > 0)
        {
            BuildRoseGroup(target, roseSeries, ctx, options);
        }

        if (polarScatterSeries.Count > 0)
        {
            BuildPolarScatterGroup(target, polarScatterSeries, ctx, options);
        }

        if (polarBubbleSeries.Count > 0)
        {
            BuildPolarBubbleGroup(target, polarBubbleSeries, ctx, options);
        }

        return true;
    }

    /// <inheritdoc/>
    public ValueTask<bool> ComposeAsync(ISurfaceRenderTarget target, CO options) => ValueTask.FromResult(Compose(target, options));

    /// <summary>
    /// Builds the polar scatter chart layer and adds it to the render target.
    /// </summary>
    /// <param name="target">The render target where the polar scatter chart layer will be added.</param>
    /// <param name="polarScatterSeries">The polar series containing the data points for the polar scatter chart.</param>
    /// <param name="ctx">The chart context containing layout and rendering information.</param>
    /// <param name="options">The chart options containing default styles and animation settings.</param>
    /// <exception cref="NotImplementedException"></exception>
    private void BuildPolarScatterGroup(
        ISurfaceRenderTarget target,
        List<PolarSerie> polarScatterSeries,
        ChartContext ctx,
        CO options)
    {
        var payloads = new List<PolarScatterPayload>(polarScatterSeries.Count);

        for (var serieIndex = 0; serieIndex < polarScatterSeries.Count; serieIndex++)
        {
            var serie = polarScatterSeries[serieIndex];
            var points = PolarScatterLayoutEngine.Layout(serie, ctx, options);
            var pointPayloads = new List<PolarScatterPointPayload>(points.Count);

            for (var i = 0; i < points.Count; i++)
            {
                var p = points[i];
                var item = serie.Items[p.Index];

                pointPayloads.Add(new PolarScatterPointPayload
                {
                    X = p.X,
                    Y = p.Y,
                    Radius = p.Radius,
                    Id = item.Id!,
                    Index = p.Index,
                    SerieIndex = serieIndex,
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

            payloads.Add(new PolarScatterPayload
            {
                Points = pointPayloads
            });
        }

        target.AddLayer(new PolarScatterLayer(new PolarScatterPayloadCollection(payloads)));
    }

    /// <summary>
    /// Builds the polar bubble chart layer and adds it to the render target.
    /// </summary>
    /// <param name="target">The render target where the polar bubble chart layer will be added.</param>
    /// <param name="polarBubbleSeries">The polar series containing the data points for the polar bubble chart.</param>
    /// <param name="ctx">The chart context containing layout and rendering information.</param>
    /// <param name="options">The chart options containing default styles and animation settings.</param>
    private void BuildPolarBubbleGroup(
        ISurfaceRenderTarget target,
        List<PolarSerie> polarBubbleSeries,
        ChartContext ctx,
        CO options)
    {
        var payloads = new List<PolarBubblePayload>(polarBubbleSeries.Count);

        for (var serieIndex = 0; serieIndex < polarBubbleSeries.Count; serieIndex++)
        {
            var serie = polarBubbleSeries[serieIndex];
            var points = PolarBubbleLayoutEngine.Layout(serie, ctx, options);
            var pointPayloads = new List<PolarBubblePointPayload>(points.Count);

            for (var i = 0; i < points.Count; i++)
            {
                var p = points[i];
                var item = serie.Items[p.Index];

                pointPayloads.Add(new PolarBubblePointPayload
                {
                    X = p.X,
                    Y = p.Y,
                    Radius = p.Radius,
                    Id = item.Id!,
                    Index = p.Index,
                    SerieIndex = serieIndex,
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

            payloads.Add(new PolarBubblePayload
            {
                Points = pointPayloads
            });
        }

        target.AddLayer(new PolarBubbleLayer(new PolarBubblePayloadCollection(payloads)));
    }

    /// <summary>
    /// Builds the rose chart layer and adds it to the render target.
    /// </summary>
    /// <param name="target">The render target where the rose chart layer will be added.</param>
    /// <param name="series">The polar series containing the data points for the rose chart.</param>
    /// <param name="ctx">The chart context containing layout and rendering information.</param>
    /// <param name="options">The chart options containing default styles and animation settings.</param>
    private void BuildRoseGroup(
        ISurfaceRenderTarget target,
        List<PolarSerie> series,
        ChartContext ctx,
        CO options)
    {
        var payloads = new List<RosePayload>(series.Count);

        for (var serieIndex = 0; serieIndex < series.Count; serieIndex++)
        {
            var serie = series[serieIndex];
            var segments = RoseLayoutEngine.Layout(serie, ctx);
            var segPayloads = new List<RoseSegmentPayload>(segments.Count);

            for (var i = 0; i < segments.Count; i++)
            {
                var seg = segments[i];
                var item = serie.Items[seg.Index];

                segPayloads.Add(new RoseSegmentPayload
                {
                    Id = item.Id!,
                    Index = seg.Index,
                    CenterX = seg.CenterX,
                    CenterY = seg.CenterY,
                    StartAngle = seg.StartAngle,
                    EndAngle = seg.EndAngle,
                    InnerRadius = seg.InnerRadius,
                    OuterRadius = seg.OuterRadius,
                    Value = seg.Value,
                    Category = seg.Category,

                    ChartId = _chartId,
                    SerieIndex = serieIndex,
                    AnimationEnabled = serie.AnimationEnabled,
                    GroupId = serie.Id,

                    Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, options.PolarBarStyles.Normal),
                    Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, options.PolarBarStyles.Hover),
                    Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, options.PolarBarStyles.Pressed),
                    Selected = ChartStyleResolver.Resolve(serie.Style?.Selected, options.PolarBarStyles.Selected),

                    Animation = ChartAnimationResolver.Resolve(null, serie.Animation, options.Animation),
                    Tooltip = new ChartTooltipPayload()
                });
            }

            payloads.Add(new RosePayload
            {
                Id = serie.Id,
                Segments = segPayloads
            });
        }

        target.AddLayer(new RoseLayer(new(payloads)));
    }

    /// <summary>
    /// Constructs and adds a polar area layer to the render target based on the provided series data and chart
    /// configuration.
    /// </summary>
    /// <param name="target">The render target where the polar area layer will be added.</param>
    /// <param name="series">The polar series containing the data points to visualize.</param>
    /// <param name="ctx">The chart context containing layout and rendering information.</param>
    /// <param name="options">The chart options containing default styles and animation settings.</param>
    private void BuildPolarAreaGroup(
        ISurfaceRenderTarget target,
        List<PolarSerie> series,
        ChartContext ctx,
        CO options)
    {
        var areaPayloads = new List<PolarAreaPayload>(series.Count);

        for (var serieIndex = 0; serieIndex < series.Count; serieIndex++)
        {
            var serie = series[serieIndex];
            var segments = PolarAreaLayoutEngine.Layout(serie, ctx);

            var segmentPayloads = new List<PolarAreaSegmentPayload>(segments.Count);

            for (var i = 0; i < segments.Count; i++)
            {
                var segment = segments[i];
                var item = serie.Items[segment.Index];

                segmentPayloads.Add(new PolarAreaSegmentPayload
                {
                    Id = item.Id!,
                    Index = segment.Index,
                    CenterX = segment.CenterX,
                    CenterY = segment.CenterY,
                    Radius = segment.Radius,
                    StartAngle = segment.StartAngle,
                    EndAngle = segment.EndAngle,
                    ChartId = _chartId,
                    SerieIndex = serieIndex,
                    AnimationEnabled = serie.AnimationEnabled,
                    GroupId = serie.Id,
                    Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, options.LineStyles.Normal),
                    Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, options.LineStyles.Hover),
                    Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, options.LineStyles.Pressed),
                    Selected = ChartStyleResolver.Resolve(serie.Style?.Selected, options.LineStyles.Selected),
                    Animation = ChartAnimationResolver.Resolve(null, serie.Animation, options.Animation),
                    Tooltip = new ChartTooltipPayload(),
                    InteractionState = item.InteractionState,
                    Trigger = item.Trigger,
                    Effect = item.Effect
                });
            }

            areaPayloads.Add(new PolarAreaPayload
            {
                Id = serie.Id,
                Segments = segmentPayloads
            });
        }

        target.AddLayer(new PolarAreaLayer(new PolarAreaPayloadCollection(areaPayloads)));
    }

    /// <summary>
    /// Builds the radar chart layer and adds it to the render target.
    /// </summary>
    /// <param name="target">The render target to which the layer will be added.</param>
    /// <param name="series">The radar series to be rendered.</param>
    /// <param name="ctx">The chart context.</param>
    /// <param name="options">The chart options.</param>
    private void BuildRadarGroup(
        ISurfaceRenderTarget target,
        List<PolarSerie> series,
        ChartContext ctx,
        CO options)
    {
        var radarPayloads = new List<RadarPayload>(series.Count);

        for (var serieIndex = 0; serieIndex < series.Count; serieIndex++)
        {
            var serie = series[serieIndex];
            var serieOptions = options.RadarOptions;
            var (path, polarPoints) = RadarLayoutEngine.Layout(serie, ctx);

            BuildPolarPointsPayload(
                serie,
                serieIndex,
                options,
                polarPoints,
                out var lineDefaults,
                out var payloadPoints);

            var payload = new RadarPayload
            {
                ChartId = _chartId,
                Id = serie.Id,
                SerieIndex = serieIndex,
                AnimationEnabled = serie.AnimationEnabled,
                GroupId = serie.Id,
                Index = serieIndex,
                Path = new PolarPathPayload
                {
                    Id = path.Id,
                    Points = polarPoints,
                    Closed = true
                },
                Points = payloadPoints,
                Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, lineDefaults.Normal),
                Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, lineDefaults.Hover),
                Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, lineDefaults.Pressed),
                Selected = ChartStyleResolver.Resolve(serie.Style?.Selected, lineDefaults.Selected),
                Animation = ChartAnimationResolver.Resolve(null, serie.Animation, options.Animation),
                Tooltip = new ChartTooltipPayload(),
                FillArea = serieOptions.FillArea,
                Grid = options.RadarAxesOptions.Grid
            };

            radarPayloads.Add(payload);
        }

        target.AddLayer(new RadarLayer(new RadarPayloadCollection(radarPayloads)));
    }

    /// <summary>
    /// Builds the polar bar chart layer and adds it to the render target, transforming the series data into payloads for rendering.
    /// </summary>
    /// <param name="target">The render target to which the layer will be added.</param>
    /// <param name="series">The polar series to be rendered.</param>
    /// <param name="ctx">The chart context.</param>
    /// <param name="options">The chart options.</param>
    private void BuildPolarBarGroup(
        ISurfaceRenderTarget target,
        List<PolarSerie> series,
        ChartContext ctx,
        CO options)
    {
        var count = series.Count;
        var bars = new List<PolarBarPayload>(count);

        for (var serieIndex = 0; serieIndex < count; serieIndex++)
        {
            var serie = series[serieIndex];
            var segments = PolarBarLayoutEngine.Layout(serie, ctx, serieIndex, count);
            var segmentPayloads = new List<PolarBarSegmentPayload>(segments.Count);

            for (var i = 0; i < segments.Count; i++)
            {
                var bar = segments[i];
                var item = serie.Items[bar.Index];

                segmentPayloads.Add(new PolarBarSegmentPayload
                {
                    Id = item.Id!,
                    Index = bar.Index,
                    CenterX = bar.CenterX,
                    CenterY = bar.CenterY,
                    StartAngle = bar.StartAngle,
                    EndAngle = bar.EndAngle,
                    InnerRadius = bar.InnerRadius,
                    OuterRadius = bar.OuterRadius,
                    ChartId = _chartId,
                    SerieIndex = serieIndex,
                    AnimationEnabled = serie.AnimationEnabled,
                    GroupId = serie.Id,
                    Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, options.PolarBarStyles.Normal),
                    Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, options.PolarBarStyles.Hover),
                    Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, options.PolarBarStyles.Pressed),
                    Selected = ChartStyleResolver.Resolve(serie.Style?.Selected, options.PolarBarStyles.Selected),
                    Animation = ChartAnimationResolver.Resolve(null, serie.Animation, options.Animation),
                    Tooltip = new ChartTooltipPayload(),
                    InteractionState = item.InteractionState,
                    Trigger = item.Trigger,
                    Effect = item.Effect,
                    Value = item.Value,
                    Category = item.Name
                });
            }

            var payload = new PolarBarPayload
            {
                Id = serie.Id,
                Segments = segmentPayloads
            };

            bars.Add(payload);
        }

        target.AddLayer(new PolarBarLayer(new PolarBarPayloadCollection(bars)));
    }

    /// <summary>
    /// Builds line point payloads from polar points for chart rendering, resolving styles and animations.
    /// </summary>
    /// <param name="serie">The polar series containing the data items.</param>
    /// <param name="index">The series index in the chart.</param>
    /// <param name="options">The chart options containing default styles and animation settings.</param>
    /// <param name="polarPoints">The collection of polar points to transform into payloads.</param>
    /// <param name="lineDefaults">Outputs the default line styles extracted from the options.</param>
    /// <param name="payloadPoints">Outputs the list of transformed line point payloads ready for rendering.</param>
    private void BuildPolarPointsPayload(
        PolarSerie serie,
        int index,
        CO options,
        IReadOnlyList<PolarPoint> polarPoints,
        out ChartLinePointStyle lineDefaults,
        out List<LinePointPayload> payloadPoints)
    {
        var markerDefaults = options.MarkerStyles;
        lineDefaults = options.LineStyles;
        payloadPoints = [.. polarPoints
            .Select(p =>
            {
                var item = serie.Items[p.CategoryIndex];

                return new LinePointPayload
                {
                    SerieIndex = index,
                    ChartId = _chartId,
                    Id = p.Id,
                    X = p.X,
                    Y = p.Y,
                    CategoryIndex = p.CategoryIndex,
                    Value = p.Value,
                    AnimationEnabled = options.AnimationEnabled,
                    InteractionState = item.InteractionState,
                    Index = p.CategoryIndex,
                    GroupId = serie.Id,
                    Trigger = item.Trigger,
                    Effect = item.Effect,
                    Normal = ChartStyleResolver.Resolve(item.Style?.Normal, markerDefaults.Normal),
                    Hover = ChartStyleResolver.Resolve(item.Style?.Hover, markerDefaults.Hover),
                    Pressed = ChartStyleResolver.Resolve(item.Style?.Pressed, markerDefaults.Pressed),
                    Selected = ChartStyleResolver.Resolve(item.Style?.Selected, markerDefaults.Selected),
                    Animation = ChartAnimationResolver.Resolve(item.Animation, serie.Animation, options.Animation),
                    Tooltip = new ChartTooltipPayload()
                };
            })];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="series"></param>
    /// <param name="ctx"></param>
    /// <param name="options"></param>
    private void BuildPolarLineGroup(
        ISurfaceRenderTarget target,
        List<PolarSerie> series,
        ChartContext ctx,
        CO options)
    {
        var payloads = new List<PolarLinePayload>(series.Count);

        for (var serieIndex = 0; serieIndex < series.Count; serieIndex++)
        {
            var serie = series[serieIndex];
            var (path, polarPoints) = RadarLayoutEngine.Layout(serie, ctx);

            BuildPolarPointsPayload(
                serie,
                serieIndex,
                options,
                polarPoints,
                out var lineDefaults,
                out var payloadPoints);

            payloads.Add(new PolarLinePayload
            {
                Id = serie.Id,
                ChartId = _chartId,
                SerieIndex = serieIndex,
                AnimationEnabled = serie.AnimationEnabled,
                GroupId = serie.Id,
                Index = serieIndex,
                Path = new PolarPathPayload
                {
                    Id = path.Id,
                    Points = polarPoints,
                    Closed = false
                },
                Points = payloadPoints,
                Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, lineDefaults.Normal),
                Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, lineDefaults.Hover),
                Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, lineDefaults.Pressed),
                Selected = ChartStyleResolver.Resolve(serie.Style?.Selected, lineDefaults.Selected),
                Animation = ChartAnimationResolver.Resolve(null, serie.Animation, options.Animation),
                Tooltip = new ChartTooltipPayload()
            });
        }

        target.AddLayer(new PolarLineGroupLayer(new(payloads)));
    }
}
