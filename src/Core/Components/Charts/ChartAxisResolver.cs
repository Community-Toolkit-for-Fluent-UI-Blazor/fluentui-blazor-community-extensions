using System.Globalization;
using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Helpers;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Charts.Utils;
using FluentUI.Blazor.Community.Components.Enums;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts;

/// <summary>
/// Represents a resolver that determines the appropriate chart axes to use based on the types of series present in the chart.
/// </summary>
internal static class ChartAxisResolver
{
    public static void Resolve(
       CO options,
       ChartContext context,
       ChartThemeContext theme,
       IReadOnlyList<ChartSerie> series)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(series);

        var layout = theme.Theme.Layout;
        var typo = theme.Theme.Typography;
        var axisArea = context.RemainingSpaceArea;

        ResolveAxes(options, context, series, axisArea);
        GenerateNumericLabels(context);
        ComputeAxisLabelGeometry(context, typo, axisArea);
        ComputeAxisMargins(context, layout);
        ComputePlotArea(context, axisArea, layout);
        FixMaps(context);
    }

    private static void ComputeAxisLabelGeometry(
        ChartContext context,
        ChartTypography typo,
        ChartRect axisArea)
    {
        if (context.YAxis is { Labels.Count: > 0 })
        {
            ComputeYAxisLabelGeometry(context.YAxis, typo.Axis.FontSize);
        }

        if (context.XAxis is { Labels.Count: > 0 })
        {
            var labelCount = context.XAxis.Labels.Count;
            var availableWidth = axisArea.Width / Math.Max(1, labelCount);
            ComputeXAxisLabelGeometry(context.XAxis, typo.Axis.FontSize, availableWidth);
        }
    }

    private static void ComputeAxisMargins(ChartContext context, ChartLayout layout)
    {
        var marginLeft =
            (context.YAxis?.LabelWidth ?? 0) +
            layout.TickLength +
            layout.AxisThickness +
            layout.AxisSpacing;

        var marginBottom =
            (context.XAxis?.LabelHeight ?? 0) +
            layout.TickLength +
            layout.AxisThickness +
            layout.AxisSpacing;

        context.AxesMargins = new Thickness(
            marginLeft,
            0,
            0,
            marginBottom);
    }

    private static void ComputePlotArea(
    ChartContext context,
    ChartRect axisArea,
    ChartLayout layout)
    {
        context.PlotArea = new ChartRect(
            axisArea.X + context.AxesMargins.Left + layout.PlotPaddingLeft,
            axisArea.Y + layout.PlotPaddingTop,
            Math.Max(0, axisArea.Width - context.AxesMargins.Left - layout.PlotPaddingLeft - layout.PlotPaddingRight),
            Math.Max(0, axisArea.Height - context.AxesMargins.Bottom - layout.PlotPaddingTop - layout.PlotPaddingBottom),
            axisArea);
    }

    private static void FixMaps(ChartContext ctx)
    {
        var plot = ctx.PlotArea;

        if (ctx.XAxis is { AxisType: ChartAxisType.Category } xcat)
        {
            var min = xcat.Minimum;
            var max = xcat.Maximum;

            xcat.Map = i =>
            {
                var t = (i - min) / (double)(max - min);

                return plot.X + t * plot.Width;
            };
        }

        if (ctx.XAxis is { AxisType: ChartAxisType.Numeric } xnum)
        {
            var min = xnum.Minimum;
            var max = xnum.Maximum;

            xnum.Map = v =>
            {
                var t = (v - min) / (max - min);
                return plot.X + t * plot.Width;
            };
        }

        if (ctx.YAxis is { AxisType: ChartAxisType.Category } ycat)
        {
            var min = ycat.Minimum;
            var max = ycat.Maximum;

            ycat.Map = i =>
            {
                var t = (i - min) / (double)(max - min);
                return plot.Y + t * plot.Height;
            };
        }

        if (ctx.YAxis is { AxisType: ChartAxisType.Numeric } ynum)
        {
            var min = ynum.Minimum;
            var max = ynum.Maximum;

            ynum.Map = v =>
            {
                var t = (v - min) / (max - min);
                return plot.Y + plot.Height - t * plot.Height;
            };
        }
    }

    /// <summary>
    /// Configures the chart axes in the specified chart context based on the provided chart series and options.
    /// </summary>
    /// <remarks>This method sets the X and Y axes of the chart context according to the chart types present
    /// in the series. Only homogeneous chart types or compatible categories are supported. For polar or hierarchical
    /// chart types, axes are set to null.</remarks>
    /// <param name="options">The chart options used to determine axis configuration, including sorting and default behaviors for different
    /// chart types.</param>
    /// <param name="context">The chart context in which axes will be resolved and set. Cannot be null.</param>
    /// <param name="series">The collection of chart series that defines the chart types and data for axis resolution. Cannot be null.</param>
    /// <param name="axisArea">The area available for axes, used to calculate appropriate axis configurations based on the chart layout.</param>
    private static void ResolveAxes(
        CO options,
        ChartContext context,
        IReadOnlyList<ChartSerie> series,
        ChartRect axisArea)
    {
        var types = series.Select(s => s.ChartType).ToList();

        if (types.TrueForAll(x => x == ChartType.Bar))
        {
            var (xAxis, yAxis) = ChartAxesFactory.Bar.CreateAxes(
                options.DefaultBarOptions.Sort, series, axisArea);
            context.XAxis = xAxis;
            context.YAxis = yAxis;
        }
        else  if (types.TrueForAll(x => x == ChartType.StackedBar))
        {
            var (xAxis, yAxis) = ChartAxesFactory.StackedBar.CreateAxes(
                options.DefaultBarOptions.Sort, series, axisArea);
            context.XAxis = xAxis;
            context.YAxis = yAxis;
        }
        else if (types.TrueForAll(x => x == ChartType.Column))
        {
            var (xAxis, yAxis) = ChartAxesFactory.Column.CreateAxes(
                options.DefaultColumnOptions.Sort, series, axisArea);
            context.XAxis = xAxis;
            context.YAxis = yAxis;
        }
        else if (types.TrueForAll(x => x == ChartType.StackedColumn))
        {
            var (xAxis, yAxis) = ChartAxesFactory.StackedColumn.CreateAxes(
                options.DefaultColumnOptions.Sort, series, axisArea);
            context.XAxis = xAxis;
            context.YAxis = yAxis;
        }
        else if (types.TrueForAll(x => x == ChartType.CategoryLine))
        {
            var (xAxis, yAxis) = ChartAxesFactory.CategoryLine.CreateAxes(
                options.DefaultCategoryLineOptions.Sort, series, axisArea);
            context.XAxis = xAxis;
            context.YAxis = yAxis;
        }
        else if (types.TrueForAll(x => x == ChartType.CategoryArea))
        {
            var (xAxis, yAxis) = ChartAxesFactory.CategoryLine.CreateAxes(
                options.DefaultCategoryLineOptions.Sort, series, axisArea);
            context.XAxis = xAxis;
            context.YAxis = yAxis;
        }
        else if (types.All(t => ChartTypeInfo.Categories[t] == ChartCategory.Category))
        {
            ChartAxesBuilder.BuildCategoryAxes(context, series);
        }
        else if (types.All(t => ChartTypeInfo.Categories[t] == ChartCategory.XY))
        {
            // XY (scatter, bubble) à implémenter plus tard
        }
        else if (types.All(t => ChartTypeInfo.Categories[t] is ChartCategory.Polar or ChartCategory.Hierarchy))
        {
            context.XAxis = null;
            context.YAxis = null;
        }
        else
        {
            throw new NotSupportedException("Mixed chart types are not supported.");
        }
    }

    /// <summary>
    /// Generates and assigns numeric axis labels for the X and Y axes of the specified chart context if they are
    /// configured as numeric axes and do not already have labels defined.
    /// </summary>
    /// <remarks>This method only generates labels for axes of type numeric and does not overwrite existing
    /// labels. It is intended to ensure that numeric axes have appropriate tick labels based on their minimum and
    /// maximum values.</remarks>
    /// <param name="context">The chart context containing axis information for which numeric labels will be generated and assigned.</param>
    private static void GenerateNumericLabels(ChartContext context)
    {
        if (context.XAxis is { AxisType: ChartAxisType.Numeric } xnum)
        {
            if (xnum.Labels is null)
            {
                var ticks = NumericTickGenerator.GenerateNice(xnum.DataMinimum, xnum.DataMaximum, maxTicks: 6);

                if (ticks.Count > 0)
                {
                    xnum.Minimum = ticks[0];
                    xnum.Maximum = ticks[^1];
                    xnum.Labels = [.. ticks.Select(v => v.ToString("G3", CultureInfo.InvariantCulture))];
                }
            }
        }

        if (context.YAxis is { AxisType: ChartAxisType.Numeric } ynum)
        {
            if (ynum.Labels is null)
            {
                var ticks = NumericTickGenerator.GenerateNice(ynum.DataMinimum, ynum.DataMaximum, maxTicks: 6);

                if (ticks.Count > 0)
                {
                    ynum.Minimum = ticks[0];
                    ynum.Maximum = ticks[^1];
                    ynum.Labels = [.. ticks.Select(v => v.ToString("G3", CultureInfo.InvariantCulture))];
                }
            }
        }
    }

    /// <summary>
    /// Calculates and sets the optimal label rotation, width, and height for the X-axis labels based on the available
    /// width per category and the specified font size.
    /// </summary>
    /// <remarks>This method updates the axis's label rotation, width, and height properties to ensure that
    /// labels fit within the available space. If no labels are present, the label geometry is reset to zero.</remarks>
    /// <param name="axis">The chart axis whose label geometry will be computed and updated. Cannot be null.</param>
    /// <param name="fontSize">The font size, in device-independent units, to use when measuring the axis labels. Must be greater than zero.</param>
    /// <param name="availableWidthPerCategory">The maximum width available for each category label on the X-axis, in device-independent units. Must be greater
    /// than zero.</param>
    private static void ComputeXAxisLabelGeometry(
    ChartAxis axis,
    double fontSize,
    double availableWidthPerCategory)
    {
        if (axis.Labels is null || axis.Labels.Count == 0)
        {
            axis.LabelRotation = 0;
            axis.LabelWidth = 0;
            axis.LabelHeight = 0;
            return;
        }

        var m0 = LabelMeasurer.MeasureMax(axis.Labels, fontSize, 0);
        var m45 = LabelMeasurer.MeasureMax(axis.Labels, fontSize, -45);
        var m90 = LabelMeasurer.MeasureMax(axis.Labels, fontSize, -90);

        var candidates = new (double angle, double w, double h)[]
        {
            (  0, m0.width,  m0.height),
            (-45, m45.width, m45.height),
            (-90, m90.width, m90.height)
        };

        (double angle, double w, double h)? best = null;

        foreach (var c in candidates)
        {
            if (c.w <= availableWidthPerCategory)
            {
                if (best == null || c.h < best.Value.h)
                {
                    best = c;
                }
            }
        }

        if (best == null)
        {
            best = candidates[0];

            foreach (var c in candidates)
            {
                if (c.h < best.Value.h)
                {
                    best = c;
                }
            }
        }

        axis.LabelRotation = best.Value.angle;
        axis.LabelWidth = best.Value.w;
        axis.LabelHeight = best.Value.h;
    }

    /// <summary>
    /// Calculates and sets the optimal rotation angle, width, and height for Y-axis labels based on the provided font
    /// size and label content.
    /// </summary>
    /// <remarks>This method evaluates multiple rotation angles to determine the most space-efficient
    /// orientation for Y-axis labels. It updates the axis with the chosen label rotation, width, and height to ensure
    /// proper layout and readability.</remarks>
    /// <param name="axis">The Y-axis whose label geometry will be computed and updated. Must not be null, and should have a non-null
    /// collection of labels.</param>
    /// <param name="fontSize">The font size, in device-independent units, to use when measuring the axis labels. Must be greater than zero.</param>
    private static void ComputeYAxisLabelGeometry(
    ChartAxis axis,
    double fontSize)
    {
        if (axis.Labels is null || axis.Labels.Count == 0)
        {
            axis.LabelRotation = 0;
            axis.LabelWidth = 0;
            axis.LabelHeight = 0;
            return;
        }

        var m0 = LabelMeasurer.MeasureMax(axis.Labels, fontSize, 0);
        var m45 = LabelMeasurer.MeasureMax(axis.Labels, fontSize, -45);
        var m90 = LabelMeasurer.MeasureMax(axis.Labels, fontSize, -90);

        var candidates = new (double angle, double w, double h)[]
        {
            (  0, m0.width,  m0.height),
            (-45, m45.width, m45.height),
            (-90, m90.width, m90.height)
        };

        var best = candidates[0];

        foreach (var c in candidates)
        {
            if (c.w < best.w)
            {
                best = c;
            }
        }

        axis.LabelRotation = best.angle;
        axis.LabelWidth = best.w;
        axis.LabelHeight = best.h;
    }
}
