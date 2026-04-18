using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Represents the chart layout engine responsible for calculating the layout of the chart.
/// </summary>
internal static class ChartLayoutEngine
{
    /// <summary>
    /// Calculates and assigns layout areas for chart elements such as the title, subtitle, legend, and plot area based
    /// on the provided chart context, theme, and configuration parameters.
    /// </summary>
    /// <param name="context">The chart context containing dimensions and layout information.</param>
    /// <param name="legendItemCount">The number of items in the legend.</param>
    /// <param name="legendPosition">The position of the legend relative to the chart area.</param>
    /// <param name="subtitle">The subtitle text of the chart.</param>
    /// <param name="subtitlePosition">The position of the subtitle relative to the chart area.</param>
    /// <param name="theme">The chart theme context containing layout and typography settings.</param>
    /// <param name="title">The title text of the chart.</param>
    /// <param name="titlePosition">The position of the title relative to the chart area.</param>
    public static void ComputeLayout(
        ChartContext context,
        ChartThemeContext theme,
        string? title,
        ChartTitlePosition titlePosition,
        string? subtitle,
        ChartTitlePosition subtitlePosition,
        int legendItemCount,
        ChartLegendPosition legendPosition)
    {
        var width = context.Width;
        var height = context.Height;
        var layout = theme.Theme.Layout;
        var typo = theme.Theme.Typography;

        var top = layout.PaddingTop;
        var bottom = layout.PaddingBottom;
        var left = layout.PaddingLeft;
        var right = layout.PaddingRight;

        var hasTitle = !string.IsNullOrWhiteSpace(title);
        var hasSubtitle = !string.IsNullOrWhiteSpace(subtitle);
        var hasLegend = legendItemCount > 0;

        // Forbidden rule: Subtitle.Top + Title.Bottom
        if (hasTitle && hasSubtitle &&
            titlePosition == ChartTitlePosition.Bottom &&
            subtitlePosition == ChartTitlePosition.Top)
        {
            throw new InvalidOperationException(
                "Subtitle cannot be at the top when Title is at the bottom.");
        }

        legendItemCount = Math.Min(legendItemCount, layout.MaxLegendItems);

        var titleHeight = hasTitle ? typo.Title.FontSize + layout.TitleSpacing : 0;
        var subtitleHeight = hasSubtitle ? typo.Subtitle.FontSize + layout.SubtitleSpacing : 0;
        var itemHeight = typo.Legend.FontSize + layout.LegendItemSpacing;
        var itemWidth =
            layout.LegendHorizontalPadding * 2 +
            layout.LegendShapeSize +
            layout.LegendShapeTextSpacing +
            layout.LegendMaxLabelCharacters * (typo.Legend.FontSize * 0.6);

        context.ChartArea = new ChartRect(0, 0, width, height);
        context.TitleArea = ChartRect.Empty;
        context.SubtitleArea = ChartRect.Empty;
        context.LegendArea = ChartRect.Empty;
        context.LegendItemCount = legendItemCount;

        switch (legendPosition)
        {
            case ChartLegendPosition.Left:
            case ChartLegendPosition.Right:
                LayoutLegendSide(
                    context,
                    width,
                    height,
                    ref top,
                    ref bottom,
                    ref left,
                    ref right,
                    hasTitle,
                    hasSubtitle,
                    hasLegend,
                    titlePosition,
                    subtitlePosition,
                    titleHeight,
                    subtitleHeight,
                    legendItemCount,
                    legendPosition,
                    layout,
                    typo);
                break;

            case ChartLegendPosition.Top:
                LayoutLegendTop(
                    context,
                    width,
                    height,
                    ref top,
                    ref bottom,
                    hasTitle,
                    hasSubtitle,
                    hasLegend,
                    titlePosition,
                    subtitlePosition,
                    titleHeight,
                    subtitleHeight,
                    itemHeight,
                    itemWidth,
                    legendItemCount);
                break;

            case ChartLegendPosition.Bottom:
                LayoutLegendBottom(
                    context,
                    width,
                    height,
                    ref top,
                    ref bottom,
                    hasTitle,
                    hasSubtitle,
                    hasLegend,
                    titlePosition,
                    subtitlePosition,
                    titleHeight,
                    subtitleHeight,
                    itemHeight,
                    itemWidth,
                    legendItemCount);
                break;
        }

        context.RemainingSpaceArea = new ChartRect(
            left,
            top,
            Math.Max(0, width - left - right),
            Math.Max(0, height - top - bottom),
            context.ChartArea);
    }

