using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Represents a builder responsible for generating SVG title, subtile and legend elements for a chart.
/// </summary>
internal static class ChartLayoutSvgBuilder
{
    /// <summary>
    /// Builds the SVG elements for a chart using the specified layout and theme context.
    /// </summary>
    /// <param name="builder">The SVG builder used to construct the chart elements.</param>
    /// <param name="chartLayoutPayload">The layout payload containing chart structure and positioning information.</param>
    /// <param name="themeContext">The theme context that provides styling and appearance settings for the chart.</param>
    internal static void Build(
        SvgBuilder builder,
        ChartLayoutPayload chartLayoutPayload,
        ChartThemeContext themeContext)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(chartLayoutPayload);
        ArgumentNullException.ThrowIfNull(themeContext);

        var hasTitle = chartLayoutPayload.TitlePayload is not null;
        var hasSubtitle = chartLayoutPayload.SubtitlePayload is not null;
        var hasLegend = chartLayoutPayload.LegendPayload is not null &&
                        chartLayoutPayload.LegendPayload.ItemCount > 0;

        if (!hasTitle &&
            !hasSubtitle &&
            !hasLegend)
        {
            return;
        }

        var group = builder.AddGroup()
                           .WithId("chart-layout");

        if (hasTitle)
        {
            RenderTitle(group, chartLayoutPayload.TitlePayload!, themeContext);
        }

        if (hasSubtitle)
        {
            RenderSubtitle(group, chartLayoutPayload.SubtitlePayload!, themeContext);
        }

        if (hasLegend)
        {
            RenderLegend(group, chartLayoutPayload.LegendPayload!, themeContext);
        }

