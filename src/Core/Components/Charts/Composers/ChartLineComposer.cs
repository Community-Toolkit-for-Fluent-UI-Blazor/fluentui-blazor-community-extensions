using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using FluentUI.Blazor.Community.Components.Enums;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;
using LS = FluentUI.Blazor.Community.Components.Charts.Series.LineSerie;

/// <summary>
/// Represents a composer responsible for generating the necessary layers to render line charts.
/// </summary>
internal sealed class ChartLineComposer
    : ISurfaceComposer<CO>
{
    private readonly string _chartId;
    private readonly Func<ChartContext> _context;
    private readonly Func<IEnumerable<LS>> _series;
    private readonly Dictionary<LineChartType, Action<ISurfaceRenderTarget, LS, List<LS>, int, ChartContext, CO, ChartMarkerLineStyle?, ChartLinePointStyle?>> _payloadBuilder;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChartLineComposer"/> class with the specified chart context, series collection, and chart identifier.
    /// </summary>
    /// <param name="context">The chart context for composition.</param>
    /// <param name="series">The collection of category line series to be rendered.</param>
    /// <param name="chartId">The unique identifier of the chart, used for interaction payloads.</param>
    public ChartLineComposer(
        string chartId,
        Func<ChartContext> context,
        Func<IEnumerable<LS>> series)
    {
        _chartId = chartId;
        _context = context;
        _series = series;

        _payloadBuilder = new(EqualityComparer<LineChartType>.Default)
        {
            [LineChartType.StackedArea] = (target, serie, filtered, index, ctx, options, markerDefaults, defaults) => BuildStackedPayload(
               target,
               serie,
               index,
               options,
               markerDefaults ?? options.DefaultMarkerStyles,
               defaults ?? options.DefaultLineStyles,
               StackedAreaLayoutEngine.Layout(serie, filtered, index, ctx, options)),

            [LineChartType.Stacked100Area] = (target, serie, filtered, index, ctx, options, markerDefaults, defaults) => BuildStackedPayload(
               target,
               serie,
               index,
               options,
               markerDefaults ?? options.DefaultMarkerStyles,
               defaults ?? options.DefaultLineStyles,
               StackedAreaLayoutEngine.Layout(serie, filtered, index, ctx, options)),

            [LineChartType.StackedStep] = (target, serie, filtered, index, ctx, options, markerDefaults, defaults) => BuildStackedPayload(
                target,
               serie,
               index,
               options,
               markerDefaults ?? options.DefaultMarkerStyles,
               defaults ?? options.DefaultLineStyles,
               StackedStepLayoutEngine.Layout(serie, filtered, index, ctx, options)),

            [LineChartType.Stacked100Step] = (target, serie, filtered, index, ctx, options, markerDefaults, defaults) => BuildStackedPayload(
               target,
               serie,
               index,
               options,
               markerDefaults ?? options.DefaultMarkerStyles,
               defaults ?? options.DefaultLineStyles,
               StackedStepLayoutEngine.Layout(serie, filtered, index, ctx, options)),

            [LineChartType.Scatter] = (target, serie, filtered, index, ctx, options, markerDefaults, defaults) => BuildScatterPayload(
               target,
               serie,
               index,
               options,
               markerDefaults ?? options.DefaultMarkerStyles,
               defaults ?? options.DefaultLineStyles,
               ScatterLayoutEngine.Layout(serie, ctx, options)),

            [LineChartType.Bubble] = (target, serie, filtered, index, ctx, options, markerDefaults, defaults) => BuildBubblePayload(
                target,
                serie,
                index,
                options,
                markerDefaults ?? options.DefaultMarkerStyles,
                defaults ?? options.DefaultLineStyles,
                BubbleLayoutEngine.Layout(serie, ctx, options)),

            [LineChartType.Step] = (target, serie, filtered, index, ctx, options, markerDefaults, defaults) => BuildLinePayload(
                target,
                serie,
                index,
                options,
                markerDefaults ?? options.DefaultMarkerStyles,
                defaults ?? options.DefaultLineStyles,
                StepLayoutEngine.Layout(serie, ctx, options)),

            [LineChartType.Line] = (target, serie, filtered, index, ctx, options, markerDefaults, defaults) => BuildLinePayload(
                target,
                serie,
                index,
                options,
                markerDefaults ?? options.DefaultMarkerStyles,
                defaults ?? options.DefaultLineStyles,
                CategoryLineLayoutEngine.Layout(serie, ctx, options)),

            [LineChartType.Area] = (target, serie, filtered, index, ctx, options, markerDefaults, defaults) => BuildAreaPayload(
                target,
                ctx,
                serie,
                index,
                options,
                markerDefaults ?? options.DefaultMarkerStyles,
                defaults ?? options.DefaultLineStyles,
                CategoryLineLayoutEngine.Layout(serie, ctx, options))
        };
    }

    /// <summary>
    /// Builds a collection of line point payloads from the given points and serie data.
    /// </summary>
    /// <param name="chartId">The unique identifier for the chart.</param>
    /// <param name="serie">The line series containing the item data.</param>
    /// <param name="index">The index of the series.</param>
    /// <param name="points">The collection of line points to transform.</param>
    /// <param name="chartMarkerLineStyle">The default marker line style to apply.</param>
    /// <param name="options">The chart options containing animation settings.</param>
    /// <returns>A list of line point payloads with resolved styles and animations.</returns>
    private static List<LinePointPayload> BuildLinePointPayload(
        string chartId,
        LS serie,
        int index,
        IReadOnlyList<LinePoint> points,
        ChartMarkerLineStyle chartMarkerLineStyle,
        CO options)
    {
        return [.. points
            .Select(p =>
            {
                var item = serie.Items[p.CategoryIndex];

                return new LinePointPayload
                {
                    SerieIndex = index,
                    ChartId = chartId,
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
                    Normal = ChartStyleResolver.Resolve(item.Style?.Normal, chartMarkerLineStyle.Normal),
                    Hover = ChartStyleResolver.Resolve(item.Style?.Hover, chartMarkerLineStyle.Hover),
                    Pressed = ChartStyleResolver.Resolve(item.Style?.Pressed, chartMarkerLineStyle.Pressed),
                    Selected = ChartStyleResolver.Resolve(item.Style?.Selected, chartMarkerLineStyle.Selected),
                    Animation = ChartAnimationResolver.Resolve(item.Animation, serie.Animation, options.Animation),
                    Tooltip = new ChartTooltipPayload()
                    {
                    }
                };
            })];
    }

    /// <summary>
    /// Builds an area chart payload from the series data and layout, then adds it as a layer to the render target.
    /// </summary>
    /// <param name="target">The surface render target to receive the area layer.</param>
    /// <param name="context">The chart context containing plot area boundaries.</param>
    /// <param name="serie">The line series containing the data to render.</param>
    /// <param name="index">The zero-based index of the series within the chart.</param>
    /// <param name="options">Chart configuration options including animation settings.</param>
    /// <param name="chartMarkerLineStyle">Style configuration for marker line points in different interaction states.</param>
    /// <param name="chartLinePointStyle">Style configuration for line points in different interaction states.</param>
    /// <param name="layout">A tuple containing the line path and collection of points that define the area geometry.</param>
    private void BuildAreaPayload(
        ISurfaceRenderTarget target,
        ChartContext context,
        LS serie,
        int index,
        CO options,
        ChartMarkerLineStyle chartMarkerLineStyle,
        ChartLinePointStyle chartLinePointStyle,
        (LinePath path, IReadOnlyList<LinePoint> points) layout)
    {
        var (path, points) = layout;

        var payloadPoints = BuildLinePointPayload(_chartId, serie, index, points, chartMarkerLineStyle, options);

        var payload = new AreaPayload
        {
            ChartId = _chartId,
            Id = serie.Id,
            SerieIndex = index,
            AnimationEnabled = serie.AnimationEnabled,
            GroupId = string.Empty,
            Index = index,
            Path = new LinePathPayload()
            {
                Id = path.Id,
                Points = path.Points,
                Smooth = path.Smooth
            },
            Points = payloadPoints,
            BaselineY = context.PlotArea.Bottom,
            Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, chartLinePointStyle.Normal),
            Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, chartLinePointStyle.Hover),
            Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, chartLinePointStyle.Pressed),
            Selected = ChartStyleResolver.Resolve(serie.Style?.Selected, chartLinePointStyle.Selected),
            Animation = ChartAnimationResolver.Resolve(null, serie.Animation, options.Animation),
            Tooltip = new ChartTooltipPayload()
            {
            }
        };

        target.AddLayer(new AreaLayer(payload));
    }

    /// <summary>
    /// Builds a line payload from the series data and layout, then adds it as a layer to the render target.
    /// </summary>
    /// <param name="target">The surface render target to receive the line layer.</param>
    /// <param name="serie">The line series containing the data and configuration.</param>
    /// <param name="index">The zero-based index of the series.</param>
    /// <param name="options">The chart options providing default configuration values.</param>
    /// <param name="chartMarkerLineStyle">The marker line style configuration.</param>
    /// <param name="chartLinePointStyle">The line point style configuration for different states.</param>
    /// <param name="layout">A tuple containing the line path and the collection of points defining the line geometry.</param>
    private void BuildLinePayload(
        ISurfaceRenderTarget target,
        LS serie,
        int index,
        CO options,
        ChartMarkerLineStyle chartMarkerLineStyle,
        ChartLinePointStyle chartLinePointStyle,
        (LinePath path, IReadOnlyList<LinePoint> points) layout)
    {
        var (path, points) = layout;

        var payloadPoints = BuildLinePointPayload(_chartId, serie, index, points, chartMarkerLineStyle, options);

        var payload = new LinePayload()
        {
            ChartId = _chartId,
            Id = serie.Id,
            SerieIndex = index,
            AnimationEnabled = serie.AnimationEnabled,
            GroupId = string.Empty,
            Index = index,
            Path = new LinePathPayload()
            {
                Id = path.Id,
                Points = path.Points,
                Smooth = path.Smooth
            },
            Points = payloadPoints,
            Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, chartLinePointStyle.Normal),
            Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, chartLinePointStyle.Hover),
            Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, chartLinePointStyle.Pressed),
            Selected = ChartStyleResolver.Resolve(serie.Style?.Selected, chartLinePointStyle.Selected),
            Animation = ChartAnimationResolver.Resolve(null, serie.Animation, options.Animation),
            Tooltip = new ChartTooltipPayload()
            {
            }
        };

        target.AddLayer(new LineLayer(payload));
    }

    /// <summary>
    /// Constructs a stacked area chart payload from the series data and adds it as a layer to the render target.
    /// </summary>
    /// <param name="target">The surface render target to which the stacked area layer will be added.</param>
    /// <param name="serie">The series data containing items and styling information.</param>
    /// <param name="index">The zero-based index of the series in the chart.</param>
    /// <param name="options">The chart options containing animation and configuration settings.</param>
    /// <param name="markerLineStyle">The style configuration for individual marker line points.</param>
    /// <param name="chartLinePointStyle">The style configuration for the chart line points.</param>
    /// <param name="value">A tuple containing the top path, bottom path, and collection of line points that define the stacked area.</param>
    private void BuildStackedPayload(
        ISurfaceRenderTarget target,
        LS serie,
        int index,
        CO options,
        ChartMarkerLineStyle markerLineStyle,
        ChartLinePointStyle chartLinePointStyle,
        (LinePath TopPath, LinePath BottomPath, IReadOnlyList<LinePoint> Points) value)
    {
        var top = value.TopPath;
        var bottom = value.BottomPath;
        var points = value.Points;
        var payloadPoints = BuildLinePointPayload(_chartId, serie, index, points, markerLineStyle, options);

        var payload = new StackedAreaPayload
        {
            ChartId = _chartId,
            Id = serie.Id,
            SerieIndex = index,
            AnimationEnabled = serie.AnimationEnabled,
            GroupId = string.Empty,
            Index = index,
            TopPath = new LinePathPayload
            {
                Id = top.Id,
                Points = top.Points,
                Smooth = top.Smooth
            },
            BottomPath = new LinePathPayload
            {
                Id = bottom.Id,
                Points = bottom.Points,
                Smooth = bottom.Smooth
            },
            Points = payloadPoints,
            Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, chartLinePointStyle.Normal),
            Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, chartLinePointStyle.Hover),
            Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, chartLinePointStyle.Pressed),
            Selected = ChartStyleResolver.Resolve(serie.Style?.Selected, chartLinePointStyle.Selected),
            Animation = ChartAnimationResolver.Resolve(null, serie.Animation, options.Animation),
            Tooltip = new ChartTooltipPayload()
        };

        target.AddLayer(new StackedAreaLayer(payload));
    }

    /// <summary>
    /// Constructs and adds a bubble layer to the render target by transforming series data and points into a bubble
    /// payload.
    /// </summary>
    /// <param name="target">The render target to which the bubble layer will be added.</param>
    /// <param name="serie">The line series containing the data items to be rendered.</param>
    /// <param name="index">The zero-based index of the series in the chart.</param>
    /// <param name="options">The chart options containing global settings such as animation configuration.</param>
    /// <param name="markerLineStyle">The default marker line style to apply when item-specific styles are not defined.</param>
    /// <param name="chartLinePointStyle">The default chart line point style to apply at the series level when styles are not defined.</param>
    /// <param name="points">The collection of bubble points to render, containing position and size information.</param>
    private void BuildBubblePayload(
        ISurfaceRenderTarget target,
        LS serie,
        int index,
        CO options,
        ChartMarkerLineStyle markerLineStyle,
        ChartLinePointStyle chartLinePointStyle,
        IReadOnlyList<BubblePoint> points)
    {
        var payloadPoints = points.Select(p =>
        {
            var item = serie.Items[p.CategoryIndex];

            return new BubblePointPayload
            {
                BubbleValue = p.BubbleValue,
                Radius = p.Radius,
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
                Normal = ChartStyleResolver.Resolve(item.Style?.Normal, markerLineStyle.Normal),
                Hover = ChartStyleResolver.Resolve(item.Style?.Hover, markerLineStyle.Hover),
                Pressed = ChartStyleResolver.Resolve(item.Style?.Pressed, markerLineStyle.Pressed),
                Selected = ChartStyleResolver.Resolve(item.Style?.Selected, markerLineStyle.Selected),
                Animation = ChartAnimationResolver.Resolve(item.Animation, serie.Animation, options.Animation),
                Tooltip = new ChartTooltipPayload()
            };
        }).ToList();

        var payload = new BubblePayload
        {
            ChartId = _chartId,
            Id = serie.Id,
            SerieIndex = index,
            AnimationEnabled = serie.AnimationEnabled,
            GroupId = string.Empty,
            Index = index,
            Points = payloadPoints,
            Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, chartLinePointStyle.Normal),
            Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, chartLinePointStyle.Hover),
            Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, chartLinePointStyle.Pressed),
            Selected = ChartStyleResolver.Resolve(serie.Style?.Selected, chartLinePointStyle.Selected),
            Animation = ChartAnimationResolver.Resolve(null, serie.Animation, options.Animation),
            Tooltip = new ChartTooltipPayload()
        };

        target.AddLayer(new BubbleLayer(payload));
    }

    /// <summary>
    /// Builds and adds a scatter chart layer to the render target using the specified series data and styles.
    /// </summary>
    /// <param name="target">The render target to add the scatter layer to.</param>
    /// <param name="serie">The series containing the data and configuration.</param>
    /// <param name="index">The index of the series in the chart.</param>
    /// <param name="options">The chart options.</param>
    /// <param name="markerLineStyle">The marker line style to apply.</param>
    /// <param name="chartLinePointStyle">The chart line point style to apply.</param>
    /// <param name="points">The collection of line points to render.</param>
    private void BuildScatterPayload(
        ISurfaceRenderTarget target,
        LS serie,
        int index,
        CO options,
        ChartMarkerLineStyle markerLineStyle,
        ChartLinePointStyle chartLinePointStyle,
        IReadOnlyList<LinePoint> points)
    {
        var payloadPoints = BuildLinePointPayload(_chartId, serie, index, points, markerLineStyle, options);

        var payload = new ScatterPayload
        {
            ChartId = _chartId,
            Id = serie.Id,
            SerieIndex = index,
            AnimationEnabled = serie.AnimationEnabled,
            GroupId = string.Empty,
            Index = index,
            Points = payloadPoints,
            Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, chartLinePointStyle.Normal),
            Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, chartLinePointStyle.Hover),
            Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, chartLinePointStyle.Pressed),
            Selected = ChartStyleResolver.Resolve(serie.Style?.Selected, chartLinePointStyle.Selected),
            Animation = ChartAnimationResolver.Resolve(null, serie.Animation, options.Animation),
            Tooltip = new ChartTooltipPayload(),
            Radius = options.DefaultScatterOptions.Radius
        };

        target.AddLayer(new ScatterLayer(payload));
    }

    /// <inheritdoc />
    public bool Compose(
        ISurfaceRenderTarget target,
        CO chartOptions)
    {
        var filtered = _series().Where(s => s.IsVisible).ToList();

        if (filtered.Count == 0)
        {
            return false;
        }

        filtered.Sort((a, b) =>
        {
            return a.Items.Sum(x => x.Value).CompareTo(b.Items.Sum(x => x.Value));
        });

        var defaults = chartOptions.DefaultLineStyles;
        var markerDefaults = chartOptions.DefaultMarkerStyles;
        var ctx = _context();

        for (var i = 0; i < filtered.Count; i++)
        {
            var serie = filtered[i];

            if (_payloadBuilder.TryGetValue(serie.LineType, out var builder))
            {
                builder(target, serie, filtered, i, ctx, chartOptions, markerDefaults, defaults);
            }
        }

        return true;
    }

    /// <inheritdoc />
    public ValueTask<bool> ComposeAsync(
        ISurfaceRenderTarget target,
        CO options)
    {
        return ValueTask.FromResult(Compose(target, options));
    }
}
