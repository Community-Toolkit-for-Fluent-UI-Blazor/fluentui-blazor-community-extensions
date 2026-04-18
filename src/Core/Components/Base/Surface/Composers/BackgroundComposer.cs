namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to render the background grid on a signature surface according to specified rendering
/// options.
/// </summary>
/// <remarks>This class is intended for internal use as part of the signature rendering pipeline. It applies grid
/// settings such as display mode, color, and cell size to the rendering target based on the provided options. The
/// renderer supports both synchronous and asynchronous rendering methods.</remarks>
internal sealed class BackgroundComposer : ISurfaceComposer<SurfaceBackgroundOptions>
{
    /// <inheritdoc />
    public void Compose(
        ISurfaceRenderTarget target,
        SurfaceBackgroundOptions options)
    {
        var payload = PayloadFactory.Create(options);

        if (payload is null)
        {
            return;
        }

        target.AddLayer(new BackgroundLayer(payload));
    }

    /// <inheritdoc />
    public ValueTask ComposeAsync(
        ISurfaceRenderTarget target,
        SurfaceBackgroundOptions options)
    {
        Compose(target, options);

        return ValueTask.CompletedTask;
    }
}
