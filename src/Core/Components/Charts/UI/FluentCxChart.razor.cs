using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Composers;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Renderers;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;
using FluentUI.Blazor.Community.Components.Components.Base;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a Fluent UI Blazor chart component that is initialized with a specified library configuration.
/// </summary>
/// <remarks>This component provides charting capabilities as part of the Fluent UI Blazor community extension. It
/// requires a valid library configuration for proper operation and integration with the Fluent UI ecosystem.</remarks>
public partial class FluentCxChart : FluentComponentBase
{
    /// <summary>
    /// Represents a collection of live series that are currently being rendered on the chart.
    /// </summary>
    private readonly Dictionary<string, ChartSerie> _liveSeries = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Represents a mapping of chart types to functions that compute the number of legend items for category-based charts.
    /// </summary>
    private static readonly Dictionary<ChartType, Func<int, int>> _singleCountCharts = new(EqualityComparer<ChartType>.Default)
    {
        [ChartType.Bar] = i => i + 1,
        [ChartType.Column] = i => i + 1,
        [ChartType.Line] = i => i + 1,
        [ChartType.Area] = i => i + 1,
        [ChartType.StackedBar] = i => i + 1,
        [ChartType.StackedColumn] = i => i + 1,
        [ChartType.StackedArea] = i => i + 1,
        [ChartType.Stacked100Bar] = i => i + 1,
        [ChartType.Stacked100Column] = i => i + 1,
        [ChartType.Stacked100Area] = i => i + 1,
        [ChartType.Step] = i => i + 1,
        [ChartType.Stacked100Step] = i => i + 1,
        [ChartType.StackedStep] = i => i + 1,
        [ChartType.Scatter] = i => i + 1,
        [ChartType.Bubble] = i => i + 1,
        [ChartType.Radar] = i => i + 1,
        [ChartType.PolarLine] = i => i + 1,
        [ChartType.PolarBar] = i => i + 1,
        [ChartType.PolarScatter] = i => i + 1,
        [ChartType.PolarBubble] = i => i + 1,
        [ChartType.XYLine] = i => i + 1,
        [ChartType.XYArea] = i => i + 1,
        [ChartType.XYColumn] = i => i + 1,
    };

    /// <summary>
    /// Represents a mapping of chart types to functions that compute the number of legend items for multi-count charts, such as pie and donut charts.
    /// </summary>
    private static readonly Dictionary<ChartType, Func<int, int, int>> _multiCountCharts = new(EqualityComparer<ChartType>.Default)
    {
        [ChartType.PolarArea] = (i, j) => i + j,
        [ChartType.Pie] = (i, j) => i + j,
        [ChartType.Donut] = (i, j) => i + j,
        [ChartType.SemiDonut] = (i, j) => i + j,
        [ChartType.PolarRose] = (i, j) => i + j,
    };

    /// <summary>
    /// Gets or sets the service responsible for providing chart theme information to the component.
    /// </summary>
    /// <remarks>This property is typically set by the dependency injection framework. It enables the
    /// component to retrieve theming details for charts, ensuring consistent appearance according to the application's
    /// theme configuration.</remarks>
    private ChartThemeProvider? _chartThemeProvider;

    /// <summary>
    /// Value indicating whether the density mode has changed since the last render.
    /// </summary>
    private bool _hasDensityChanged;

    /// <summary>
    /// Indicates whether the palette style has changed.
    /// </summary>
    private bool _hasPaletteStyleChanged;

    /// <summary>
    /// Value indicating whether the contrast mode has changed since the last render.
    /// </summary>
    private bool _hasContrastChanged;

    /// <summary>
    /// Value indicating whether the theme override has changed since the last render.
    /// </summary>
    private bool _hasThemeOverrideChanged;

    /// <summary>
    /// Represents if the chart needs to be redrawn due to changes in options or data.
    /// </summary>
    private bool _needRedraw;

    /// <summary>
    /// Value indicating whether the chart palette needs to be rebuilt due to changes in the palette style or theme.
    /// </summary>
    private bool _rebuildPalette;

    /// <summary>
    /// Value indicating whether the theme has been set from <see cref="SetThemeAsync(ChartThemeRequest)" />
    ///  to avoid the override of the palette colors.
    /// </summary>
    private bool _setFromThemeAsync;

    /// <summary>
    /// Represents the javaScript module reference used for interop calls related to this chart component.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents the collection of child series that are part of this chart.
    /// </summary>
    private readonly List<SerieBase> _children = [];

    /// <summary>
    /// Represents the chart configuration options used to customize the behavior and appearance of the chart.
    /// </summary>
    private readonly Charts.Options.ChartOptions _options = new();

    /// <summary>
    /// Represents the chart context.
    /// </summary>
    private readonly ChartContext _chartContext = new();

    /// <summary>
    /// Represents the chart theme context that provides styling information for rendering the chart according to the current theme.
    /// </summary>
    private ChartThemeContext _chartThemeContext = new();

    /// <summary>
    /// Represents the currently selected chart theme, or null if no theme is selected.
    /// </summary>
    private ChartTheme? _currentTheme;

