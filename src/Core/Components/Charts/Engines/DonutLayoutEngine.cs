using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using DS = FluentUI.Blazor.Community.Components.Charts.Series.DonutSerie;

/// <summary>
/// Provides methods for calculating the layout of donut chart slices based on data series, chart context, and rendering
/// options.
/// </summary>
/// <remarks>This class is intended for internal use in chart rendering scenarios where donut or pie chart layouts
/// are required. It positions each slice proportionally to its value and determines label positions when label or
/// percentage display is enabled. The layout is calculated relative to the plot area defined in the chart
/// context.</remarks>
internal static class DonutLayoutEngine
{
    /// <summary>
    /// Calculates the layout of donut chart slices based on the provided data series, chart context, and rendering
    /// options.
    /// </summary>
    /// <remarks>The method positions each slice proportionally to its value and calculates label positions if
    /// label or percentage display is enabled in the options. The layout is determined relative to the plot area
    /// defined in the chart context.</remarks>
    /// <param name="serie">The data series containing the items to be represented as slices in the pie chart. Only visible items are
    /// included in the layout calculation.</param>
    /// <param name="context">The chart context that provides information about the plot area and overall chart dimensions.</param>
    /// <param name="options">The options that control the appearance and behavior of the pie chart, such as outer radius and label display
    /// settings.</param>
    /// <returns>A read-only list of ChartPieSlice objects representing the calculated slices of the pie chart. Returns an empty
    /// list if there are no visible items or if the total value is less than or equal to zero.</returns>

    public static IReadOnlyList<ChartPieSlice> Layout(
        DS serie,
        ChartContext context,
        RadialSerieOptions options)
    {
        ArgumentNullException.ThrowIfNull(serie);
        ArgumentNullException.ThrowIfNull(context);

        var items = serie.Items.Where(i => i.IsVisible).ToList();

        if (items.Count == 0)
        {
            return [];
        }

        var total = items.Sum(i => i.Value);

        if (total <= 0)
        {
            return [];
        }

        var plot = context.PlotArea;
        var center = new ChartPoint(
            plot.X + plot.Width / 2.0,
            plot.Y + plot.Height / 2.0
        );

        var outerRadius = Math.Min(plot.Width, plot.Height) / 2.0 * options.OuterRadius;
        var innerRadius = outerRadius * options.InnerRadius;
        var sweep = options.EndAngle - options.StartAngle;
        var anglePerUnit = sweep / total;
        var currentAngle = options.StartAngle;
        var slices = new List<ChartPieSlice>(items.Count);

        for (var index = 0; index < items.Count; index++)
        {
            var item = items[index];
            var sliceValue = item.Value;
            var start = currentAngle;
            var end = start + sliceValue * anglePerUnit;
            var mid = (start + end) / 2.0;

            ChartPoint? labelPos = null;

            if (options.ShowLabels ||
                options.ShowPercentages)
            {
                var labelRadius = (innerRadius + outerRadius) / 2.0;
                var rad = mid * Math.PI / 180.0;

                labelPos = new ChartPoint(
                    center.X + Math.Cos(rad) * labelRadius,
                    center.Y + Math.Sin(rad) * labelRadius
                );
            }

            slices.Add(new ChartPieSlice
            {
                Id = item.Id ?? $"slice-{Guid.NewGuid()}",
                Index = index,
                Value = sliceValue,
                StartAngle = start,
                EndAngle = end,
                MidAngle = mid,
                LabelPosition = labelPos,
                GroupId = item.SerieId,
                InteractionState = item.InteractionState
            });

            currentAngle = end;
        }

        return slices;
    }
}
