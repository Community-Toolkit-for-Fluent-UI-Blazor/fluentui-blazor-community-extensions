using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using FluentUI.Blazor.Community.Components.Enums;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;
using DS = FluentUI.Blazor.Community.Components.Charts.Series.DonutSerie;
using MDS = FluentUI.Blazor.Community.Components.Charts.Series.MultiDonutSerie;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

/// <summary>
/// Represents a composer responsible for generating the necessary layers to render donuts charts.
/// </summary>
/// <param name="chartId">The unique identifier of the chart, used for interaction payloads.</param>
/// <param name="context">The chart context for composition.</param>
/// <param name="series">The collection of donut series to be rendered.</param>
internal sealed class ChartMultiDonutComposer(
    string chartId,
    Func<ChartContext> context,
    Func<IEnumerable<MDS>> series)
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

        var defaults = chartOptions.DefaultPieStyle;
        var ctx = context();

        for (var serieIndex = 0; serieIndex < filtered.Count; serieIndex++)
        {
            var multi = filtered[serieIndex];

            if (multi is null)
            {
                continue;
            }

            var rings = MultiDonutLayoutEngine.Layout(multi, ctx);

            if (rings.Count == 0)
            {
                continue;
            }

            var outerMax = rings.Max(r => r.OuterRadius);
            var innerMin = rings.Min(r => r.InnerRadius);
            var ringCount = rings.Count;
            var totalRadius = outerMax - innerMin;
            var thickness = totalRadius / ringCount;
            var globalColorIndex = 0;
            var slicesPerRing = new List<List<PiePayload>>(ringCount);
            var optsPerRing = new List<RadialSerieOptions>(ringCount);

            for (var r = 0; r < ringCount; r++)
            {
                var ring = rings[r];
                var donutSerie = multi.Series[r];
                var opts = donutSerie.Options ?? new RadialSerieOptions();
                optsPerRing.Add(opts);

                var items = donutSerie.Items.Where(i => i.IsVisible).ToList();
                var total = items.Sum(i => i.Value);
                var payloads = new List<PiePayload>(ring.Slices.Count);

                for (var i = 0; i < ring.Slices.Count; i++)
                {
                    var slice = ring.Slices[i];
                    var item = donutSerie.Items[slice.Index];

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
                        label = label is null ? $"{pct:0.#}%" : $"{label} ({pct:0.#}%)";
                    }

                    payloads.Add(new PiePayload
                    {
                        SerieIndex = serieIndex,
                        ChartId = chartId,
                        Id = slice.Id,
                        StartAngle = slice.StartAngle,
                        EndAngle = slice.EndAngle,
                        MidAngle = slice.MidAngle,
                        Label = label,
                        LabelPosition = slice.LabelPosition,
                        Value = slice.Value,
                        GroupId = slice.GroupId,
                        Index = slice.Index,
                        InteractionState = slice.InteractionState,
                        Normal = normal,
                        Hover = hover,
                        Pressed = pressed,
                        Selected = selected,
                        Trigger = item.Trigger,
                        Effect = item.Effect,
                        Animation = ChartAnimationResolver.Resolve(item.Animation, multi.Animation, chartOptions.Animation),
                        AnimationEnabled = multi.AnimationEnabled,
                        AlternateAnimation = multi.AlternateAnimation ?? donutSerie.AlternateAnimation,
                        IsMultiDonutSlice = true,
                        ColorIndex = ComputeSliceColorIndex(multi, donutSerie, slice.Index, ref globalColorIndex),
                        Tooltip = new ChartTooltipPayload()
                        {
                        }
                    });

                    item.Trigger = ChartAnimationTrigger.None;
                }

                slicesPerRing.Add(payloads);
            }

            var donutCollections = new List<DonutPayloadCollection>(ringCount);

            for (var i = 0; i < ringCount; i++)
            {
                var ringOuter = outerMax - (ringCount - 1 - i) * thickness;
                var ringInner = ringOuter - thickness;
                var opts = optsPerRing[i];

                donutCollections.Add(new DonutPayloadCollection
                {
                    Center = rings[0].Center,
                    InnerRadius = ringInner,
                    Radius = ringOuter,
                    Slices = slicesPerRing[i],
                    ShowLabels = opts.ShowLabels,
                    ShowPercentages = opts.ShowPercentages
                });
            }

            target.AddLayer(new MultiDonutLayer(
                new MultiDonutPayloadCollection
                {
                    Center = rings[0].Center,
                    Rings = donutCollections
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

    /// <summary>
    /// Computes the color palette index for a donut slice based on the palette mode.
    /// </summary>
    /// <param name="multi">The multi-donut series containing the palette mode configuration.</param>
    /// <param name="donut">The specific donut series containing the slice.</param>
    /// <param name="sliceIndex">The index of the slice within the donut series.</param>
    /// <param name="globalIndex">The global index of the slice across all donuts.</param>
    /// <returns>The computed color palette index based on the configured palette mode.</returns>
    private static int ComputeSliceColorIndex(
        MDS multi,
        DS donut,
        int sliceIndex,
        ref int globalIndex)
    {
        var index = multi.PaletteMode switch
        {
            DonutPaletteMode.ByCategory => sliceIndex,
            DonutPaletteMode.ByDonut => multi.Series.IndexOf(donut),
            DonutPaletteMode.BySlice => globalIndex,
            _ => sliceIndex
        };

        if (multi.PaletteMode == DonutPaletteMode.BySlice)
        {
            globalIndex++;
        }

        return index;
    }
}
