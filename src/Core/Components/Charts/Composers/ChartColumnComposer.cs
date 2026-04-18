using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using CS = FluentUI.Blazor.Community.Components.Charts.Series.ColumnSerie;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

/// <summary>
/// Represents a composer responsible for rendering column series in a chart.
/// </summary>
/// <param name="chartId">The unique identifier for the chart instance, used to associate rendered elements with the correct chart context.</param>
/// <param name="context">The chart context containing necessary information for rendering.</param>
/// <param name="series">The collection of column series to be rendered.</param>
internal sealed class ChartColumnComposer(
    string chartId,
    Func<ChartContext> context,
    Func<IEnumerable<CS>> series)
    : ISurfaceComposer<CO>
{
    /// <inheritdoc/>
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
        var minValue = filtered.SelectMany(s => s.Items.Select(i => i.Value)).Min();
        var maxValue = filtered.SelectMany(s => s.Items.Select(i => i.Value)).Max();
        var ctx = context();

        for (var serieIndex = 0; serieIndex < filtered.Count; serieIndex++)
        {
            var serie = filtered[serieIndex];
            var columns = ColumnLayoutEngine.Layout(serie, filtered, minValue, maxValue, serieIndex, context());
            var payloads = new List<ColumnPayload>(columns.Count);

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                var item = serie.Items[column.CategoryIndex];

                payloads.Add(new ColumnPayload
                {
                    SerieIndex = serieIndex,
                    ChartId = chartId,
                    Id = column.Id,
                    X = column.X,
                    Y = column.Y,
                    Width = column.Width,
                    Height = column.Height,
                    CategoryIndex = column.CategoryIndex,
                    Value = column.Value,
                    Index = i,
                    GroupId = serie.Id,
                    InteractionState = item.InteractionState,
                    Trigger = item.Trigger,
                    Effect = item.Effect,
                    Normal = ChartStyleResolver.Resolve(item.Style?.Normal, defaults.Normal),
                    Hover = ChartStyleResolver.Resolve(item.Style?.Hover, defaults.Hover),
                    Pressed = ChartStyleResolver.Resolve(item.Style?.Pressed, defaults.Pressed),
                    Selected = ChartStyleResolver.Resolve(item.Style?.Selected, defaults.Selected),
                    Animation = ChartAnimationResolver.Resolve(item.Animation, serie.Animation, chartOptions.Animation),
                    AnimationEnabled = chartOptions.AnimationEnabled,
                    Tooltip = new ChartTooltipPayload()
                    {
                    }
                });

                item.Trigger = Enums.ChartAnimationTrigger.None;
            }

            target.AddLayer(new ColumnLayer(new ColumnPayloadCollection(payloads)));
        }
    }

    /// <inheritdoc/>
    public ValueTask ComposeAsync(
        ISurfaceRenderTarget target,
        CO chartOptions)
    {
        Compose(target, chartOptions);

        return ValueTask.CompletedTask;
    }
}
