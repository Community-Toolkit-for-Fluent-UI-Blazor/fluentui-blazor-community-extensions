using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components;

internal sealed class ChartComposer
{
    private readonly List<ISurfaceComposer<CO>> _composers = [];

    public void AddRange(params ISurfaceComposer<CO>[] composers)
    {
        _composers.AddRange(composers);
    }

    public void Compose(
        ISurfaceRenderTarget renderTarget,
        CO options)
    {
        foreach (var composer in _composers)
        {
            composer.Compose(renderTarget, options);
        }
    }

    public async ValueTask ComposeAsync(
        ISurfaceRenderTarget renderTarget,
        CO options)
    {
        foreach (var composer in _composers)
        {
            await composer.ComposeAsync(renderTarget, options);
        }
    }
}
