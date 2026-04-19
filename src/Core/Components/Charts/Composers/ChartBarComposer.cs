using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using FluentUI.Blazor.Community.Components.Enums;
using BS = FluentUI.Blazor.Community.Components.Charts.Series.BarSerie;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;
using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

/// <summary>
/// Composes bar chart layers for a given chart context and series collection.
/// </summary>
/// <remarks>This composer is responsible for generating the visual representation of bar series within a chart.
/// It filters out any series that are not visible and applies default or item-specific styles as needed. The resulting
/// layers are added to the rendering target for display.</remarks>
/// <param name="chartId">The unique identifier for the chart instance, used for associating rendered elements with the correct chart context.</param>
/// <param name="context">The chart context that provides coordinate and layout information for rendering the bar chart.</param>
/// <param name="series">The collection of bar series to be rendered. Only visible series are included in the composition.</param>
internal sealed class ChartBarComposer(
    string chartId,
    Func<ChartContext> context,
    Func<IEnumerable<BS>> series)
    : ISurfaceComposer<CO>
{
    /// <inheritdoc />
    public void Compose(
        ISurfaceRenderTarget target,
        CO chartOptions)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(chartOptions);

        var filtered = series().Where(s => s.IsVisible).ToList();

        if (filtered.Count == 0)
        {
            return;
        }

        var defaults = chartOptions.DefaultBarStyle;
        var ctx = context();
        var payloads = new List<BarPayload>();
        var minValue = 0.0;
        var maxValue = 0.0;

        if (filtered.Any(s => s.IsStacked))
        {
            var seriesCount = filtered.Count;
            var maxCategoryCount = 0;

            for (var s = 0; s < seriesCount; s++)
            {
                var count = filtered[s].Items.Count;

                if (count > maxCategoryCount)
                {
                    maxCategoryCount = count;
                }
            }

            for (var cat = 0; cat < maxCategoryCount; cat++)
            {
                var sum = 0.0;

                for (var s = 0; s < seriesCount; s++)
                {
                    var serie = filtered[s];

                    if (cat < serie.Items.Count)
                    {
                        sum += serie.Items[cat].Value;
                    }
                }

                if (sum > maxValue)
                {
                    maxValue = sum;
                }
            }
        }
        else
        {
            minValue = filtered.SelectMany(s => s.Items.Select(i => i.Value)).Min();
            maxValue = filtered.SelectMany(s => s.Items.Select(i => i.Value)).Max();
        }

        for (var serieIndex = 0; serieIndex < filtered.Count; serieIndex++)
        {
            var serie = filtered[serieIndex];
            var bars = serie.IsStacked ? StackedBarLayoutEngine.Layout(serie, filtered, serieIndex, ctx) :
                                         BarLayoutEngine.Layout(serie, filtered, minValue, maxValue, serieIndex, ctx);

            for (var i = 0; i < bars.Count; i++)
            {
                var bar = bars[i];
                var item = serie.Items[bar.CategoryIndex];

                payloads.Add(new BarPayload
                {
                    SerieIndex = serieIndex,
                    Id = bar.Id,
                    X = bar.X,
                    Y = bar.Y,
                    Width = bar.Width,
                    Height = bar.Height,
                    CategoryIndex = bar.CategoryIndex,
                    Value = bar.Value,
                    Index = i,
                    GroupId = serie.Id,
                    ChartId = chartId,
                    InteractionState = item.InteractionState,
                    Trigger = item.Trigger,
                    Effect = item.Effect,
                    Normal = ChartStyleResolver.Resolve(item.Style?.Normal, defaults.Normal),
                    Hover = ChartStyleResolver.Resolve(item.Style?.Hover, defaults.Hover),
                    Pressed = ChartStyleResolver.Resolve(item.Style?.Pressed, defaults.Pressed),
                    Selected = ChartStyleResolver.Resolve(item.Style?.Selected, defaults.Selected),
                    AnimationEnabled = chartOptions.AnimationEnabled,
                    Animation = ChartAnimationResolver.Resolve(item.Animation, serie.Animation, chartOptions.Animation),
                    Tooltip = GetTooltipPayload(item)
                });

                item.Trigger = ChartAnimationTrigger.None;
            }
        }

        if (payloads.Count > 0)
        {
            payloads.Sort((a, b) =>
            {
                return a.CategoryIndex.CompareTo(b.CategoryIndex);
            });

            target.AddLayer(new BarLayer(new BarPayloadCollection($"chart-bars-group", payloads)));
        }
    }

    private static ChartTooltipPayload GetTooltipPayload(CategoryItem item)
    {
        var tooltip = item.Tooltip;

        return new ChartTooltipPayload()
        {
            IsVisible = tooltip?.Visible ?? false,
            Text = tooltip?.Text,
            Position = tooltip?.Placement ?? ChartTooltipPlacement.Pointer,
        };
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
