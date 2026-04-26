using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options that apply to chart components, allowing customization of series appearance and
/// behavior.
/// </summary>
/// <remarks>Use this class to specify default options for all bar series in a chart. Individual series can
/// override these options by providing their own settings. This class is typically used as a parameter in chart
/// components to centralize common configuration.</remarks>
public partial class ChartOptions : ComponentBase
{
    /// <summary>
    /// Stores the configuration options for a column series in a chart.
    /// </summary>
    private Charts.Options.ColumnSerieOptions? _columnSerieOptions;

    /// <summary>
    /// Stores the configuration options for a bar series in a chart.
    /// </summary>
    private Charts.Options.BarSerieOptions? _barSerieOptions;

    /// <summary>
    /// Stores the configuration options for a category line series in a chart.
    /// </summary>
    private Charts.Options.CategoryLineOptions? _categoryLineOptions;

    /// <summary>
    /// Represents the chart axis options used to configure the appearance and behavior of chart axes.
    /// </summary>
    private Charts.Options.ChartAxisOptions? _axisOptions;

    /// <summary>
    /// Represents the radar chart axis options used to configure the appearance and behavior of radar chart axes.
    /// </summary>
    private Charts.Options.RadarAxesOptions? _radarAxesOptions;

    /// <summary>
    /// Represents the radar series options used to configure the appearance and behavior of radar chart series.
    /// </summary>
    private Charts.Options.RadarSerieOptions? _radarSerieOptions;

    /// <summary>
    /// Represents the semi-donut series options used to configure the appearance and behavior of semi-donut chart series.
    /// </summary>
    private Charts.Options.RadialSerieOptions? _semiDonutSerieOptions;

    /// <summary>
    /// Represents the chart bar options associated with this chart component.
    /// </summary>
    private ChartBarOptions? _chartBarOptions;

    /// <summary>
    /// Represents the chart column options associated with this chart component.
    /// </summary>
    private ChartColumnOptions? _chartColumnOptions;

    /// <summary>
    /// Represents the options for configuring category lines in the chart.
    /// </summary>
    private ChartCategoryLineOptions? _chartCategoryLineOptions;

    /// <summary>
    /// Represents the current axis options for the chart, or null if no options are set.
    /// </summary>
    private ChartAxisOptions? _chartAxisOptions;

    /// <summary>
    /// Represents the axis options for the radar chart, or null if no options are set.
    /// </summary>
    private RadarChartAxesOptions? _radarChartAxesOptions;

    /// <summary>
    /// Represents the options for the radar serie, or null if no options are set.
    /// </summary>
    private RadarSerieOptions? _radarChartSerieOptions;

    /// <summary>
    /// Represents the options for the semi-donut serie, or null if no options are set.
    /// </summary>
    private SemiDonutSerieOptions? _semiDonutChartSerieOptions;

    /// <summary>
    /// Gets or sets the content to be rendered inside this component.
    /// </summary>
    /// <remarks>Use this parameter to specify the child elements or markup that will be displayed within the
    /// component. Typically, this is set using Razor syntax between the component's opening and closing tags.</remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the parent chart component in the cascading parameter hierarchy.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a parent FluentCxChart. It enables child components to access shared chart context and configuration
    /// from their parent chart.</remarks>
    [CascadingParameter]
    private FluentCxChart? Parent { get; set; }

    /// <summary>
    /// Sets the chart bar options to the specified value.
    /// </summary>
    /// <param name="value">The <see cref="ChartBarOptions"/> instance to assign. Cannot be null.</param>
    internal void AddBarOptions(ChartBarOptions value)
    {
        _chartBarOptions = value;
    }

    /// <summary>
    /// Notify that the bar options have changed.
    /// </summary>
    internal void NotifyBarOptionsChanged()
    {
        if (_chartBarOptions is null)
        {
            return;
        }

        _barSerieOptions = new Charts.Options.BarSerieOptions
        {
            BarHeight = _chartBarOptions.BarHeight,
            ShowLabels = _chartBarOptions.ShowLabels,
            ShowValues = _chartBarOptions.ShowValues,
            Sort = _chartBarOptions.Sort
        };

        Parent!.UpdateBarOptions(_barSerieOptions);
    }