    /// <summary>
    /// Represents the path to the JavaScript module used for interop with this chart component.
    /// </summary>
    private const string JavaScriptModulePath = FluentCxConstants.JAVASCRIPT_ROOT + "Charts/UI/FluentCxChart.razor.js";

    /// <summary>
    /// Holds a reference to the current instance for JavaScript interop callbacks.
    /// </summary>
    /// <remarks>This reference is used to enable JavaScript code to invoke .NET instance methods on the
    /// associated FluentCxChart component. It is typically passed to JavaScript when registering for interop
    /// events.</remarks>
    private DotNetObjectReference<FluentCxChart>? _dotNetObjectReference;

    /// <summary>
    /// Provides an instance of the ChartComposer used to compose chart elements within the containing class.
    /// </summary>
    private readonly ChartComposer _chartComposer = new();

    /// <summary>
    /// Value indicating whether the animation enabled state has changed since the last render, which may require a redraw of the chart to reflect the new animation settings.
    /// </summary>
    private bool _hasAnimationEnabledChanged;

    /// <summary>
    /// Provides access to the chart interaction engine used to manage user interactions with the chart.
    /// </summary>
    private readonly ChartInteractionEngine _interactionEngine;

    /// <summary>
    /// Indicates whether the chart is visible.
    /// </summary>
    private bool _isVisible = true;

    /// <summary>
    /// Value indicating whether the chart is currently in the process of rendering.
    /// </summary>
    private bool _isRendering;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxChart"/> class with the specified configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the chart component. This parameter is required for proper initialization of the component.</param>
    public FluentCxChart(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
        _interactionEngine = new ChartInteractionEngine(() => _liveSeries.Values);
        _chartContext = _chartContext with
        {
            ChartId = Id
        };
    }

    /// <summary>
    /// Gets or sets the intersection ratio threshold at which the observer callback is triggered.
    /// </summary>
    [Parameter]
    public double? IntersectionThreshold { get; set; }

    /// <summary>
    /// Gets or sets the intersection ratio threshold at which the observer callback is triggered.
    /// </summary>
    [Parameter]
    public VisibilityIntersectionTheshold VisibilityIntersectionThreshold { get; set; } = VisibilityIntersectionTheshold.Balanced;

    /// <summary>
    /// Gets or sets the content to render as options within the component.
    /// </summary>
    /// <remarks>Use this property to provide custom option markup using a RenderFragment. The content
    /// specified will be rendered in place of the default options.</remarks>
    [Parameter]
    public RenderFragment? Options { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether animations are enabled for the chart.
    /// </summary>
    /// <remarks>
    /// The value is set to true by default, enabling animations for the chart.
    /// </remarks>
    [Parameter]
    public bool AnimationEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the content to be rendered inside this component.
    /// </summary>
    /// <remarks>Use this parameter to specify the child elements or markup that will be displayed within the
    /// component. Typically, this is set in Razor markup by placing content between the component's opening and closing
    /// tags.</remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the surface render target used for drawing operations.
    /// </summary>
    /// <remarks>Set this property to specify a custom render target for surface rendering. If not set, the
    /// default rendering behavior is used.</remarks>
    [Parameter]
    public ISurfaceRenderTarget? RenderTarget { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the operation should be performed asynchronously.
    /// </summary>
    [Parameter]
    public bool IsAsync { get; set; }

    /// <summary>
    /// Gets or sets the density mode used to render the chart elements.
    /// </summary>
    /// <remarks>Use this property to control the spacing and compactness of chart elements. Different density
    /// modes may affect the visual appearance and readability of the chart, especially when displaying large data
    /// sets.</remarks>
    [Parameter]
    public ChartDensityMode Density { get; set; } = ChartDensityMode.Normal;

    /// <summary>
    /// Gets or sets the contrast mode used for rendering the chart.
    /// </summary>
    /// <remarks>Use this property to adjust the visual contrast of the chart for accessibility or stylistic
    /// purposes. The default value is ChartContrastMode.Normal.</remarks>
    [Parameter]
    public ChartContrastMode Contrast { get; set; } = ChartContrastMode.Normal;

    /// <summary>
    /// Gets or sets the vision type used for rendering the chart.
    /// </summary>
    [Parameter]
    public ColorVisionType Vision { get; set; } = ColorVisionType.Normal;

    /// <summary>
    /// Gets or sets the theme override to apply to the chart component.
    /// </summary>
    /// <remarks>Use this property to customize the appearance of the chart independently of the application's
    /// global theme. If not set, the chart uses the default theme.</remarks>
    [Parameter]
    public ChartThemeOverride? ThemeOverride { get; set; }

    /// <summary>
    /// Gets or sets the theme options used to customize the appearance of the chart.
    /// </summary>
    /// <remarks>Use this property to specify colors, fonts, and other visual settings for the chart. If not
    /// set, default theme options are applied.</remarks>
    [Parameter]
    public ChartThemeOptions? ThemeOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether the dark theme is enabled.
    /// </summary>
    [Parameter]
    public bool IsDark { get; set; }

    /// <summary>
    /// Gets or sets the palette style used for the chart's color scheme.
    /// </summary>
    [Parameter]
    public ChartPaletteStyle? PaletteStyle { get; set; }

    /// <summary>
    /// Gets or sets the title text of the chart.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the position of the chart title.
    /// </summary>
    [Parameter]
    public ChartTitlePosition TitlePosition { get; set; } = ChartTitlePosition.Top;

    /// <summary>
    /// Gets or sets the subtitle text of the chart.
    /// </summary>
    [Parameter]
    public string? Subtitle { get; set; }

    /// <summary>
    /// Gets or sets the position of the chart subtitle.
    /// </summary>
    [Parameter]
    public ChartTitlePosition SubtitlePosition { get; set; } = ChartTitlePosition.Top;

    /// <summary>
    /// Gets or sets the template used to render each legend item in the chart legend.
    /// </summary>
    [Parameter]
    public ChartLegendItemShape LegendItemShape { get; set; } = ChartLegendItemShape.Circle;

    /// <summary>
    /// Gets or sets the position of the chart legend.
    /// </summary>
    [Parameter]
    public ChartLegendPosition LegendPosition { get; set; } = ChartLegendPosition.Left;

    /// <summary>
    /// Gets or sets the mode of the tooltip.
    /// </summary>
    [Parameter]
    public ChartTooltipMode TooltipMode { get; set; } = ChartTooltipMode.Simple;

    /// <summary>
    /// Gets or sets the position of the tooltip relative to the chart.
    /// </summary>
    [Parameter]
    public ChartTooltipPlacement TooltipPosition { get; set; } = ChartTooltipPlacement.Pointer;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSModule.ImportJavaScriptModuleAsync(JavaScriptModulePath);
            _dotNetObjectReference = DotNetObjectReference.Create(this);

            _chartThemeProvider = new ChartThemeProvider(new ChartPaletteProviders(), new ChartColorResolver(_module));
            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.Charts.Initialize", Id, IntersectionThreshold ?? GetVisibilityIntersectionThresholdValue(), _dotNetObjectReference);

            if (_currentTheme is null)
            {
                await CreateThemeAsync();
            }
        }

        if (!firstRender &&
            !_needRedraw || _currentTheme is null)
        {
            return;
        }

        _needRedraw = false;
        await RenderAsync();
    }

