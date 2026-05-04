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
    public bool Compose(
        ISurfaceRenderTarget target,
        SurfaceBackgroundOptions options)
    {
        var payload = PayloadFactory.Create(options);

        if (payload is null)
        {
            return false;
        }

        target.AddLayer(new BackgroundLayer(payload));

        return true;
    }

    /// <inheritdoc />
    public ValueTask<bool> ComposeAsync(
        ISurfaceRenderTarget target,
        SurfaceBackgroundOptions options)
    {
        return ValueTask.FromResult(Compose(target, options));
    }
}