    /// <summary>
    /// Removes options from the current instance.
    /// </summary>
    /// <remarks>Call this method to reset or clear any previously set chart bar options. After calling this
    /// method, the chart will have no options configured until new options are set.</remarks>
    internal void RemoveBarOptions()
    {
        _chartBarOptions = null;
    }

    /// <summary>
    /// Sets the column options for the chart configuration.
    /// </summary>
    /// <param name="value">The <see cref="ChartColumnOptions"/> instance containing the configuration settings to apply to the chart
    /// columns.</param>
    internal void AddColumnOptions(ChartColumnOptions value)
    {
        _chartColumnOptions = value;
    }

    /// <summary>
    /// Notify that the columns options have changed.
    /// </summary>
    internal void NotifyColumnOptionsChanged()
    {
        if (_chartColumnOptions is null)
        {
            return;
        }

        _columnSerieOptions = new Charts.Options.ColumnSerieOptions
        {
            ColumnWidth = _chartColumnOptions.ColumnWidth,
            ShowLabels = _chartColumnOptions.ShowLabels,
            ShowValues = _chartColumnOptions.ShowValues,
            Sort = _chartColumnOptions.Sort
        };

        Parent!.UpdateColumnOptions(_columnSerieOptions);
    }

    /// <summary>
    /// Removes the current chart column options and resets the related configuration to its default state.
    /// </summary>
    internal void RemoveColumnOptions()
    {
        _chartColumnOptions = null;
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(ChartOptions)} must be used within a {nameof(FluentCxChart)} component.");
        }
    }

    /// <summary>
    /// Adds the specified category line options to the chart configuration, allowing customization of category line appearance and behavior.
    /// </summary>
    /// <param name="chartCategoryLineOptions">The <see cref="ChartCategoryLineOptions"/> instance containing the configuration settings to apply to the chart category lines.</param>
    internal void AddCategoryLineOptions(ChartCategoryLineOptions chartCategoryLineOptions)
    {
        _chartCategoryLineOptions = chartCategoryLineOptions;
    }

    /// <summary>
    /// Notifies the parent component that the category line options have changed and updates the parent with the new
    /// options.
    /// </summary>
    /// <remarks>This method should be called whenever the category line options are modified to ensure that
    /// the parent component reflects the latest configuration. The method does nothing if the current chart category
    /// line options are not set.</remarks>
    internal void NotifyCategoryLineOptionsChanged()
    {
        if (_chartCategoryLineOptions is null)
        {
            return;
        }

        _categoryLineOptions = new Charts.Options.CategoryLineOptions
        {
            ShowLabels = _chartCategoryLineOptions.ShowLabels,
            ShowValues = _chartCategoryLineOptions.ShowValues,
            Sort = _chartCategoryLineOptions.Sort,
            Smooth = _chartCategoryLineOptions.Smooth,
            ShowMarkers = _chartCategoryLineOptions.ShowMarkers,
            MarkerSize = _chartCategoryLineOptions.MarkerSize,
        };

        Parent!.UpdateCategoryLineOptions(_categoryLineOptions);
    }

    /// <summary>
    /// Removes the current category line options from the chart.
    /// </summary>
    /// <remarks>Call this method to clear any previously set category line options. After calling this
    /// method, the chart will no longer display category lines until new options are set.</remarks>
    internal void RemoveCategoryLineOptions()
    {
        _chartCategoryLineOptions = null;
    }

    /// <summary>
    /// Sets the column options for the chart configuration.
    /// </summary>
    /// <param name="value">The <see cref="ChartAxisOptions"/> instance containing the configuration settings to apply to the chart
    /// axes.</param>
    internal void AddChartAxisOptions(ChartAxisOptions value)
    {
        _chartAxisOptions = value;
    }

    /// <summary>
    /// Notify that the chart axis options have changed.
    /// </summary>
    internal void NotifyChartAxisOptionsChanged()
    {
        if (_chartAxisOptions is null)
        {
            return;
        }

        _axisOptions = new Charts.Options.ChartAxisOptions
        {
            Color = _chartAxisOptions.Color,
            ShowTicks = _chartAxisOptions.ShowTicks,
            DashArray = _chartAxisOptions.DashArray,
            ShowLabels = _chartAxisOptions.ShowLabels,
            LabelOffset = _chartAxisOptions.LabelOffset,
            LabelOptions = _chartAxisOptions.LabelOptions,
            Layer = _chartAxisOptions.Layer,
            MaxTicks = _chartAxisOptions.MaxTicks,
            NumericFormat = _chartAxisOptions.NumericFormat,
            TickLength = _chartAxisOptions.TickLength,
            Opacity = _chartAxisOptions.Opacity,
            Show = _chartAxisOptions.Show,
            StrokeWidth = _chartAxisOptions.StrokeWidth
        };

        Parent!.UpdateAxisOptions(_axisOptions);
    }

    /// <summary>
    /// Removes the current chart axis options and resets the related configuration to its default state.
    /// </summary>
    internal void RemoveChartAxisOptions()
    {
        _chartAxisOptions = null;
    }

    internal void AddRadarChartAxesOptions(RadarChartAxesOptions radarChartAxesOptions)
    {
        _radarChartAxesOptions = radarChartAxesOptions;
    }

    internal void NotifyRadarChartAxesOptionsChanged()
    {
        if (_radarChartAxesOptions is null)
        {
            return;
        }

        _radarAxesOptions = new Charts.Options.RadarAxesOptions
        {
            Grid = _radarChartAxesOptions.Grid,
            GridLevels = _radarChartAxesOptions.Levels
        };

        Parent!.UpdateRadarChartAxesOptions(_radarAxesOptions);
    }

    internal void RemoveRadarChartAxesOptions()
    {
        _radarChartAxesOptions = null;
    }

    internal void AddRadarSerieOptions(RadarSerieOptions radarSerieOptions)
    {
        _radarChartSerieOptions = radarSerieOptions;
    }

    internal void RemoveRadarSerieOptions()
    {
        _radarChartSerieOptions = null;
    }

    internal void NotifyRadarSerieOptionsChanged()
    {
        if (_radarChartSerieOptions is null)
        {
            return;
        }

        _radarSerieOptions = new Charts.Options.RadarSerieOptions
        {
            FillArea = _radarChartSerieOptions.FillArea
        };

        Parent!.UpdateRadarSerieOptions(_radarSerieOptions);
    }

    internal void RemoveSemiDonutSerieOptions()
    {
        _semiDonutChartSerieOptions = null;
    }

    internal void AddSemiDonutSerieOptions(SemiDonutSerieOptions semiDonutSerieOptions)
    {
        _semiDonutChartSerieOptions = semiDonutSerieOptions;
    }

    internal void NotifySemiDonutSerieOptionsChanged()
    {
        if (_semiDonutChartSerieOptions is null)
        {
            return;
        }

        _semiDonutSerieOptions = new Charts.Options.RadialSerieOptions
        {
            StartAngle = _semiDonutChartSerieOptions.StartAngle,
            EndAngle = _semiDonutChartSerieOptions.EndAngle
        };

        Parent!.UpdateSemiDonutSerieOptions(_semiDonutSerieOptions);
    }

    /*    [Parameter]
        public ChartLineStyle? LineStyles { get; set; }
       [Parameter]
       public ChartMarkerLineStyle? MarkerStyles { get; set; }
        
        [Parameter]
        public ChartGridOptions? GridOptions { get; set; }
        [Parameter]
        public ChartBarStyle? BarStyle { get; set; }
        [Parameter]
        public ChartColumnStyle? ColumnStyle { get; set; }
*/
}
