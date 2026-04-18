using System.Globalization;
using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Enums;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts;

/// <summary>
/// Represents a resolver that determines the appropriate chart axes to use based on the types of series present in the chart.
/// </summary>
internal static class ChartAxisResolver
{
    /// <summary>
    /// Represents the cosine of a 45-degree angle.
    /// </summary>
    private const double Cos45 = 0.70710678;

    /// <summary>
    /// Represents the sine of a 45-degree angle.
    /// </summary>
    private const double Sin45 = 0.70710678;

    public static void Resolve(
        CO options,
        ChartContext context,
        ChartThemeContext theme,
        IReadOnlyList<ChartSerie> series)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(series);

        var axisArea = context.RemainingSpaceArea;
        var layout = theme.Theme.Layout;
        var typo = theme.Theme.Typography;

        ResolveAxes(options, context, series, axisArea);
        GenerateNumericLabels(context);

        if (context.YAxis is not null && context.YAxis.Labels is { Count: > 0 })
        {
            ComputeYAxisLabelGeometry(context.YAxis, typo.Axis.FontSize);
        }

        if (context.XAxis is not null && context.XAxis.Labels is { Count: > 0 })
        {
            var labelCount = context.XAxis.Labels.Count;
            var availableWidthPerCategory = axisArea.Width / Math.Max(1, labelCount);
            ComputeXAxisLabelGeometry(context.XAxis, typo.Axis.FontSize, availableWidthPerCategory);
        }

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

        context.PlotArea = new ChartRect(
            axisArea.X + marginLeft + layout.PlotPaddingLeft,
            axisArea.Y + layout.PlotPaddingTop,
            Math.Max(0, axisArea.Width - marginLeft - layout.PlotPaddingLeft - layout.PlotPaddingRight),
            Math.Max(0, axisArea.Height - marginBottom - layout.PlotPaddingTop - layout.PlotPaddingBottom),
            axisArea);