    /// <summary>
    /// Creates and configures the chart theme asynchronously by building a default theme request, generating the theme
    /// through the provider, and setting up the theme context with density and contrast settings.
    /// </summary>
    /// <returns></returns>
    private async Task CreateThemeAsync()
    {
        if (_chartThemeProvider is null)
        {
            return;
        }

        var request = BuildDefaultThemeRequest(PaletteStyle.GetValueOrDefault());
        _currentTheme = await _chartThemeProvider!.CreateThemeAsync(request);

        _chartThemeContext = ChartThemeContextFactory.Create(
            _currentTheme,
            ThemeOverride,
            new ChartDensity { Mode = Density },
            new ChartContrast { Mode = Contrast }
        );

        _needRedraw = true;
    }

    /// <summary>
    /// Gets the threshold value for visibility intersection detection.
    /// </summary>
    /// <returns>The intersection threshold value as a double between 0 and 1.</returns>
    /// <exception cref="NotImplementedException">The method is not yet implemented.</exception>
    private double GetVisibilityIntersectionThresholdValue()
    {
        return VisibilityIntersectionThreshold switch
        {
            VisibilityIntersectionTheshold.Smooth => 0.1,
            VisibilityIntersectionTheshold.Balanced => 0.25,
            VisibilityIntersectionTheshold.Strict => 0.5,
            _ => 0.25
        };
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        _chartComposer.AddRange(
            new ChartBarComposer(Id!, () => _chartContext, GetSeries<Charts.Series.BarSerie>),
            new ChartColumnComposer(Id!, () => _chartContext, GetSeries<Charts.Series.ColumnSerie>),
            new ChartLineComposer(Id!, () => _chartContext, GetSeries<Charts.Series.LineSerie>),
            new ChartXYComposer(Id!, () => _chartContext, GetSeries<Charts.Series.XYSerie>),
            new ChartAxesComposer(() => _chartContext, () => _options.Axis, IsAxesSeries),
            new ChartGridComposer(() => _chartContext, () => _options.Grid),
            new ChartPieComposer(Id!, () => _chartContext, GetSeries<Charts.Series.PieSerie>),
            new ChartDonutComposer(Id!, () => _chartContext, GetSeries<Charts.Series.DonutSerie>),
            new ChartSemiDonutComposer(Id!, () => _chartContext, GetSeries<Charts.Series.SemiDonutSerie>),
            new ChartMultiDonutComposer(Id!, () => _chartContext, GetSeries<Charts.Series.MultiDonutSerie>),
            new ChartClipPathComposer(() => _chartContext),
            new ChartLayoutComposer(
                () => _chartContext,
                () => Title,
                () => Subtitle,
                () => LegendItemShape,
                () => _liveSeries.Values),
            new ChartPolarAxesComposer(() => _chartContext, () => _options.Axis, GetSeries<PolarSerie>),
            new ChartPolarComposer(Id!, () => _chartContext, GetSeries<PolarSerie>),
            new ChartHistogramComposer(Id!, () => _chartContext, GetSeries<Charts.Series.HistogramSerie>)
        );

        RenderTarget ??= new ChartSvgRenderTarget(Id!, () => _chartContext, () => _chartThemeContext, () => _options);
    }

