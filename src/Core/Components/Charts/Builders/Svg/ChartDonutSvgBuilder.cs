using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Represents a builder for generating SVG markup for donut chart layers based on a collection of donut slice payloads.
/// </summary>
internal static class ChartDonutSvgBuilder
{
    /// <summary>
    /// Builds the SVG elements for a pie chart layer using the provided payload collection,
    /// which includes information about the slices, their styles, and label settings.
    /// </summary>
    /// <param name="svg">The SvgBuilder instance used to construct the SVG elements.</param>
    /// <param name="payload">The DonutPayloadCollection containing the data and styling information for the pie chart slices.</param>
    /// <param name="context">The ChartThemeContext providing theme-related information for styling the chart elements.</param>
    /// <param name="renderLabelsDirect">A boolean flag indicating whether to render labels directly after the slices are rendered. If true, labels will be rendered immediately; if false, label rendering can be deferred or handled separately.</param>
    public static void Build(
        SvgBuilder svg,
        DonutPayloadCollection payload,
        ChartThemeContext context,
        bool renderLabelsDirect = true)
    {
        if (payload.Slices.Count == 0)
        {
            return;
        }

        var cx = payload.Center.X;
        var cy = payload.Center.Y;
        var outerRadius = payload.Radius;
        var innerRadius = payload.InnerRadius;
        var id = $"donut-clip-{Guid.NewGuid()}";

        svg.AddClipPath()
            .WithId(id)
            .AddPath()
            .MoveTo(cx + outerRadius, cy)
            .ArcTo(outerRadius, outerRadius, 0, true, true, cx - outerRadius, cy)
            .ArcTo(outerRadius, outerRadius, 0, true, true, cx + outerRadius, cy)
            .LineTo(cx + innerRadius, cy)
            .ArcTo(innerRadius, innerRadius, 0, true, false, cx - innerRadius, cy)
            .ArcTo(innerRadius, innerRadius, 0, true, false, cx + innerRadius, cy)
            .ClosePath()
            .Close();

        var group = svg.AddGroup()
                       .WithId($"chart-donut-{payload.Id}")
                       .WithClipPath(id);

        RenderSlices(group, payload, context);

        group.Close();

        if (renderLabelsDirect)
        {
            RenderLabels(svg, payload, context);
        }
    }

    /// <summary>
    /// Renders label elements for each slice in the donut chart if labels or percentages are enabled.
    /// </summary>
    /// <remarks>Labels are only rendered if either the ShowLabels or ShowPercentages property of the payload
    /// is set to true.</remarks>
    /// <param name="svg">The SVG builder used to construct and append label elements to the chart.</param>
    /// <param name="payload">The data payload containing the donut chart slices and label display options.</param>
    /// <param name="context">The chart theme context providing styling information for the labels.</param>
    internal static void RenderLabels(
        SvgBuilder svg,
        DonutPayloadCollection payload,
        ChartThemeContext context)
    {
        if (payload.ShowLabels || payload.ShowPercentages)
        {
            var labelGroup = svg.AddGroup()
                                .WithId("chart-donut-labels");

            foreach (var slice in payload.Slices)
            {
                RenderLabel(labelGroup, slice, context);
            }

            labelGroup.Close();
        }
    }

    /// <summary>
    /// Renders all donut chart slices into the specified SVG group using the provided payload data.
    /// </summary>
    /// <remarks>Each slice in the payload is rendered using the center, radius, and inner radius values
    /// specified in the payload. This method is intended for internal use when constructing donut chart
    /// visualizations.</remarks>
    /// <param name="group">The SVG group builder that receives the rendered donut slices.</param>
    /// <param name="payload">The payload containing the collection of donut slices and their layout parameters.</param>
    /// <param name="context">The chart theme context providing styling information for the slices.</param>
    private static void RenderSlices(
        SvgGroupBuilder group,
        DonutPayloadCollection payload,
        ChartThemeContext context)
    {
        for (var sliceIndex = 0; sliceIndex < payload.Slices.Count; sliceIndex++)
        {
            var slice = payload.Slices[sliceIndex];
            var reversed = slice.AlternateAnimation && (sliceIndex % 2 == 1);
            var center = payload.Center;

            var sliceGroup = group.AddGroup()
                                  .WithTransform($"translate({center.X.ToSvg()}, {center.Y.ToSvg()})");

            RenderSlice(
                sliceGroup,
                slice,
                center,
                payload.Radius,
                payload.InnerRadius,
                reversed,
                context);

            sliceGroup.Close();
        }
    }