    /// <summary>
    /// Calculates and assigns the layout areas for the chart's title, subtitle, and legend when these elements are
    /// positioned on the sides of the chart.
    /// </summary>
    /// <remarks>This method updates the provided margin references to account for the space required by the
    /// title, subtitle, and legend, ensuring that the chart's plot area is correctly sized. It is intended for use
    /// during chart layout calculations and does not render any visual elements.</remarks>
    /// <param name="context">The chart context in which layout areas for the title, subtitle, and legend will be set.</param>
    /// <param name="width">The total width available for the chart layout, in device-independent units.</param>
    /// <param name="height">The total height available for the chart layout, in device-independent units.</param>
    /// <param name="top">A reference to the current top margin. Updated to reflect space consumed by title, subtitle, or legend at the
    /// top.</param>
    /// <param name="bottom">A reference to the current bottom margin. Updated to reflect space consumed by title or subtitle at the bottom.</param>
    /// <param name="left">A reference to the current left margin. Updated to reflect space consumed by the legend when positioned on the
    /// left.</param>
    /// <param name="right">A reference to the current right margin. Updated to reflect space consumed by the legend when positioned on the
    /// right.</param>
    /// <param name="hasTitle">true if the chart has a title to be displayed; otherwise, false.</param>
    /// <param name="hasSubtitle">true if the chart has a subtitle to be displayed; otherwise, false.</param>
    /// <param name="hasLegend">true if the chart has a legend to be displayed; otherwise, false.</param>
    /// <param name="titlePosition">The position of the chart title, indicating whether it appears at the top or bottom.</param>
    /// <param name="subtitlePosition">The position of the chart subtitle, indicating whether it appears at the top or bottom.</param>
    /// <param name="titleHeight">The height allocated for the chart title, in device-independent units.</param>
    /// <param name="subtitleHeight">The height allocated for the chart subtitle, in device-independent units.</param>
    /// <param name="legendItemCount">The total number of legend items to be displayed.</param>
    /// <param name="legendPosition">The position of the legend, indicating whether it appears on the left or right side of the chart.</param>
    /// <param name="layout">The layout configuration containing padding, sizing, and spacing information for the legend.</param>
    /// <param name="typo">The typography settings used for legend text, affecting label sizing.</param>
    private static void LayoutLegendSide(
        ChartContext context,
        double width,
        double height,
        ref double top,
        ref double bottom,
        ref double left,
        ref double right,
        bool hasTitle,
        bool hasSubtitle,
        bool hasLegend,
        ChartTitlePosition titlePosition,
        ChartTitlePosition subtitlePosition,
        double titleHeight,
        double subtitleHeight,
        int legendItemCount,
        ChartLegendPosition legendPosition,
        ChartLayout layout,
        ChartTypography typo)
    {
        if (hasTitle && titlePosition == ChartTitlePosition.Top)
        {
            context.TitleArea = new ChartRect(0, top, width, titleHeight);
            top += titleHeight;
        }

        if (hasSubtitle && subtitlePosition == ChartTitlePosition.Top)
        {
            context.SubtitleArea = new ChartRect(0, top, width, subtitleHeight);
            top += subtitleHeight;
        }

        if (hasLegend && legendItemCount > 0)
        {
            var legendWidth = Math.Max(
                layout.LegendMinWidth,
                (width - left - right) * 0.3
            );

            var textHeight = typo.Legend.FontSize * 1.2;
            var realItemHeight = Math.Max(layout.LegendShapeSize, textHeight) + layout.LegendVerticalPadding;

            var availableHeight = height - top - bottom;
            var maxVisibleItems = (int)Math.Floor(availableHeight / realItemHeight);
            maxVisibleItems = Math.Max(0, maxVisibleItems);

            var visibleCount = Math.Min(legendItemCount, maxVisibleItems);
            context.LegendItemCount = visibleCount;

            if (visibleCount > 0)
            {
                var legendHeight = visibleCount * realItemHeight;

                var legendX = legendPosition == ChartLegendPosition.Left
                    ? left
                    : width - right - legendWidth;

                context.LegendArea = new ChartRect(
                    legendX,
                    top,
                    legendWidth,
                    legendHeight
                );

                if (legendPosition == ChartLegendPosition.Left)
                {
                    left += legendWidth;
                }
                else
                {
                    right += legendWidth;
                }
            }
        }

        if (hasSubtitle && subtitlePosition == ChartTitlePosition.Bottom)
        {
            context.SubtitleArea = new ChartRect(
                0,
                height - bottom - subtitleHeight,
                width,
                subtitleHeight);

            bottom += subtitleHeight;
        }

        if (hasTitle && titlePosition == ChartTitlePosition.Bottom)
        {
            context.TitleArea = new ChartRect(
                0,
                height - bottom - titleHeight,
                width,
                titleHeight);

            bottom += titleHeight;
        }
    }

