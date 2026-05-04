using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Surface.Payloads;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Represents a builder responsible for generating SVG markup for the grid layer of a chart based on the provided payload data.
/// </summary>
internal static class ChartGridSvgBuilder
{
    /// <summary>
    /// Builds an SVG representation of a grid layer using the specified builder and payload.
    /// </summary>
    /// <param name="builder">The SVG builder used to construct the SVG elements for the grid layer.</param>
    /// <param name="payload">The payload containing the data and configuration for the grid layer to be built.</param>
    /// <param name="context">The theme context providing styling information for the chart, which may influence the appearance of the grid.</param>
    internal static void Build(
        SvgBuilder builder,
        GridPayload payload,
        ChartThemeContext context)
    {
        var group = builder.AddGroup().WithId("chart-grid");

        switch (payload.DisplayMode)
        {
            case GridDisplayMode.Lines:
                {
                    RenderLines(group, payload, context);
                }

                break;

            case GridDisplayMode.Dots:
                {
                    RenderDots(group, payload, context);
                }

                break;
        }

        group.Close();
    }

    /// <summary>
    /// Renders a series of dots onto the specified SVG builder using the provided grid payload.
    /// </summary>
    /// <param name="builder">The SVG builder used to construct the graphical representation of the dots.</param>
    /// <param name="payload">The grid payload containing the data and configuration for rendering the dots.</param>
    /// <param name="context">The theme context providing styling information for the chart, which may influence the appearance of the dots.</param>
    private static void RenderDots(
        SvgGroupBuilder builder,
        GridPayload payload,
        ChartThemeContext context)
    {
        var color = context.ComputedValues.FinalGridColor.ToString();
        var thickness = context.ComputedValues.FinalGridThickness;

        foreach (var dot in payload.Points)
        {
            var radius = payload.PointRadius;

            if (dot.IsBold)
            {
                radius *= 1.5;
            }

            builder.AddCircle(dot.Position.X, dot.Position.Y, radius)
                .WithStroke(color)
                .WithStrokeWidth(thickness)
                .WithOpacity(payload.Opacity);
        }
    }

    /// <summary>
    /// Renders all horizontal and vertical grid lines to the specified SVG builder using the provided grid payload.
    /// </summary>
    /// <param name="svg">The SVG builder to which the grid lines will be rendered. Cannot be null.</param>
    /// <param name="payload">The grid payload containing the definitions of horizontal and vertical lines to render. Cannot be null.</param>
    /// <param name="context">The theme context providing styling information for the chart, which may influence the appearance of the grid lines. Cannot be null.</param>
    private static void RenderLines(
        SvgGroupBuilder svg,
        GridPayload payload,
        ChartThemeContext context)
    {
        foreach (var line in payload.HorizontalLines)
        {
            RenderLine(svg, line, payload, context);
        }

        foreach (var line in payload.VerticalLines)
        {
            RenderLine(svg, line, payload, context);
        }
    }

    /// <summary>
    /// Renders a single grid line onto the specified SVG builder using the provided line and grid payload settings.
    /// </summary>
    /// <remarks>The appearance of the rendered line is determined by the properties of both the line and
    /// payload parameters, including whether the line is bold and the stroke style to apply.</remarks>
    /// <param name="svg">The SVG builder to which the grid line will be rendered. Cannot be null.</param>
    /// <param name="line">The line definition containing coordinates and style information for the grid line to render. Cannot be null.</param>
    /// <param name="payload">The grid payload containing rendering options such as stroke width, color, opacity, and dash pattern. Cannot be
    /// null.</param>
    /// <param name="context">The theme context providing styling information for the chart, which may influence the appearance of the grid line. Cannot be null.</param>
    private static void RenderLine(
        SvgGroupBuilder svg,
        GridLinePayload line,
        GridPayload payload,
        ChartThemeContext context)
    {
        var color = context.ComputedValues.FinalGridColor.ToString();
        var strokeWidth = line.IsBold
            ? context.ComputedValues.FinalGridThickness * 1.5
            : context.ComputedValues.FinalGridThickness;

        svg.AddPath()
            .MoveTo(line.StartPoint.X, line.StartPoint.Y)
            .LineTo(line.EndPoint.X, line.EndPoint.Y)
            .WithStroke(color)
            .WithStrokeWidth(strokeWidth)
            .WithOpacity(payload.Opacity)
            .WithStrokeDashArray(payload.DashArray)
            .ClosePath();
    }
}
