using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

/// <summary>
/// Represents a composer responsible for generating the necessary layers to render line charts.
/// </summary>
/// <param name="context">The chart context for composition.</param>
/// <param name="series">The collection of category line series to be rendered.</param>
/// <param name="chartId">The unique identifier of the chart, used for interaction payloads.</param>
internal sealed class ChartLineComposer(
    string chartId,
    Func<ChartContext> context,
    Func<IEnumerable<CategoryLineSerie>> series)
    : ISurfaceComposer<CO>
{
    /// <inheritdoc />
    public void Compose(
        ISurfaceRenderTarget target,
        CO chartOptions)
    {
        var filtered = series().Where(s => s.IsVisible).ToList();

        if (filtered.Count == 0)
        {
            return;
        }

        filtered.Sort((a, b) =>
        {
            return a.Items.Sum(x => x.Value).CompareTo(b.Items.Sum(x => x.Value));
        });

        var defaults = chartOptions.DefaultLineStyles;
        var markerDefaults = chartOptions.DefaultMarkerStyles;
        var ctx = context();

        for (var i = 0; i < filtered.Count; i++)
        {
            var serie = filtered[i];

            if (serie.IsStacked)
            {
                var (top, bottom, points) = StackedAreaLayoutEngine.Layout(serie, filtered, i, ctx);

                var payloadPoints = points.Select(p =>
                {
                    var item = serie.Items[p.CategoryIndex];

                    return new LinePointPayload
                    {
                        SerieIndex = i,
                        ChartId = chartId,
                        Id = p.Id,
                        X = p.X,
                        Y = p.Y,
                        CategoryIndex = p.CategoryIndex,
                        Value = p.Value,
                        AnimationEnabled = chartOptions.AnimationEnabled,
                        InteractionState = item.InteractionState,
                        Index = p.CategoryIndex,
                        GroupId = serie.Id,
                        Trigger = item.Trigger,
                        Effect = item.Effect,
                        Normal = ChartStyleResolver.Resolve(item.Style?.Normal, markerDefaults.Normal),
                        Hover = ChartStyleResolver.Resolve(item.Style?.Hover, markerDefaults.Hover),
                        Pressed = ChartStyleResolver.Resolve(item.Style?.Pressed, markerDefaults.Pressed),
                        Selected = ChartStyleResolver.Resolve(item.Style?.Selected, markerDefaults.Selected),
                        Animation = ChartAnimationResolver.Resolve(item.Animation, serie.Animation, chartOptions.Animation),
                        Tooltip = new ChartTooltipPayload()
                    };
                }).ToList();

                var payload = new StackedAreaPayload
                {
                    ChartId = chartId,
                    Id = serie.Id,
                    SerieIndex = i,
                    AnimationEnabled = serie.AnimationEnabled,
                    GroupId = string.Empty,
                    Index = i,
                    TopPath = new LinePathPayload
                    {
                        Id = top.Id,
                        Points = top.Points,
                        Smooth = top.Smooth
                    },
                    BottomPath = new LinePathPayload
                    {
                        Id = bottom.Id,
                        Points = bottom.Points,
                        Smooth = bottom.Smooth
                    },
                    Points = payloadPoints,
                    Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, defaults.Normal),
                    Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, defaults.Hover),
                    Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, defaults.Pressed),
                    Selected = ChartStyleResolver.Resolve(serie.Style?.Selected, defaults.Selected),
                    Animation = ChartAnimationResolver.Resolve(null, serie.Animation, chartOptions.Animation),
                    Tooltip = new ChartTooltipPayload()
                };

                target.AddLayer(new StackedAreaLayer(payload));
            }
            else
            {
                var (path, points) = CategoryLineLayoutEngine.Layout(serie, ctx);

                var payloadPoints = points
                    .Select(p =>
                    {
                        var item = serie.Items[p.CategoryIndex];

                        return new LinePointPayload
                        {
                            SerieIndex = i,
                            ChartId = chartId,
                            Id = p.Id,
                            X = p.X,
                            Y = p.Y,
                            CategoryIndex = p.CategoryIndex,
                            Value = p.Value,
                            AnimationEnabled = chartOptions.AnimationEnabled,
                            InteractionState = item.InteractionState,
                            Index = p.CategoryIndex,
                            GroupId = serie.Id,
                            Trigger = item.Trigger,
                            Effect = item.Effect,
                            Normal = ChartStyleResolver.Resolve(item.Style?.Normal, markerDefaults.Normal),
                            Hover = ChartStyleResolver.Resolve(item.Style?.Hover, markerDefaults.Hover),
                            Pressed = ChartStyleResolver.Resolve(item.Style?.Pressed, markerDefaults.Pressed),
                            Selected = ChartStyleResolver.Resolve(item.Style?.Selected, markerDefaults.Selected),
                            Animation = ChartAnimationResolver.Resolve(item.Animation, serie.Animation, chartOptions.Animation),
                            Tooltip = new ChartTooltipPayload()
                            {
                            }
                        };
                    })
                    .ToList();

                if (serie.IsArea)
                {
                    var payload = new AreaPayload
                    {
                        ChartId = chartId,
                        Id = serie.Id,
                        SerieIndex = i,
                        AnimationEnabled = serie.AnimationEnabled,
                        GroupId = string.Empty,
                        Index = i,
                        Path = new LinePathPayload()
                        {
                            Id = path.Id,
                            Points = path.Points,
                            Smooth = path.Smooth
                        },
                        Points = payloadPoints,
                        BaselineY = ctx.PlotArea.Bottom,
                        Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, defaults.Normal),
                        Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, defaults.Hover),
                        Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, defaults.Pressed),
                        Selected = ChartStyleResolver.Resolve(serie.Style?.Selected, defaults.Selected),
                        Animation = ChartAnimationResolver.Resolve(null, serie.Animation, chartOptions.Animation),
                        Tooltip = new ChartTooltipPayload()
                        {
                        }
                    };

                    target.AddLayer(new AreaLayer(payload));
                }
                else
                {
                    var payload = new LinePayload()
                    {
                        ChartId = chartId,
                        Id = serie.Id,
                        SerieIndex = i,
                        AnimationEnabled = serie.AnimationEnabled,
                        GroupId = string.Empty,
                        Index = i,
                        Path = new LinePathPayload()
                        {
                            Id = path.Id,
                            Points = path.Points,
                            Smooth = path.Smooth
                        },
                        Points = payloadPoints,
                        Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, defaults.Normal),
                        Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, defaults.Hover),
                        Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, defaults.Pressed),
                        Selected = ChartStyleResolver.Resolve(serie.Style?.Selected, defaults.Selected),
                        Animation = ChartAnimationResolver.Resolve(null, serie.Animation, chartOptions.Animation),
                        Tooltip = new ChartTooltipPayload()
                        {
                        }
                    };

                    target.AddLayer(new LineLayer(payload));
                }
            }
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
