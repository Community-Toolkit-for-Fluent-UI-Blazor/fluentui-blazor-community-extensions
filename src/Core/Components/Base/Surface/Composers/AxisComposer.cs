namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to render X and Y axes on a signature grid canvas when axes display is enabled in the
/// options.
/// </summary>
/// <remarks>This renderer draws horizontal and vertical axes centered on the canvas, using the appearance and
/// scaling options specified in the provided configuration. It is typically used as part of a signature grid rendering
/// pipeline to visually indicate the center or reference axes for user input.</remarks>
internal sealed class AxesComposer : ISurfaceComposer<SurfaceAxesOptions>
{
    /// <inheritdoc />
    public bool Compose(
        ISurfaceRenderTarget target,
        SurfaceAxesOptions axesOptions)
    {
        var payload = PayloadFactory.Create(axesOptions);

        if (payload is null)
        {
            return false;
        }

        target.AddLayer(new AxesLayer(payload));

        return true;
    }

    /// <inheritdoc />
    public ValueTask<bool> ComposeAsync(ISurfaceRenderTarget target, SurfaceAxesOptions options)
    {
        return ValueTask.FromResult(Compose(target, options));
    }
}
