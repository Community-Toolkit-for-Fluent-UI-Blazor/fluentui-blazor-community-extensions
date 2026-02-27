namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a renderer that draws a grid on a signature surface using either dots or lines, based on the specified
/// display mode.
/// </summary>
/// <remarks>This class delegates rendering to either a dots or lines grid renderer depending on the grid display
/// mode set in the signature options. It is intended for use with signature surfaces that require a visual grid
/// background for guidance or alignment.</remarks>
internal sealed class GridRenderer : ISignatureSurfaceRenderer
{
    /// <inheritdoc />
    public void Render(
        ISurfaceRenderTarget target,
        SignatureRenderingOptions options)
    {
        var grid = options.Grid;
        var payload = PayloadFactory.Create(grid);

        if (grid.Layer == GridLayer.Background)
        {
            target.StaticBackLayer.SetGrid(payload);
        }
        else
        {
            target.StaticFrontLayer.SetGrid(payload);
        }
    }

    /// <inheritdoc />
    public ValueTask RenderAsync(
        ISurfaceRenderTarget target,
        SignatureRenderingOptions options)
    {
        Render(target, options);

        return ValueTask.CompletedTask;
    }
}
