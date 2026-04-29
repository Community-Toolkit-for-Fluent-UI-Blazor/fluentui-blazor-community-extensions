using FluentUI.Blazor.Community.Components.Charts.Drawing;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Represents the layout engine responsible for calculating the positions of points and the path for a category line series in a chart.
/// </summary>
internal static class CategoryLineLayoutEngine
{
    /// <summary>
    /// Calculates the layout for a category line series, generating the line path and corresponding point payloads for
    /// rendering in a chart.
    /// </summary>
    /// <remarks>The method sorts the series items according to the category axis options before mapping them
    /// to chart coordinates. The resulting path and points can be used for rendering smooth or straight lines,
    /// depending on the series options.</remarks>
    /// <param name="serie">The category line series containing the data points and options to be laid out.</param>
    /// <param name="context">The chart context providing axis mapping and rendering information.</param>
    /// <param name="options">The chart options containing default settings for category line series.</param>
    /// <returns>A tuple containing the generated line path payload and a read-only list of point payloads representing the
    /// mapped chart points.</returns>
    public static (LinePath Path, IReadOnlyList<LinePoint> Points) Layout(
        Series.LineSerie serie,
        ChartContext context,
        CO options)
    {
        var categoryOptions = options.CategoryLineOptions;
        var items = CategoryAxisEngine.Sort(serie.Items, categoryOptions);
        var points = new List<ChartPoint>(items.Count);
        var pointPayloads = new List<LinePoint>(items.Count);

        for (var i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var x = context.XAxis!.Map(i);
            var y = context.YAxis!.Map(item.Value);
            points.Add(new ChartPoint(x, y));

            pointPayloads.Add(new LinePoint
            {
                Id = item.Id ?? $"pt-{Guid.NewGuid()}",
                X = x,
                Y = y,
                CategoryIndex = i,
                Value = item.Value
            });
        }

        var path = new LinePath
        {
            Id = serie.Id ?? $"line-{Guid.NewGuid()}",
            Points = points,
            Smooth = categoryOptions.Smooth
        };

        return (path, pointPayloads);
    }
}
