using FluentUI.Blazor.Community.Components.Charts.Styles;

namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents the global configuration options for a chart.
/// </summary>
internal sealed class ChartOptions
{
    /// <summary>
    /// Gets the default options applied to all bar series in the chart unless overridden.
    /// </summary>
    /// <remarks>Use this property to specify common configuration settings for bar series. Individual series
    /// can override these defaults by providing their own options.</remarks>
    public BarSerieOptions DefaultBarOptions { get; set; } = new();

    /// <summary>
    /// Gets the default options applied to all column series in the chart unless overridden by specific series
    /// settings.
    /// </summary>
    /// <remarks>Use this property to configure common appearance or behavior for all column series.
    /// Individual series can override these options by specifying their own settings.</remarks>
    public ColumnSerieOptions DefaultColumnOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options used for rendering category lines.
    /// </summary>
    /// <remarks>These options are applied to category lines unless explicitly overridden. Adjusting these
    /// settings affects the appearance and behavior of all category lines that do not have their own specific
    /// options.</remarks>
    public CategoryLineOptions DefaultCategoryLineOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options applied to scatter series in the chart.
    /// </summary>
    public ScatterSerieOptions DefaultScatterOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options applied to bubble series in the chart.
    /// </summary>
    public BubbleSerieOptions DefaultBubbleOptions { get; set; } = new();

    /// <summary>
    /// Gets the default line options applied to category lines when no specific options are provided.
    /// </summary>
    public ChartLinePointStyle DefaultLineStyles { get; set; } = new();

    /// <summary>
    /// Gets the default marker line styles applied to chart markers.
    /// </summary>
    /// <remarks>Use this property to specify the baseline appearance for marker lines in the chart. These
    /// styles are applied unless overridden by specific marker settings.</remarks>
    public ChartMarkerLineStyle DefaultMarkerStyles { get; set; } = new();

    /// <summary>
    /// Gets the default axis options applied to all axes in the chart unless overridden by specific axis settings.
    /// </summary>
    public ChartAxisOptions DefaultAxisOptions { get; set; } = new()
    {
        Layer = Enums.AxesLayerOrder.Foreground
    };

    /// <summary>
    /// Gets the default grid options used for chart rendering.
    /// </summary>
    public ChartGridOptions DefaultGridOptions { get; set; } = new();

    /// <summary>
    /// Gets the default bar style applied to all bar elements in the chart unless overridden by specific series or element settings.
    /// </summary>
    public ChartBarStyle DefaultBarStyle { get; set; } = new();

    /// <summary>
    /// Gets the default style settings applied to chart columns.
    /// </summary>
    /// <remarks>Use this property to specify the appearance and formatting options that are applied to all
    /// columns in the chart unless overridden by individual column settings.</remarks>
    public ChartColumnStyle DefaultColumnStyle { get; set; } = new();

    /// <summary>
    /// Gets the default label options applied to all axis labels in the chart unless overridden by specific axis or label settings.
    /// </summary>
    public ChartAxisLabelOptions Label { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options applied to pie chart series.
    /// </summary>
    /// <remarks>Use this property to configure the appearance and behavior of pie chart series when no
    /// specific option is set for an individual series.</remarks>
    public RadialSerieOptions DefaultPieOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options applied to donut chart series.
    /// </summary>
    public RadialSerieOptions DefaultDonutOptions { get; set;  } = new();

    /// <summary>
    /// Gets or sets the default options for radar series.
    /// </summary>
    public RadarSerieOptions DefaultRadarOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the default options for radar axes in the chart.
    /// </summary>
    public RadarAxesOptions DefaultRadarAxesOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the style settings for the pie chart elements.
    /// </summary>
    /// <remarks>Use this property to customize the appearance of radial chart components, such as colors,
    /// line thickness, and marker styles. Modifying these settings allows for consistent theming and visual distinction
    /// within the chart.</remarks>
    public ChartPieStyle DefaultPieStyle { get; set; } = new();

    /// <summary>
    /// Gets or sets the style settings for the donut chart elements.
    /// </summary>
    /// <remarks>Use this property to customize the appearance of radial chart components, such as colors,
    /// line thickness, and marker styles. Modifying these settings allows for consistent theming and visual distinction
    /// within the chart.</remarks>
    public ChartPieStyle DefaultDonutStyle { get; set; } = new();

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
}