    /// <summary>
    /// Calculates and assigns the layout areas for the chart's title, subtitle, and legend when these elements are
    /// positioned at the top or bottom of the chart.
    /// </summary>
    /// <remarks>This method updates the layout areas in the provided chart context based on the presence and
    /// positioning of the title, subtitle, and legend. It ensures that elements do not overlap and that the legend is
    /// displayed with a maximum of four lines, adjusting the visible legend items as needed to fit the available
    /// space.</remarks>
    /// <param name="context">The chart context in which the layout areas for the title, subtitle, and legend will be set.</param>
    /// <param name="width">The total width available for the chart layout, in device-independent units.</param>
    /// <param name="height">The total height available for the chart layout, in device-independent units.</param>
    /// <param name="top">A reference to the current top offset. This value is updated to reflect the space consumed by top-positioned
    /// elements.</param>
    /// <param name="bottom">A reference to the current bottom offset. This value is updated to reflect the space consumed by
    /// bottom-positioned elements.</param>
    /// <param name="hasTitle">true if the chart includes a title; otherwise, false.</param>
    /// <param name="hasSubtitle">true if the chart includes a subtitle; otherwise, false.</param>
    /// <param name="hasLegend">true if the chart includes a legend; otherwise, false.</param>
    /// <param name="titlePosition">The position of the chart title, indicating whether it should be placed at the top or bottom of the chart.</param>
    /// <param name="subtitlePosition">The position of the chart subtitle, indicating whether it should be placed at the top or bottom of the chart.</param>
    /// <param name="titleHeight">The height allocated for the chart title, in device-independent units.</param>
    /// <param name="subtitleHeight">The height allocated for the chart subtitle, in device-independent units.</param>
    /// <param name="itemHeight">The height of each legend item, in device-independent units.</param>
    /// <param name="itemWidth">The width of each legend item, in device-independent units.</param>
    /// <param name="legendItemCount">The total number of legend items to be displayed in the legend area.</param>
    private static void LayoutLegendTop(
        ChartContext context,
        double width,
        double height,
        ref double top,
        ref double bottom,
        bool hasTitle,
        bool hasSubtitle,
        bool hasLegend,
        ChartTitlePosition titlePosition,
        ChartTitlePosition subtitlePosition,
        double titleHeight,
        double subtitleHeight,
        double itemHeight,
        double itemWidth,
        int legendItemCount)
    {
        // TOP : Title, Subtitle
        if (hasTitle && titlePosition == ChartTitlePosition.Top)
        {
            context.TitleArea = new ChartRect(0, top, width, titleHeight);
            top += titleHeight;
        }

        if (hasSubtitle && subtitlePosition == ChartTitlePosition.Top)
        {
            context.SubtitleArea = new ChartRect(0, top, width, subtitleHeight);
            top += subtitleHeight;
        }

        // Legend.Top : horizontal, max 4 lignes
        if (hasLegend && legendItemCount > 0)
        {
            var availableHeight = height - top - bottom;
            var maxLinesByHeight = (int)Math.Floor(availableHeight / itemHeight);
            maxLinesByHeight = Math.Max(0, maxLinesByHeight);

            if (maxLinesByHeight > 0)
            {
                var itemsPerLine = Math.Max(1, (int)Math.Floor(width / itemWidth));
                var maxLines = Math.Min(4, maxLinesByHeight);

                var maxVisibleItems = maxLines * itemsPerLine;
                var visibleCount = Math.Min(legendItemCount, maxVisibleItems);
                context.LegendItemCount = visibleCount;

                if (visibleCount > 0)
                {
                    var lineCount = (int)Math.Ceiling((double)visibleCount / itemsPerLine);
                    var legendHeight = lineCount * itemHeight;

                    context.LegendArea = new ChartRect(0, top, width, legendHeight);
                    top += legendHeight;
                }
            }
            else
            {
                context.LegendItemCount = 0;
            }
        }

        // BOTTOM : Subtitle, Title
        if (hasSubtitle && subtitlePosition == ChartTitlePosition.Bottom)
        {
            context.SubtitleArea = new ChartRect(
                0,
                height - bottom - subtitleHeight,
                width,
                subtitleHeight);

            bottom += subtitleHeight;
        }

        if (hasTitle && titlePosition == ChartTitlePosition.Bottom)
        {
            context.TitleArea = new ChartRect(
                0,
                height - bottom - titleHeight,
                width,
                titleHeight);

            bottom += titleHeight;
        }
    }

