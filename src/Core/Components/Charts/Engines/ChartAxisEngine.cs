using System.Globalization;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Helpers;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Surface.Engines;
using FluentUI.Blazor.Community.Components.Surface.Payloads;
using CAO = FluentUI.Blazor.Community.Components.Charts.Options.ChartAxisOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Represents an engine responsible for building the axes payload for a chart based on the provided chart context.
/// </summary>
internal sealed class ChartAxisEngine
    : IAxisEngine<ChartContext, CAO>
{
    /// <summary>
    /// Represents the culture information used for formatting axis labels and other culture-sensitive data.
    /// </summary>
    private static readonly CultureInfo s_culture = CultureInfo.InvariantCulture;

    /// <inheritdoc />
    public AxisPayload? Build(
        ChartContext view,
        CAO options)
    {
        if (!options.Show ||
            view.XAxis == null ||
            view.YAxis == null)
        {
            return null;
        }

        var plot = view.PlotArea;
        var xAxis = BuildAxisGeometry(view, view.XAxis, plot, isXAxis: true, options);
        var yAxis = BuildAxisGeometry(view, view.YAxis, plot, isXAxis: false, options);

        return new AxisPayload
        {
            XAxis = xAxis,
            YAxis = yAxis,
            Color = options.Color,
            Opacity = options.Opacity,
            StrokeWidth = options.StrokeWidth,
            DashArray = SurfaceMathUtils.ToDashArray(options.DashArray),
            Layer = options.Layer
        };
    }

    /// <summary>
    /// Builds the geometry payload for a chart axis, including axis line, ticks, and labels, based on the specified
    /// options.
    /// </summary>
    /// <param name="context">The chart context containing information about the chart area and plot area, which may be used to calculate label positions and rotations.</param>
    /// <param name="axis">The axis definition containing scale and configuration information for the chart axis.</param>
    /// <param name="plot">The plot area that defines the bounds and layout for rendering the axis.</param>
    /// <param name="isXAxis">true to build geometry for the X axis; otherwise, false to build geometry for the Y axis.</param>
    /// <param name="options">The options that specify whether to display ticks and labels, and other axis rendering preferences.</param>
    /// <returns>An AxisGeometryPayload containing the computed start and end points, ticks, and labels for the axis.</returns>
    private static AxisGeometryPayload BuildAxisGeometry(
        ChartContext context,
        ChartAxis axis,
        ChartRect plot,
        bool isXAxis,
        CAO options)
    {
        var (start, end) = isXAxis
            ? BuildXAxisLine(plot)
            : BuildYAxisLine(plot);

        var ticks = options.ShowTicks
            ? BuildTicks(axis, plot, isXAxis, options.MaxTicks)
            : [];

        var labels = options.ShowLabels
            ? BuildLabels(context, axis, isXAxis, options, ticks)
            : [];

        return new AxisGeometryPayload
        {
            StartPoint = start,
            EndPoint = end,
            Ticks = ticks,
            Labels = labels
        };
    }

    /// <summary>
    /// Calculates the start and end points of the X-axis line for the specified plot area.
    /// </summary>
    /// <param name="plot">The plot area for which to compute the X-axis line coordinates.</param>
    /// <returns>A tuple containing the start and end points of the X-axis line as ChartPoint values.</returns>
    private static (ChartPoint Start, ChartPoint End) BuildXAxisLine(ChartRect plot)
    {
        var y = plot.Y + plot.Height;

        return (new ChartPoint(plot.X, y), new ChartPoint(plot.X + plot.Width, y));
    }

    /// <summary>
    /// Builds the start and end points for a vertical Y-axis line within the specified plot area.
    /// </summary>
    /// <param name="plot">The plot area that defines the bounds and coordinates for the Y-axis line.</param>
    /// <returns>A tuple containing the start and end points of the Y-axis line as ChartPoint instances.</returns>
    private static (ChartPoint Start, ChartPoint End) BuildYAxisLine(ChartRect plot)
    {
        var x = plot.X;

        return (new ChartPoint(x, plot.Y), new ChartPoint(x, plot.Y + plot.Height));
    }

    /// <summary>
    /// Builds the collection of tick points for the specified chart axis based on its type.
    /// </summary>
    /// <param name="axis">The chart axis for which to generate tick points. The axis type determines the tick generation strategy.</param>
    /// <param name="plot">The plot area associated with the chart, used to calculate tick positions.</param>
    /// <param name="isXAxis">true to build ticks for the X axis; otherwise, false to build ticks for the Y axis.</param>
    /// <param name="maxTicks">The maximum number of ticks to generate for numeric axes. This parameter is ignored for category axes.</param>
    /// <returns>A list of ChartPoint objects representing the calculated tick positions for the axis. Returns an empty
    /// list if the axis type is Time or unrecognized.</returns>
    private static List<ChartPoint> BuildTicks(
        ChartAxis axis,
        ChartRect plot,
        bool isXAxis,
        int maxTicks)
    {
        return axis.AxisType switch
        {
            ChartAxisType.Category => BuildCategoryTicks(axis, plot, isXAxis),
            ChartAxisType.Numeric => BuildNumericTicks(axis, plot, isXAxis, maxTicks),
            ChartAxisType.Time => [],
            _ => []
        };
    }

    /// <summary>
    /// Builds a collection of tick points for a category axis based on the specified axis orientation and plot area.
    /// </summary>
    /// <remarks>The number of ticks generated is determined by the range between the axis's Minimum and
    /// Maximum values, inclusive. The orientation of the ticks depends on whether the axis is horizontal or
    /// vertical.</remarks>
    /// <param name="axis">The axis for which to generate tick points. The axis's Minimum and Maximum values determine the number of ticks.</param>
    /// <param name="plot">The plot area that defines the coordinate space for the ticks.</param>
    /// <param name="isXAxis">true to generate ticks for the X axis; false to generate ticks for the Y axis.</param>
    /// <returns>A read-only list of ChartPoint objects representing the positions of the category ticks along the specified
    /// axis.</returns>
    private static List<ChartPoint> BuildCategoryTicks(
        ChartAxis axis,
        ChartRect plot,
        bool isXAxis)
    {
        var list = new List<ChartPoint>();
        var count = (int)(axis.Maximum - axis.Minimum + 1);

        for (var i = 0; i < count; i++)
        {
            var pos = axis.Map(i);

            list.Add(isXAxis
                ? new ChartPoint(pos, plot.Y + plot.Height)
                : new ChartPoint(plot.X, pos));
        }

        return list;
    }

    /// <summary>
    /// Generates a collection of tick points for a numeric axis based on the specified axis and plot area.
    /// </summary>
    /// <remarks>The returned tick points are positioned according to the axis orientation and the plot area.
    /// This method uses the axis's mapping function to convert tick values to plot coordinates.</remarks>
    /// <param name="axis">The axis for which numeric tick points are generated. The axis's minimum and maximum values define the range of
    /// ticks.</param>
    /// <param name="plot">The plot area that provides the coordinate space for mapping tick points.</param>
    /// <param name="isXAxis">true to generate tick points for the X axis; false to generate tick points for the Y axis.</param>
    /// <param name="maxTicks">The maximum number of ticks to generate for the numeric axis. This parameter guides the tick generation algorithm to produce a reasonable number of ticks within the axis range.</param>
    /// <returns>A read-only list of ChartPoint objects representing the positions of numeric ticks on the specified axis.</returns>
    private static List<ChartPoint> BuildNumericTicks(
        ChartAxis axis,
        ChartRect plot,
        bool isXAxis,
        int maxTicks)
    {
        var list = new List<ChartPoint>();
        var values = NumericTickGenerator.Generate(axis.Minimum, axis.Maximum, maxTicks);

        foreach (var v in values)
        {
            var pos = axis.Map(v);

            list.Add(isXAxis
                ? new ChartPoint(pos, plot.Y + plot.Height)
                : new ChartPoint(plot.X, pos));
        }

        return list;
    }

    /// <summary>
    /// Builds a collection of axis label payloads for the specified chart axis based on its type.
    /// </summary>
    /// <param name="context">The chart context containing information about the chart area and plot area, which may be used to calculate label positions and rotations.</param>
    /// <param name="axis">The chart axis for which to generate label payloads.</param>
    /// <param name="isXAxis">A value indicating whether the labels are for the X axis (true) or Y axis (false).</param>
    /// <param name="options">The axis options that may include formatting and label generation functions.</param>
    /// <param name="ticks">The collection of tick points for the axis, used to position the labels accordingly.</param>
    /// <returns>Returns a read-only list of <see cref="AxisLabelPayload"/> objects representing the labels for the axis.</returns>
    private static List<AxisLabelPayload> BuildLabels(
        ChartContext context,
        ChartAxis axis,
        bool isXAxis,
        CAO options,
        IReadOnlyList<ChartPoint> ticks)
    {
        return axis.AxisType switch
        {
            ChartAxisType.Category => BuildCategoryLabels(context, axis, isXAxis, options, ticks),
            ChartAxisType.Numeric => BuildNumericLabels(axis, isXAxis, options, ticks),
            ChartAxisType.Time => [],
            _ => []
        };
    }

    /// <summary>
    /// Builds a collection of axis category label payloads for the specified chart axis based on its type.
    /// </summary>
    /// <param name="context">The chart context containing information about the chart area and plot area, which may be used to calculate label positions and rotations.</param>
    /// <param name="axis">The chart axis for which to generate label payloads.</param>
    /// <param name="isXAxis">A value indicating whether the labels are for the X axis (true) or Y axis (false).</param>
    /// <param name="options">The axis options that may include formatting and label generation functions.</param>
    /// <param name="ticks">The collection of tick points for the axis, used to position the labels accordingly.</param>
    /// <returns>Returns a read-only list of <see cref="AxisLabelPayload"/> objects representing the labels for the axis.</returns>
    private static List<AxisLabelPayload> BuildCategoryLabels(
        ChartContext context,
        ChartAxis axis,
        bool isXAxis,
        CAO options,
        IReadOnlyList<ChartPoint> ticks)
    {
        var list = new List<AxisLabelPayload>();
        var fontSize = options.LabelOptions?.FontSize ?? 12;

        var maxLabelWidth = context.PlotArea.Width * 0.10;

        for (var i = 0; i < ticks.Count; i++)
        {
            var tick = ticks[i];
            var text = axis.Labels?[i] ?? i.ToString(s_culture);
            var w0 = text.Length * fontSize * 0.6;
            var h0 = fontSize;
            var w45 = w0 * 0.707 + h0 * 0.707;
            var w90 = h0;

            var rotation = 0.0;
            var display = text;
            SvgTextAnchor anchor;

            if (w0 <= maxLabelWidth)
            {
                anchor = SvgTextAnchor.Middle;
            }
            else if (w45 <= maxLabelWidth)
            {
                rotation = -45;
                anchor = SvgTextAnchor.End;
            }
            else if (w90 <= maxLabelWidth)
            {
                rotation = -90;
                anchor = SvgTextAnchor.End;
            }
            else
            {
                rotation = 0;
                anchor = SvgTextAnchor.Middle;
                display = Ellipsize(text, fontSize, maxLabelWidth);
            }

            if (isXAxis)
            {
                list.Add(new AxisLabelPayload
                {
                    Text = display,
                    Position = new ChartPoint(tick.X, tick.Y + options.LabelOffset),
                    Anchor = anchor,
                    FontSize = fontSize,
                    Rotation = rotation
                });
            }
            else
            {
                list.Add(new AxisLabelPayload
                {
                    Text = display,
                    Position = new ChartPoint(
                        context.PlotArea.X - options.LabelOffset,
                        tick.Y),
                    Anchor = SvgTextAnchor.End,
                    FontSize = fontSize,
                    Rotation = rotation
                });
            }
        }

        return list;
    }

    /// <summary>
    /// Builds a collection of axis numeric label payloads for the specified chart axis based on its type.
    /// </summary>
    /// <param name="axis">The chart axis for which to generate label payloads.</param>
    /// <param name="isXAxis">A value indicating whether the labels are for the X axis (true) or Y axis (false).</param>
    /// <param name="options">The axis options that may include formatting and label generation functions.</param>
    /// <param name="ticks">The collection of tick points for the axis, used to position the labels accordingly.</param>
    /// <returns>Returns a list of <see cref="AxisLabelPayload"/> objects representing the labels for the axis.</returns>
    private static List<AxisLabelPayload> BuildNumericLabels(
        ChartAxis axis,
        bool isXAxis,
        CAO options,
        IReadOnlyList<ChartPoint> ticks)
    {
        var list = new List<AxisLabelPayload>();
        var fontSize = options.LabelOptions?.FontSize ?? 12;

        for (var i = 0; i < ticks.Count; i++)
        {
            var tick = ticks[i];
            var text = axis.Labels?[i] ?? "";

            list.Add(new AxisLabelPayload
            {
                Text = text,
                Position = isXAxis
                    ? new ChartPoint(tick.X, tick.Y + options.LabelOffset)
                    : new ChartPoint(tick.X - options.LabelOffset, tick.Y),
                Anchor = isXAxis ? SvgTextAnchor.Middle : SvgTextAnchor.End,
                FontSize = fontSize,
                Rotation = axis.LabelRotation
            });
        }

        return list;
    }

    private static string Ellipsize(string text, double fontSize, double maxWidth)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        var charWidth = fontSize * 0.6;

        if (text.Length * charWidth <= maxWidth)
        {
            return text;
        }

        var ellipsisWidth = charWidth;
        var maxChars = (int)((maxWidth - ellipsisWidth) / charWidth);

        if (maxChars <= 0)
        {
            return "…";
        }

        return string.Concat(text.AsSpan(0, maxChars), "…");
    }
}
