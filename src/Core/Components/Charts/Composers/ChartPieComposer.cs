using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using FluentUI.Blazor.Community.Components.Enums;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;
using PS = FluentUI.Blazor.Community.Components.Charts.Series.PieSerie;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

/// <summary>
/// Represents a composer responsible for generating the necessary layers to render line charts.
/// </summary>
/// <param name="chartId">The unique identifier of the chart, used for interaction payloads.</param>
/// <param name="context">The chart context for composition.</param>
/// <param name="series">The collection of pie series to be rendered.</param>
internal sealed class ChartPieComposer(
    string chartId,
    Func<ChartContext> context,
    Func<IEnumerable<PS>> series)
    : ISurfaceComposer<CO>
{
    /// <inheritdoc />
    public bool Compose(
        ISurfaceRenderTarget target,
        CO chartOptions)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(chartOptions);

        var filtered = series().Where(s => s.IsVisible).ToList();

        if (filtered.Count == 0)
        {
            return false;
        }

        var defaults = chartOptions.PieStyle;
        var opts = chartOptions.Pie;

        for (var serieIndex = 0; serieIndex < filtered.Count; serieIndex++)
        {
            var serie = filtered[serieIndex];
            var ctx = context();

            var slices = PieLayoutEngine.Layout(serie, ctx, opts);

            if (slices.Count == 0)
            {
                continue;
            }

            var plot = ctx.PlotArea;

            var center = new ChartPoint(
                plot.X + plot.Width / 2.0,
                plot.Y + plot.Height / 2.0
            );

            var radius = Math.Min(plot.Width, plot.Height) / 2.0 * opts.OuterRadius;
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
                    SerieIndex = serieIndex,
                    ChartId = chartId,
                    ColorIndex = i,
                    Id = slice.Id,
                    StartAngle = slice.StartAngle,
                    EndAngle = slice.EndAngle,
                    MidAngle = slice.MidAngle,
                    Label = label,
                    LabelPosition = slice.LabelPosition,
                    Value = slice.Value,
                    Index = i,
                    GroupId = slice.GroupId,
                    InteractionState = slice.InteractionState,
                    AlternateAnimation = serie.AlternateAnimation,
                    Normal = normal,
                    Hover = hover,
                    Pressed = pressed,
                    Selected = selected,
                    Trigger = item.Trigger,
                    Effect = item.Effect,
                    Animation = ChartAnimationResolver.Resolve(item.Animation, serie.Animation, chartOptions.Animation),
                    AnimationEnabled = chartOptions.AnimationEnabled,
                    IsMultiDonutSlice = false,
                    Tooltip = new ChartTooltipPayload()
                    {
                    }
                });

                item.Trigger = ChartAnimationTrigger.None;
            }

            target.AddLayer(new PieLayer(
                new PiePayloadCollection
                {
                    Center = center,
                    Radius = radius,
                    TotalValue = total,
                    ShowLabels = opts.ShowLabels,
                    ShowPercentages = opts.ShowPercentages,
                    Slices = payloads
                }
            ));
        }

        return true;
    }

    /// <inheritdoc />
    public ValueTask<bool> ComposeAsync(
        ISurfaceRenderTarget target,
        CO options)
    {
        return ValueTask.FromResult(Compose(target, options));
    }
}

