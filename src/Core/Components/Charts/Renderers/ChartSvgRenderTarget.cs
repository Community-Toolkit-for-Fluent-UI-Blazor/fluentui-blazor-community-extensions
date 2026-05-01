using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Surface.Payloads;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Renderers;

/// <summary>
/// Represents a render target for generating SVG output from chart layers.
/// </summary>
internal sealed class ChartSvgRenderTarget
    : SvgRenderTarget
{
    private readonly Func<ChartContext> _context;
    private readonly Func<CO> _options;
    private readonly Func<ChartThemeContext> _themeContext;
    private ChartThemeContext? _resolvedThemeContext;
    private static readonly Dictionary<string, Action<SvgBuilder, ILayerPayload, ChartContext, ChartThemeContext, Options.ChartAxisOptions>> _layerRenderes = new()
    {
        ["line"] = (builder, payload, context, themeContext, axisOptions) => ChartLineSvgBuilder.Build(builder, (LinePayload)payload, themeContext),
        ["bar"] = (builder, payload, context, themeContext, axisOptions) => ChartBarSvgBuilder.Build(builder, (BarPayloadCollection)payload, themeContext, context),
        ["column"] = (builder, payload, context, themeContext, axisOptions) => ChartColumnSvgBuilder.Build(builder, (ColumnPayloadCollection)payload, context, themeContext),
        ["axes"] = (builder, payload, context, themeContext, axisOptions) => ChartAxesSvgBuilder.Build(builder, (AxisPayload)payload, axisOptions, themeContext),
        ["grid"] = (builder, payload, context, themeContext, axisOptions) => ChartGridSvgBuilder.Build(builder, (GridPayload)payload, themeContext),
        ["pie"] = (builder, payload, context, themeContext, axisOptions) => ChartPieSvgBuilder.Build(builder, (PiePayloadCollection)payload, themeContext),
        ["donut"] = (builder, payload, context, themeContext, axisOptions) => ChartDonutSvgBuilder.Build(builder, (DonutPayloadCollection)payload, themeContext),
        ["multi-donut"] = (builder, payload, context, themeContext, axisOptions) => ChartMultiDonutSvgBuilder.Build(builder, (MultiDonutPayloadCollection)payload, themeContext),
        ["clip-path"] = (builder, payload, context, themeContext, axisOptions) => ChartClipPathSvgBuilder.Build(builder, (ClipPathPayload)payload!),
        ["chart-layout"] = (builder, payload, context, themeContext, axisOptions) => ChartLayoutSvgBuilder.Build(builder, (ChartLayoutPayload)payload!, themeContext),
        ["area"] = (builder, payload, context, themeContext, axisOptions) => ChartAreaSvgBuilder.Build(builder, (AreaPayload)payload!, themeContext),
        ["stacked-area"] = (builder, payload, context, themeContext, axisOptions) => ChartStackedAreaSvgBuilder.Build(builder, (StackedAreaPayload)payload!, themeContext),
        ["radar"] = (builder, payload, context, themeContext, axisOptions) => ChartRadarSvgBuilder.Build(builder, (RadarPayloadCollection)payload!, themeContext),
        ["polar-axes"] = (builder, payload, context, themeContext, axisOptions) => ChartPolarAxesSvgBuilder.Build(builder, (PolarAxesPayload)payload!, themeContext),
        ["polar-area"] = (builder, payload, context, themeContext, axisOptions) => ChartPolarAreaSvgBuilder.Build(builder, (PolarAreaPayloadCollection)payload!, themeContext),
        ["semi-donut"] = (builder, payload, context, themeContext, axisOptions) => ChartDonutSvgBuilder.Build(builder, (DonutPayloadCollection)payload!, themeContext),
        ["polar-bar"] = (builder, payload, context, themeContext, axisOptions) => ChartPolarBarSvgBuilder.Build(builder, (PolarBarPayloadCollection)payload!, themeContext),
        ["polar-line"] = (builder, payload, context, themeContext, axisOptions) => ChartPolarLineSvgBuilder.Build(builder, (PolarLinePayloadCollection)payload!, themeContext),
        ["rose"] = (builder, payload, context, themeContext, axisOptions) => ChartRoseSvgBuilder.Build(builder, (RosePayloadCollection)payload!, themeContext),
        ["polar-scatter"] = (builder, payload, context, themeContext, axisOptions) => PolarScatterSvgBuilder.Build(builder, (PolarScatterPayloadCollection)payload!, themeContext),
        ["polar-bubble"] = (builder, payload, context, themeContext, axisOptions) => PolarBubbleSvgBuilder.Build(builder, (PolarBubblePayloadCollection)payload!, themeContext),
        ["xy-scatter"] = (builder, payload, context, themeContext, axisOptions) => ChartScatterSvgBuilder.Build(builder, (XYScatterPayloadCollection)payload!, themeContext),
        ["xy-bubble"] = (builder, payload, context, themeContext, axisOptions) => ChartBubbleSvgBuilder.Build(builder, (XYBubblePayloadCollection)payload!, themeContext),
        ["xy-line"] = (builder, payload, context, themeContext, axisOptions) => ChartXYLineSvgBuilder.Build(builder, (XYLinePayloadCollection)payload!, themeContext),
        ["xy-area"] = (builder, payload, context, themeContext, axisOptions) => ChartXYAreaSvgBuilder.Build(builder, (XYAreaPayloadCollection)payload!, themeContext),
        ["histogram"] = (builder, payload, context, themeContext, axisOptions) => ChartHistogramSvgBuilder.Build(builder, (HistogramPayloadCollection)payload!, themeContext),
        ["xy-column"] = (builder, payload, context, themeContext, axisOptions) => ChartXYColumnSvgBuilder.Build(builder, (PayloadCollection<XYColumnPayloadCollection>)payload!, themeContext),
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="ChartSvgRenderTarget"/> class with the specified parameters.
    /// </summary>
    /// <param name="chartId">The unique identifier for the chart.</param>
    /// <param name="context">A function that provides the chart context.</param>
    /// <param name="themeContext">A function that provides the chart theme context.</param>
    /// <param name="options">A function that provides the chart options.</param>
    public ChartSvgRenderTarget(
        string chartId,
        Func<ChartContext> context,
        Func<ChartThemeContext> themeContext,
        Func<CO> options)
        : base(chartId)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(themeContext, nameof(themeContext));
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        _context = context;
        _options = options;
        _themeContext = themeContext;
        ShapeRendering = SvgShapeRendering.None;
    }

    /// <inheritdoc />
    protected override void BeforeRender()
    {
        _resolvedThemeContext = _themeContext() ?? throw new InvalidOperationException("Theme context cannot be null when rendering chart SVG.");
    }

    /// <inheritdoc />
    protected override (double width, double height) ComputeViewBox(IEnumerable<ILayer> layers)
    {
        var resolve = _context();

        return (resolve.ChartArea.Width, resolve.ChartArea.Height);
    }

    /// <inheritdoc />
    protected override void RenderLayer(SvgBuilder builder, ILayer layer)
    {
        ArgumentNullException.ThrowIfNull(layer);

        if (_layerRenderes.TryGetValue(layer.Key, out var renderer))
        {
            renderer(builder, layer.LayerPayload, _context(), _resolvedThemeContext!, _options().Axis);
        }
    }

    /// <inheritdoc />
    protected override void ValidateLayer(ILayer layer)
    {
        if (layer is not IChartLayer &&
            layer is not AxesLayer &&
            layer is not GridLayer)
        {
            throw new InvalidOperationException($"Unsupported layer type: {layer.GetType().Name}");
        }
    }
}
