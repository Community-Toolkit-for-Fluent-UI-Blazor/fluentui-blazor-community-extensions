using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Represents a builder for generating SVG markup for bar chart layers.
/// </summary>
internal static class ChartBarSvgBuilder
{
    /// <summary>
    /// Builds the SVG elements representing a collection of columns and adds them to the specified SVG builder.
    /// </summary>
    /// <remarks>This method groups all rendered columns within a single SVG group element to optimize DOM
    /// diffing and enable smoother animations.</remarks>
    /// <param name="svg">The SVG builder to which the bar elements will be added. Cannot be null.</param>
    /// <param name="bars">A read-only list of bar payloads that define the bars to render. If the list is empty, no bars are
    /// rendered.</param>
    /// <param name="chartContext">The chart context providing information about the chart's dimensions, scales, and other rendering parameters. Cannot be null.</param>
    /// <param name="chartThemeContext">The chart theme context providing styling information for rendering the bars. Cannot be null.</param>
    public static void Build(
        SvgBuilder svg,
        BarPayloadCollection bars,
        ChartThemeContext chartThemeContext,
        ChartContext chartContext)
    {
        if (bars.Bars.Count == 0)
        {
            return;
        }

        var group = svg.AddGroup()
                       .WithId(bars.Id)
                       .WithClipPath(chartContext.ClipPathId);

        foreach (var bar in bars.Bars)
        {
            RenderBar(group, bar, chartThemeContext);
        }

        group.Close();
    }

    /// <summary>
    /// Renders a bar element as an SVG rectangle with associated metadata attributes.
    /// </summary>
    /// <param name="svg">The SVG group builder used to construct and append the rectangle element.</param>
    /// <param name="bar">The payload containing the bar's position, size, style, and metadata to be rendered.</param>
    /// <param name="context">The chart theme context providing styling information for the bar.</param>
    private static void RenderBar(
        SvgGroupBuilder svg,
        BarPayload bar,
        ChartThemeContext context)
    {
        var style = bar.GetStyleForState();
        var fill = context.Theme.Palette.Series.Count == 0 ? style.Fill ?? context.Theme.Palette.Series[bar.SerieIndex % context.Theme.Palette.Series.Count].ToString() : context.Theme.Palette.Series[bar.SerieIndex % context.Theme.Palette.Series.Count].ToString();
        var opacity = style.Opacity ?? 1.0;
        var stroke = context.Theme.Palette.Series.Count == 0 ? style.Stroke ?? context.Theme.Palette.Axis.ToString() : context.Theme.Palette.Axis.ToString();
        var strokeWidth = style.StrokeWidth ?? context.ComputedValues.FinalAxisThickness;
        var radius = context.ComputedValues.FinalBarRadius;
        var group = svg.AddGroup().WithTransform($"translate({bar.X.ToSvg()}, {bar.Y.ToSvg()})");

        var rectGroup = group.AddGroup()
            .WithId($"{bar.Id}-rect")
            .WithAttribute("data-id", bar.Id)
            .WithAttribute("data-group", bar.GroupId)
            .WithPointerEvents("visiblePainted")
            .WithAttribute("data-category", bar.CategoryIndex)
            .WithAttribute("data-value", bar.Value);

        rectGroup.AddRect(0,
                      -bar.Height / 2.0,
                      bar.Width,
                      bar.Height)
            .WithRx(radius)
            .WithRy(radius)
            .WithId(bar.Id)
            .WithAttribute("data-id", bar.Id)
            .WithAttribute("data-group", bar.GroupId)
            .WithPointerEvents("visiblePainted")
            .WithAttribute("data-category", bar.CategoryIndex)
            .WithAttribute("data-value", bar.Value)
            .WithFill(fill)
            .WithOpacity(opacity)
            .WithStroke(stroke)
            .WithStrokeWidth(strokeWidth)
            .Close();

        ChartAnimationEngine<SvgGroupBuilder>.Apply(rectGroup, bar, context.Theme.Strategies);

        rectGroup.Close();

        if (bar.Tooltip.IsVisible &&
            !string.IsNullOrEmpty(bar.Tooltip.Text))
        {
            var tooltipGroup = group.AddGroup()
                .WithId($"{bar.Id}-tooltip")
                .WithPointerEvents("none");

            var tx = bar.Width / 2.0;
            var ty = 0;

            tooltipGroup.AddRect(tx - 40, ty - 10, 80, 20)
                .WithRx(4)
                .WithRy(4)
                .WithFill(context.Theme.Palette.Background.ToString())
                .WithStroke(context.Theme.Palette.Foreground.ToString())
                .WithStrokeWidth(1)
                .Close();

            tooltipGroup.AddText(tx - 36, ty + 3, bar.Tooltip.Text)
                .WithFill(context.Theme.Palette.Foreground.ToString())
                .WithFontSize(12)
                .Close();
        }

        group.Close();
    }
}