    /// <summary>
    /// Renders a single slice of a pie or donut chart as an SVG path using the specified geometry and style
    /// information.
    /// </summary>
    /// <remarks>This method constructs the SVG path for a chart slice based on the provided start and end
    /// angles, radii, and style. The resulting path is appended to the specified SVG group and includes attributes for
    /// fill, stroke, opacity, and a data identifier. The method supports both standard pie slices and donut slices by
    /// varying the inner radius.</remarks>
    /// <param name="svg">The SVG group builder used to construct and append the path element representing the slice.</param>
    /// <param name="slice">The data payload containing the slice's angles, style, and identifier.</param>
    /// <param name="center">The center point of the pie or donut chart, used as the origin for the slice geometry.</param>
    /// <param name="outerRadius">The outer radius of the slice, defining the distance from the center to the outer edge.</param>
    /// <param name="innerRadius">The inner radius of the slice, defining the distance from the center to the inner edge. Set to zero for a
    /// standard pie slice, or a positive value for a donut slice.</param>
    /// <param name="reversed">A boolean flag indicating whether the animation for this slice should be reversed, typically used to alternate animation directions for adjacent slices.</param>
    /// <param name="context">The chart theme context providing styling information for the slice, such as fill and stroke colors.</param>
    private static void RenderSlice(
        SvgGroupBuilder svg,
        PiePayload slice,
        ChartPoint center,
        double outerRadius,
        double innerRadius,
        bool reversed,
        ChartThemeContext context)
    {
        var colorIndex = slice.ColorIndex;
        var palette = context.Theme.Palette;
        var state = slice.GetStyleForState();
        var fill = palette.Series.Count == 0 ? state.Fill : palette.Series[colorIndex % palette.Series.Count].ToString();
        var stroke = palette.StrokeSeries.Count == 0 ? state.Stroke : palette.StrokeSeries[colorIndex % palette.StrokeSeries.Count].ToString();
        var strokeWidth = context.ComputedValues.FinalStrokeThickness;
        strokeWidth = state.StrokeWidth ?? strokeWidth;

        var startRad = slice.StartAngle * Math.PI / 180.0;
        var endRad = slice.EndAngle * Math.PI / 180.0;

        var x1o = outerRadius * Math.Cos(startRad);
        var y1o = outerRadius * Math.Sin(startRad);
        var x2o = outerRadius * Math.Cos(endRad);
        var y2o = outerRadius * Math.Sin(endRad);

        var x1i = innerRadius * Math.Cos(endRad);
        var y1i = innerRadius * Math.Sin(endRad);
        var x2i = innerRadius * Math.Cos(startRad);
        var y2i = innerRadius * Math.Sin(startRad);

        var largeArc = (slice.EndAngle - slice.StartAngle) > 180;

        var fillPath = svg.AddPath()
            .MoveTo(x1o, y1o)
            .ArcTo(outerRadius, outerRadius, 0, largeArc, sweep: true, x2o, y2o)
            .LineTo(x1i, y1i)
            .ArcTo(innerRadius, innerRadius, 0, largeArc, sweep: false, x2i, y2i)
            .ClosePath()
            .WithFill(fill)
            .WithStroke(stroke)
            .WithStrokeWidth(strokeWidth)
            .WithOpacity(1)
            .WithAttribute("data-id", slice.Id)
            .WithAttribute("data-group", slice.GroupId)
            .WithAttribute("class", "chart-interactive");

        if (!slice.AnimationEnabled ||
            slice.Animation is null ||
            slice.Trigger == ChartAnimationTrigger.None)
        {
            fillPath.Close();
            return;
        }

        if (slice.Effect != ChartAnimationEffect.Sweep)
        {
            ChartAnimationEngine<SvgPathBuilder>.Apply(fillPath, slice, context.Theme.Strategies);
            fillPath.Close();
            return;
        }

        var anim = slice.Animation!;

        var contourStart = reversed ? endRad : startRad;
        var contourEnd = reversed ? startRad : endRad;
        var contourSweepFlag = !reversed;

        var x1c = outerRadius * Math.Cos(contourStart);
        var y1c = outerRadius * Math.Sin(contourStart);
        var x2c = outerRadius * Math.Cos(contourEnd);
        var y2c = outerRadius * Math.Sin(contourEnd);

        var contourPath = svg.AddPath()
            .MoveTo(x1c, y1c)
            .ArcTo(outerRadius, outerRadius, 0, largeArc, contourSweepFlag, x2c, y2c)
            .LineTo(
                center.X + innerRadius * Math.Cos(contourEnd),
                center.Y + innerRadius * Math.Sin(contourEnd))
            .ArcTo(innerRadius, innerRadius, 0, largeArc, !contourSweepFlag,
                center.X + innerRadius * Math.Cos(contourStart),
                center.Y + innerRadius * Math.Sin(contourStart))
            .ClosePath()
            .WithFill("none")
            .WithStroke(stroke)
            .WithStrokeWidth(1.5)
            .WithAttribute("pathLength", "1")
            .WithAttribute("data-id", slice.Id)
            .WithAttribute("data-group", slice.GroupId)
            .WithAttribute("class", "chart-interactive");

        var midRadius = (outerRadius + innerRadius) / 2.0;
        var thickness = outerRadius - innerRadius;

        var sweepStart = reversed ? endRad : startRad;
        var sweepEnd = reversed ? startRad : endRad;
        var sweepFlag = !reversed;

        var x1m = midRadius * Math.Cos(sweepStart);
        var y1m = midRadius * Math.Sin(sweepStart);
        var x2m = midRadius * Math.Cos(sweepEnd);
        var y2m = midRadius * Math.Sin(sweepEnd);

        var sweepPath = svg.AddPath()
            .MoveTo(x1m, y1m)
            .ArcTo(midRadius, midRadius, 0, largeArc, sweep: sweepFlag, x2m, y2m)
            .WithFill("none")
            .WithStroke(fill)
            .WithStrokeWidth(thickness)
            .WithAttribute("pathLength", "1")
            .WithAttribute("data-id", slice.Id)
            .WithAttribute("data-group", slice.GroupId)
            .WithAttribute("class", "chart-interactive");

        ChartAnimationEngine<SvgPathBuilder>.Apply(contourPath, slice, context.Theme.Strategies);
        ChartAnimationEngine<SvgPathBuilder>.Apply(sweepPath, slice, context.Theme.Strategies);

        fillPath.WithOpacity(0);
        fillPath.AddAnimate("opacity", a => a
            .From(0).To(1)
            .Begin(anim.Delay + anim.Duration * 2)
            .Duration(TimeSpan.FromMilliseconds(150))
            .Fill());

        contourPath.Close();
        sweepPath.Close();
        fillPath.Close();
    }

    /// <summary>
    /// Renders the label for a pie chart slice at the specified position within the SVG group, if a label and position
    /// are defined.
    /// </summary>
    /// <remarks>The label is only rendered if the slice has a non-empty label and a valid label position. The
    /// label is centered at the specified coordinates with a default font size and color.</remarks>
    /// <param name="svg">The SVG group builder used to add the label text element.</param>
    /// <param name="slice">The pie chart slice containing the label text and its position information.</param>
    /// <param name="context">The chart theme context providing styling information for the label, such as font and color.</param>
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
            .WithPointerEvents("none")
            .WithDominantBaseline("middle");
    }
}
