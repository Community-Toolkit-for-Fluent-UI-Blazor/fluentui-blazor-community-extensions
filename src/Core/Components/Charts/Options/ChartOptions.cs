using FluentUI.Blazor.Community.Components.Charts.Styles;

namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents the global configuration options for a chart.
/// </summary>
internal sealed class ChartOptions
{
    /// <summary>
    /// Gets the default axis options applied to all axes in the chart unless overridden by specific axis settings.
    /// </summary>
    public ChartAxisOptions Axis { get; set; } = new()
    {
        Layer = Enums.AxesLayerOrder.Background
    };

    /// <summary>
    /// Gets the default options applied to all bar series in the chart unless overridden.
    /// </summary>
    /// <remarks>Use this property to specify common configuration settings for bar series. Individual series
    /// can override these defaults by providing their own options.</remarks>
    public BarSerieOptions Bar { get; set; } = new();

    /// <summary>
    /// Gets the default options applied to all column series in the chart unless overridden by specific series
    /// settings.
    /// </summary>
    /// <remarks>Use this property to configure common appearance or behavior for all column series.
    /// Individual series can override these options by specifying their own settings.</remarks>
    public ColumnSerieOptions Column { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options used for rendering category lines.
    /// </summary>
    /// <remarks>These options are applied to category lines unless explicitly overridden. Adjusting these
    /// settings affects the appearance and behavior of all category lines that do not have their own specific
    /// options.</remarks>
    public CategoryLineOptions CategoryLine { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options applied to scatter series in the chart.
    /// </summary>
    public ScatterSerieOptions Scatter { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options applied to bubble series in the chart.
    /// </summary>
    public BubbleSerieOptions Bubble { get; set; } = new();

    /// <summary>
    /// Gets the default line options applied to category lines when no specific options are provided.
    /// </summary>
    public ChartLinePointStyle LineStyles { get; set; } = new();

    /// <summary>
    /// Gets the default marker line styles applied to chart markers.
    /// </summary>
    /// <remarks>Use this property to specify the baseline appearance for marker lines in the chart. These
    /// styles are applied unless overridden by specific marker settings.</remarks>
    public ChartMarkerLineStyle MarkerStyles { get; set; } = new();

    /// <summary>
    /// Gets the default bar styles applied to polar bar series in the chart.
    /// </summary>
    public ChartPolarBarStyle PolarBarStyles { get; set; } = new();

    /// <summary>
    /// Gets the default grid options used for chart rendering.
    /// </summary>
    public ChartGridOptions Grid { get; set; } = new();

    /// <summary>
    /// Gets the default bar style applied to all bar elements in the chart unless overridden by specific series or element settings.
    /// </summary>
    public ChartBarStyle BarStyles { get; set; } = new();

    /// <summary>
    /// Gets the default style settings applied to chart columns.
    /// </summary>
    /// <remarks>Use this property to specify the appearance and formatting options that are applied to all
    /// columns in the chart unless overridden by individual column settings.</remarks>
    public ChartColumnStyle ColumnStyle { get; set; } = new();

    /// <summary>
    /// Gets the default label options applied to all axis labels in the chart unless overridden by specific axis or label settings.
    /// </summary>
    public ChartAxisLabelOptions Label { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options applied to pie chart series.
    /// </summary>
    /// <remarks>Use this property to configure the appearance and behavior of pie chart series when no
    /// specific option is set for an individual series.</remarks>
    public RadialSerieOptions Pie { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options applied to donut chart series.
    /// </summary>
    public RadialSerieOptions Donut { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options for radar series.
    /// </summary>
    public RadarSerieOptions Radar { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options for radar axes in the chart.
    /// </summary>
    public RadarAxesOptions RadarAxes { get; set; } = new();

    /// <summary>
    /// Gets or sets the style settings for the pie chart elements.
    /// </summary>
    /// <remarks>Use this property to customize the appearance of radial chart components, such as colors,
    /// line thickness, and marker styles. Modifying these settings allows for consistent theming and visual distinction
    /// within the chart.</remarks>
    public ChartPieStyle PieStyle { get; set; } = new();

    /// <summary>
    /// Gets or sets the style settings for the donut chart elements.
    /// </summary>
    /// <remarks>Use this property to customize the appearance of radial chart components, such as colors,
    /// line thickness, and marker styles. Modifying these settings allows for consistent theming and visual distinction
    /// within the chart.</remarks>
    public ChartPieStyle DonutStyle { get; set; } = new();

    /// <summary>
    /// Gets or sets the style settings for the semi-donut chart elements.
    /// </summary>
    /// <remarks>Use this property to customize the appearance of radial chart components, such as colors,
    /// line thickness, and marker styles. Modifying these settings allows for consistent theming and visual distinction
    /// within the chart.</remarks>
    public ChartPieStyle SemiDonutStyle { get; set; } = new();

    /// <summary>
    /// Gets or sets the style settings for the XY area chart elements.
    /// </summary>
    public ChartXYAreaStyle XYAreaStyle { get; set; } = new();

    /// <summary>
    /// Gets or sets the animation options for the chart.
    /// </summary>
    /// <remarks>Use this property to configure how chart animations behave, such as enabling or disabling
    /// animations or customizing their duration and easing. The specific options available depend on the implementation
    /// of the ChartAnimationOptions class.</remarks>
    public ChartAnimationOptions Animation { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether animations are enabled for the chart.
    /// </summary>
    public bool AnimationEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the default options for step series in the chart.
    /// </summary>
    public CategoryLineOptions Step { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options for semi-donut series in the chart.
    /// </summary>
    public RadialSerieOptions SemiDonut { get; set; } = new()
    {
        StartAngle = 190,
        EndAngle = 350
    };

    /// <summary>
    /// Gets or sets the default options for polar scatter series in the chart.
    /// </summary>
    public PolarScatterOptions PolarScatter { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options for polar bubble series in the chart.
    /// </summary>
    public PolarBubbleOptions PolarBubble { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options for histogram series in the chart.
    /// </summary>
    public HistogramOptions Histogram { get; set; } = new();

    /// <summary>
    /// Gets or sets the style configuration for the histogram chart.
    /// </summary>
    public ChartHistogramStyle HistogramStyle { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options for XY column series in the chart.
    /// </summary>
    public XYColumnSerieOptions XYColumnSerie { get; set; } = new();

    /// <summary>
    /// Gets or sets the style configuration for the XY column chart.
    /// </summary>
    public ChartXYColumnStyle XYColumnStyle { get; set; } = new();
}
