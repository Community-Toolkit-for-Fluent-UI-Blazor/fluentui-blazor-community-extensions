namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a renderer that draws a grid on a signature surface using either dots or lines, based on the specified
/// display mode.
/// </summary>
/// <remarks>This class delegates rendering to either a dots or lines grid renderer depending on the grid display
/// mode set in the signature options. It is intended for use with signature surfaces that require a visual grid
/// background for guidance or alignment.</remarks>
internal sealed class GridRenderer : ISurfaceRenderer<SurfaceGridOptions>
{
    /// <inheritdoc />
    public void Render(
        ISurfaceRenderTarget target,
        SurfaceGridOptions options)
    {
        var payload = PayloadFactory.Create(options);

        if (payload is null)
        {
            return;
        }

        target.AddLayer(new GridLayer(payload));
    }

    /// <inheritdoc />
    public ValueTask RenderAsync(
        ISurfaceRenderTarget target,
        SurfaceGridOptions options)
    {
        Render(target, options);

        return ValueTask.CompletedTask;
    }
}