        group.Close();
    }

    /// <summary>
    /// Renders the chart title text within the specified SVG group using the provided payload and theme context.
    /// </summary>
    /// <param name="group">The SVG group builder to which the title text will be added.</param>
    /// <param name="payload">The payload containing the title text and layout area information.</param>
    /// <param name="themeContext">The theme context that provides typography and styling information for rendering the title.</param>
    private static void RenderTitle(
        SvgGroupBuilder group,
        ChartTitlePayload payload,
        ChartThemeContext themeContext)
    {
        var typo = themeContext.Theme.Typography.Title;
        var area = payload.Area;

        var x = area.X + area.Width / 2.0;
        var y = area.Y + area.Height / 2.0;

        group.AddText(x, y, payload.Text)
             .WithFontFamily(typo.FontFamily)
             .WithFontSize(typo.FontSize)
             .WithFontWeight(typo.FontWeight)
             .WithFill(typo.Color.ToString())
             .WithTextAnchor(SvgTextAnchor.Middle)
             .WithDominantBaseline("middle")
             .Close();
    }

    /// <summary>
    /// Renders the chart subtitle text within the specified SVG group using the provided subtitle payload and theme
    /// context.
    /// </summary>
    /// <remarks>The subtitle is centered within the specified area and styled according to the theme's
    /// subtitle typography settings.</remarks>
    /// <param name="group">The SVG group builder to which the subtitle text will be added.</param>
    /// <param name="payload">The payload containing subtitle text and layout area information.</param>
    /// <param name="themeContext">The theme context that provides typography and styling information for rendering the subtitle.</param>
    private static void RenderSubtitle(
        SvgGroupBuilder group,
        ChartSubtitlePayload payload,
        ChartThemeContext themeContext)
    {
        var typo = themeContext.Theme.Typography.Subtitle;
        var area = payload.Area;

        var x = area.X + area.Width / 2.0;
        var y = area.Y + area.Height / 2.0;

        group.AddText(x, y, payload.Text)
             .WithFontFamily(typo.FontFamily)
             .WithFontSize(typo.FontSize)
             .WithFontWeight(typo.FontWeight)
             .WithFill(typo.Color.ToString())
             .WithTextAnchor(SvgTextAnchor.Middle)
             .WithDominantBaseline("middle")
             .Close();
    }

    /// <summary>
    /// Renders the chart legend items within the specified SVG group using the provided legend payload and theme
    /// context.
    /// </summary>
    /// <remarks>The legend layout adapts to the available area, arranging items horizontally or vertically
    /// based on the aspect ratio. No legend is rendered if the item count is zero or the area is empty.</remarks>
    /// <param name="group">The SVG group builder to which the legend items will be added.</param>
    /// <param name="payload">The payload containing legend area, shape, and item count information used to render the legend.</param>
    /// <param name="themeContext">The theme context that provides layout and typography settings for rendering the legend.</param>
    private static void RenderLegend(
        SvgGroupBuilder group,
        ChartLegendPayload payload,
        ChartThemeContext themeContext)
    {
        var area = payload.Area;
        var items = payload.Items;
        var count = items.Count;

        if (count == 0)
        {
            return;
        }

        var layout = themeContext.Theme.Layout;
        var typo = themeContext.Theme.Typography.Legend;

        var textHeight = typo.FontSize * 1.2;
        var itemHeight = Math.Max(layout.LegendShapeSize, textHeight) + layout.LegendVerticalPadding;

        var maxTextWidth =
            area.Width
            - layout.LegendHorizontalPadding * 2
            - layout.LegendShapeSize
            - layout.LegendShapeTextSpacing;

        var itemWidth =
            layout.LegendHorizontalPadding * 2
            + layout.LegendShapeSize
            + layout.LegendShapeTextSpacing
            + maxTextWidth;

        var horizontal = area.Width >= area.Height;

        if (horizontal)
        {
            var itemsPerRow = Math.Max(1, (int)(area.Width / itemWidth));
            var rowCount = (int)Math.Ceiling((double)count / itemsPerRow);

            var index = 0;

            for (var row = 0; row < rowCount; row++)
            {
                for (var col = 0; col < itemsPerRow && index < count; col++, index++)
                {
                    var x = area.X + col * itemWidth;
                    var y = area.Y + row * itemHeight;

                    var item = items[index];
                    var color = themeContext.Theme.Palette.Series[item.ColorIndex % themeContext.Theme.Palette.Series.Count].ToString();

                    RenderLegendItem(group, x, y, layout, typo, payload.Shape, item.Label, color, maxTextWidth, itemHeight);
                }
            }
        }
        else
        {
            var itemsPerColumn = Math.Max(1, (int)(area.Height / itemHeight));
            var columnCount = (int)Math.Ceiling((double)count / itemsPerColumn);

            var index = 0;

            for (var col = 0; col < columnCount; col++)
            {
                for (var row = 0; row < itemsPerColumn && index < count; row++, index++)
                {
                    var x = area.X + col * itemWidth;
                    var y = area.Y + row * itemHeight;

                    var item = items[index];
                    var color = themeContext.Theme.Palette.Series[item.ColorIndex % themeContext.Theme.Palette.Series.Count].ToString();

                    RenderLegendItem(group, x, y, layout, typo, payload.Shape, item.Label, color, maxTextWidth, itemHeight);
                }
            }
        }
    }

    /// <summary>
    /// Renders a legend item shape and its associated label within the specified SVG group at the given coordinates.
    /// </summary>
    /// <remarks>The label text is rendered as a placeholder and should be replaced with actual content as
    /// needed. The method supports rendering different legend shapes based on the specified shape parameter.</remarks>
    /// <param name="group">The SVG group builder to which the legend item elements are added.</param>
    /// <param name="itemX">The x-coordinate of the top-left corner where the legend item shape is rendered.</param>
    /// <param name="itemY">The y-coordinate of the top-left corner where the legend item shape is rendered.</param>
    /// <param name="layout">The chart layout information that provides sizing and spacing details for the legend item.</param>
    /// <param name="typo">The text style to apply to the legend label, including font and color information.</param>
    /// <param name="shape">The shape to render for the legend item, such as circle, square, or line.</param>
    /// <param name="color">The color to use for the legend item shape, typically derived from the chart's color palette.</param>
    /// <param name="label">The text label associated with the legend item, which may be used to display the series name or category.</param>
    /// <param name="itemHeight">The height of the legend item, which is used to vertically center the shape and label within the allocated space.</param>
    /// <param name="maxTextWidth">The maximum width allowed for the legend label text, which may be used to truncate or ellipsize the text if it exceeds this width.</param>
    private static void RenderLegendItem(
        SvgGroupBuilder group,
        double itemX,
        double itemY,
        ChartLayout layout,
        ChartTextStyle typo,
        ChartLegendItemShape shape,
        string? label,
        string color,
        double maxTextWidth,
        double itemHeight)
    {
        var size = layout.LegendShapeSize;
        var padding = layout.LegendHorizontalPadding;
        var item = group.AddGroup()
                        .WithTransform($"translate({itemX.ToSvg()}, {itemY.ToSvg()})");

        var centerY = itemHeight / 2.0;
        var centerCircleY = centerY - 2.0;
        var shapeX = padding;
        var stroke = typo.Color.Darken(15).ToString();

        switch (shape)
        {
            case ChartLegendItemShape.Circle:
                item.AddCircle(shapeX + size / 2.0, centerCircleY, size / 2.0)
                    .WithFill(color)
                    .WithStroke(stroke)
                    .WithStrokeWidth(1)
                    .Close();
                break;

            case ChartLegendItemShape.Square:
                item.AddRect(shapeX, centerCircleY - size / 2.0, size, size)
                    .WithFill(color)
                    .WithStroke(stroke)
                    .WithStrokeWidth(1)
                    .Close();
                break;

            case ChartLegendItemShape.RoundSquare:
                item.AddRect(shapeX, centerCircleY - size / 2.0, size, size)
                    .WithRx(2)
                    .WithRy(2)
                    .WithFill(color)
                    .WithStroke(stroke)
                    .WithStrokeWidth(1)
                    .Close();
                break;

            case ChartLegendItemShape.Diamond:
                var cx = shapeX + size / 2.0;
                item.AddPolygon()
                    .WithPoints([
                        (cx, centerCircleY - size / 2.0),
                    (cx + size / 2.0, centerCircleY),
                    (cx, centerCircleY + size / 2.0),
                    (cx - size / 2.0, centerCircleY)
                    ])
                    .WithFill(color)
                    .WithStroke(stroke)
                    .WithStrokeWidth(1)
                    .Close();
                break;

            case ChartLegendItemShape.Triangle:
                item.AddPolygon()
                    .WithPoints([
                        (shapeX, centerCircleY + size / 2.0),
                    (shapeX + size, centerCircleY + size / 2.0),
                    (shapeX + size / 2.0, centerCircleY - size / 2.0)
                    ])
                    .WithFill(color)
                    .WithStroke(stroke)
                    .WithStrokeWidth(1)
                    .Close();
                break;
        }

        var textX = shapeX + size + layout.LegendShapeTextSpacing;
        var display = Ellipsize(label, typo.FontSize, maxTextWidth);
        //var needsTooltip = display != label;
        item.AddText(textX, centerY, display)
            .WithFontFamily(typo.FontFamily)
            .WithFontSize(typo.FontSize)
            .WithFill(typo.Color.ToString())
            .WithDominantBaseline("middle")
            .Close();

        /*  if (needsTooltip)
              text.AddTitle(label);*/

        item.Close();
    }

    /// <summary>
    /// Truncates the specified text with an ellipsis if it exceeds the maximum width when rendered at the given font
    /// size.
    /// </summary>
    /// <remarks>The method estimates character width based on the provided font size. The result may not
    /// precisely match actual rendered width for all fonts or styles.</remarks>
    /// <param name="text">The text to be truncated if it exceeds the maximum width.</param>
    /// <param name="fontSize">The font size, in pixels, used to estimate the rendered width of the text.</param>
    /// <param name="maxWidth">The maximum allowed width, in pixels, for the rendered text including the ellipsis.</param>
    /// <returns>A string that fits within the specified maximum width. If truncation is necessary, the returned string ends with
    /// an ellipsis.</returns>
    private static string Ellipsize(string? text, double fontSize, double maxWidth)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        var charWidth = fontSize * 0.6;
        var ellipsisWidth = charWidth;

        if (text.Length * charWidth <= maxWidth)
        {
            return text;
        }

        var maxChars = (int)((maxWidth - ellipsisWidth) / charWidth);

        if (maxChars <= 0)
        {
            return "…";
        }

        return string.Concat(text.AsSpan(0, maxChars), "…");
    }
}
