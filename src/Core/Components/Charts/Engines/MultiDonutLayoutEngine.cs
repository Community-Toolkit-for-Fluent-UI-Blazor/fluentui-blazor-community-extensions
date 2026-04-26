using FluentUI.Blazor.Community.Components.Charts.Drawing;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;
using MDS = FluentUI.Blazor.Community.Components.Charts.Series.MultiDonutSerie;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// 
/// </summary>
/// <param name="Center"></param>
/// <param name="OuterRadius"></param>
/// <param name="InnerRadius"></param>
/// <param name="Slices"></param>
internal sealed record DonutRingLayout(
    ChartPoint Center,
    double OuterRadius,
    double InnerRadius,
    IReadOnlyList<ChartPieSlice> Slices);

/// <summary>
/// Represents the layout engine responsible for calculating the positions and dimensions
///  of concentric donut chart rings based on the provided data series and chart context.
/// </summary>
internal static class MultiDonutLayoutEngine
{
    /// <summary>
    /// Calculates the layout for each visible donut ring in a multi-series donut chart, determining the position and
    /// geometry of each ring and its slices based on the provided data and chart context.
    /// </summary>
    /// <remarks>Each ring corresponds to a visible series in the input, and only series and items marked as
    /// visible with positive values are included in the layout. The method reserves a margin within the plot area to
    /// ensure rings do not touch the chart edges. Label positions are calculated if label or percentage display options
    /// are enabled for a series.</remarks>
    /// <param name="serie">The multi-series donut data to be rendered. Must not be null and must contain at least one visible series with
    /// non-zero values.</param>
    /// <param name="options">The chart options containing configuration settings for the donut layout, such as inner radius factors and label display options. Must not be null.</param>
    /// <param name="context">The chart context that defines the plot area and layout parameters. Must not be null.</param>
    /// <returns>A read-only list of donut ring layouts, each describing the geometry and slice arrangement for a visible series.
    /// Returns an empty list if there are no visible series with data.</returns>
    public static IReadOnlyList<DonutRingLayout> Layout(
        MDS serie,
        ChartContext context,
        CO options)
    {
        ArgumentNullException.ThrowIfNull(serie);
        ArgumentNullException.ThrowIfNull(context);

        if (serie.Series.Count == 0)
        {
            return [];
        }

        var plot = context.PlotArea;
        var center = new ChartPoint(
            plot.X + plot.Width / 2.0,
            plot.Y + plot.Height / 2.0
        );

        var maxRadius = Math.Min(plot.Width, plot.Height) / 2.0;
        var ringCount = serie.Series.Count;
        var usableRadius = maxRadius * 0.9;
        var bandThickness = usableRadius / ringCount;
        var rings = new List<DonutRingLayout>(ringCount);
        var opts = options.DefaultDonutOptions;

        for (var index = 0; index < ringCount; index++)
        {
            var donutSerie = serie.Series[index];

            if (!donutSerie.IsVisible)
            {
                continue;
            }

            var items = donutSerie.Items.Where(i => i.IsVisible).ToList();

            if (items.Count == 0)
            {
                continue;
            }

            var total = items.Sum(i => i.Value);

            if (total <= 0)
            {
                continue;
            }

            var outerRadius = usableRadius - index * bandThickness;
            var innerFactor = opts.InnerRadius <= 0 ? 0.5 : opts.InnerRadius;
            var innerRadius = outerRadius * innerFactor;
            var anglePerUnit = 360.0 / total;
            var slices = new List<ChartPieSlice>(items.Count);
            var currentAngle = 0.0;

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var sliceValue = item.Value;
                var start = currentAngle;
                var end = start + sliceValue * anglePerUnit;
                var mid = (start + end) / 2.0;

                ChartPoint? labelPos = null;

                if (opts.ShowLabels ||
                    opts.ShowPercentages)
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
                    Index = i,
                    Value = sliceValue,
                    StartAngle = start,
                    EndAngle = end,
                    MidAngle = mid,
                    LabelPosition = labelPos,
                    GroupId = donutSerie.Id,
                    InteractionState = item.InteractionState
                });

                currentAngle = end;
            }

            rings.Add(new DonutRingLayout(
                Center: center,
                OuterRadius: outerRadius,
                InnerRadius: innerRadius,
                Slices: slices));
        }

        return rings;
    }
}