    /// <summary>
    /// Returns a collection of series of the specified type generated from the child chart elements.
    /// </summary>
    /// <typeparam name="T">The type of series to retrieve.</typeparam>
    /// <returns>An enumerable collection of series of the specified type. The collection is empty if no child elements have the specified chart type.</returns>
    private IEnumerable<T> GetSeries<T>()
    {
        return _liveSeries.Values.OfType<T>();
    }

    /// <summary>
    /// Gets a value indicating whether all child series are of a type that uses axes.
    /// </summary>
    /// <returns>Returns <see langword="true" /> if all child series use axes; otherwise, <see langword="false" />.</returns>
    private bool IsAxesSeries()
    {
        return _liveSeries.Values.All(x => x.ChartType is ChartType.Bar or
            ChartType.Column or
            ChartType.Line or
            ChartType.Area or
            ChartType.StackedBar or
            ChartType.StackedColumn or
            ChartType.StackedArea or
            ChartType.Stacked100Bar or
            ChartType.Stacked100Column or
            ChartType.Stacked100Area or
            ChartType.Step or
            ChartType.Stacked100Step or
            ChartType.StackedStep or
            ChartType.Scatter or
            ChartType.Bubble or
            ChartType.XYLine or
            ChartType.Histogram or
            ChartType.XYColumn or
            ChartType.XYArea);
    }

    /// <summary>
    /// Updates the default bar series options for the chart.
    /// </summary>
    /// <param name="options">The bar series options to apply as the new default configuration.</param>
    internal void UpdateBarOptions(BarSerieOptions options)
    {
        _options.Bar = options;
        Refresh();
    }

    /// <summary>
    /// Updates the default column options for the chart.
    /// </summary>
    /// <param name="options">The new column options to apply as the default settings.</param>
    internal void UpdateColumnOptions(ColumnSerieOptions options)
    {
        _options.Column = options;
        Refresh();
    }

    /// <summary>
    /// Updates the default axis options for the chart.
    /// </summary>
    /// <param name="axisOptions">The new axis options to apply as the default configuration for all axes in the chart. This will affect the appearance and behavior of axes across the entire chart unless overridden by specific axis configurations.</param>
    internal void UpdateAxisOptions(Charts.Options.ChartAxisOptions axisOptions)
    {
        _options.Axis = axisOptions;
        Refresh();
    }

    /// <summary>
    /// Updates the default radar chart axes options for the chart.
    /// </summary>
    /// <param name="radarAxesOptions">The new radar chart axes options to apply as the default configuration for all radar chart axes in the chart. This will affect the appearance and behavior of radar chart axes across the entire chart unless overridden by specific axis configurations.</param>
    internal void UpdateRadarChartAxesOptions(RadarAxesOptions radarAxesOptions)
    {
        _options.RadarAxes = radarAxesOptions;
        Refresh();
    }

    /// <summary>
    /// Updates the default radar series options for the chart.
    /// </summary>
    /// <param name="radarSerieOptions">The new radar series options to apply as the default configuration for all radar series in the chart. This will affect the appearance and behavior of radar series across the entire chart unless overridden by specific series configurations.</param>
    internal void UpdateRadarSerieOptions(Charts.Options.RadarSerieOptions radarSerieOptions)
    {
        _options.Radar = radarSerieOptions;
        Refresh();
    }

    /// <summary>
    /// Updates the default semi-donut series options for the chart.
    /// </summary>
    /// <param name="semiDonutSerieOptions">The new semi-donut series options to apply as the default configuration for all semi-donut series in the chart. This will affect the appearance and behavior of semi-donut series across the entire chart unless overridden by specific series configurations.</param>
    internal void UpdateSemiDonutSerieOptions(RadialSerieOptions semiDonutSerieOptions)
    {
        _options.SemiDonut = semiDonutSerieOptions;
        Refresh();
    }

