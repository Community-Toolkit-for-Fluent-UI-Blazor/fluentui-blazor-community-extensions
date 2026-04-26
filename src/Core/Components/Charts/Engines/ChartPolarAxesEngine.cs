using System.Globalization;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Helpers;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;
using CAO = FluentUI.Blazor.Community.Components.Charts.Options.ChartAxisOptions;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal static class ChartPolarAxesEngine
{
    public static PolarAxesPayload? Build(
        ChartContext ctx,
        CAO options,
        CO chartOptions,
        PolarChartType polarChartType)
    {
        if (!options.Show ||
            ctx.XAxis is null ||
            ctx.YAxis is null)
        {
            return null;
        }

        var angleAxis = ctx.XAxis;
        var radiusAxis = ctx.YAxis;

        return new PolarAxesPayload
        {
            RadialAxes = BuildRadialAxes(ctx, angleAxis),
            ConcentricGrid = BuildConcentricGrid(ctx, radiusAxis, options, chartOptions),
            AngleLabels = BuildAngleLabels(ctx, angleAxis, options),
            RadiusLabels = BuildRadiusLabels(ctx, radiusAxis, options, chartOptions),
            Grid = chartOptions.DefaultRadarAxesOptions.Grid,
            ChartType = polarChartType
        };
    }

    private static List<PolarAxisLinePayload> BuildRadialAxes(
        ChartContext ctx,
        ChartAxis angleAxis)
    {
        var list = new List<PolarAxisLinePayload>();
        var (cx, cy, maxRadius) = PolarHelper.GetPolarFrame(ctx);

        var count = angleAxis.Labels?.Count ?? 0;

        for (var i = 0; i < count; i++)
        {
            var angle = angleAxis.Map(i);
            var r = maxRadius;

            var x = cx + r * Math.Cos(angle);
            var y = cy - r * Math.Sin(angle);

            list.Add(new PolarAxisLinePayload
            {
                Start = new ChartPoint(cx, cy),
                End = new ChartPoint(x, y)
            });
        }

        return list;
    }

    private static List<PolarGridCirclePayload> BuildConcentricGrid(
        ChartContext ctx,
        ChartAxis radiusAxis,
        CAO options,
        CO chartOptions)
    {
        var list = new List<PolarGridCirclePayload>();
        var (cx, cy, maxRadius) = PolarHelper.GetPolarFrame(ctx);
        var maxTicks = chartOptions.DefaultRadarAxesOptions.GridLevels ?? options.MaxTicks;
        var ticks = NumericTickGenerator.GenerateNice(0, radiusAxis.DataMaximum, maxTicks);
        var max = ticks.Count > 0 ? ticks[^1] : 0;
        ctx.PolarNiceMax = max;

        foreach (var v in ticks)
        {
            var t = v / max;
            var r = t * maxRadius;

            list.Add(new PolarGridCirclePayload
            {
                Center = new ChartPoint(cx, cy),
                Radius = r
            });
        }

        return list;
    }

    private static List<PolarLabelPayload> BuildAngleLabels(
        ChartContext ctx,
        ChartAxis angleAxis,
        CAO options)
    {
        var list = new List<PolarLabelPayload>();
        var plot = ctx.PlotArea;
        var count = angleAxis.Labels?.Count ?? 0;
        var (cx, cy, maxRadius) = PolarHelper.GetPolarFrame(ctx);
        var r = maxRadius + options.LabelOffset;

        for (var i = 0; i < count; i++)
        {
            var angle = angleAxis.Map(i);
            var label = angleAxis.Labels![i];

            var x = cx + r * Math.Cos(angle);
            var y = cy - r * Math.Sin(angle);

            list.Add(new PolarLabelPayload
            {
                Text = label,
                Position = new ChartPoint(x, y),
                Anchor = SvgTextAnchor.Middle,
                FontSize = options.LabelOptions?.FontSize ?? 12
            });
        }

        return list;
    }

    private static List<PolarLabelPayload> BuildRadiusLabels(
        ChartContext ctx,
        ChartAxis radiusAxis,
        CAO options,
        CO chartOptions)
    {
        var maxTicks = chartOptions.DefaultRadarAxesOptions.GridLevels ?? options.MaxTicks;
        var list = new List<PolarLabelPayload>();
        var (cx, cy, maxRadius) = PolarHelper.GetPolarFrame(ctx);
        var ticks = NumericTickGenerator.GenerateNice(0, radiusAxis.DataMaximum, maxTicks);
        var max = ticks.Count > 0 ? ticks[^1] : 0;

        foreach (var v in ticks)
        {
            var t = v / max;
            var r = t * maxRadius;

            list.Add(new PolarLabelPayload
            {
                Text = v.ToString(CultureInfo.InvariantCulture),
                Position = new ChartPoint(cx, cy - r),
                Anchor = SvgTextAnchor.Start,
                FontSize = options.LabelOptions?.FontSize ?? 12
            });
        }

        return list;
    }
}
