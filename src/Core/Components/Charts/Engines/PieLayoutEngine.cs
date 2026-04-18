using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using PS = FluentUI.Blazor.Community.Components.Charts.Series.PieSerie;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Provides layout calculation functionality for pie chart components.
/// </summary>
/// <remarks>This class is intended for internal use within the charting infrastructure and is not intended to be
/// used directly from application code.</remarks>
internal static class PieLayoutEngine
{
    /// <summary>
    /// Calculates the layout of pie chart slices based on the provided data series, chart context, and rendering
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
        PS serie,
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

        var radius = Math.Min(plot.Width, plot.Height) / 2.0 * options.OuterRadius;
        var anglePerUnit = 360.0 / total;
        var slices = new List<ChartPieSlice>(items.Count);
        var currentAngle = 0.0;

        for (var index = 0; index < items.Count; index++)
        {
            var item = items[index];
            var sliceValue = item.Value;

            var start = currentAngle;
            var end = start + sliceValue * anglePerUnit;
            var mid = (start + end) / 2.0;

            ChartPoint? labelPos = null;

            if (options.ShowLabels || options.ShowPercentages)
            {
                var labelRadius = radius * 0.65;
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
                GroupId = serie.Id,
                InteractionState = item.InteractionState
            });

            currentAngle = end;
        }

        return slices;
    }
}