    /// <summary>
    /// Performs the chart rendering operation asynchronously, composing and flushing the chart to the render target.
    /// </summary>
    /// <remarks>This method determines whether to render the chart using asynchronous or synchronous
    /// composition based on the current configuration. It ensures that all axes are prepared before rendering and
    /// updates the component state after the rendering is complete.</remarks>
    /// <returns>A task that represents the asynchronous rendering operation.</returns>
    private async Task RenderAsync()
    {
        if (!_isVisible)
        {
            return;
        }

        var sw = System.Diagnostics.Stopwatch.StartNew();
        await BenchmarkAsync(async () =>
        {
            if (_chartContext.Width == 0 ||
                _chartContext.Height == 0 ||
                _children.Count == 0)
            {
                return;
            }

            if (_isRendering)
            {
                return;
            }

            _isRendering = true;
            var seriesChanged = SyncLiveSeries();

            if (_liveSeries.Count == 0)
            {
                return;
            }

            EnsureLayoutAndAxes();

            if (!_setFromThemeAsync &&
                _currentTheme is not null)
            {
                if (seriesChanged || _rebuildPalette)
                {
                    _rebuildPalette = false;

                    var paletteRequest = new ChartPaletteRequest
                    {
                        ColorCount = ComputeLegendItemCount(),
                        Options = IsDark ? ChartThemeOptions.Dark : ChartThemeOptions.Light,
                        Vision = Vision,
                        WorkingSpace = _currentTheme.WorkingSpace
                    };

                    var palette = await _currentTheme.PaletteProviders.Get(_currentTheme.PaletteStyle).GenerateAsync(paletteRequest);

                    _currentTheme = _currentTheme with
                    {
                        Palette = palette
                    };

                    _chartThemeContext = ChartThemeContextFactory.Create(
                        _currentTheme,
                        ThemeOverride,
                        new ChartDensity { Mode = Density },
                        new ChartContrast { Mode = Contrast }
                    );
                }
            }

            var composed = false;

            if (IsAsync)
            {
                composed |= await _chartComposer.ComposeAsync(RenderTarget!, _options);
            }
            else
            {
                composed |= _chartComposer.Compose(RenderTarget!, _options);
            }

            if (composed)
            {
                await RenderTarget!.FlushAsync();
                await InvokeAsync(StateHasChanged);
            }

            _isRendering = false;
        });
        sw.Stop();

        Console.WriteLine("Chart " + Id + "    RenderAsync : " + sw.ElapsedTicks + " ticks");
    }

    private static async Task BenchmarkAsync(Func<Task> callback)
    {
        await callback();
    }

    /// <summary>
    /// Ensures that the chart layout is computed and up to date before rendering.
    /// </summary>
    private void EnsureLayoutAndAxes()
    {
        if (_chartContext.Dirty)
        {
            ChartLayoutEngine.ComputeLayout(
                _chartContext,
                _chartThemeContext,
                Title,
                TitlePosition,
                Subtitle,
                SubtitlePosition,
                ComputeLegendItemCount(),
                LegendPosition);

            ChartAxisResolver.Resolve(
                _options,
                _chartContext,
                _chartThemeContext,
                [.. _children.Select(x => x.Create())]
            );

            _chartContext.Dirty = false;
        }
    }

    /// <summary>
    /// Computes the total number of legend items that will be displayed in the chart's legend based on the current live series and their respective chart types.
    /// </summary>
    /// <returns></returns>
    private int ComputeLegendItemCount()
    {
        if (_liveSeries.Count == 0)
        {
            return 0;
        }

        var count = 0;
        var series = GetLegendSeries();

        foreach (var serie in series)
        {
            if (_singleCountCharts.TryGetValue(serie.ChartType, out var countFunc))
            {
                count = countFunc(count);
                continue;
            }
            else if (_multiCountCharts.TryGetValue(serie.ChartType, out var multiCountFunc))
            {
                count = multiCountFunc(count, serie.RawItems.Count);
                continue;
            }
            else
            {
                switch (serie.ChartType)
                {
                    case ChartType.Histogram:
                        {
                            count = _options.Histogram.BinCount;
                        }

                        break;

                    case ChartType.MultiDonut:
                        {
                            count += ComputeMultiDonutLegendCount((Charts.Series.MultiDonutSerie)serie);
                        }

                        break;
                }
            }
        }

        return count;
    }

    /// <summary>
    /// Gets the series that should be included in the chart legend.
    /// </summary>
    /// <returns>Returns an enumerable of <see cref="ChartSerie"/> representing the series to be included in the legend.</returns>
    private IEnumerable<ChartSerie> GetLegendSeries()
    {
        foreach (var serie in _liveSeries.Values)
        {
            if (serie.ChartType == ChartType.Donut &&
                serie is Charts.Series.DonutSerie ds &&
                ds.IsPartOfMultiDonut)
            {
                continue;
            }

            yield return serie;
        }
    }

    /// <summary>
    /// Computes the number of legend entries for a multi-donut chart based on its palette mode.
    /// </summary>
    /// <param name="multi">The multi-donut series to analyze.</param>
    /// <returns>The number of legend entries required: distinct category count for ByCategory mode, series count for ByDonut
    /// mode, total item count for BySlice mode, or 0 for unknown modes.</returns>
    private static int ComputeMultiDonutLegendCount(Charts.Series.MultiDonutSerie multi)
    {
        return multi.PaletteMode switch
        {
            DonutPaletteMode.ByCategory => multi.Series.SelectMany(d => d.Items).Select(i => i.Name).ToHashSet().Count,
            DonutPaletteMode.ByDonut => multi.Series.Count,
            DonutPaletteMode.BySlice => multi.Series.Sum(d => d.ItemsCount),
            _ => 0
        };
    }

    /// <summary>
    /// Gets the markup string representation of the current render target, which can be used for rendering the chart's visual output.
    /// </summary>
    /// <returns>A <see cref="MarkupString"/> representing the current render target's visual output.</returns>
    private MarkupString GetRenderTargetMarkup()
    {
        var data = RenderTarget?.GetNativeHandle();

        if (data == null)
        {
            return new MarkupString(string.Empty);
        }

        if (data is MarkupString ms)
        {
            return ms;
        }

        return new MarkupString(data.ToString() ?? string.Empty);
    }

