using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;
using CAO = FluentUI.Blazor.Community.Components.Charts.Options.ChartAxisOptions;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

internal sealed class ChartPolarAxesComposer(
    Func<ChartContext> context,
    Func<CAO> axisOptions,
    Func<IEnumerable<PolarSerie>> polarSeries)
    : ISurfaceComposer<CO>
{
    /// <inheritdoc/>
    public bool Compose(
        ISurfaceRenderTarget target,
        CO options)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(options);

        var ps = polarSeries();

        if (!ps.Any())
        {
            return false;
        }

        var ctx = context();
        var axisOpts = axisOptions();
        var first = ps.FirstOrDefault();
        var payload = ChartPolarAxesEngine.Build(ctx, axisOpts, options, first!.PolarType);

        if (payload is null)
        {
            return false;
        }

        target.AddLayer(new ChartPolarAxesLayer(payload));

        return true;
    }

    /// <inheritdoc/>
    public ValueTask<bool> ComposeAsync(ISurfaceRenderTarget target, CO options) => ValueTask.FromResult(Compose(target, options));
}
