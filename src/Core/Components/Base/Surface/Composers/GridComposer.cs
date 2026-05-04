namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a renderer that draws a grid on a signature surface using either dots or lines, based on the specified
/// display mode.
/// </summary>
/// <remarks>This class delegates rendering to either a dots or lines grid renderer depending on the grid display
/// mode set in the signature options. It is intended for use with signature surfaces that require a visual grid
/// background for guidance or alignment.</remarks>
internal sealed class GridComposer : ISurfaceComposer<SurfaceGridOptions>
{
    /// <inheritdoc />
    public bool Compose(
        ISurfaceRenderTarget target,
        SurfaceGridOptions options)
    {
        var payload = PayloadFactory.Create(options);

        if (payload is null)
        {
            return false;
        }

        target.AddLayer(new GridLayer(payload));

        return true;
    }

    /// <inheritdoc />
    public ValueTask<bool> ComposeAsync(
        ISurfaceRenderTarget target,
        SurfaceGridOptions options)
    {
        return ValueTask.FromResult(Compose(target, options));
    }
}
