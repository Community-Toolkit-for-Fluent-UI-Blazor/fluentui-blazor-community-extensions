using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using FluentUI.Blazor.Community.Components.Enums;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

internal sealed class ChartPolarComposer : ISurfaceComposer<CO>
{
    private readonly string _chartId;
    private readonly Func<ChartContext> _context;
    private readonly Func<IEnumerable<PolarSerie>> _series;

    public ChartPolarComposer(
        string chartId,
        Func<ChartContext> context,
        Func<IEnumerable<PolarSerie>> series)
    {
        _chartId = chartId;
        _context = context;
        _series = series;
    }

    public bool Compose(ISurfaceRenderTarget target, CO options)
    {
        var filtered = _series().Where(s => s.IsVisible).ToList();

        if (filtered.Count == 0)
        {
            return false;
        }

        var ctx = _context();

        for (var i = 0; i < filtered.Count; i++)
        {
            var serie = filtered[i];

            switch (serie.PolarType)
            {
                case PolarChartType.Radar:
                    BuildRadar(target, serie, i, ctx, options);
                    break;
            }
        }

        return true;
    }

    public ValueTask<bool> ComposeAsync(ISurfaceRenderTarget target, CO options)
        => ValueTask.FromResult(Compose(target, options));

    private void BuildRadar(
        ISurfaceRenderTarget target,
        PolarSerie serie,
        int index,
        ChartContext ctx,
        CO options)
    {
        var (path, polarPoints) = RadarLayoutEngine.Layout(serie, ctx);

        var markerDefaults = options.DefaultMarkerStyles;
        var lineDefaults = options.DefaultLineStyles;

        var payloadPoints = polarPoints
            .Select(p =>
            {
                var item = serie.Items[p.CategoryIndex];

                return new LinePointPayload
                {
                    SerieIndex = index,
                    ChartId = _chartId,
                    Id = p.Id,
                    X = p.X,
                    Y = p.Y,
                    CategoryIndex = p.CategoryIndex,
                    Value = p.Value,
                    AnimationEnabled = options.AnimationEnabled,
                    InteractionState = item.InteractionState,
                    Index = p.CategoryIndex,
                    GroupId = serie.Id,
                    Trigger = item.Trigger,
                    Effect = item.Effect,
                    Normal = ChartStyleResolver.Resolve(item.Style?.Normal, markerDefaults.Normal),
                    Hover = ChartStyleResolver.Resolve(item.Style?.Hover, markerDefaults.Hover),
                    Pressed = ChartStyleResolver.Resolve(item.Style?.Pressed, markerDefaults.Pressed),
                    Selected = ChartStyleResolver.Resolve(item.Style?.Selected, markerDefaults.Selected),
                    Animation = ChartAnimationResolver.Resolve(item.Animation, serie.Animation, options.Animation),
                    Tooltip = new ChartTooltipPayload()
                };
            })
            .ToList();

        var serieOptions = serie.Options as RadarSerieOptions;

        var payload = new RadarPayload
        {
            ChartId = _chartId,
            Id = serie.Id,
            SerieIndex = index,
            AnimationEnabled = serie.AnimationEnabled,
            GroupId = string.Empty,
            Index = index,
            Path = new PolarPathPayload
            {
                Id = path.Id,
                Points = polarPoints,
                Closed = true
            },
            Points = payloadPoints,
            Normal = ChartStyleResolver.Resolve(serie.Style?.Normal, lineDefaults.Normal),
            Hover = ChartStyleResolver.Resolve(serie.Style?.Hover, lineDefaults.Hover),
            Pressed = ChartStyleResolver.Resolve(serie.Style?.Pressed, lineDefaults.Pressed),
            Selected = ChartStyleResolver.Resolve(serie.Style?.Selected, lineDefaults.Selected),
            Animation = ChartAnimationResolver.Resolve(null, serie.Animation, options.Animation),
            Tooltip = new ChartTooltipPayload(),
            FillArea = serieOptions?.FillArea.HasValue ?? false ? serieOptions.FillArea.GetValueOrDefault() : options.DefaultRadarOptions.FillArea ?? false,
            Grid = options.DefaultRadarAxesOptions.Grid
        };

        target.AddLayer(new RadarLayer(payload));
    }
}