    /// <summary>
    /// Calculates and assigns layout areas for the chart's title, subtitle, and legend when the legend is positioned at
    /// the bottom of the chart.
    /// </summary>
    /// <remarks>This method updates the provided ChartContext with the calculated rectangles for the title,
    /// subtitle, and legend areas based on the specified layout parameters. It adjusts the top and bottom offsets to
    /// account for the space used by these elements, ensuring that the chart content is correctly positioned within the
    /// remaining area.</remarks>
    /// <param name="context">The chart context that receives the calculated layout areas for the title, subtitle, and legend.</param>
    /// <param name="width">The total width available for the chart layout, in device-independent units.</param>
    /// <param name="height">The total height available for the chart layout, in device-independent units.</param>
    /// <param name="top">A reference to the current top offset. This value is updated to reflect the space consumed by top-aligned
    /// elements.</param>
    /// <param name="bottom">A reference to the current bottom offset. This value is updated to reflect the space consumed by bottom-aligned
    /// elements.</param>
    /// <param name="hasTitle">true if the chart includes a title; otherwise, false.</param>
    /// <param name="hasSubtitle">true if the chart includes a subtitle; otherwise, false.</param>
    /// <param name="hasLegend">true if the chart includes a legend; otherwise, false.</param>
    /// <param name="titlePosition">The position of the chart title, indicating whether it is placed at the top or bottom of the chart.</param>
    /// <param name="subtitlePosition">The position of the chart subtitle, indicating whether it is placed at the top or bottom of the chart.</param>
    /// <param name="titleHeight">The height allocated for the chart title, in device-independent units.</param>
    /// <param name="subtitleHeight">The height allocated for the chart subtitle, in device-independent units.</param>
    /// <param name="itemHeight">The height of each legend item, in device-independent units.</param>
    /// <param name="itemWidth">The width of each legend item, in device-independent units.</param>
    /// <param name="legendItemCount">The total number of legend items to be displayed.</param>
    private static void LayoutLegendBottom(
        ChartContext context,
        double width,
        double height,
        ref double top,
        ref double bottom,
        bool hasTitle,
        bool hasSubtitle,
        bool hasLegend,
        ChartTitlePosition titlePosition,
        ChartTitlePosition subtitlePosition,
        double titleHeight,
        double subtitleHeight,
        double itemHeight,
        double itemWidth,
        int legendItemCount)
    {
        // TOP : Title, Subtitle
        if (hasTitle && titlePosition == ChartTitlePosition.Top)
        {
            context.TitleArea = new ChartRect(0, top, width, titleHeight);
            top += titleHeight;
        }

        if (hasSubtitle && subtitlePosition == ChartTitlePosition.Top)
        {
            context.SubtitleArea = new ChartRect(0, top, width, subtitleHeight);
            top += subtitleHeight;
        }

        // Calculate legend height based on available space and item count, with a maximum of 4 lines
        var legendHeight = 0.0;
        var visibleCount = 0;

        if (hasLegend && legendItemCount > 0)
        {
            var availableHeight = height - top - bottom;
            var maxLinesByHeight = (int)Math.Floor(availableHeight / itemHeight);
            maxLinesByHeight = Math.Max(0, maxLinesByHeight);

            if (maxLinesByHeight > 0)
            {
                var itemsPerLine = Math.Max(1, (int)Math.Floor(width / itemWidth));
                var maxLines = Math.Min(4, maxLinesByHeight);

                var maxVisibleItems = maxLines * itemsPerLine;
                visibleCount = Math.Min(legendItemCount, maxVisibleItems);
                context.LegendItemCount = visibleCount;

                if (visibleCount > 0)
                {
                    var lineCount = (int)Math.Ceiling((double)visibleCount / itemsPerLine);
                    legendHeight = lineCount * itemHeight;
                }
            }
            else
            {
                context.LegendItemCount = 0;
            }
        }

        // BOTTOM STACK (du bas vers le haut) : Subtitle.Bottom, Title.Bottom, Legend
        if (hasSubtitle && subtitlePosition == ChartTitlePosition.Bottom)
        {
            context.SubtitleArea = new ChartRect(
                0,
                height - bottom - subtitleHeight,
                width,
                subtitleHeight);

            bottom += subtitleHeight;
        }

        if (hasTitle && titlePosition == ChartTitlePosition.Bottom)
        {
            context.TitleArea = new ChartRect(
                0,
                height - bottom - titleHeight,
                width,
                titleHeight);

            bottom += titleHeight;
        }

        if (hasLegend && visibleCount > 0 && legendHeight > 0)
        {
            context.LegendArea = new ChartRect(
                0,
                height - bottom - legendHeight,
                width,
                legendHeight);

            bottom += legendHeight;
        }
    }
}
