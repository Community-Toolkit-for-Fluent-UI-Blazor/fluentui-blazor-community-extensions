using System.Globalization;
using System.Text;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a base class for building SVG document payloads from a specified surface payload and export options.
/// </summary>
/// <remarks>This abstract class defines the contract for constructing SVG documents by implementing the
/// WriteContent method. Derived classes should provide the logic for writing SVG content based on the payload and
/// export options. The class is intended for scenarios where SVG export functionality is required for custom payload
/// types.</remarks>
/// <typeparam name="TPayload">The type of the payload data used to generate the SVG content.</typeparam>
public abstract class SvgDocumentPayloadBuilderBase<TPayload>
    : IDocumentPayloadBuilder<TPayload>
{
    /// <summary>
    /// Provides a default culture that uses invariant culture settings for formatting and parsing operations.
    /// </summary>
    /// <remarks>This static field is intended for scenarios where culture-independent behavior is required,
    /// such as consistent formatting of numbers and dates regardless of the user's locale.</remarks>
    protected static readonly CultureInfo s_culture = CultureInfo.InvariantCulture;

    /// <inheritdoc />
    public ValueTask<byte[]> BuildAsync(
        SurfacePayload<TPayload> payload,
        SurfaceExportOptions options)
    {
        var sb = new StringBuilder();

        var width = payload.View?.Width ?? 800;
        var height = payload.View?.Height ?? 600;

        sb.Append(
            CultureInfo.InvariantCulture,
            $"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{width}\" height=\"{height}\"");

        if (options.IncludeView)
        {
            WriteView(sb, payload.View);
        }

        sb.AppendLine(">");

        // BACK LAYER
        if (options.IncludeBackground)
        {
            WriteBackground(sb, payload.Background);
        }

        if (options.IncludeGrid)
        {
            WriteGrid(sb, payload.Grid, GridLayer.Background, width, height);
        }

        if (options.IncludeAxes)
        {
            WriteAxes(sb, payload.Axes, GridLayer.Background, width, height);
        }

        // CONTENT
        if (payload.Content is not null)
        {
            WriteContent(sb, payload.Content, options);
        }

        // FRONT LAYER
        if (options.IncludeGrid)
        {
            WriteGrid(sb, payload.Grid, GridLayer.Foreground, width, height);
        }

        if (options.IncludeAxes)
        {
            WriteAxes(sb, payload.Axes, GridLayer.Foreground, width, height);
        }

        if (options.IncludeWatermark)
        {
            WriteWatermark(sb, payload.Watermark, width, height);
        }

        sb.AppendLine("</svg>");

        return ValueTask.FromResult(Encoding.UTF8.GetBytes(sb.ToString()));
    }

    /// <summary>
    /// Adds the data attributes representing the view properties to the SVG element being constructed in the specified string builder.
    /// </summary>
    /// <param name="sb">The string builder to which the SVG background element will be appended.</param>
    /// <param name="view">The view payload containing width, height, offset, rendersize, dpi and scale information.</param>
    private static void WriteView(StringBuilder sb, ViewPayload? view)
    {
        sb.Append(s_culture, $" viewBox=\"{0} {0} {view?.Width} {view?.Height}\"");
        sb.Append(s_culture, $" data-view-render-width = \"{view?.RenderWidth}\"");
        sb.Append(s_culture, $" data-view-render-height = \"{view?.RenderHeight}\"");
        sb.Append(s_culture, $" data-view-scale = \"{view?.Scale}\"");
        sb.Append(s_culture, $" data-view-dpi = \"{view?.Dpi}\"");
        sb.Append(s_culture, $" data-view-offsetX = \"{view?.OffsetX}\"");
        sb.Append(s_culture, $" data-view-offsetY = \"{view?.OffsetY}\"");
    }

    /// <summary>
    /// Appends an SVG rectangle element representing the background to the specified string builder.
    /// </summary>
    /// <remarks>If the background color is not specified, the method uses white ('#FFFFFF') as the default
    /// fill color.</remarks>
    /// <param name="sb">The string builder to which the SVG background element will be appended.</param>
    /// <param name="bg">The background payload containing color information. If null, no background is written.</param>
    protected virtual void WriteBackground(StringBuilder sb, BackgroundPayload? bg)
    {
        if (bg is null)
        {
            return;
        }

        var color = bg.Color ?? "#FFFFFF";
        sb.AppendLine(s_culture, $"<rect id=\"surface-background\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" fill=\"{color}\" />");
    }

    /// <summary>
    /// Renders a grid overlay to the specified StringBuilder for the given layer, if the grid is visible and matches
    /// the layer.
    /// </summary>
    /// <remarks>This method appends SVG markup representing grid lines to the StringBuilder. The grid is
    /// rendered only if it is visible and its layer matches the specified layer. Override this method to customize grid
    /// rendering behavior.</remarks>
    /// <param name="sb">The StringBuilder to which the grid SVG markup will be appended.</param>
    /// <param name="grid">The grid payload containing grid configuration and visibility information. May be null.</param>
    /// <param name="layer">The layer for which the grid should be rendered. The grid is rendered only if its layer matches this value.</param>
    /// <param name="height">The height of the grid area, used to determine the length of vertical grid lines.</param>
    /// <param name="width">The width of the grid area, used to determine the length of horizontal grid lines.</param>
    protected virtual void WriteGrid(
        StringBuilder sb,
        GridPayload? grid,
        GridLayer layer,
        double width,
        double height)
    {
        if (grid is null || grid.DisplayMode == GridDisplayMode.None)
        {
            return;
        }

        if (grid.Layer != layer)
        {
            return;
        }

        sb.Append(s_culture, $"<g id=\"surface-grid-{grid.Layer.ToString().ToLowerInvariant()}\"");
        sb.Append(s_culture, $"stroke=\"{grid.Color}\" ");
        sb.Append(s_culture, $"stroke-width=\"{grid.StrokeWidth.ToString(s_culture)}\" ");
        sb.Append(s_culture, $"opacity=\"{grid.Opacity.ToString(s_culture)}\" ");

        if (grid.DashArray.Length > 0)
        {
            sb.Append(s_culture, $"stroke-dasharray=\"{string.Join(",", grid.DashArray)}\" ");
        }

        sb.Append(s_culture, $"data-display-mode=\"{grid.DisplayMode}\" ");
        sb.Append(s_culture, $"data-cell-size=\"{grid.CellSize}\" ");
        sb.Append(s_culture, $"data-bold-every=\"{grid.BoldEvery}\" ");
        sb.Append(s_culture, $"data-point-radius=\"{grid.PointRadius}\" ");

        sb.AppendLine(">");

        if (grid.DisplayMode == GridDisplayMode.Lines)
        {
            var index = 0;

            for (var x = 0.0; x <= width; x += grid.CellSize, index++)
            {
                var strokeWidth = (index % grid.BoldEvery == 0)
                    ? grid.StrokeWidth * 2
                    : grid.StrokeWidth;

                sb.AppendLine(
                    s_culture,
                    $"<line x1=\"{x}\" y1=\"0\" x2=\"{x}\" y2=\"{height}\" stroke-width=\"{strokeWidth}\" />");
            }

            index = 0;

            for (var y = 0.0; y <= height; y += grid.CellSize, index++)
            {
                var strokeWidth = (index % grid.BoldEvery == 0)
                    ? grid.StrokeWidth * 2
                    : grid.StrokeWidth;

                sb.AppendLine(
                    s_culture,
                    $"<line x1=\"0\" y1=\"{y}\" x2=\"{width}\" y2=\"{y}\" stroke-width=\"{strokeWidth}\" />");
            }
        }
        else if (grid.DisplayMode == GridDisplayMode.Dots)
        {
            for (var x = 0.0; x <= width; x += grid.CellSize)
            {
                for (var y = 0.0; y <= height; y += grid.CellSize)
                {
                    sb.AppendLine(
                        s_culture,
                        $"<circle cx=\"{x}\" cy=\"{y}\" r=\"{grid.PointRadius}\" fill=\"{grid.Color}\" opacity=\"{grid.Opacity}\" />");
                }
            }
        }

        sb.AppendLine("</g>");
    }

    /// <summary>
    /// Writes SVG axis lines to the specified string builder for the given grid layer, if axes are visible and match
    /// the layer.
    /// </summary>
    /// <remarks>This method appends SVG markup for horizontal and vertical axes centered within the specified
    /// grid dimensions. Axes are rendered only if the payload is visible and corresponds to the provided
    /// layer.</remarks>
    /// <param name="sb">The string builder to which the SVG axis lines are written.</param>
    /// <param name="axes">The axes payload containing visibility and layer information. If null or not visible, no axes are written.</param>
    /// <param name="layer">The grid layer for which axes should be rendered. Axes are written only if their layer matches this value.</param>
    /// <param name="width">The width of the grid area, used to determine the position of the vertical axis line.</param>
    /// <param name="height">The height of the grid area, used to determine the position of the horizontal axis line.</param>
    protected virtual void WriteAxes(
        StringBuilder sb,
        AxesPayload? axes,
        GridLayer layer,
        double width,
        double height)
    {
        if (axes is null)
        {
            return;
        }

        if (axes.Layer != layer)
        {
            return;
        }

        var originX = width / 2.0;
        var originY = height / 2.0;

        sb.Append(s_culture, $"<g id=\"surface-axes-{layer.ToString().ToLowerInvariant()}\"");
        sb.Append(s_culture, $"stroke=\"{axes.Color}\" ");
        sb.Append(s_culture, $"stroke-width=\"{axes.StrokeWidth.ToString(s_culture)}\" ");
        sb.Append(s_culture, $"opacity=\"{axes.Opacity.ToString(s_culture)}\" ");

        if (axes.DashArray.Length > 0)
        {
            sb.Append(s_culture, $"stroke-dasharray=\"{string.Join(",", axes.DashArray)}\" ");
        }

        sb.AppendLine(">");

        sb.AppendLine(
            s_culture,
            $"<line x1=\"0\" y1=\"{originY}\" x2=\"{width}\" y2=\"{originY}\" />");

        sb.AppendLine(
            s_culture,
            $"<line x1=\"{originX}\" y1=\"0\" x2=\"{originX}\" y2=\"{height}\" />");

        sb.AppendLine("</g>");
    }

    /// <summary>
    /// Writes a watermark as an SVG text element to the specified string builder if the watermark payload is valid.
    /// </summary>
    /// <remarks>The watermark is rendered as an SVG text element using the properties provided in the
    /// payload. The text is escaped to ensure proper SVG formatting.</remarks>
    /// <param name="sb">The string builder to which the SVG watermark text element will be appended.</param>
    /// <param name="wm">The watermark payload containing the text and formatting information. If null or the text is empty, no watermark
    /// is written.</param>
    /// <param name="height">The height of the area for which the watermark is being rendered, used to determine positioning.</param>
    /// <param name="width">The width of the area for which the watermark is being rendered, used to determine positioning.</param>
    protected virtual void WriteWatermark(
        StringBuilder sb,
        WatermarkPayload? wm,
        double width,
        double height)
    {
        if (wm is null)
        {
            return;
        }

        var hasText = !string.IsNullOrWhiteSpace(wm.Text);
        var hasImage = !string.IsNullOrWhiteSpace(wm.ImageUrl);

        if (!hasText && !hasImage)
        {
            return;
        }

        sb.Append("<g id=\"surface-watermark\"");
        sb.Append(s_culture, $"opacity=\"{wm.Opacity.ToString(s_culture)}\" ");
        sb.Append(s_culture, $"data-reapeat=\"{wm.Repeat}\" ");
        sb.Append(s_culture, $"data-mode=\"{wm.Mode}\" ");
        sb.Append(s_culture, $"data-color=\"{wm.Color}\" ");
        sb.Append(s_culture, $"data-text=\"{wm.Text}\" ");
        sb.Append(s_culture, $"data-text-opacity=\"{wm.TextOpacity}\" ");
        sb.Append(s_culture, $"data-font-size=\"{wm.FontSize}\" ");
        sb.Append(s_culture, $"data-font-family=\"{wm.FontFamily}\" ");
        sb.Append(s_culture, $"data-font-weight=\"{wm.FontWeight}\" ");
        sb.Append(s_culture, $"data-letter-spacing=\"{wm.LetterSpacing}\" ");
        sb.Append(s_culture, $"data-scale=\"{wm.Scale}\" ");
        sb.Append(s_culture, $"data-rotation=\"{wm.Rotation}\" ");
        sb.Append(s_culture, $"data-url=\"{wm.ImageUrl}\" ");
        sb.Append(s_culture, $"data-image-opacity=\"{wm.ImageOpacity}\" ");
        sb.Append(s_culture, $"data-position-x=\"{wm.PositionX}\" ");
        sb.Append(s_culture, $"data-position-y=\"{wm.PositionY}\" ");
        sb.Append(s_culture, $"data-repeat-spacing-x=\"{wm.RepeatSpacingX}\" ");
        sb.Append(s_culture, $"data-repeat-spacing-y=\"{wm.RepeatSpacingY}\" ");
        sb.Append(s_culture, $"data-horizontal-alignement=\"{wm.HorizontalAlignment}\" ");
        sb.Append(s_culture, $"data-vertical-alignement=\"{wm.VerticalAlignment}\" ");
        sb.Append(s_culture, $"data-visual-bias=\"{wm.VisualBias}\" ");

        sb.AppendLine(">");

        void DrawSingle(double x, double y)
        {
            sb.AppendLine("<g ");

            sb.Append(s_culture, $"transform=\"translate({x},{y}) ");

            if (wm.Rotation != 0)
            {
                sb.Append(s_culture, $"rotate({wm.Rotation.ToString(s_culture)}) ");
            }

            if (wm.Scale != 1)
            {
                sb.Append(s_culture, $"scale({wm.Scale.ToString(s_culture)}) ");
            }

            sb.AppendLine("\">");

            if (hasImage)
            {
                sb.AppendLine(s_culture, $"<image href=\"{wm.ImageUrl}\" ")
                  .AppendLine(s_culture, $"opacity=\"{wm.ImageOpacity.ToString(s_culture)}\" ")
                  .AppendLine(s_culture, $"x=\"0\" y=\"0\" ")
                  .AppendLine(s_culture, $"preserveAspectRatio=\"none\" />");
            }

            if (hasText)
            {
                sb.Append("<text ");

                sb.Append(s_culture, $"fill=\"{wm.Color}\" ");
                sb.Append(s_culture, $"opacity=\"{wm.TextOpacity.ToString(s_culture)}\" ");
                sb.Append(s_culture, $"font-size=\"{wm.FontSize.ToString(s_culture)}\" ");
                sb.Append(s_culture, $"font-family=\"{wm.FontFamily}\" ");
                sb.Append(s_culture, $"font-weight=\"{wm.FontWeight}\" ");

                if (wm.LetterSpacing != 0)
                {
                    sb.Append(s_culture, $"letter-spacing=\"{wm.LetterSpacing}\" ");
                }

                sb.Append(wm.HorizontalAlignment switch
                {
                    0 => "text-anchor=\"start\" ",   // left
                    1 => "text-anchor=\"middle\" ",  // center
                    2 => "text-anchor=\"end\" ",     // right
                    _ => ""
                });

                sb.AppendLine(">");
                sb.AppendLine(System.Security.SecurityElement.Escape(wm.Text));

                sb.AppendLine("</text>");
            }

            sb.AppendLine("</g>");
        }

        if (wm.Repeat)
        {
            for (var x = wm.PositionX; x < width; x += wm.RepeatSpacingX)
            {
                for (var y = wm.PositionY; y < height; y += wm.RepeatSpacingY)
                {
                    DrawSingle(x, y);
                }
            }
        }
        else
        {
            DrawSingle(wm.PositionX, wm.PositionY);
        }

        sb.AppendLine("</g>");
    }

    /// <summary>
    /// Writes the formatted content representing the specified payload to the provided string builder using the given
    /// export options.
    /// </summary>
    /// <remarks>Implementations should ensure that content is written in accordance with the specified export
    /// options. The method does not clear or reset the string builder; content is appended to its existing
    /// value.</remarks>
    /// <param name="sb">The string builder to which the formatted content will be appended. Must not be null.</param>
    /// <param name="payload">The payload containing the data to be exported. Must not be null.</param>
    /// <param name="options">The export options that control formatting and serialization behavior. Must not be null.</param>
    protected abstract void WriteContent(
        StringBuilder sb,
        TPayload payload,
        SurfaceExportOptions options);
}
