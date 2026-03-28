namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to render X and Y axes on a signature grid canvas when axes display is enabled in the
/// options.
/// </summary>
/// <remarks>This renderer draws horizontal and vertical axes centered on the canvas, using the appearance and
/// scaling options specified in the provided configuration. It is typically used as part of a signature grid rendering
/// pipeline to visually indicate the center or reference axes for user input.</remarks>
internal sealed class AxesRenderer : ISurfaceRenderer<SurfaceAxesOptions>
{
    /// <inheritdoc />
    public void Render(
        ISurfaceRenderTarget target,
        SurfaceAxesOptions axesOptions)
    {
        var payload = PayloadFactory.Create(axesOptions);

        if (payload is null)
        {
            return;
        }

        target.AddLayer(new AxesLayer(payload));
    }

    /// <inheritdoc />
    public ValueTask RenderAsync(ISurfaceRenderTarget target, SurfaceAxesOptions options)
    {
        Render(target, options);

        return ValueTask.CompletedTask;
    }
}
