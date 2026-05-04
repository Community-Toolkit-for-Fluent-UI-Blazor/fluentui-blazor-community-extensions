using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Helpers;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Surface.Engines;
using FluentUI.Blazor.Community.Components.Surface.Payloads;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Represents the engine responsible for building the grid payload for a chart based on the
///  provided chart context and grid options. This engine generates the necessary data for rendering
///  grid lines or dots on the chart, depending on the specified display mode and configuration options.
/// </summary>
internal sealed class ChartGridEngine
    : IGridEngine<ChartContext, ChartGridOptions>
{
    /// <summary>
    /// Builds a grid payload for the chart based on the specified context and grid options.
    /// </summary>
    /// <remarks>The returned grid payload depends on the display mode specified in the options. If the grid
    /// is not set to be shown, the method returns null.</remarks>
    /// <param name="view">The chart context that provides data and state information required to construct the grid.</param>
    /// <param name="options">The grid display options that determine whether the grid is shown and how it is rendered.</param>
    /// <returns>A GridPayload representing the constructed grid if the grid is enabled; otherwise, null.</returns>
    public GridPayload? Build(ChartContext view, ChartGridOptions options)
    {
        if (!options.ShowHorizontal &&
            !options.ShowVertical)
        {
            return null;
        }

        return options.DisplayMode switch
        {
            GridDisplayMode.Lines => BuildLines(view, options),
            GridDisplayMode.Dots => BuildDots(view, options),
            _ => null
        };
    }

    /// <summary>
    /// Builds a grid payload containing horizontal and vertical lines based on the chart context and grid options.
    /// </summary>
    /// <param name="view">View context containing the plot area and axes information needed to calculate the positions of the grid lines.</param>
    /// <param name="options">Options specifying how the grid lines should be generated, including whether to snap to ticks, cell size, and styling preferences.</param>
    /// <returns>Returns a <see cref="GridPayload"/> representing the constructed grid lines.</returns>
    private static GridPayload BuildLines(ChartContext view, ChartGridOptions options)
    {
        if (view.XAxis == null || view.YAxis == null)
        {
            return new GridPayload
            {
                HorizontalLines = [],
                VerticalLines = [],
                Points = [],
                Color = options.Color,
                Opacity = options.Opacity,
                StrokeWidth = options.StrokeWidth,
                DashArray = SurfaceMathUtils.ToDashArray(options.DashArray),
                Layer = options.Layer
            };
        }

        var plot = view.PlotArea;

        var horizontal = options.ShowHorizontal
            ? BuildHorizontalLines(view.YAxis, plot, options)
            : [];

        var vertical = options.ShowVertical
            ? BuildVerticalLines(view.XAxis, plot, options)
            : [];

        return new GridPayload
        {
            HorizontalLines = horizontal,
            VerticalLines = vertical,
            Points = [],
            Color = options.Color,
            Opacity = options.Opacity,
            StrokeWidth = options.StrokeWidth,
            DashArray = SurfaceMathUtils.ToDashArray(options.DashArray),
            Layer = options.Layer
        };
    }

    /// <summary>
    /// Builds a collection of horizontal grid line payloads for the specified plot area, based on the provided axis and grid options.
    /// </summary>
    /// <param name="yAxis">The Y-axis of the chart, used to determine the positions of the horizontal grid lines.</param>
    /// <param name="plot">The plot area of the chart, defining the bounds for the grid lines.</param>
    /// <param name="options">The grid options specifying how the grid lines should be generated.</param>
    /// <returns>Returns a list of GridLinePayload objects representing the horizontal grid lines.</returns>
    private static List<GridLinePayload> BuildHorizontalLines(
        ChartAxis yAxis,
        ChartRect plot,
        ChartGridOptions options)
    {
        return options.SnapToTicks
            ? BuildHorizontalLinesFromTicks(yAxis, plot, options)
            : BuildHorizontalLinesFromCellSize(plot, options);
    }

    /// <summary>
    /// Builds a collection of horizontal grid lines for a numeric Y-axis based on the specified tick values and grid
    /// options.
    /// </summary>
    /// <remarks>Grid lines are generated only for numeric axes. The boldness of lines is determined by the
    /// BoldEvery property in the options. The returned lines span the width of the plot area at each tick
    /// value.</remarks>
    /// <param name="yAxis">The Y-axis for which to generate horizontal grid lines. Must be of type Numeric.</param>
    /// <param name="plot">The plot area that defines the horizontal span and vertical mapping for the grid lines.</param>
    /// <param name="options">The grid options that determine tick frequency and bold line intervals.</param>
    /// <returns>A list of GridLinePayload objects representing the horizontal grid lines to be rendered. The list is empty if
    /// the axis is not numeric.</returns>
    private static List<GridLinePayload> BuildHorizontalLinesFromTicks(
        ChartAxis yAxis,
        ChartRect plot,
        ChartGridOptions options)
    {
        var lines = new List<GridLinePayload>();

        if (yAxis.AxisType != ChartAxisType.Numeric)
        {
            return lines;
        }

        var values = NumericTickGenerator.GenerateNice(
            yAxis.Minimum,
            yAxis.Maximum,
            options.BoldEvery > 0 ? options.BoldEvery : 6);

        var index = 0;

        foreach (var v in values)
        {
            var y = yAxis.Map(v);

            lines.Add(new GridLinePayload
            {
                StartPoint = new ChartPoint(plot.X, y),
                EndPoint = new ChartPoint(plot.X + plot.Width, y),
                IsBold = (index++ % options.BoldEvery == 0)
            });
        }

        return lines;
    }

    /// <summary>
    /// Builds a collection of horizontal grid lines for the specified plot area based on the provided cell size
    /// options.
    /// </summary>
    /// <remarks>Lines are spaced according to the CellSize property in options. Every Nth line, where N is
    /// specified by BoldEvery, is marked as bold. The method does not generate lines if CellSize is zero or
    /// negative.</remarks>
    /// <param name="plot">The plot area for which horizontal grid lines are generated. Defines the bounds and position of the grid.</param>
    /// <param name="options">The grid options specifying the cell size and bold line frequency. Cell size determines the spacing between
    /// lines.</param>
    /// <returns>A list of GridLinePayload objects representing the horizontal grid lines to be rendered within the plot area.
    /// The list is empty if the cell size is less than or equal to zero.</returns>
    private static List<GridLinePayload> BuildHorizontalLinesFromCellSize(
        ChartRect plot,
        ChartGridOptions options)
    {
        var lines = new List<GridLinePayload>();

        if (options.CellSize <= 0)
        {
            return lines;
        }

        var y = plot.Y;
        var maxY = plot.Y + plot.Height;
        var index = 0;

        while (y <= maxY + 0.5)
        {
            lines.Add(new GridLinePayload
            {
                StartPoint = new ChartPoint(plot.X, y),
                EndPoint = new ChartPoint(plot.X + plot.Width, y),
                IsBold = (index % options.BoldEvery == 0)
            });

            index++;
            y += options.CellSize;
        }

        return lines;
    }

    /// <summary>
    /// Builds a collection of vertical grid line payloads for the specified plot area, based on the provided axis and
    /// grid options.
    /// </summary>
    /// <remarks>If the SnapToTicks option is enabled, vertical lines are aligned with the axis ticks;
    /// otherwise, they are spaced according to the cell size defined in the grid options.</remarks>
    /// <param name="xAxis">The horizontal axis configuration used to determine the positions of vertical grid lines.</param>
    /// <param name="plot">The plot area for which vertical grid lines are to be generated.</param>
    /// <param name="options">The grid options that control how vertical lines are calculated, including whether to align lines with axis
    /// ticks.</param>
    /// <returns>A list of grid line payloads representing the vertical grid lines to be rendered in the plot area.</returns>
    private static List<GridLinePayload> BuildVerticalLines(
        ChartAxis xAxis,
        ChartRect plot,
        ChartGridOptions options)
    {
        return options.SnapToTicks
            ? BuildVerticalLinesFromTicks(xAxis, plot, options)
            : BuildVerticalLinesFromCellSize(plot, options);
    }

    /// <summary>
    /// Builds a collection of vertical grid lines for a chart based on the tick values of the specified X axis.
    /// </summary>
    /// <remarks>The generated grid lines correspond to the tick marks on the X axis and span the full height
    /// of the plot area. Lines are marked as bold at intervals specified by the BoldEvery property in the
    /// options.</remarks>
    /// <param name="xAxis">The chart axis representing the horizontal (X) axis from which tick values are derived.</param>
    /// <param name="plot">The plot area of the chart that defines the vertical range for the grid lines.</param>
    /// <param name="options">The grid options that specify formatting details such as the interval for bold lines.</param>
    /// <returns>A list of GridLinePayload objects representing the vertical grid lines positioned according to the axis ticks.</returns>
    private static List<GridLinePayload> BuildVerticalLinesFromTicks(
        ChartAxis xAxis,
        ChartRect plot,
        ChartGridOptions options)
    {
        var lines = new List<GridLinePayload>();

        var values = xAxis.AxisType switch
        {
            ChartAxisType.Category => Enumerable.Range(0, (int)(xAxis.Maximum - xAxis.Minimum + 1)).Select(i => (double)i),
            ChartAxisType.Numeric => NumericTickGenerator.GenerateNice(
                xAxis.Minimum,
                xAxis.Maximum,
                options.BoldEvery > 0 ? options.BoldEvery : 6),
            _ => []
        };

        var index = 0;

        foreach (var v in values)
        {
            var x = xAxis.Map(v);

            lines.Add(new GridLinePayload
            {
                StartPoint = new ChartPoint(x, plot.Y),
                EndPoint = new ChartPoint(x, plot.Y + plot.Height),
                IsBold = (index++ % options.BoldEvery == 0)
            });
        }

        return lines;
    }

    /// <summary>
    /// Builds a collection of vertical grid lines for a chart plot area based on the specified cell size and grid
    /// options.
    /// </summary>
    /// <remarks>Lines are spaced according to the CellSize property in options. Every Nth line, where N is
    /// specified by BoldEvery, is marked as bold.</remarks>
    /// <param name="plot">The plot area of the chart for which vertical grid lines are generated.</param>
    /// <param name="options">The grid options that define the cell size and bold line frequency for the vertical lines.</param>
    /// <returns>A list of GridLinePayload objects representing the vertical grid lines to be rendered within the plot area. The
    /// list is empty if the cell size is less than or equal to zero.</returns>
    private static List<GridLinePayload> BuildVerticalLinesFromCellSize(
        ChartRect plot,
        ChartGridOptions options)
    {
        var lines = new List<GridLinePayload>();

        if (options.CellSize <= 0)
        {
            return lines;
        }

        var x = plot.X;
        var maxX = plot.X + plot.Width;
        var index = 0;

        while (x <= maxX + 0.5)
        {
            lines.Add(new GridLinePayload
            {
                StartPoint = new ChartPoint(x, plot.Y),
                EndPoint = new ChartPoint(x, plot.Y + plot.Height),
                IsBold = (index % options.BoldEvery == 0)
            });

            index++;
            x += options.CellSize;
        }

        return lines;
    }

    /// <summary>
    /// Builds a grid payload containing dot points for rendering within the specified chart context and grid options.
    /// </summary>
    /// <param name="view">The chart context that defines the plot area and rendering environment for the grid.</param>
    /// <param name="options">The grid options that specify appearance and configuration settings such as color, opacity, stroke width, and
    /// layer.</param>
    /// <returns>A GridPayload instance containing the generated dot points and associated visual properties for the grid.</returns>
    private static GridPayload BuildDots(ChartContext view, ChartGridOptions options)
    {
        var plot = view.PlotArea;

        var points = BuildDotGrid(plot, options);

        return new GridPayload
        {
            HorizontalLines = [],
            VerticalLines = [],
            Points = points,
            Color = options.Color,
            Opacity = options.Opacity,
            StrokeWidth = options.StrokeWidth,
            DashArray = [],
            Layer = options.Layer
        };
    }

    /// <summary>
    /// Builds a collection of grid points representing a dot grid within the specified plot area, using the provided
    /// grid options.
    /// </summary>
    /// <remarks>Grid points are spaced according to the cell size specified in options. Points are marked as
    /// bold based on the BoldEvery property, which determines the frequency of bold points along both axes.</remarks>
    /// <param name="plot">The plot area that defines the bounds within which the dot grid is generated.</param>
    /// <param name="options">The grid options that specify cell size and bolding frequency for the generated grid points. The cell size must
    /// be greater than zero.</param>
    /// <returns>A list of GridPointPayload objects representing the positions and boldness of each grid point within the plot
    /// area. The list is empty if the cell size is less than or equal to zero.</returns>
    private static List<GridPointPayload> BuildDotGrid(
        ChartRect plot,
        ChartGridOptions options)
    {
        var points = new List<GridPointPayload>();

        if (options.CellSize <= 0)
        {
            return points;
        }

        var x = plot.X;
        var maxX = plot.X + plot.Width;
        var ix = 0;

        while (x <= maxX + 0.5)
        {
            var y = plot.Y;
            var maxY = plot.Y + plot.Height;
            var iy = 0;

            while (y <= maxY + 0.5)
            {
                points.Add(new GridPointPayload
                {
                    Position = new ChartPoint(x, y),
                    IsBold = (ix % options.BoldEvery == 0) && (iy % options.BoldEvery == 0)
                });

                iy++;
                y += options.CellSize;
            }

            ix++;
            x += options.CellSize;
        }

        return points;
    }
}
