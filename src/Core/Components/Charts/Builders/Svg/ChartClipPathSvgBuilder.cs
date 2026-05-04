using FluentUI.Blazor.Community.Components.Charts.Payloads;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Represents a builder for generating SVG markup for bar chart layers.
/// </summary>
internal static class ChartClipPathSvgBuilder
{
    /// <summary>
    /// Builds the SVG elements representing a collection of columns and adds them to the specified SVG builder.
    /// </summary>
    /// <remarks>This method groups all rendered columns within a single SVG group element to optimize DOM
    /// diffing and enable smoother animations.</remarks>
    /// <param name="svg">The SVG builder to which the bar elements will be added. Cannot be null.</param>
    /// <param name="clipPath">The payload containing the clip path's position, size.</param>
    public static void Build(
        SvgBuilder svg,
        ClipPathPayload clipPath)
    {
        if (clipPath == null)
        {
            return;
        }

        var group = svg.AddClipPath()
                       .WithId(clipPath.Id);

        group.AddRect(clipPath.X,
                      clipPath.Y,
                      clipPath.Width,
                      clipPath.Height)
            .WithFill("transparent")
             .Close();

        group.Close();
    }
}
