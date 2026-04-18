using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Surface.Payloads;
using CAO = FluentUI.Blazor.Community.Components.Charts.Options.ChartAxisOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Provides static methods for constructing SVG elements that represent chart axes, including axis lines, ticks, and
/// labels.
/// </summary>
/// <remarks>This class is intended for internal use when rendering chart axes as SVG graphics. It generates the
/// necessary SVG paths and text elements based on the provided axis geometry and style payloads. The methods in this
/// class are not thread-safe.</remarks>
internal static class ChartAxesSvgBuilder
{
    /// <summary>
    /// Renders the X and Y axes onto the specified SVG builder using the provided axis payload.
    /// </summary>
    /// <remarks>Both the X and Y axes are rendered if present in the payload. If either axis is null, it will
    /// be skipped.</remarks>
    /// <param name="svg">The SVG builder to which the axes will be rendered.</param>
    /// <param name="payload">The payload containing axis configuration and data for rendering.</param>
    /// <param name="options">The chart options that may influence the appearance of the axes.</param>
    /// <param name="context">The theme context providing styling information for the chart.</param>
    public static void Build(
        SvgBuilder svg,
        AxisPayload payload,
        CAO options,
        ChartThemeContext context)
    {
        if (!options.Show)
        {
            return;
        }

        var group = svg.AddGroup().WithId("chart-axis");

        if (payload.XAxis is not null)
        {
            RenderAxis(group, payload.XAxis, payload, options, context);
        }

        if (payload.YAxis is not null)
        {
            RenderAxis(group, payload.YAxis, payload, options, context);
        }

        group.Close();
    }

    /// <summary>
    /// Renders the axis line, tick marks, and labels for a chart axis using the specified SVG builder and axis
    /// configuration.
    /// </summary>
    /// <param name="group">The SVG group builder used to construct the graphical elements of the axis.</param>
    /// <param name="axis">The geometry payload that defines the position and layout of the axis.</param>
    /// <param name="style">The style payload that specifies visual properties such as colors, thickness, and label formatting for the axis.</param>
    /// <param name="options">The chart axis options that may affect the rendering of ticks and labels, such as whether to show them and their formatting.</param>
    /// <param name="context">The theme context providing styling information for the chart, which may influence the appearance of the axis elements.</param>
    private static void RenderAxis(
        SvgGroupBuilder group,
        AxisGeometryPayload axis,
        AxisPayload style,
        CAO options,
        ChartThemeContext context)
    {
        RenderAxisLine(group, axis, style, context);
        RenderTicks(group, axis, style, options, context);
        RenderLabels(group, axis, style, options, context);
    }

    /// <summary>
    /// Renders an axis line on the specified SVG builder using the provided axis geometry and style settings.
    /// </summary>
    /// <param name="svg">The SVG builder to which the axis line will be added.</param>
    /// <param name="axis">The geometry payload that defines the start and end points of the axis line.</param>
    /// <param name="style">The style payload that specifies the color, stroke width, opacity, and dash pattern for the axis line.</param>
    /// <param name="context">The theme context providing styling information for the chart, which may influence the appearance of the axis line.</param>
    private static void RenderAxisLine(
        SvgGroupBuilder svg,
        AxisGeometryPayload axis,
        AxisPayload style,
        ChartThemeContext context)
    {
        svg.AddPath()
            .MoveTo(axis.StartPoint.X, axis.StartPoint.Y)
            .LineTo(axis.EndPoint.X, axis.EndPoint.Y)
            .WithStroke(context.Theme.Palette.Axis.ToString())
            .WithStrokeWidth(context.ComputedValues.FinalAxisThickness)
            .WithOpacity(style.Opacity)
            .WithStrokeDashArray(style.DashArray)
            .Close();
    }

    /// <summary>
    /// Renders tick marks along the specified axis using the provided SVG builder and style settings.
    /// </summary>
    /// <remarks>This method draws each tick as a line perpendicular to the axis at the specified tick
    /// positions. The appearance of the ticks is determined by the style parameter.</remarks>
    /// <param name="svg">The SVG builder used to construct the tick mark paths.</param>
    /// <param name="axis">The axis geometry and tick positions to use for rendering.</param>
    /// <param name="style">The style settings that define the appearance of the tick marks, such as color, stroke width, and opacity.</param>
    /// <param name="options">The chart axis options that may influence whether ticks are rendered and their length.</param>
    /// <param name="context">The theme context providing styling information for the chart, which may influence the appearance of the tick marks.</param>
    private static void RenderTicks(
        SvgGroupBuilder svg,
        AxisGeometryPayload axis,
        AxisPayload style,
        CAO options,
        ChartThemeContext context)
    {
        if (!options.ShowTicks)
        {
            return;
        }

        var dx = axis.EndPoint.X - axis.StartPoint.X;
        var dy = axis.EndPoint.Y - axis.StartPoint.Y;
        var length = Math.Sqrt(dx * dx + dy * dy);

        if (length <= 0)
        {
            return;
        }

        var nx = -dy / length;
        var ny = dx / length;
        var half = options.TickLength / 2.0;

        foreach (var tick in axis.Ticks)
        {
            var x1 = tick.X - nx * half;
            var y1 = tick.Y - ny * half;

            var x2 = tick.X + nx * half;
            var y2 = tick.Y + ny * half;

            svg.AddPath()
                .MoveTo(x1, y1)
                .LineTo(x2, y2)
                .WithStroke(context.Theme.Palette.Axis.ToString())
                .WithStrokeWidth(context.ComputedValues.FinalAxisThickness)
                .WithOpacity(style.Opacity)
                .WithStrokeDashArray(style.DashArray)
                .Close();
        }
    }

    /// <summary>
    /// Renders axis label text elements onto the specified SVG builder using the provided axis geometry and style
    /// information.
    /// </summary>
    /// <param name="svg">The SVG builder to which the label text elements are added.</param>
    /// <param name="axis">The axis geometry payload containing label positions, text, and layout information.</param>
    /// <param name="style">The axis style payload specifying color, opacity, and other visual properties for the labels.</param>
    /// <param name="options">The chart axis options that may affect whether labels are rendered and their formatting.</param>
    /// <param name="context">The theme context providing styling information for the chart, which may influence the appearance of the labels.</param>
    private static void RenderLabels(
        SvgGroupBuilder svg,
        AxisGeometryPayload axis,
        AxisPayload style,
        CAO options,
        ChartThemeContext context)
    {
        if (!options.ShowLabels)
        {
            return;
        }

        var typo = context.Theme.Typography.Label;

        foreach (var label in axis.Labels)
        {
            svg.AddText(label.Position.X, label.Position.Y, label.Text)
                .WithFill(typo.Color.ToString())
                .WithOpacity(style.Opacity)
                .WithTextAnchor(label.Anchor)
                .WithFontFamily(typo.FontFamily)
                .WithDominantBaseline("middle")
                .WithFontSize(typo.FontSize)
                .WithRotation(label.Rotation, label.Position.X, label.Position.Y);
        }
    }
}
