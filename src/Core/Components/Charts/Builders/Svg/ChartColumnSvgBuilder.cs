using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Represents a builder for generating SVG markup for column chart layers.
/// </summary>
internal static class ChartColumnSvgBuilder
{
    /// <summary>
    /// Builds the SVG elements representing a collection of columns and adds them to the specified SVG builder.
    /// </summary>
    /// <remarks>This method groups all rendered columns within a single SVG group element to optimize DOM
    /// diffing and enable smoother animations.</remarks>
    /// <param name="svg">The SVG builder to which the column elements will be added. Cannot be null.</param>
    /// <param name="columnCollection">A read-only list of column payloads that define the columns to render. If the list is empty, no columns are
    /// rendered.</param>
    /// <param name="chartContext">The chart context providing information about the chart's dimensions, layout, and rendering state. Cannot be null.</param>
    /// <param name="context">The chart theme context providing styling information for rendering the columns. Cannot be null.</param>
    public static void Build(
        SvgBuilder svg,
        ColumnPayloadCollection columnCollection,
        ChartContext chartContext,
        ChartThemeContext context)
    {
        var columns = columnCollection.Columns;

        if (columns.Count == 0)
        {
            return;
        }

        var group = svg.AddGroup()
                       .WithId("chart-columns")
                       .WithClipPath(chartContext.ClipPathId);

        foreach (var col in columns)
        {
            RenderColumn(group, col, context);
        }

        group.Close();
    }

    /// <summary>
    /// Renders a column as an SVG rectangle using the specified payload and styling information.
    /// </summary>
    /// <remarks>The rendered rectangle includes data attributes for identification and categorization, as
    /// well as visual styling based on the provided payload.</remarks>
    /// <param name="svg">The SVG group builder used to construct and append the rectangle element.</param>
    /// <param name="col">The payload containing the column's position, dimensions, style, and metadata to be rendered.</param>
    /// <param name="context">The chart theme context providing styling information for the column.</param>
    private static void RenderColumn(
        SvgGroupBuilder svg,
        ColumnPayload col,
        ChartThemeContext context)
    {
        var style = col.GetStyleForState();
        var fill = context.Theme.Palette.Series.Count == 0 ? style.Fill : context.Theme.Palette.Series[col.SerieIndex % context.Theme.Palette.Series.Count].ToString();
        var opacity = style.Opacity ?? 1.0;
        var stroke = context.Theme.Palette.StrokeSeries.Count == 0 ? style.Stroke : context.Theme.Palette.StrokeSeries[col.SerieIndex % context.Theme.Palette.StrokeSeries.Count].ToString();
        var strokeWidth = style.StrokeWidth ?? context.ComputedValues.FinalAxisThickness;

        var radius = context.ComputedValues.FinalBarRadius;

        var group = svg.AddGroup()
                       .WithTransform($"translate({col.X.ToSvg()}, {col.Y.ToSvg()})");

        var anim = group.AddGroup().WithId($"column-{col.Id}");

        anim.AddRect(-col.Width / 2.0, 0, col.Width, col.Height)
            .WithRx(radius)
            .WithRy(radius)
            .WithAttribute("data-id", col.Id)
            .WithAttribute("data-group", col.GroupId)
            .WithAttribute("data-category", col.CategoryIndex)
            .WithAttribute("data-value", col.Value)
            .WithPointerEvents("visiblePainted")
            .WithFill(fill)
            .WithOpacity(opacity)
            .WithStroke(stroke)
            .WithStrokeWidth(strokeWidth)
            .Close();

        ChartAnimationEngine<SvgGroupBuilder>.Apply(anim, col, context.Theme.Strategies);

        group.Close();
    }
}
