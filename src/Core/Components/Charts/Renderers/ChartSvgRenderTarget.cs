using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Components.Charts.Layers;
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
        switch (layer)
        {
            case LineLayer line:
                {
                    ChartLineSvgBuilder.Build(builder, (LinePayload)line.LayerPayload, _resolvedThemeContext!);
                }

                break;

            case BarLayer bar:
                {
                    ChartBarSvgBuilder.Build(builder, (BarPayloadCollection)bar.LayerPayload, _resolvedThemeContext!, _context());
                }

                break;

            case ColumnLayer column:
                {
                    ChartColumnSvgBuilder.Build(builder, (ColumnPayloadCollection)column.LayerPayload, _context(), _resolvedThemeContext!);
                }

                break;

            case AxesLayer axis:
                {
                    ChartAxesSvgBuilder.Build(builder, (AxisPayload)axis.LayerPayload, _options().DefaultAxisOptions, _resolvedThemeContext!);
                }

                break;

            case GridLayer grid:
                {
                    ChartGridSvgBuilder.Build(builder, (GridPayload)grid.LayerPayload, _resolvedThemeContext!);
                }

                break;

            case PieLayer pie:
                {
                    ChartPieSvgBuilder.Build(builder, (PiePayloadCollection)pie.LayerPayload, _resolvedThemeContext!);
                }

                break;

            case MultiDonutLayer multiDonut:
                {
                    ChartMultiDonutSvgBuilder.Build(builder, (MultiDonutPayloadCollection)multiDonut.LayerPayload, _resolvedThemeContext!);
                }

                break;

            case DonutLayer donut:
                {
                    ChartDonutSvgBuilder.Build(builder, (DonutPayloadCollection)donut.LayerPayload, _resolvedThemeContext!);
                }

                break;

            case ClipPathLayer clipPath:
                {
                    ChartClipPathSvgBuilder.Build(builder, (ClipPathPayload)clipPath.LayerPayload!);
                }

                break;

            case ChartLayoutLayer layout:
                {
                    ChartLayoutSvgBuilder.Build(builder, (ChartLayoutPayload)layout.LayerPayload!, _resolvedThemeContext!);
                }

                break;

            case AreaLayer area:
                {
                    ChartAreaSvgBuilder.Build(builder, (AreaPayload)area.LayerPayload, _resolvedThemeContext!);
                }

                break;
        }
    }

    /// <inheritdoc />
    protected override void ValidateLayer(ILayer layer)
    {
        if (layer is not LineLayer &&
            layer is not BarLayer &&
            layer is not ColumnLayer &&
            layer is not AxesLayer &&
            layer is not GridLayer &&
            layer is not PieLayer &&
            layer is not DonutLayer &&
            layer is not MultiDonutLayer &&
            layer is not ClipPathLayer &&
            layer is not ChartLayoutLayer &&
            layer is not AreaLayer)
        {
            throw new InvalidOperationException($"Unsupported layer type: {layer.GetType().Name}");
        }
    }
}
