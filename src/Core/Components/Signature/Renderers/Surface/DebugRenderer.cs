namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a debug implementation of the signature surface renderer that displays diagnostic information such as DPI,
/// stroke count, and point count on the canvas.
/// </summary>
/// <remarks>This renderer is intended for development and debugging purposes. It overlays diagnostic text on the
/// canvas to assist with troubleshooting and verifying signature input data. It is not intended for use in production
/// environments.</remarks>
internal sealed class DebugRenderer : ISignatureStrokeRenderer
{
    /// <inheritdoc />
    public void Render(
        ISurfaceRenderTarget target,
        IReadOnlyList<SignatureStroke> strokes,
        SignatureRenderingOptions options)
    {
        var payload = PayloadFactory.CreateDebug(target.View.Dpi, strokes.Count, strokes.Sum(s => s.Points.Count), options.Debug);

        target.AddLayer(new DebugLayer(payload));
    }

    /// <inheritdoc />
    public ValueTask RenderAsync(
        ISurfaceRenderTarget target,
        IReadOnlyList<SignatureStroke> strokes,
        SignatureRenderingOptions options)
    {
        Render(target, strokes, options);

        return ValueTask.CompletedTask;
    }
}
