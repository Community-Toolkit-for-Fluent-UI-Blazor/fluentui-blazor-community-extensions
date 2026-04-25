using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;
using CAO = FluentUI.Blazor.Community.Components.Charts.Options.ChartAxisOptions;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Layers;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

internal sealed class ChartPolarAxesComposer(
    Func<ChartContext> context,
    Func<CAO> axisOptions,
    Func<bool> isPolarSeries)
    : ISurfaceComposer<CO>
{
    /// <inheritdoc/>
    public bool Compose(
        ISurfaceRenderTarget target,
        CO options)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(options);

        if (!isPolarSeries())
        {
            return false;
        }

        var ctx = context();
        var axisOpts = axisOptions();
        var payload = ChartPolarAxesEngine.Build(ctx, axisOpts, options);

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
