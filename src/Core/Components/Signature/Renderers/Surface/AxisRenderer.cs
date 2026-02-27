namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to render X and Y axes on a signature grid canvas when axes display is enabled in the
/// options.
/// </summary>
/// <remarks>This renderer draws horizontal and vertical axes centered on the canvas, using the appearance and
/// scaling options specified in the provided configuration. It is typically used as part of a signature grid rendering
/// pipeline to visually indicate the center or reference axes for user input.</remarks>
internal sealed class AxesRenderer : ISignatureSurfaceRenderer
{
    /// <inheritdoc />
    public void Render(
        ISurfaceRenderTarget target,
        SignatureRenderingOptions options)
    {
        var axes = options.Axes;
        var payload = PayloadFactory.Create(axes);

        if (axes.Layer == GridLayer.Background)
        {
            target.StaticBackLayer.SetAxes(payload);
        }
        else
        {
            target.StaticFrontLayer.SetAxes(payload);
        }
    }

    /// <inheritdoc />
    public ValueTask RenderAsync(ISurfaceRenderTarget target, SignatureRenderingOptions options)
    {
        Render(target, options);

        return ValueTask.CompletedTask;
    }
}