    /// <summary>
    /// Updates the default category line options for the chart.
    /// </summary>
    /// <param name="categoryLineOptions">The new category line options to apply as the default configuration for category line series in the chart.</param>
    internal void UpdateCategoryLineOptions(CategoryLineOptions categoryLineOptions)
    {
        _options.CategoryLine = categoryLineOptions;
        Refresh();
    }

    /// <summary>
    /// Adds a new series to the chart's collection of child series.
    /// </summary>
    /// <param name="value">The series to add to the chart.</param>
    internal void AddSerie(SerieBase value)
    {
        ValidateSerie(value);
        _children.Add(value);
        _chartContext.Dirty = true;
        Refresh();
    }

    /// <summary>
    /// Validates that the specified series belongs to the same chart type category as all existing child series.
    /// </summary>
    /// <param name="serie">The series to validate against the chart type categories of existing child series. Cannot be null.</param>
    /// <exception cref="InvalidOperationException">Thrown if the chart type category of the specified series does not match the category of any existing child
    /// series.</exception>
    private void ValidateSerie(SerieBase serie)
    {
        if (_children.Count == 0)
        {
            return;
        }

        var newRule = ChartTypeRuleValidator.Rules[serie.ChartType];
        var newSerie = serie.Create();
        var count = _children.Count;

        foreach (var existing in _children)
        {
            var existingSerie = existing.Create();
            var existingRule = ChartTypeRuleValidator.Rules[existing.ChartType];

            ChartTypeRuleValidator.ValidateCategory(existingRule, newRule);

            if (!serie.HasParent)
            {
                ChartTypeRuleValidator.ValidateMultipleSeries(newRule, () => count > 1);
            }

            ChartTypeRuleValidator.ValidateSameCategoryCount(newRule, () => existingSerie.RawItems.Count == newSerie.RawItems.Count);
            ChartTypeRuleValidator.ValidateSameCategoryLabels(newRule, () => existingSerie.RawItems.Select(i => i.Name).SequenceEqual(newSerie.RawItems.Select(i => i.Name)));
        }
    }

    /// <summary>
    /// Removes the specified series from the collection of child series.
    /// </summary>
    /// <param name="value">The series to remove from the collection. Cannot be null.</param>
    internal void RemoveSerie(SerieBase value)
    {
        _children.Remove(value);
        _chartContext.Dirty = true;
        Refresh();
    }

    /// <summary>
    /// Refreshes the chart by marking it as needing redraw and triggering a state change to re-render the component.
    /// </summary>
    internal void Refresh()
    {
        _needRedraw = true;
        StateHasChanged();
    }

    /// <inheritdoc /> 
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (_hasAnimationEnabledChanged ||
            _hasContrastChanged ||
            _hasDensityChanged ||
            _hasThemeOverrideChanged)
        {
            _options.AnimationEnabled = AnimationEnabled;
            _chartThemeContext = ChartThemeContextFactory.Create(
                _currentTheme!,
                ThemeOverride,
                new ChartDensity() { Mode = Density },
                new ChartContrast() { Mode = Contrast }
            );

            _hasAnimationEnabledChanged = false;
            _hasContrastChanged = false;
            _hasDensityChanged = false;
            _hasThemeOverrideChanged = false;
            Refresh();
        }
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        if (_hasPaletteStyleChanged)
        {
            _hasPaletteStyleChanged = false;
            await CreateThemeAsync();
            _rebuildPalette = true;
        }

        await base.OnParametersSetAsync();
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasAnimationEnabledChanged = parameters.TryGetValue<bool>(nameof(AnimationEnabled), out var newAnimationEnabled) && newAnimationEnabled != AnimationEnabled;
        _hasContrastChanged = parameters.TryGetValue<ChartContrastMode>(nameof(Contrast), out var newContrast) && newContrast != Contrast;
        _hasDensityChanged = parameters.TryGetValue<ChartDensityMode>(nameof(Density), out var newDensity) && newDensity != Density;
        _hasThemeOverrideChanged = parameters.TryGetValue<ChartThemeOverride>(nameof(ThemeOverride), out var newThemeOverride) && newThemeOverride != ThemeOverride;
        _hasPaletteStyleChanged = parameters.TryGetValue<ChartPaletteStyle?>(nameof(PaletteStyle), out var newPaletteStyle) && newPaletteStyle != PaletteStyle;

