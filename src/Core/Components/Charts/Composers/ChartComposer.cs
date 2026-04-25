using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components;

internal sealed class ChartComposer
{
    private readonly List<ISurfaceComposer<CO>> _composers = [];

    public void AddRange(params ISurfaceComposer<CO>[] composers)
    {
        _composers.AddRange(composers);
    }

    public bool Compose(
        ISurfaceRenderTarget renderTarget,
        CO options)
    {
        var composed = false;

        foreach (var composer in _composers)
        {
            composed |= composer.Compose(renderTarget, options);
        }

        return composed;
    }

    public async ValueTask<bool> ComposeAsync(
        ISurfaceRenderTarget renderTarget,
        CO options)
    {
        var composed = false;

        foreach (var composer in _composers)
        {
            composed |= await composer.ComposeAsync(renderTarget, options);
        }

        return composed;
    }
}
