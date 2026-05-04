using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Represents a builder for generating SVG markup for pie chart layers based on a collection of pie slice payloads.
/// </summary>
internal class ChartPieSvgBuilder
{
    /// <summary>
    /// Builds the SVG elements for a pie chart layer using the provided payload collection,
    /// which includes information about the slices, their styles, and label settings.
    /// </summary>
    /// <param name="svg">The SvgBuilder instance used to construct the SVG elements.</param>
    /// <param name="payload">The PiePayloadCollection containing the data and styling information for the pie chart slices.</param>
    /// <param name="context">The ChartThemeContext providing styling information for the chart.</param>
    public static void Build(
        SvgBuilder svg,
        PiePayloadCollection payload,
        ChartThemeContext context)
    {
        if (payload.Slices.Count == 0)
        {
            return;
        }

        var group = svg.AddGroup()
                       .WithId("chart-pie");

        for (var sliceIndex = 0; sliceIndex < payload.Slices.Count; sliceIndex++)
        {
            var slice = payload.Slices[sliceIndex];
            RenderSlice(group, slice, payload.Center, payload.Radius, context);
        }

        group.Close();

        if (payload.ShowLabels || payload.ShowPercentages)
        {
            var labelGroup = svg.AddGroup()
                                .WithId("chart-pie-labels");

            foreach (var slice in payload.Slices)
            {
                RenderLabel(labelGroup, slice, context);
            }

            labelGroup.Close();
        }
    }

    /// <summary>
    /// Renders an individual pie slice as an SVG path element based
    ///  on the provided slice payload, center point, and radius.
    /// </summary>
    /// <param name="svg">The SvgGroupBuilder instance used to construct the SVG elements for the slice.</param>
    /// <param name="slice">The PiePayload containing the data and styling information for the slice.</param>
    /// <param name="center">The center point of the pie chart.</param>
    /// <param name="radius">The radius of the pie chart.</param>
    /// <param name="context">The ChartThemeContext providing styling information for the chart.</param>
    private static void RenderSlice(
        SvgGroupBuilder svg,
        PiePayload slice,
        ChartPoint center,
        double radius,
        ChartThemeContext context)
    {
        var state = slice.GetStyleForState();
        var fill = context.Theme.Palette.Series.Count == 0 ? state.Fill : context.Theme.Palette.Series[slice.ColorIndex % context.Theme.Palette.Series.Count].ToString();
        var stroke = context.Theme.Palette.StrokeSeries.Count == 0 ? state.Stroke : context.Theme.Palette.StrokeSeries[slice.ColorIndex % context.Theme.Palette.StrokeSeries.Count].ToString();
        var strokeWidth = state.StrokeWidth ?? context.ComputedValues.FinalStrokeThickness;
        var opacity = state.Opacity ?? 1.0;

        var startRad = slice.StartAngle * Math.PI / 180.0;
        var endRad = slice.EndAngle * Math.PI / 180.0;

        var x1 = center.X + radius * Math.Cos(startRad);
        var y1 = center.Y + radius * Math.Sin(startRad);

        var x2 = center.X + radius * Math.Cos(endRad);
        var y2 = center.Y + radius * Math.Sin(endRad);

        var largeArc = (slice.EndAngle - slice.StartAngle) > 180;

        var fillPath = svg.AddPath()
            .MoveTo(center.X, center.Y)
            .LineTo(x1, y1)
            .ArcTo(radius, radius, 0, largeArc, sweep: true, x2, y2)
            .ClosePath()
            .WithFill(fill)
            .WithStroke(stroke)
            .WithStrokeWidth(strokeWidth)
            .WithOpacity(opacity)
            .WithAttribute("data-id", slice.Id)
            .WithAttribute("data-group", slice.GroupId)
            .WithAttribute("class", "chart-interactive");

        if (slice.AnimationEnabled &&
            slice.Animation is not null &&
            slice.Effect == ChartAnimationEffect.Sweep)
        {
            var strokeSlice = svg.AddPath()
                .MoveTo(center.X, center.Y)
                .LineTo(x1, y1)
                .ArcTo(radius, radius, 0, largeArc, sweep: true, x2, y2)
                .ClosePath()
                .WithFill("none")
                .WithStroke(stroke)
                .WithStrokeWidth(strokeWidth)
                .WithAttribute("pathLength", "1")
                .WithAttribute("stroke-dasharray", "1")
                .WithAttribute("stroke-dashoffset", "1")
                .WithAttribute("data-id", slice.Id)
                .WithAttribute("data-group", slice.GroupId);

            ChartAnimationEngine<SvgPathBuilder>.Apply(strokeSlice, slice, context.Theme.Strategies);

            fillPath.WithOpacity(0);
            fillPath.AddAnimate("opacity", a => a
                .From(0)
                .To(1)
                .Begin(slice.Animation.Delay + slice.Animation.Duration)
                .Duration(TimeSpan.FromMilliseconds(200))
                .Fill());

            strokeSlice.Close();
        }
        else
        {
            ChartAnimationEngine<SvgPathBuilder>.Apply(fillPath, slice, context.Theme.Strategies);
        }

        fillPath.Close();
    }

    /// <summary>
    /// Renders the label for a pie chart slice at the specified position within the SVG group, if a label and position
    /// are defined.
    /// </summary>
    /// <remarks>The label is only rendered if the slice has a non-empty label and a valid label position. The
    /// label is centered at the specified coordinates with a default font size and color.</remarks>
    /// <param name="svg">The SVG group builder used to add the label text element.</param>
    /// <param name="slice">The pie chart slice containing the label text and its position.</param>
    /// <param name="context">The chart theme context providing styling information for the label.</param>
    private static void RenderLabel(
        SvgGroupBuilder svg,
        PiePayload slice,
        ChartThemeContext context)
    {
        if (string.IsNullOrWhiteSpace(slice.Label) ||
            slice.LabelPosition is not ChartPoint lp)
        {
            return;
        }

        var typo = context.Theme.Typography.Label;

        svg.AddText(lp.X, lp.Y, slice.Label)
            .WithFontSize(typo.FontSize)
            .WithFontFamily(typo.FontFamily)
            .WithFill(typo.Color.ToString())
            .WithTextAnchor(SvgTextAnchor.Middle)
            .WithDominantBaseline("middle")
            .WithPointerEvents("none");
    }
}