        return base.SetParametersAsync(parameters);
    }

    /// <summary>
    /// Invoked from JavaScript when the component's visibility state changes.
    /// </summary>
    /// <remarks>Triggers a component redraw when the component becomes visible.</remarks>
    /// <param name="visible">The visibility state of the component.</param>
    [JSInvokable]
    public void OnVisibilityChanged(bool visible)
    {
        _isVisible = visible;

        if (visible)
        {
            Refresh();
        }
    }

    /// <summary>
    /// Handles a resize event by updating the component's width and height.
    /// </summary>
    /// <remarks>This method is typically invoked from JavaScript to notify the component of size changes.
    /// After updating the dimensions, the component is marked for re-rendering.</remarks>
    /// <param name="size">The new size of the component.</param>
    [JSInvokable]
    public void OnResize(ChartSize size)
    {
        if (size.Width == _chartContext.Width &&
            size.Height == _chartContext.Height)
        {
            return;
        }

        _needRedraw = true;

        _chartContext.Width = size.Width;
        _chartContext.Height = size.Height;
        _chartContext.Dirty = true;
        Refresh();
    }

    /// <summary>
    /// Occurs when a pointer enters the chart area.
    /// </summary>
    /// <param name="pointerEvent">Event data associated with the pointer event.</param>
    [JSInvokable]
    public void OnPointerEnter(ChartPointerEvent pointerEvent)
    {
        if (_interactionEngine.PointerEnter(
            pointerEvent.GroupId,
            pointerEvent.Id))
        {
            _interactionEngine.UpdateTooltip(
                pointerEvent.GroupId,
                pointerEvent.Id,
                pointerEvent.Position,
                TooltipPosition,
                TooltipMode);

            Refresh();
        }
    }

    /// <summary>
    /// Occurs when a pointer leaves the chart area.
    /// </summary>
    [JSInvokable]
    public void OnPointerLeave()
    {
        if (_interactionEngine.PointerLeave())
        {
            _interactionEngine.HideTooltips();
            Refresh();
        }
    }

    /// <summary>
    /// Occurs when a pointer moves inside the chart area.
    /// </summary>
    /// <param name="pointerEvent">Event data associated with the pointer event.</param>
    [JSInvokable]
    public void OnPointerMove(ChartPointerEvent pointerEvent)
    {
    }

    /// <summary>
    /// Occurs when a pointer down the chart area.
    /// </summary>
    /// <param name="pointerEvent">Event data associated with the pointer event.</param>
    [JSInvokable]
    public void OnPointerDown(ChartPointerEvent pointerEvent)
    {
        if (_interactionEngine.PointerDown(
            pointerEvent.GroupId,
            pointerEvent.Id))
        {
            Refresh();
        }
    }

    /// <summary>
    /// Occurs when a pointer up the chart area.
    /// </summary>
    /// <param name="pointerEvent">Event data associated with the pointer event.</param>
    [JSInvokable]
    public void OnPointerUp(ChartPointerEvent pointerEvent)
    {
        if (_interactionEngine.PointerUp(
            pointerEvent.GroupId,
            pointerEvent.Id))
        {
            Refresh();
        }
    }

    /// <summary>
    /// Occurs when a pointer clicks the chart area.
    /// </summary>
    /// <param name="pointerEvent">Event data associated with the pointer event.</param>
    [JSInvokable]
    public void OnClick(ChartPointerEvent pointerEvent)
    {
        if (_interactionEngine.Select(
            pointerEvent.GroupId,
            pointerEvent.Id))
        {
            Refresh();
        }
    }

    /// <summary>
    /// Synchronises the live series collection with the current set of child series,
    ///  ensuring that any series that are no longer present in the child collection are
    ///  removed from the live series, and any new series are created and added to the
    ///  live series collection. This method is responsible for maintaining consistency
    ///  between the child series defined in the component and the actual series being
    ///  rendered on the chart. It performs necessary updates to reflect changes in the
    ///  child series configuration, such as additions, removals, or updates to existing series.
    /// </summary>
    private bool SyncLiveSeries()
    {
        var changed = false;

        var removedIds = _liveSeries.Keys
            .Except(_children.Select(c => c.Id))
            .ToList();

        if (removedIds.Count > 0)
        {
            changed = true;
        }

        foreach (var id in removedIds)
        {
            if (!string.IsNullOrEmpty(id))
            {
                _liveSeries.Remove(id);
            }
        }

        foreach (var child in _children)
        {
            if (string.IsNullOrEmpty(child.Id))
            {
                continue;
            }

            if (!_liveSeries.TryGetValue(child.Id, out var live))
            {
                live = child.Create();
                _liveSeries[child.Id] = live;
                changed = true;
            }
            else
            {
                UpdateLiveSerie(live, child, ref changed);
            }
        }

        return changed;
    }

    /// <summary>
    /// Synchronizes the state of a live chart series with the latest data and options from a definition series.
    /// </summary>
    /// <remarks>This method updates the items and options of the live chart series to match those of the
    /// definition. Only series of type ChartSerie{ChartItem, IChartSerieOptions} are synchronized; other types are
    /// ignored.</remarks>
    /// <param name="live">The chart series instance to update with new data and options.</param>
    /// <param name="definition">The definition series used to create a snapshot containing the latest data and options.</param>
    /// <param name="changed">A reference to a boolean flag that indicates whether any changes were made to the live series. This flag is set to true if any updates occur.</param>
    private static void UpdateLiveSerie(
        ChartSerie live,
        SerieBase definition,
        ref bool changed)
    {
        var snapshot = definition.Create();

        switch (live, snapshot)
        {
            case (ChartSerie<ChartItem> l,
                  ChartSerie<ChartItem> s):
                {
                    SyncItems(l, s, ref changed);
                }

                break;
        }
    }

    /// <summary>
    /// Synchronizes the items in the live chart series with those in the snapshot, adding, removing, or updating items
    /// as necessary to match the snapshot state.
    /// </summary>
    /// <remarks>This method updates the live series by removing items not present in the snapshot, adding new
    /// items from the snapshot, and updating existing items to reflect the properties of their counterparts in the
    /// snapshot. Item identity is determined by the Id property.</remarks>
    /// <typeparam name="TItem">The type of chart item contained in the series. Must inherit from ChartItem.</typeparam>
    /// <param name="live">The chart series to be updated so that its items match those in the snapshot.</param>
    /// <param name="snapshot">The chart series representing the desired state to synchronize with.</param>
    /// <param name="changed">A reference to a boolean flag that indicates whether any changes were made to the live series. This flag is set to true if any updates occur.</param>
    private static void SyncItems<TItem>(
        ChartSerie<TItem> live,
        ChartSerie<TItem> snapshot,
        ref bool changed)
        where TItem : ChartItem
    {
        var removedIds = live.Items.Select(i => i.Id)
            .Except(snapshot.Items.Select(i => i.Id))
            .ToList();

        if (removedIds.Count > 0)
        {
            changed = true;
        }

        foreach (var id in removedIds)
        {
            var item = live.Items.First(i => i.Id == id);
            live.RemoveItem(item);
        }

        var addedIds = snapshot.Items.Select(i => i.Id)
            .Except(live.Items.Select(i => i.Id))
            .ToList();

        if (addedIds.Count > 0)
        {
            changed = true;
        }

        foreach (var id in addedIds)
        {
            var item = snapshot.Items.First(i => i.Id == id);
            live.AddItem(item);
        }

        foreach (var liveItem in live.Items.OfType<ChartItem>())
        {
            var snapItem = snapshot.Items.OfType<ChartItem>().First(i => i.Id == liveItem.Id);

            if (!ItemsEqual(liveItem, snapItem))
            {
                changed = true;

                liveItem.Label = snapItem.Label;
                liveItem.Style = snapItem.Style;
                liveItem.Animation = snapItem.Animation;
                liveItem.Interaction = snapItem.Interaction;
            }
        }
    }

    /// <summary>
    /// Determines whether two ChartItem instances are equal by comparing their relevant properties.
    /// </summary>
    /// <remarks>This method compares the Label, Tag, Style, Tooltip, Animation, and Interaction properties of
    /// the specified ChartItem instances. It is intended for internal use when determining item equality within
    /// chart-related operations.</remarks>
    /// <param name="a">The first ChartItem to compare.</param>
    /// <param name="b">The second ChartItem to compare.</param>
    /// <returns>Returns <see langword="true" /> if all compared properties of both ChartItem instances are equal; otherwise, <see langword="false" />.</returns>
    private static bool ItemsEqual(ChartItem a, ChartItem b)
    {
        return a.Label == b.Label &&
               Equals(a.Style, b.Style) &&
               Equals(a.Tooltip, b.Tooltip) &&
               Equals(a.Animation, b.Animation) &&
               Equals(a.Interaction, b.Interaction);
    }

    /// <summary>
    /// Applies a new chart theme based on the provided request.
    /// </summary>
    /// <param name="request">The request containing the details for the new chart theme.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetThemeAsync(ChartThemeRequest request)
    {
        if (_chartThemeProvider is null)
        {
            return;
        }

        _setFromThemeAsync = true;
        _currentTheme = await _chartThemeProvider.CreateThemeAsync(request);

        _chartThemeContext = ChartThemeContextFactory.Create(
            _currentTheme,
            ThemeOverride,
            new ChartDensity() { Mode = ChartDensityMode.Normal },
            new ChartContrast() { Mode = ChartContrastMode.Normal });

        Refresh();
    }

    /// <summary>
    /// Builds a default theme request for the chart using the current options and child elements.
    /// </summary>
    /// <param name="paletteStyle">The palette style to use for the theme request.</param>
    /// <remarks>The returned theme request uses the current palette style, vision, working space, and
    /// options, as well as the number of series and categories determined by the chart's children and legend
    /// items.</remarks>
    /// <returns>A <see cref="ChartThemeRequest"/> instance representing the default theme configuration for the chart.</returns>
    private ChartThemeRequest BuildDefaultThemeRequest(ChartPaletteStyle paletteStyle)
    {
        return new ChartThemeRequest
        {
            Name = "Default",
            PaletteStyle = paletteStyle,
            Vision = ColorVisionType.Normal,
            WorkingSpace = RgbWorkingSpace.Create(ColorSpace.Icc.IccProfileName.Srgb),
            Options = IsDark ? ChartThemeOptions.Dark : ChartThemeOptions.Light
        };
    }

    /// <summary>
    /// Clears all child series from the chart and marks the chart as dirty, triggering a refresh to update the chart's visual representation.
    /// </summary>
    public void Reset()
    {
        _children.Clear();
        Invalidate();
    }

    /// <summary>
    /// Clears all live series and marks the chart as dirty, triggering a refresh to update the chart's visual representation.
    /// </summary>
    public void Invalidate()
    {
        _liveSeries.Clear();
        _chartContext.Dirty = true;
        Refresh();
    }
}