        FixAxisMaps(context);
    }

    private static void FixAxisMaps(ChartContext ctx)
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
        else if (types.TrueForAll(x => x == ChartType.Column))
        {
            var (xAxis, yAxis) = ChartAxesFactory.Column.CreateAxes(
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
        if (context.XAxis is { AxisType: ChartAxisType.Numeric })
        {
            context.XAxis.Labels ??= GenerateTicks(context.XAxis.Minimum, context.XAxis.Maximum);
        }

        if (context.YAxis is { AxisType: ChartAxisType.Numeric })
        {
            context.YAxis.Labels ??= GenerateTicks(context.YAxis.Minimum, context.YAxis.Maximum);
        }
    }

    /// <summary>
    /// Generates a sequence of evenly spaced tick label strings between the specified minimum and maximum values.
    /// </summary>
    /// <remarks>The method always generates five tick labels unless the range is invalid or degenerate. The
    /// labels are formatted using the invariant culture and up to three significant digits.</remarks>
    /// <param name="min">The minimum value of the range for which to generate tick labels.</param>
    /// <param name="max">The maximum value of the range for which to generate tick labels.</param>
    /// <returns>A read-only list of strings representing evenly spaced tick labels formatted with up to three significant
    /// digits. If the range is invalid or the minimum and maximum are equal, the list contains only the minimum value.</returns>
    private static List<string> GenerateTicks(double min, double max)
    {
        const int tickCount = 5;
        var list = new List<string>();

        if (tickCount <= 1 ||
            double.IsNaN(min) ||
            double.IsNaN(max) ||
            min == max)
        {
            list.Add(min.ToString("G3", CultureInfo.InvariantCulture));

            return list;
        }

        for (var i = 0; i < tickCount; i++)
        {
            var t = (double)i / (tickCount - 1);
            var v = min + t * (max - min);

            list.Add(v.ToString("G3", CultureInfo.InvariantCulture));
        }

        return list;
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

        var (w0, h0) = MeasureMaxLabel(fontSize, axis.Labels, 0);

        var candidates = new List<(double angle, double width, double height)>
        {
            (0, w0, h0),
            (-45, MeasureMaxLabel(fontSize, axis.Labels, -45).width,
                  MeasureMaxLabel(fontSize, axis.Labels, -45).height),
            (-90, MeasureMaxLabel(fontSize, axis.Labels, -90).width,
                   MeasureMaxLabel(fontSize, axis.Labels, -90).height)
        };

        var valid = candidates
            .Where(c => c.width <= availableWidthPerCategory)
            .ToList();

        (double angle, double width, double height) chosen;

        if (valid.Count > 0)
        {
            chosen = valid.OrderBy(c => c.height).First();
        }
        else
        {
            chosen = candidates.OrderBy(c => c.height).First();
        }

        axis.LabelRotation = chosen.angle;
        axis.LabelWidth = chosen.width;
        axis.LabelHeight = chosen.height;
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

        var candidates = new List<(double angle, double width, double height)>
        {
            (0,   MeasureMaxLabel(fontSize, axis.Labels, 0).width,
                  MeasureMaxLabel(fontSize, axis.Labels, 0).height),
            (-45, MeasureMaxLabel(fontSize, axis.Labels, -45).width,
                  MeasureMaxLabel(fontSize, axis.Labels, -45).height),
            (-90, MeasureMaxLabel(fontSize, axis.Labels, -90).width,
                  MeasureMaxLabel(fontSize, axis.Labels, -90).height)
        };

        var (angle, width, height) = candidates.OrderBy(c => c.width).First();

        axis.LabelRotation = angle;
        axis.LabelWidth = width;
        axis.LabelHeight = height;
    }

    /// <summary>
    /// Calculates the maximum width and height required to display a set of text labels using a specified font size and
    /// rotation angle.
    /// </summary>
    /// <remarks>This method uses a typographical approximation to estimate label dimensions and accounts for
    /// rotation at 0, -45, and -90 degrees. The result can be used to determine layout constraints for rendering text
    /// labels in graphical components.</remarks>
    /// <param name="fontSize">The font size, in device-independent units (DIPs), to use when measuring the labels. Must be greater than zero.</param>
    /// <param name="labels">A read-only list of strings representing the labels to measure. Cannot be null or empty.</param>
    /// <param name="angle">The rotation angle, in degrees, to apply to each label when measuring. Supported values are 0, -45, and -90.</param>
    /// <returns>A tuple containing the maximum width and height, in device-independent units, required to display the labels
    /// with the specified font size and rotation angle. Returns (0, 0) if the labels list is empty.</returns>
    private static (double width, double height) MeasureMaxLabel(
        double fontSize,
        IReadOnlyList<string> labels,
        double angle)
    {
        if (labels.Count == 0)
        {
            return (0, 0);
        }

        var maxW = 0.0;
        var maxH = 0.0;

        foreach (var label in labels)
        {
            var w = fontSize * 0.6 * label.Length;
            var h = fontSize * 1.2;

            (var rw, var rh) = angle switch
            {
                0 => (w, h),
                -45 => Rotate(w, h, Cos45, Sin45),
                -90 => (h, w),
                _ => (w, h)
            };

            if (rw > maxW)
            {
                maxW = rw;
            }

            if (rh > maxH)
            {
                maxH = rh;
            }
        }

        return (maxW, maxH);
    }

    /// <summary>
    /// Calculates the width and height of a rectangle after rotation by a specified angle, using the provided cosine
    /// and sine values.
    /// </summary>
    /// <remarks>This method assumes the rotation is performed around the origin and that the provided cosine
    /// and sine values correspond to the same angle.</remarks>
    /// <param name="w">The original width of the rectangle.</param>
    /// <param name="h">The original height of the rectangle.</param>
    /// <param name="cos">The cosine of the rotation angle.</param>
    /// <param name="sin">The sine of the rotation angle.</param>
    /// <returns>A tuple containing the width and height of the rectangle after rotation.</returns>
    private static (double width, double height) Rotate(
        double w,
        double h,
        double cos,
        double sin)
    {
        var rw = w * cos + h * sin;
        var rh = w * sin + h * cos;

        return (rw, rh);
    }
}
