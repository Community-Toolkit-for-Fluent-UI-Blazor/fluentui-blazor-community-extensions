using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using FluentUI.Blazor.Community.Components.Enums;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;
using DS = FluentUI.Blazor.Community.Components.Charts.Series.DonutSerie;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

/// <summary>
/// Represents a composer responsible for generating the necessary layers to render donuts charts.
/// </summary>
/// <param name="chartId">The unique identifier of the chart, used for interaction payloads.</param>
/// <param name="context">The chart context for composition.</param>
/// <param name="series">The collection of donut series to be rendered.</param>
internal sealed class ChartDonutComposer(
    string chartId,
    Func<ChartContext> context,
    Func<IEnumerable<DS>> series)
    : ISurfaceComposer<CO>
{
    /// <inheritdoc />
    public void Compose(
        ISurfaceRenderTarget target,
        CO chartOptions)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(chartOptions);

        var filtered = series().Where(s => s.IsVisible && !s.IsPartOfMultiDonut).ToList();

        if (filtered.Count == 0)
        {
            return;
        }

        var defaults = chartOptions.DefaultDonutStyle;
        var ctx = context();

        for (var serieIndex = 0; serieIndex < filtered.Count; serieIndex++)
        {
            var serie = filtered[serieIndex];
            var opts = serie.Options ?? new RadialSerieOptions();
            var slices = DonutLayoutEngine.Layout(serie, ctx, opts);

            if (slices.Count == 0)
            {
                continue;
            }

            var plot = ctx.PlotArea;
            var center = new ChartPoint(
                plot.X + plot.Width / 2.0,
                plot.Y + plot.Height / 2.0
            );

            var rawInnerRadius = opts.InnerRadius == 0 ? 0.1 : opts.InnerRadius;
            var outerRadius = Math.Min(plot.Width, plot.Height) / 2.0 * opts.OuterRadius;
            var innerRadius = outerRadius * rawInnerRadius;
            var total = serie.Items.Where(i => i.IsVisible).Sum(i => i.Value);
            var payloads = new List<PiePayload>(slices.Count);

            for (var i = 0; i < slices.Count; i++)
            {
                var slice = slices[i];
                var item = serie.Items[slice.Index];
                var normal = ChartStyleResolver.Resolve(item.Style?.Normal, defaults.Normal);
                var hover = ChartStyleResolver.Resolve(item.Style?.Hover, defaults.Hover);
                var pressed = ChartStyleResolver.Resolve(item.Style?.Pressed, defaults.Pressed);
                var selected = ChartStyleResolver.Resolve(item.Style?.Selected, defaults.Selected);
                string? label = null;

                if (opts.ShowLabels)
                {
                    label = item.Name ?? item.Label;
                }

                if (opts.ShowPercentages)
                {
                    var pct = slice.Value / total * 100.0;
                    label = label is null ? $"{pct:0.##}%" : $"{label} ({pct:0.##}%)";
                }

                payloads.Add(new PiePayload
                {
                    ColorIndex = slice.Index,
                    SerieIndex = serieIndex,
                    ChartId = chartId,
                    Id = slice.Id,
                    StartAngle = slice.StartAngle,
                    EndAngle = slice.EndAngle,
                    MidAngle = slice.MidAngle,
                    Label = label,
                    LabelPosition = slice.LabelPosition,
                    Value = slice.Value,
                    Normal = normal,
                    Hover = hover,
                    Pressed = pressed,
                    Selected = selected,
                    GroupId = serie.Id,
                    Index = i,
                    InteractionState = item.InteractionState,
                    Trigger = item.Trigger,
                    Effect = item.Effect,
                    Animation = ChartAnimationResolver.Resolve(item.Animation, serie.Animation, chartOptions.Animation),
                    AnimationEnabled = chartOptions.AnimationEnabled,
                    AlternateAnimation = serie.AlternateAnimation,
                    IsMultiDonutSlice = false,
                    Tooltip = new ChartTooltipPayload()
                    {
                    }
                });

                item.Trigger = ChartAnimationTrigger.None;
            }

            target.AddLayer(new DonutLayer(
                new DonutPayloadCollection
                {
                    Center = center,
                    Radius = outerRadius,
                    InnerRadius = innerRadius,
                    ShowLabels = opts.ShowLabels,
                    ShowPercentages = opts.ShowPercentages,
                    Slices = payloads
                }
            ));
        }
    }

    /// <inheritdoc />
    public ValueTask ComposeAsync(
        ISurfaceRenderTarget target,
        CO chartOptions)
    {
        Compose(target, chartOptions);

        return ValueTask.CompletedTask;
    }
}
