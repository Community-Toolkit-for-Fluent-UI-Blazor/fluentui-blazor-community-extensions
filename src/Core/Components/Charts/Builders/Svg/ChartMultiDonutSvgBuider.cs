using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Renders a multi-donut chart by delegating the rendering of each donut ring
/// to the <see cref="ChartDonutSvgBuilder"/>.
/// </summary>
internal static class ChartMultiDonutSvgBuilder
{
    /// <summary>
    /// Builds the SVG elements for a multi-donut chart layer.
    /// </summary>
    /// <param name="svg">The SVG builder used to construct the markup.</param>
    /// <param name="payload">The payload containing all donut rings to render.</param>
    /// <param name="context">The theme context providing styling information for the chart.</param>
    public static void Build(
        SvgBuilder svg,
        MultiDonutPayloadCollection payload,
        ChartThemeContext context)
    {
        if (payload.Rings.Count == 0)
        {
            return;
        }

        foreach (var ring in payload.Rings)
        {
            ChartDonutSvgBuilder.Build(svg, ring, context, false);
        }

        foreach (var ring in payload.Rings)
        {
            ChartDonutSvgBuilder.RenderLabels(svg, ring, context);
        }
    }
}
