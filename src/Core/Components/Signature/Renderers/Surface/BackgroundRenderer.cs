namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to render the background grid on a signature surface according to specified rendering
/// options.
/// </summary>
/// <remarks>This class is intended for internal use as part of the signature rendering pipeline. It applies grid
/// settings such as display mode, color, and cell size to the rendering target based on the provided options. The
/// renderer supports both synchronous and asynchronous rendering methods.</remarks>
internal sealed class BackgroundRenderer : ISignatureSurfaceRenderer
{
    /// <inheritdoc />
    public void Render(
        ISurfaceRenderTarget target,
        SignatureRenderingOptions options)
    {
        var background = options.Background;
        var payload = PayloadFactory.Create(background);

        target.StaticBackLayer.SetBackground(payload);
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
